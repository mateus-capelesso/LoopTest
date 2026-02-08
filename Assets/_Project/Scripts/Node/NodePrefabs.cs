using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Node
{
	[CreateAssetMenu(fileName = "NodesPrefabs", menuName = "InfinityLoop/Node Prefabs", order = 0)]
	public class NodePrefabs : ScriptableObject
	{
		public List<NodePrefab> prefabs;
		
		public NodeView GetPrefab(NodeType type) => prefabs.Find(p => p.type == type).prefab;
	}

	[Serializable]
	public struct NodePrefab
	{
		public NodeType type;
		public NodeView prefab;
	}
}