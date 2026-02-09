using System.Collections.Generic;
using _Project.Scripts.Level;
using _Project.Scripts.View;
using UnityEngine;

namespace _Project.Scripts.Game
{
	public class GameView : MonoBehaviour
	{
		[SerializeField] private GameController _controller;
		[SerializeField] private List<ViewDictionary> _viewDictionary;
		[SerializeField] private ViewState _initialState = ViewState.MainMenu;
		
		private ViewStateMachine _viewStateMachine;

		public void Initialize(GameController controller)
		{
			_controller = controller;
			
			_viewStateMachine = new ViewStateMachine(this, _viewDictionary);
			SetupListeners();
			
			_viewStateMachine.ChangeState(_initialState);
		}

		private void SetupListeners()
		{
			_viewStateMachine.OnPlayButtonPressed += _controller.StartCurrentLevel;
			_viewStateMachine.OnNextLevelPressed += _controller.StartNextLevel;
			_viewStateMachine.OnLevelSelected += _controller.StartLevel;
			_viewStateMachine.OnLevelCleared += _controller.ClearLevel;
		}

		public void LevelCompleteHandler()
		{
			_viewStateMachine.ChangeState(ViewState.EndGame);
		}

		public void LevelStartHandler()
		{
			_viewStateMachine.ChangeState(ViewState.InGame);
		}

		public int GetMaxLevel()
		{
			return _controller.MaxLevel;
		}

		public LevelData[] GetAvailableLevels()
		{
			return _controller.AvailableLevels;
		}

		public LevelData GetCurrentLevel()
		{
			return _controller.CurrentLevel;
		}

		public int GetCurrentScore()
		{
			return _controller.CurrentScore;
		}
	}
}