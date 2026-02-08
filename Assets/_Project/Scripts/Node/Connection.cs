
namespace _Project.Scripts.Node
{
	public struct Connection
	{
		public Direction Dir;
		public NodeModel TargetNode;

		public Connection(Direction dir, NodeModel node)
		{
			Dir = dir;
			TargetNode = node;
		}
	}
}