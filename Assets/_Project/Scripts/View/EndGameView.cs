using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.View
{
	public class EndGameView : BaseView
	{
		[SerializeField] private Button _backButton;
		[SerializeField] private Button _nextLevelButton;
		[SerializeField] private TMP_Text _headerText;
		
		private const string HeaderFormat = "Level {0} Complete!\n Total Score: {1}";

		public override void EnterView()
		{
			_backButton.onClick.AddListener(BackButtonHandler);
			_nextLevelButton.onClick.AddListener(NextLevelButtonHandler);
			
			SetupHeader();
			base.EnterView();
		}

		public override void ExitView()
		{
			_backButton.onClick.RemoveAllListeners();
			_nextLevelButton.onClick.RemoveAllListeners();
			
			base.ExitView();
		}

		private void SetupHeader()
		{
			var currentLevel = _stateMachine.CurrentLevel.LevelName;
			var currentScore = _stateMachine.CurrentScore;
			
			_headerText.text = string.Format(HeaderFormat, currentLevel, currentScore);
		}

		private void BackButtonHandler()
		{
			_stateMachine.ChangeState(ViewState.MainMenu);
			_stateMachine.LevelClearedHandler();
		}

		private void NextLevelButtonHandler()
		{
			_stateMachine.NextLevelButtonHandler();
		}
	}
}