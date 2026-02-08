using UnityEngine;

namespace _Project.Scripts.Node
{
	public class NodeBuilder : MonoBehaviour
	{
		[Header("Level Design Settings")]
		[SerializeField] private NodeType _typeId;
		[SerializeField] private Direction _direction;
		
		public NodeType Type => _typeId;
		public Direction Direction => _direction;
		public bool IsSource => _typeId == NodeType.Source;
		public bool IsTarget => _typeId == NodeType.Target;
		
		// Small method to simply rotation control when building levels		
#if UNITY_EDITOR
		
		private Direction _lastDirection;
		private void OnValidate()
		{
			if (_lastDirection != _direction)
			{
				_lastDirection = _direction;
				RotateToDirection();
			}
		}

		private void RotateToDirection()
		{
			var angle = (-360 / System.Enum.GetValues(typeof(Direction)).Length) * (int)_direction;

			transform.localEulerAngles = new Vector3(0, 0, angle);

		}
#endif
	}
}