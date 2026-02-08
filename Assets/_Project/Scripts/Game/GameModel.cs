using System;

namespace _Project.Scripts.Game
{
	[Serializable]
	public class GameModel
	{
		public int _score;
		public int _level;
		
		public int Score => _score;
		public int Level => _level;

		public GameModel(){}
		public GameModel(int score, int level)
		{
			_score = score;
			_level = level;
		}

		public void IncreaseLevel()
		{
			_level++;
		}

		public void AddScore(int points)
		{
			_score += points;
		}
	}
}