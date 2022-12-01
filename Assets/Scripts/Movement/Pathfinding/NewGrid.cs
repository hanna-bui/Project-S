using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Movement.Pathfinding
{
    public class NewGrid : MonoBehaviour
    {
        [TextArea]// Do not place your note/comment here. Enter your note in the Unity Editor.
        public string notes = "Floor and Walkable should always be placed in the positive x and y axis. " + 
                              "It cannot be placed in the negative x and/or y axis. In the sample scene, " + 
                              "the white square indicates the (0, 0) point. The grid/tilemap should be placed above " + 
                              "and to the right of the white square. "; 
        
        
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

        // private void Update()
        // {
        //     UpdateStart();
        // }

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

        public void UpdateStart(Transform target)
        {
            var targetPos = walkableMap.WorldToCell(target.position);
            start = nodeToNodes[targetPos.x, targetPos.y];
        }
    }
}