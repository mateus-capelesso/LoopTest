using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Node;
using UnityEngine;

namespace _Project.Scripts.Graph
{
	public class CameraController : MonoBehaviour
	{
		public void CenterCamera(IEnumerable<NodeView> views, float cellSize)
		{
			if (views == null || !views.Any()) return;
	    
			float minX = float.MaxValue;
			float maxX = float.MinValue;
			float minY = float.MaxValue;
			float maxY = float.MinValue;

			foreach (var view in views)
			{
				Vector3 pos = view.transform.position;
	        
				if (pos.x < minX) minX = pos.x;
				if (pos.x > maxX) maxX = pos.x;
				if (pos.y < minY) minY = pos.y;
				if (pos.y > maxY) maxY = pos.y;
			}
	    
			float margin = cellSize * 0.5f; 
			minX -= margin;
			maxX += margin;
			minY -= margin;
			maxY += margin;
	    
			float width = maxX - minX;
			float height = maxY - minY;
	    
			Vector3 centerPos = new Vector3(minX + width / 2f, minY + height / 2f, -10f);
			Camera.main.transform.position = centerPos;
	    
			float screenRatio = (float)Screen.width / Screen.height;
			float targetRatio = width / height;

			float padding = 1.2f; // 20% padding around the edges

			if (screenRatio >= targetRatio)
			{
				Camera.main.orthographicSize = (height / 2f) * padding;
			}
			else
			{
				float differenceInSize = targetRatio / screenRatio;
				Camera.main.orthographicSize = (height / 2f * differenceInSize) * padding;
			}
		}
	}
}