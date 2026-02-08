using System.Collections.Generic;

namespace _Project.Scripts.Node
{
	public class NodeController
	{
		private NodeModel _model;
		private NodeView _view;
		
		private Dictionary<Direction, NodeController> _neighbors;

		public bool IsSource => _model.isSource;
		public bool IsTarget => _model.isTarget;
		public bool IsPowered => _model.isPowered;
		public int CurrentRotation => _model.rotationIndex;

		public NodeController(NodeModel model, NodeView view)
		{
			_model = model;
			_view = view;
			_neighbors = new Dictionary<Direction, NodeController>();

			RefreshView();
		}

		public void RegisterNeighbor(Direction dir, NodeController neighbor)
		{
			if (!_neighbors.ContainsKey(dir))
			{
				_neighbors.Add(dir, neighbor);
			}
			else
			{
				_neighbors[dir] = neighbor;
			}
		}

		public void RotateClockwise()
		{
			_model.rotationIndex = (_model.rotationIndex + 1) % 4;
			
			float targetAngle = -90f * _model.rotationIndex;
			_view.AnimateRotation(targetAngle);
		}

		public void SetPowered(bool state)
		{
			if (_model.isPowered != state)
			{
				_model.isPowered = state;
				_view.SetPoweredState(state);
			}
		}
		
		public bool HasOutput(Direction dir)
		{
			int sideIndex = ((int)dir - _model.rotationIndex + 4) % 4;
			byte checkMask = (byte)(8 >> sideIndex);

			return (_model.baseShape & checkMask) != 0;
		}

		public NodeController GetNeighbor(Direction dir)
		{
			return _neighbors.ContainsKey(dir) ? _neighbors[dir] : null;
		}

		public void RefreshView()
		{
			_view.SetRotationSnap(-90f * _model.rotationIndex);
			_view.SetPoweredState(_model.isPowered);
		}
	}
}