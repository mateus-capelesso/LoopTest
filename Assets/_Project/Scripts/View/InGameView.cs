using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.View
{
	public class InGameView: BaseView
	{
		[SerializeField] private Button _backButton;
		public override void EnterView()
		{
			_backButton.onClick.AddListener(OnBackButtonClicked);
			base.EnterView();
		}

		private void OnBackButtonClicked()
		{
			_stateMachine.LevelClearedHandler();
			_stateMachine.ChangeState(ViewState.MainMenu);
		}

		public override void ExitView()
		{
			_backButton.onClick.RemoveAllListeners();
			base.ExitView();
		}
	}
}