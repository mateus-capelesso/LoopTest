using _Project.Scripts.Level;
using UnityEngine;

namespace _Project.Scripts.Game
{
	[CreateAssetMenu(fileName = "GameConfig", menuName = "InfinityLoop/Game Config", order = 0)]
	public class GameConfig : ScriptableObject
	{
		[SerializeField] private string userKey = "user_config";
		[SerializeField] private int pointsPerPiece = 10;
		[SerializeField] private LevelData[] levels;
		
		public string UserKey => userKey;
		public int PointsPerPiece => pointsPerPiece;
		public LevelData GetLevel(int index) => levels[index];
	}
}