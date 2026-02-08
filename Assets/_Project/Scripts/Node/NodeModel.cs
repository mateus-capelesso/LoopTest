using _Project.Scripts.Node;

[System.Serializable]
public class NodeModel
{
	public NodeType typeId;
	public byte baseShape;
	public bool isSource;
	public bool isTarget;

	public int rotationIndex;
	public bool isPowered;

	public NodeModel(NodeType typeId, byte baseShape, bool isSource, bool isTarget, int initialRotation = 0)
	{
		this.typeId = typeId;
		this.baseShape = baseShape;
		this.isSource = isSource;
		this.isTarget = isTarget;
		rotationIndex = initialRotation;
        
		isPowered = this.isSource;
	}
}