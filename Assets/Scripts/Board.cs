using System;
using System.Collections.Generic;
using Tetris_RL.Actions;
using UnityEngine;

namespace Tetris_RL
{
    [CreateAssetMenu(menuName = "Board")]
    public class Board : ScriptableObject
    {
        public Vector2Int gridSize;
        public int nodeSize;
        public GameObject nodePrefab;

        [NonSerialized] 
        private Vector2 _offset;
        [NonSerialized] 
        private BoardNode[,] _nodes;

        public void Init(BoardNode[,] n, Vector2 off)
        {
            _offset = off;
            _nodes = n;
        }
    
        public bool IsTileAvailable(Vector2Int node)
        {
            return IsTileExist(node) && !IsTileOccupated(node);
        }
    
        private bool IsTileExist(Vector2Int node)
        {
            return node.x >= 0 && node.x < gridSize.x && node.y>= 0 && node.y < gridSize.y;
        }
    
        private bool IsTileOccupated(Vector2Int node)
        {
            return _nodes[node.x, node.y].IsNodeOccupated();
        }

        public Vector3 GetWorldPos(Vector2Int pos)
        {
            return new Vector3(pos.x * nodeSize - _offset.x, -pos.y * nodeSize + _offset.y, 0);
        }
    
        public void SetTileToNode(Tile go, Vector2Int pos, Status status)
        {
            if (IsTileExist(pos))
            {
                if (!_nodes[pos.x, pos.y].IsNodeOccupated())
                {
                    _nodes[pos.x,pos.y].SetTile(go, status);
                }
                else
                {
                    Debug.LogWarning("Node " + pos + " is already occupated!");
                }
            }
            else
            {
                Debug.LogWarning("Position " + pos + " is out of range!");
            }
        }

        public void RemoveTileFromNode(Vector2Int pos)
        {
            if (IsTileExist(pos))
            {
                _nodes[pos.x,pos.y].SetTile(null, Status.AVAILABLE);
            }
            else
            {
                Debug.LogWarning("Position " + pos + " is out of range!");
            }
        }
    
        // Returning line indexes which needs to be destroyed 
        public Queue<int> GetLinesToClear()
        {
            Queue<int> lines = new Queue<int>();

            // Going from top to bottom
            for (int y = 0; y < gridSize.y; y++)
            {
                bool clearLine = true;
            
                for (int x = 0; x < gridSize.x; x++)
                {
                    if (!_nodes[x, y].IsNodeOccupated())
                    {
                        clearLine = false;
                        break;
                    }
                }
            
                if(clearLine)
                    lines.Enqueue(y);
            }

            return lines;
        }

        public void ClearNode(Vector2Int pos)
        {
            if(_nodes[pos.x, pos.y].GetTile() != null)
                Destroy(_nodes[pos.x, pos.y].GetTile().go);
            RemoveTileFromNode(pos);
        }

        public BoardNode[,] GetNodes()
        {
            return _nodes;
        }

        public void MoveDownAbove(int line)
        {
            // Going through every line above given
            for (int y = line - 1; y >= 0; y--)
            {
                for (int x = gridSize.x - 1; x >= 0; x--)
                {
                    // Checking if tile exist on current node
                    if (_nodes[x, y].IsNodeOccupated())
                    {
                        Tile t = _nodes[x, y].GetTile();
                    
                        if (IsTileAvailable(new Vector2Int(x, y + 1)))
                        {
                            // Unsubscribe thi tile from current node
                            RemoveTileFromNode(t.GetLocalPosition());
                        
                            // Calculating new local and world positions
                            Vector2Int localPos = t.GetLocalPosition() + Vector2Int.up;
                            Vector3 worldPos = GetWorldPos(localPos);
                        
                            // Setting new positions to the tile
                            t.UpdateLocalPosition(localPos);
                            t.UpdateWorldPosition(worldPos);

                            // Subscribe the tile to a new node
                            SetTileToNode(t, t.GetLocalPosition(), Status.OCCUPATED);
                        }
                    }
                }
            }
        }

        
        public void ResetBoard()
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    ClearNode(new Vector2Int(x,y));
                }
            }
        }
    }
}

