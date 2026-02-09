using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Graph;
using _Project.Scripts.Level;
using _Project.Scripts.Node;

public class GraphController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private NodePrefabs _prefabs;
	
    [Header("Settings")]
    [Tooltip("The parent transform where level prefabs will be instantiated.")]
    [SerializeField] private Transform _gridContainer;
	
    private Dictionary<NodeView, NodeController> _nodeControllerMap = new Dictionary<NodeView, NodeController>();
    private List<NodeController> _nodes = new ();
    private BFSController _bfsController = new ();
    private bool _isLevelActive;
    private GameObject _currentLevelObject;
    
    public bool IsLevelActive => _isLevelActive;
    public Dictionary<NodeView, NodeController> NodeControllerMap => _nodeControllerMap;
    public List<NodeController> Nodes => _nodes;

    private static readonly Dictionary<NodeType, byte> _shapeLibrary = new()
    {
	    { NodeType.Line, 10 },		// 1010 (Up + Down)
	    { NodeType.LShape, 12 },	// 1100 (Up + Right)
	    { NodeType.TShape, 14 },	// 1110 (Up + Right + Down)
	    { NodeType.Cross, 15 },		// 1111 (All sides)
	    { NodeType.Source, 8 },		// 1000 (Single output Up)
	    { NodeType.Target, 8 }		// 1000 (Single input Up)
    };
    
    public event Action OnLevelCompleted;

    public void LoadLevel(LevelData levelData)
    {
        ClearLevel();

        if (levelData.LevelPrefab == null)
        {
            Debug.LogError("[GraphController] Level Data is missing the Prefab!");
            return;
        }

        _currentLevelObject = Instantiate(levelData.LevelPrefab, _gridContainer);

        var cellSize = levelData.CellSize;
        var builderNodes = _currentLevelObject.GetComponentsInChildren<NodeBuilder>();
        if (builderNodes.Length == 0) return;
        
        var gridMap = new Dictionary<Vector2, NodeController>();
        float minX = builderNodes.Min(v => v.transform.position.x);
        float maxY = builderNodes.Max(v => v.transform.position.y);

        foreach (var node in builderNodes)
        {
            int x = Mathf.RoundToInt((node.transform.position.x - minX) / cellSize);
            int y = Mathf.RoundToInt((maxY - node.transform.position.y) / cellSize); // Y grows downwards
            var pos = new Vector2(x, y);
            
            byte baseShape = 0;
            if (_shapeLibrary.TryGetValue(node.Type, out var value))
            {
                baseShape = value;
            }
            else
            {
                Debug.LogWarning($"[GraphController] Unknown Shape ID: {node.Type}. Defaulting to 0.");
            }
            
            var prefab = _prefabs.GetPrefab(node.Type);
            var view = Instantiate(prefab, node.transform.position, Quaternion.identity, _currentLevelObject.transform);
            var model = new NodeModel(node.Type, baseShape, node.Direction);
            var ctrl = new NodeController(model, view);
            
            node.gameObject.SetActive(false);
            
            _nodes.Add(ctrl);
            _nodeControllerMap.Add(view, ctrl);
            if (!gridMap.ContainsKey(pos)) gridMap.Add(pos, ctrl);
        }

        foreach (var node in builderNodes)
        {
	        Destroy(node.gameObject);
        }

        foreach (var kvp in gridMap)
        {
            var pos = kvp.Key;
            var current = kvp.Value;
            
            LinkIfExists(current, Direction.Up,    pos + Vector2.down * cellSize, gridMap);
            LinkIfExists(current, Direction.Down,  pos + Vector2.up * cellSize,   gridMap); 
            LinkIfExists(current, Direction.Right, pos + Vector2.right * cellSize, gridMap);
            LinkIfExists(current, Direction.Left,  pos + Vector2.left * cellSize,  gridMap);
        }

        _cameraController.CenterCamera(_nodes.Select(i => i.View), cellSize);
        
        _isLevelActive = true;
        RecalculateFlow();
    }

    private void LinkIfExists(NodeController current, Direction dir, Vector2 neighborPos, Dictionary<Vector2, NodeController> map)
    {
        if (map.TryGetValue(neighborPos, out var neighbor))
        {
            current.RegisterNeighbor(dir, neighbor);
        }
    }

    private void ClearLevel()
    {
        _nodeControllerMap.Clear();

        if (_nodes.Count > 0)
        {
	        foreach (var node in _nodes)
	        {
		        Destroy(node.View.gameObject);
	        }
	        
	        _nodes.Clear();
        }
        
        if (_currentLevelObject != null)
        {
            Destroy(_currentLevelObject);
        }
        
        _isLevelActive = false;
    }
	
    public void OnNodeInteracted(NodeController node)
    {
        if (!_isLevelActive) return;

        node.RotateClockwise();

        RecalculateFlow();
    }
    
    private void RecalculateFlow()
    {
        _bfsController.Run(_nodes);

        if (CheckWinCondition())
        {
            StartCoroutine(VictoryDelay());
        }
    }
    
    IEnumerator VictoryDelay()
    {
	    _isLevelActive = false;
	    Debug.Log("[GraphController] LEVEL COMPLETE!");
	    
        yield return new WaitForSeconds(1f);
	    
	    OnLevelCompleted?.Invoke();
    }

    private bool CheckWinCondition()
    {
        bool allTargetsPowered = _nodes
            .Where(n => n.IsTarget)
            .All(n => n.IsPowered);

        return allTargetsPowered;
    }
}