using System.Collections.Generic;
using _Project.Scripts.Node;


namespace _Project.Scripts.Graph
{
	public class BFSController
	{
		private Queue<NodeController> _queue = new Queue<NodeController>();
		private HashSet<NodeController> _visited = new HashSet<NodeController>();

		public void Run(List<NodeController> allNodes)
		{
			foreach (var node in allNodes)
			{
				bool isSource = node.IsSource; 
				node.SetPowered(isSource);
			}

			_queue.Clear();
			_visited.Clear();
			
			foreach (var node in allNodes)
			{
				if (node.IsSource)
				{
					_queue.Enqueue(node);
					_visited.Add(node);
				}
			}

			while (_queue.Count > 0)
			{
				var current = _queue.Dequeue();
				
				foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
				{
					if (!current.HasOutput(dir)) continue;

					var neighbor = current.GetNeighbor(dir);
					if (neighbor == null) continue;

					if (_visited.Contains(neighbor)) continue;

					Direction incomingDir = GetOppositeDirection(dir);
					if (neighbor.HasOutput(incomingDir))
					{
						neighbor.SetPowered(true);
                    
						_visited.Add(neighbor);
						_queue.Enqueue(neighbor);
					}
				}
			}
		}

		private Direction GetOppositeDirection(Direction dir)
		{
			return (Direction)(((int)dir + 2) % 4);
		}
	}
}