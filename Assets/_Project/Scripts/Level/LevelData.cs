using UnityEngine;

namespace _Project.Scripts.Level
{
	[CreateAssetMenu(fileName = "NewLevel", menuName = "InfinityLoop/Level Prefab Data")]
	public class LevelData : ScriptableObject
	{
		[SerializeField] private GameObject _levelPrefab;
		[SerializeField] private float _cellSize;
		
		public GameObject LevelPrefab => _levelPrefab;
		public float CellSize => _cellSize;
	}
}