using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

// ReSharper disable NotAccessedField.Global
// ReSharper disable IdentifierTypo
// ReSharper disable Unity.UnknownTag

namespace Movement.Pathfinding
{
    public class NewGrid : MonoBehaviour
    {
        [TextArea]// Do not place your note/comment here. Enter your note in the Unity Editor.
        public string notes = "Floor and Walkable should always be placed in the positive x and y axis. It cannot be placed in the negative x and/or y axis. In the sample scene, the white square indicates the (0, 0) point. The grid/tilemap should be placed above and to the right of the white square. "; 
        
        [SerializeField] private Tilemap walkableMap;
        [SerializeField] private Tilemap floorMap;
        private Node[,] nodeToNodes;

        private Vector3? spawnPt = null;

        private BoundsInt FloorBounds { get; set; }

        private BoundsInt WalkBounds { get; set; }


        public void InitializeGrid()
        {
            walkableMap.CompressBounds();
            floorMap.CompressBounds();
            
            WalkBounds = walkableMap.cellBounds;

            CreateGrid();
        }

        public void UpdateTilemap(Tilemap newWalkMap, Tilemap newFloorMap, Vector3 point)
        {
            var walkBounds = newFloorMap.cellBounds;
            var walkTiles = newWalkMap.GetTilesBlock(walkBounds);
            
            var floorBounds = newFloorMap.cellBounds;
            var floorTiles = newFloorMap.GetTilesBlock(floorBounds);

            var flag = Random.Range(0, 2)==1;

            for (var x = 0; x < floorBounds.size.x; x++) {
                for (var y = 0; y < floorBounds.size.y; y++) {
                    var floorTile = floorTiles[x + y * floorBounds.size.x];
                    if (floorTile != null) {
                        floorMap.SetTile(new Vector3Int((int)(point.x/ 15 + x) + 3, (int)(point.y/15 + y) - 14,0), floorTile);
                    }
                }
            }
            for (var x = 0; x < walkBounds.size.x; x++) {
                for (var y = 0; y < walkBounds.size.y; y++) {
                    var walkTile = walkTiles[x + y * walkBounds.size.x];
                    if (walkTile != null) {
                        walkableMap.SetTile(new Vector3Int((int)(point.x/ 15 + x) - 3, (int)(point.y/15 + y) - 14,0), walkTile);
                        if (spawnPt==null && flag) 
                            spawnPt = new Vector3((int)(point.x/ 15 + x) - 3, (int)(point.y/15 + y) - 14,0);
                    }
                }
            }
        }

        public List<Vector3> CreatePath(Vector3 character, Vector3 targetPosition)
        {
            
            var characterPos = walkableMap.WorldToCell(character);
            var start = nodeToNodes[characterPos.x, characterPos.y];
            
            var gridPos = walkableMap.WorldToCell(targetPosition);

            if (gridPos is { x: >= 0, y: >= 0 } && gridPos.x <= nodeToNodes.GetUpperBound(0) &&
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
        
        public bool IsWalkable(int x, int y)
        {
            var gridPos = walkableMap.WorldToCell(new Vector3(x, y, 0));
            return nodeToNodes[gridPos.x, gridPos.y] != null;
        }

        public bool IsWalkable(Vector3 position)
        {
            var gridPos = walkableMap.WorldToCell(position);
            return nodeToNodes[gridPos.x, gridPos.y] != null;
        }

        public Vector3? GetSpawnPt()
        {
            return spawnPt;
        }
        
        public Vector3 GetRandomCoords()
        {
            var x = Random.Range(WalkBounds.xMin, WalkBounds.size.x);
            var y = Random.Range(WalkBounds.yMin, WalkBounds.size.y);
            
            return walkableMap.CellToWorld(new Vector3Int(x, y, 0));
        }

        private void CreateGrid()
        {
            FloorBounds = floorMap.cellBounds;
            
            nodeToNodes = new Node[FloorBounds.size.x, FloorBounds.size.y];
            for (var x = FloorBounds.xMin; x < FloorBounds.size.x; x++)
            for (var y = FloorBounds.yMin; y < FloorBounds.size.y; y++)
            {
                if (walkableMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    var newNode = new Node(x, y);
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