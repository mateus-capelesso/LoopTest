using UnityEngine;

namespace _Project.Scripts.View
{
	public abstract class BaseView : MonoBehaviour
	{
		protected ViewStateMachine _stateMachine;

		public virtual void InitializeView(ViewStateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public virtual void EnterView()
		{
			SetVisible(true);
		}

		public virtual void ExitView()
		{
			SetVisible(false);
		}
		
		public void SetVisible(bool visible) => gameObject.SetActive(visible);
	}
}