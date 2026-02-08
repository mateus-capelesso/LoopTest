using UnityEngine;

namespace _Project.Scripts.Level
{
	[CreateAssetMenu(fileName = "NewLevel", menuName = "InfinityLoop/Level Prefab Data")]
	public class LevelData : ScriptableObject
	{
		[Tooltip("The prefab containing the pre-arranged grid of NodeViews")]
		public GameObject LevelPrefab;
	}
}