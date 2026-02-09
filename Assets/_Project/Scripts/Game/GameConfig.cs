using _Project.Scripts.Level;
using UnityEngine;

namespace _Project.Scripts.Game
{
	[CreateAssetMenu(fileName = "GameConfig", menuName = "InfinityLoop/Game Config", order = 0)]
	public class GameConfig : ScriptableObject
	{
		[SerializeField] private string _userKey = "user_config";
		[SerializeField] private int _pointsPerPiece = 10;
		[SerializeField] private LevelData[] _levels;
		
		public string UserKey => _userKey;
		public int PointsPerPiece => _pointsPerPiece;
		public LevelData[] Levels => _levels;
		public LevelData GetLevel(int index) => _levels[index];
	}
}