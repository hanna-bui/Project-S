using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utils;

namespace Pathfinding
{
	public class _Grid
	{
		private int width;
		private int height;
		private float cellSize;

		private int[,] gridArray;

		public _Grid(int width, int height, float cellSize)
		{
			this.width = width;
			this.height = height;
			this.cellSize = cellSize;

			gridArray = new int[width, height];

			GameObject gameObject = new GameObject("GridTexts");

			for (int x = 0; x < gridArray.GetLength(0); x++)
				for (int y = 0; y < gridArray.GetLength(1); y++)
				{
					UtilsClass.CreateWorldText(gridArray[x,y].ToString(), gameObject.GetComponent<Transform>(), GetWorldPosition(x, y), 120, Color.white, TextAnchor.MiddleCenter);
				}
		}

		private Vector3 GetWorldPosition (int x, int y)
		{
			var temp = new Vector3(x, -1*y, 0) * cellSize;
			// temp.x += cellSize/2;
			// temp.y += cellSize/2;
			return temp;
		}

		public void printInfo()
		{
			Debug.Log("width = " + width + ", height = " + height);
		}
	}
}