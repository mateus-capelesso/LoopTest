using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Graph;
using _Project.Scripts.Level;
using _Project.Scripts.Node;

public class GraphController : MonoBehaviour
{
    public event System.Action OnLevelCompleted;
	
    [Header("Settings")]
    [Tooltip("The parent transform where level prefabs will be instantiated.")]
    [SerializeField] private Transform _gridContainer;

    [Tooltip("Size of one grid cell (Unity Units). Used to detect neighbors.")]
    [SerializeField] private float _cellSize = 1.0f; 
	
    private List<NodeController> _nodes = new ();
    private BFSController _bfsController = new ();
    private bool _isLevelActive = false;
    private GameObject _currentLevelObject;
	
    private static readonly Dictionary<NodeType, byte> _shapeLibrary = new()
    {
        { NodeType.Line,       10 },	// 1010 (Up + Down)
        { NodeType.LShape,    12 },		// 1100 (Up + Right)
        { NodeType.TShape,    14 },		// 1110 (Up + Right + Down)
        { NodeType.Cross,      15 },	// 1111 (All sides)
        { NodeType.Source,     8  },	// 1000 (Single output Up)
        { NodeType.Bulb,       8  }		// 1000 (Single input Up)
    };

    public void LoadLevel(LevelData levelData)
    {
        ClearLevel();

        if (levelData.LevelPrefab == null)
        {
            Debug.LogError("[GraphController] Level Data is missing the Prefab!");
            return;
        }

        _currentLevelObject = Instantiate(levelData.LevelPrefab, _gridContainer);
        
        NodeView[] views = _currentLevelObject.GetComponentsInChildren<NodeView>();
        
        if (views.Length == 0) return;
        
        Dictionary<Vector2, NodeController> gridMap = new Dictionary<Vector2, NodeController>();
        
        float minX = views.Min(v => v.transform.position.x);
        float maxY = views.Max(v => v.transform.position.y);

        foreach (NodeView view in views)
        {
            int x = Mathf.RoundToInt((view.transform.position.x - minX) / _cellSize);
            int y = Mathf.RoundToInt((maxY - view.transform.position.y) / _cellSize); // Y grows downwards
            Vector2Int pos = new Vector2Int(x, y);
            
            float zAngle = view.transform.eulerAngles.z;
            int rotIndex = Mathf.RoundToInt(zAngle / 90f);
            
            if (zAngle > 180) rotIndex = (4 - (Mathf.RoundToInt((360 - zAngle) / 90f))) % 4;
            else rotIndex = (Mathf.RoundToInt(-zAngle / 90f) + 4) % 4;
            
            byte baseShape = 0;
            if (_shapeLibrary.ContainsKey(view.Type))
            {
                baseShape = _shapeLibrary[view.Type];
            }
            else
            {
                Debug.LogWarning($"[GraphController] Unknown Shape ID: {view.Type}. Defaulting to 0.");
            }

            var model = new NodeModel(view.Type, baseShape, view.IsSource, view.IsTarget, rotIndex);
            var ctrl = new NodeController(model, view);

            view.OnViewClicked += () => OnNodeInteracted(ctrl);

            _nodes.Add(ctrl);
            if (!gridMap.ContainsKey(pos)) gridMap.Add(pos, ctrl);
        }

        foreach (var kvp in gridMap)
        {
            var pos = kvp.Key;
            var current = kvp.Value;
            
            LinkIfExists(current, Direction.Up,    pos + Vector2.up, gridMap);
            LinkIfExists(current, Direction.Down,  pos + Vector2.down,   gridMap); 
            LinkIfExists(current, Direction.Right, pos + Vector2.right, gridMap);
            LinkIfExists(current, Direction.Left,  pos + Vector2.left,  gridMap);
        }

        CenterCamera(views);
        
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
    
    private void CenterCamera(IEnumerable<NodeView> views)
	{
	    if (views == null || !views.Any()) return;
	    
	    float minX = float.MaxValue;
	    float maxX = float.MinValue;
	    float minY = float.MaxValue;
	    float maxY = float.MinValue;

	    foreach (var view in views)
	    {
	        Vector3 pos = view.transform.position;
	        
	        if (pos.x < minX) minX = pos.x;
	        if (pos.x > maxX) maxX = pos.x;
	        if (pos.y < minY) minY = pos.y;
	        if (pos.y > maxY) maxY = pos.y;
	    }
	    
	    float margin = _cellSize * 0.5f; 
	    minX -= margin;
	    maxX += margin;
	    minY -= margin;
	    maxY += margin;

	    // 3. Move Camera to Center
	    float width = maxX - minX;
	    float height = maxY - minY;
	    
	    Vector3 centerPos = new Vector3(minX + width / 2f, minY + height / 2f, -10f);
	    Camera.main.transform.position = centerPos;
	    
	    float screenRatio = (float)Screen.width / Screen.height;
	    float targetRatio = width / height;

	    float padding = 1.2f; // 20% padding around the edges

	    if (screenRatio >= targetRatio)
	    {
	        Camera.main.orthographicSize = (height / 2f) * padding;
	    }
	    else
	    {
	        float differenceInSize = targetRatio / screenRatio;
	        Camera.main.orthographicSize = (height / 2f * differenceInSize) * padding;
	    }
	}

    private void ClearLevel()
    {
        _isLevelActive = false;
        _nodes.Clear();
        
        if (_currentLevelObject != null)
        {
            Destroy(_currentLevelObject);
        }
    }
	
    private void OnNodeInteracted(NodeController node)
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
            HandleVictory();
        }
    }

    private bool CheckWinCondition()
    {
        bool allTargetsPowered = _nodes
            .Where(n => n.IsTarget)
            .All(n => n.IsPowered);

        return allTargetsPowered;
    }

    private void HandleVictory()
    {
        Debug.Log("[GraphController] LEVEL COMPLETE!");
        _isLevelActive = false;
        
        OnLevelCompleted?.Invoke();
    }
}