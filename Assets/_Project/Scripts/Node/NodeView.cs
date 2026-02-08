using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Node
{
	public class NodeView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;

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
			float targetAlpha = isPowered ? 1.0f : 0.5f;
			_spriteRenderer.DOFade(targetAlpha, 0.3f);
		}
	}
}