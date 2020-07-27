using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class BoardNode
{
    private Status _nodeStatus;
    
    private Tile _occupatedTile;
    private SpriteRenderer _renderer;
    private Vector3 _worldPosition;
    public BoardNode(GameObject go)
    {
        _renderer = go.GetComponent<SpriteRenderer>();
        _worldPosition = go.transform.position;
        _nodeStatus = Status.AVAILABLE;
    }

    public bool IsNodeOccupated()
    {
        return _nodeStatus == Status.OCCUPATED;
    }

    public Tile GetTile()
    {
        return _occupatedTile;
    }

    public void SetTile([CanBeNull] Tile t, Status status)
    {
        _occupatedTile = t;
        _nodeStatus = status;
    }

    public void DestroyTile()
    {
        _occupatedTile.DestroyTile();
    }
}

public enum Status
{
    AVAILABLE,
    TEMPORARY,
    OCCUPATED
}