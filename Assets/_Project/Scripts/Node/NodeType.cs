namespace _Project.Scripts.Node
{
	public enum NodeType
	{
		Line,       // Straight I-shape
		LShape,     // Corner
		TShape,     // T-junction
		Cross,      // + shape
		Source,     // Power generator
		Target        // Power consumer
	}
}