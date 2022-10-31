using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Movement.Pathfinding
{
    public class Grid : MonoBehaviour
    {
        public Transform target;
        private Astar astar;
        private BoundsInt bounds;
        private new Camera camera;
        private Tilemap floorMap;
        private Tilemap walkableMap;
        private Astar.Node[,] nodeToNodes;
        private List<Vector3> roadPath;
        private Astar.Node start;

        private void Start()
        {
            roadPath = new List<Vector3>();

            walkableMap = GameObject.Find("Grid/Walkable").GetComponent<Tilemap>();
            floorMap = GameObject.Find("Grid/Floor").GetComponent<Tilemap>();
            walkableMap.CompressBounds();
            floorMap.CompressBounds();
            bounds = floorMap.cellBounds;
            camera = Camera.main;

            CreateGrid();
            astar = new Astar();
        }

        public List<Vector3> CreatePath()
        {
            var world = camera.ScreenToWorldPoint(Input.mousePosition);
            var gridPos = walkableMap.WorldToCell(world);

            if (gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x <= nodeToNodes.GetUpperBound(0) &&
                gridPos.y <= nodeToNodes.GetUpperBound(1))
            {
                var endNode = nodeToNodes[gridPos.x, gridPos.y];
                if (endNode!=null && start!=null)
                {
                    return astar.PathfindingAstar(start, nodeToNodes[gridPos.x, gridPos.y], walkableMap);
                    
                }
            }
            return roadPath;
        }

        private void Update()
        {
            UpdateStart();
        }

        private void CreateGrid()
        {
            nodeToNodes = new Astar.Node[bounds.size.x, bounds.size.y];
            for (int x = bounds.xMin; x < bounds.size.x; x++)
            for (int y = bounds.yMin; y < bounds.size.y; y++)
            {
                if (walkableMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    var newNode = new Astar.Node(x, y);
                    nodeToNodes[x, y] = newNode;
                }
                else
                {
                    nodeToNodes[x, y] = null;
                }

                Debug.Log(nodeToNodes[x, y] + ", x = " + x + ", y = " + y);
            }

            for (var x = bounds.xMin; x < bounds.size.x; x++)
            for (var y = bounds.yMin; y < bounds.size.y; y++)
            {
                if (walkableMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    var currentNode = nodeToNodes[x, y];
                    if (currentNode == null) continue;
                    currentNode.AddNeighbours(nodeToNodes, currentNode);
                }
            }
        }

        private void UpdateStart()
        {
            var targetPos = walkableMap.WorldToCell(target.position);
            start = nodeToNodes[targetPos.x, targetPos.y];
        }
    }
}