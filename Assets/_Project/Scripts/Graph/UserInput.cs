using System;
using _Project.Scripts.Node;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Graph
{
	public class UserInput : MonoBehaviour
	{
		[SerializeField] private GraphController _graphController;
		[SerializeField] private PlayerInput _playerInput;

		private InputAction _touchPress;
		private InputAction _touchPosition;

		private void Awake()
		{
			if (_playerInput == null)
			{
				GetComponent<PlayerInput>();
			}
			
			_touchPress = _playerInput.actions["TouchPress"];
			_touchPosition = _playerInput.actions["TouchPosition"];
			
		}

		private void OnEnable()
		{
			_touchPress.performed += TouchPressHandler;
		}

		private void OnDisable()
		{
			_touchPress.performed -= TouchPressHandler;
		}

		private void TouchPressHandler(InputAction.CallbackContext context)
		{
			var position = _touchPosition.ReadValue<Vector2>();
			PerformRaycast(position);
		}

		private void PerformRaycast(Vector2 screenPos)
		{
			var worldPoint = Camera.main.ScreenToWorldPoint(screenPos);
			var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			
			if (hit.collider != null)
			{
				if (hit.collider.TryGetComponent<NodeView>(out var view))
				{
					if (_graphController.NodeControllerMap.TryGetValue(view, out var ctrl))
					{
						_graphController.OnNodeInteracted(ctrl);
					}
				}
			}
		}
	}
}