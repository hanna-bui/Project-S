using System.Collections.Generic;
using UnityEngine;
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
        
        private Tilemap walkableMap;
        private Astar.Node[,] nodeToNodes;
        
        private BoundsInt floorBounds;
        private BoundsInt walkBounds;
        
        protected BoundsInt FloorBounds
        {
            get => floorBounds;
            set => floorBounds = value;
        }
        
        
        protected BoundsInt WalkBounds
        {
            get => walkBounds;
            set => walkBounds = value;
        }

        private void Start()
        {
            walkableMap = GameObject.Find("Grid/Walkable").GetComponent<Tilemap>();
            
            walkableMap.CompressBounds();
            
            WalkBounds = walkableMap.cellBounds;

            CreateGrid();
        }

        public List<Vector3> CreatePath(Vector3 character, Vector3 targetPosition)
        {
            var characterPos = walkableMap.WorldToCell(character);
            var start = nodeToNodes[characterPos.x, characterPos.y];
            
            var gridPos = walkableMap.WorldToCell(targetPosition);

            if (gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x <= nodeToNodes.GetUpperBound(0) &&
                gridPos.y <= nodeToNodes.GetUpperBound(1))
            {
                var endNode = nodeToNodes[gridPos.x, gridPos.y];
                if (endNode!=null && start!=null)
                {
                    return new Astar().PathfindingAstar(start, endNode, walkableMap);
                    
                }
            }
            return null;
        }

        public bool IsWalkable(Vector3 position)
        {
            var gridPos = walkableMap.WorldToCell(position);
            return nodeToNodes[gridPos.x, gridPos.y] != null;
        }
        
        public Vector3 GetRandomCoords()
        {
            var x = Random.Range(WalkBounds.xMin, WalkBounds.size.x);
            var y = Random.Range(WalkBounds.yMin, WalkBounds.size.y);
            
            return walkableMap.CellToWorld(new Vector3Int(x, y, 0));
        }

        private void CreateGrid()
        {
            var floorMap = GameObject.Find("Grid/Floor").GetComponent<Tilemap>();
            floorMap.CompressBounds();
            
            FloorBounds = floorMap.cellBounds;
            
            nodeToNodes = new Astar.Node[FloorBounds.size.x, FloorBounds.size.y];
            for (var x = FloorBounds.xMin; x < FloorBounds.size.x; x++)
            for (var y = FloorBounds.yMin; y < FloorBounds.size.y; y++)
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

            for (var x = FloorBounds.xMin; x < FloorBounds.size.x; x++)
            for (var y = FloorBounds.yMin; y < FloorBounds.size.y; y++)
            {
                if (walkableMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    var currentNode = nodeToNodes[x, y];
                    if (currentNode == null) continue;
                    currentNode.AddNeighbours(nodeToNodes, currentNode);
                }
            }
        }
    }
}