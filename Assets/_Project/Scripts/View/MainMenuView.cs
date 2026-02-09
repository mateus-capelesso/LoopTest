using UnityEngine.UI;

namespace _Project.Scripts.View
{
	public class MainMenuView: BaseView
	{
		public Button _playButton;
		public Button _levelsButton;

		public override void EnterView()
		{
			_playButton.onClick.AddListener(PlayButtonHandler);
			_levelsButton.onClick.AddListener(LevelsButtonHandler);
			
			base.EnterView();
		}

		private void PlayButtonHandler()
		{
			_stateMachine.PlayButtonHandler();
		}
		
		private void LevelsButtonHandler()
		{
			_stateMachine.ChangeState(ViewState.Levels);
		}

		public override void ExitView()
		{
			_playButton.onClick.RemoveAllListeners();
			_levelsButton.onClick.RemoveAllListeners();
			base.ExitView();
		}
	}
}