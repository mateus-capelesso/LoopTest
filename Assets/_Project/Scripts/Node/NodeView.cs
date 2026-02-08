using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Node
{
	public class NodeView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private float _rotationDuration = 0.25f;

		public void AnimateRotation(float angle)
		{
			transform.DORotate(new Vector3(0, 0, angle), _rotationDuration).SetEase(Ease.OutQuad);
		}

		public void SetRotationSnap(float angle)
		{
			transform.localEulerAngles = new Vector3(0, 0, angle);
		}

		public void SetPoweredState(bool isPowered)
		{
			float targetAlpha = isPowered ? 1.0f : 0.5f;
			_spriteRenderer.DOFade(targetAlpha, 0.3f);
		}
	}
}