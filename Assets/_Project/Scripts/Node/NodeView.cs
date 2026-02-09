using _Project.Scripts.Audio;
using DG.Tweening;
using UnityEngine;
using AudioType = _Project.Scripts.Audio.AudioType;

namespace _Project.Scripts.Node
{
	public class NodeView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private float _rotationDuration = 0.25f;
		[SerializeField] private Color _completeColor = new Color(1f, 0.9f, 0.55f);

		public void AnimateRotation(float angle)
		{
			AudioController.Instance.PlaySoundEffect(AudioType.Press);
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

		public void SetCompleteNode()
		{
			_spriteRenderer.DOColor(_completeColor, 0.5f);
		}
	}
}