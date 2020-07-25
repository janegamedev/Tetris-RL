using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class BoardNode
{
    private GameObject _occupatedTile;
    private SpriteRenderer _renderer;
    private Vector3 _worldPosition;
    public BoardNode(GameObject go)
    {
        _renderer = go.GetComponent<SpriteRenderer>();
        _worldPosition = go.transform.position;
    }

    public GameObject GetTile()
    {
        return _occupatedTile;
    }

    public void SetTile([CanBeNull] GameObject t)
    {
        _occupatedTile = t;
    }
}