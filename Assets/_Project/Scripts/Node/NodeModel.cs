using _Project.Scripts.Node;

[System.Serializable]
public class NodeModel
{
	public NodeType typeId;
	public byte baseShape;
	public Direction direction;
	public bool isPowered;
	
	public bool IsSource => typeId == NodeType.Source;
	public bool IsTarget => typeId == NodeType.Target;

	public NodeModel(NodeType typeId, byte baseShape, Direction initialDirection)
	{
		this.typeId = typeId;
		this.baseShape = baseShape;
		direction = initialDirection;
        
		isPowered = this.IsSource;
	}
}