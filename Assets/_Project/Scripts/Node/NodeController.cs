using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Node
{
	public class NodeController
	{
		private NodeModel _model;
		private NodeView _view;
		
		private Dictionary<Direction, NodeController> _neighbors;
		private int _directionCount;

		public NodeView View => _view;
		public bool IsSource => _model.IsSource;
		public bool IsTarget => _model.IsTarget;
		public bool IsPowered => _model.isPowered;

		public NodeController(NodeModel model, NodeView view)
		{
			_model = model;
			_view = view;
			_neighbors = new Dictionary<Direction, NodeController>();

			_directionCount = System.Enum.GetValues(typeof(Direction)).Length;
			RefreshView();
		}

		public void RegisterNeighbor(Direction dir, NodeController neighbor)
		{
			_neighbors[dir] = neighbor;
		}

		public void RotateClockwise()
		{
			var index = ((int)_model.direction + 1) % _directionCount;
			_model.direction = (Direction)index;
			
			Debug.Log($"{_model.typeId} rotated clockwise to {_model.direction}");

			var angle = GetAngle(_model.direction);
			_view.AnimateRotation(angle);
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
			int sideIndex = ((int)dir - (int)_model.direction + _directionCount) % _directionCount;
			byte checkMask = (byte)((1 << (_directionCount - 1)) >> sideIndex);

			return (_model.baseShape & checkMask) != 0;
		}

		public NodeController GetNeighbor(Direction dir)
		{
			return _neighbors.ContainsKey(dir) ? _neighbors[dir] : null;
		}

		public void RefreshView()
		{
			var angle = GetAngle(_model.direction);
			_view.SetRotationSnap(angle);
			_view.SetPoweredState(_model.isPowered);
		}

		private float GetAngle(Direction dir)
		{
			return (-360 / _directionCount) * (int)dir;
		}
	}
}