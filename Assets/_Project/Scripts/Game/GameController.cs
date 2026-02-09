using System.Collections.Generic;
using _Project.Scripts.Analytics;
using _Project.Scripts.Audio;
using _Project.Scripts.Level;
using UnityEngine;
using AudioType = _Project.Scripts.Audio.AudioType;

namespace _Project.Scripts.Game
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private GameConfig _config;
		[SerializeField] private GraphController _graphController;
		[SerializeField] private GameView _gameView;
		
		// Simple shortcut for Analytics purposes
		public GameModel GameModel => _model;
		public LevelData[] AvailableLevels => _config.Levels;
		public int MaxLevel => _model.MaxLevel;
		public LevelData CurrentLevel => _config.GetLevel(_model.CurrentLevel);
		public int CurrentScore => _model.Score;
		
		private GameModel _model;
		private GamePersistentData _persistentData;
		private AnalyticsWrapper _analyticsWrapper;
			

		private void Start()
		{
			_persistentData = new GamePersistentData();
			_analyticsWrapper = new AnalyticsWrapper(this);
			_model = _persistentData.RestoreGameModel(_config.UserKey);
			
			_graphController.OnLevelCompleted += LevelCompleteHandler;
			
			_gameView.Initialize(this);
			_analyticsWrapper.SendEvent(AnalyticsEvent.GameStarted);
		}
		
		public void StartNextLevel()
		{
			_model.IncreaseCurrentLevel();
			
			StartLevel(_model.CurrentLevel);
		}

		public void StartCurrentLevel()
		{
			StartLevel(_model.CurrentLevel);
		}

		public void StartLevel(int index)
		{
			var level = _config.GetLevel(index);
			
			_model.SetCurrentLevel(index);
			_graphController.LoadLevel(level);
			_gameView.LevelStartHandler();

			_analyticsWrapper.SendEvent(AnalyticsEvent.LevelStarted);
		}

		public void ClearLevel()
		{
			_graphController.ClearLevel();
		}

		private void LevelCompleteHandler()
		{
			if (_model.CurrentLevel == _model.MaxLevel)
			{
				UpdateGameData();
			}
			
			AudioController.Instance.PlaySoundEffect(AudioType.Complete);
			
			_gameView.LevelCompleteHandler();
			_analyticsWrapper.SendEvent(AnalyticsEvent.LevelCompleted);
		}
		
		public void UpdateGameData()
		{
			var earningPoints = _graphController.Nodes.Count * _config.PointsPerPiece;
			
			_model.IncreaseMaxLevel();
			_model.AddScore(earningPoints);
				
			_persistentData.SaveGameModel(_config.UserKey, _model);
			_analyticsWrapper.SendEvent(AnalyticsEvent.LevelUnblocked);
		}
	}
}