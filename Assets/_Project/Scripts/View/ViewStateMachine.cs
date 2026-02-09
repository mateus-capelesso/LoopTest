using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Game;
using _Project.Scripts.Level;
using UnityEngine;

namespace _Project.Scripts.View
{
	public class ViewStateMachine
	{
		private List<ViewDictionary> _viewDictionary;
		
		private GameView _gameView;
		private ViewState _currentState = ViewState.None;
		private BaseView _currentView;
		
		public event Action OnPlayButtonPressed;
		public event Action OnNextLevelPressed;
		public event Action<int> OnLevelSelected;
		public event Action OnLevelCleared;
		
		public LevelData[] AvailableLevels => _gameView.GetAvailableLevels();
		public int MaxLevel => _gameView.GetMaxLevel();
		public LevelData CurrentLevel => _gameView.GetCurrentLevel();
		public int CurrentScore => _gameView.GetCurrentScore();
		
		public ViewStateMachine(GameView gameView, List<ViewDictionary> viewDictionary)
		{
			_gameView = gameView;
			_viewDictionary = viewDictionary;
			
			InitializeViews();
		}

		private void InitializeViews()
		{
			foreach (var view in _viewDictionary)
			{
				view.baseView.InitializeView(this);
				view.baseView.SetVisible(false);
			}
		}
		
		public void ChangeState(ViewState nextState)
		{
			if (nextState == _currentState) return;

			if (_currentView != null)
			{
				_currentView.ExitView();
			}
			
			_currentState = nextState;
			_currentView = _viewDictionary.FirstOrDefault(v => v.state == _currentState).baseView;
			
			_currentView.EnterView();
		}

		public void PlayButtonHandler()
		{
			OnPlayButtonPressed?.Invoke();
		}

		public void LevelSelectedHandler(int level)
		{
			OnLevelSelected?.Invoke(level);
		}
		
		public void NextLevelButtonHandler()
		{
			OnNextLevelPressed?.Invoke();
		}

		public void LevelClearedHandler()
		{
			OnLevelCleared?.Invoke();
		}
	}

	[Serializable]
	public struct ViewDictionary
	{
		public ViewState state;
		public BaseView baseView;
	}
}