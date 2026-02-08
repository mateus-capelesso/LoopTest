using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Node
{
	public class NodeView : MonoBehaviour
	{
		[Header("Level Design Settings")]
		[SerializeField] private NodeType _typeId; 
		[SerializeField] private bool _isSource;
		[SerializeField] private bool _isTarget;
		
		[SerializeField] private SpriteRenderer _artRenderer;
		[SerializeField] private SpriteRenderer _glowRenderer;
		
		public NodeType Type => _typeId;
		public bool IsSource => _isSource;
		public bool IsTarget => _isTarget;

		public System.Action OnViewClicked;

		private void OnMouseDown()
		{
			OnViewClicked?.Invoke();
		}

		public void AnimateRotation(float targetZ)
		{
			transform.DORotate(new Vector3(0, 0, targetZ), 0.2f).SetEase(Ease.OutQuad);
		}

		public void SetRotationSnap(float targetZ)
		{
			transform.localEulerAngles = new Vector3(0, 0, targetZ);
		}

		public void SetPoweredState(bool isPowered)
		{
			float targetAlpha = isPowered ? 1.0f : 0.0f;
			_glowRenderer.DOFade(targetAlpha, 0.3f);
		}
	}
}