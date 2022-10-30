using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class GridTesting : MonoBehaviour
{

	[SerializeField] private float cellSize;
    private void Start()
    {
        _Grid grid = new _Grid(15, 14, cellSize);
		// grid.printInfo();
    }
}
