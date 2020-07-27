using System;
using System.Collections.Generic;
using UnityEngine;

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
    [NonSerialized] 
    private List<Piece> _pieces = new List<Piece>();

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
    
    private Vector2Int GetGridPosition(Vector3 pos)
    {
        return new Vector2Int(Mathf.CeilToInt(pos.x), Mathf.CeilToInt(pos.y));
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

    public void AddPiece(Piece p)
    {
        _pieces.Add(p);
    }
    
    public void CheckForLines()
    {
        for (int y = gridSize.y - 1; y > -1; y--)
        {
            bool clearLine = true;
            for (int x = gridSize.x - 1; x > -1; x--)
            {
                if (!_nodes[x, y].IsNodeOccupated())
                {
                    clearLine = false;
                    break;
                }
            }
            
            if(clearLine)
                ClearLine(y);
        }
    }

    private void ClearLine(int y)
    {
        for (int x = gridSize.x - 1; x > -1; x--)
        {
            _nodes[x, y].DestroyTile();
        }

        
        //TODO::???
        foreach (Piece piece in _pieces)
        {
            for (int i = 0; i < 2; i++)
            {
                piece.MoveDown();
            }
        }
    }
}