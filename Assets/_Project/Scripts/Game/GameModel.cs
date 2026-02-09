using System;

namespace _Project.Scripts.Game
{
	[Serializable]
	public class GameModel
	{
		private int _score;
		private int _maxMaxLevel;
		private int _currentLevel;
		
		public int Score => _score;
		public int MaxLevel => _maxMaxLevel;
		public int CurrentLevel => _currentLevel;

		public GameModel(){}
		public GameModel(int score, int maxLevel)
		{
			_score = score;
			_maxMaxLevel = maxLevel;
		}

		public void SetCurrentLevel(int level)
		{
			_currentLevel = level;
		}

		public void IncreaseCurrentLevel()
		{
			_currentLevel++;
		}

		public void IncreaseMaxLevel()
		{
			_maxMaxLevel++;
		}

		public void AddScore(int points)
		{
			_score += points;
		}
	}
}