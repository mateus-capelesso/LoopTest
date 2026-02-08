using _Project.Scripts.Node;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Graph
{
	public class UserInput : MonoBehaviour
	{
		[SerializeField] private GraphController _graphController;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
				{
					return;
				}
			
				var pos = Input.mousePosition;
				PerformRaycast(pos);
			}
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