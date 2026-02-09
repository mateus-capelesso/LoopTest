using UnityEngine;

namespace _Project.Scripts.Game
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private GameConfig _config;
		[SerializeField] private GraphController _graphController;
		
		private GameModel _model;
		private GamePersistentData _persistentData;

		private void Start()
		{
			_persistentData = new GamePersistentData();
			_model = _persistentData.RestoreGameModel(_config.UserKey);
			
			_graphController.OnLevelCompleted += LevelCompleteHandler;
			
			StartLevel(_model.MaxLevel);
			// Show main men UI
		}

		private void StartLevel(int index)
		{
			var level = _config.GetLevel(index);
			_model.SetCurrentLevel(index);
			_graphController.LoadLevel(level);
		}

		private void LevelCompleteHandler()
		{
			if (_model.CurrentLevel == _model.MaxLevel)
			{
				UpdateGameData();
			}
			
			// Show level complete UI
		}

		public void TriggerNextLevel()
		{
			_model.IncreaseCurrentLevel();
			
			StartLevel(_model.CurrentLevel);
		}

		public void UpdateGameData()
		{
			var earningPoints = _graphController.Nodes.Count * _config.PointsPerPiece;
			
			_model.IncreaseMaxLevel();
			_model.AddScore(earningPoints);
				
			_persistentData.SaveGameModel(_config.UserKey, _model);
		}
	}
}