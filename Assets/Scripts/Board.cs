using System;
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

    public void Init(BoardNode[,] n, Vector2 off)
    {
        _offset = off;
        _nodes = n;
    }
    
    public bool IsTileAvailable(Vector2Int node)
    {
        return IsTileExist(node) && IsTileFree(node);
    }
    
    private bool IsTileExist(Vector2Int node)
    {
        return node.x >= 0 && node.x < gridSize.x && node.y>= 0 && node.y < gridSize.y;
    }
    
    private bool IsTileFree(Vector2Int node)
    {
        return _nodes[node.x, node.y].GetTile() == null;
    }
    
    private Vector2Int GetGridPosition(Vector3 pos)
    {
        return new Vector2Int(Mathf.CeilToInt(pos.x), Mathf.CeilToInt(pos.y));
    }

    public Vector3 GetWorldPos(Vector2Int pos)
    {
        return new Vector3(pos.x * nodeSize - _offset.x, -pos.y * nodeSize + _offset.y, 0);
    }
    
    public void SetTileToNode(GameObject go, Vector2Int pos)
    {
        if (IsTileExist(pos))
        {
            if (_nodes[pos.x, pos.y].GetTile() == null)
            {
                _nodes[pos.x,pos.y].SetTile(go);
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
            _nodes[pos.x,pos.y].SetTile(null);
        }
        else
        {
            Debug.LogWarning("Position " + pos + " is out of range!");
        }
    }
}