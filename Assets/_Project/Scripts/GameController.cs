using System.Collections.Generic;
using _Project.Scripts.Graph;
using UnityEngine;

namespace _Project.Scripts
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private GraphController _graphController;

		private void Start()
		{ 
			_graphController.OnLevelCompleted += HandleLevelComplete;
		}

		private void HandleLevelComplete()
		{
			
		}
	}
}