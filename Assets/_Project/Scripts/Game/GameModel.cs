using System;

namespace _Project.Scripts.Game
{
	[Serializable]
	public class GameModel
	{
		private int _score;
		private int _maxLevel;
		private int _currentLevel;
		
		public int Score => _score;
		public int MaxLevel => _maxLevel;
		public int CurrentLevel => _currentLevel;

		public GameModel(){}
		public GameModel(int score, int maxLevel)
		{
			_score = score;
			_maxLevel = maxLevel;
			_currentLevel = maxLevel;
		}

		public void SetCurrentLevel(int level)
		{
			_currentLevel = level;
		}

		public void IncreaseCurrentLevel()
		{
			_currentLevel++;
		}

		public void ResetCurrentLevel()
		{
			_currentLevel = 0;
		}

		public void IncreaseMaxLevel()
		{
			_maxLevel++;
		}

		public void AddScore(int points)
		{
			_score += points;
		}
	}
}