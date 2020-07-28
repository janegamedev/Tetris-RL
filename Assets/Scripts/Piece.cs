using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(menuName = "Piece/Piece Shape")]
public class Piece : ScriptableObject
{
    public event Action OnDropFinished;
    public Vector2Int[] tilesPositions;
    public Color color;
    
    
    private List<Tile> _tiles = new List<Tile>();
    private Transform _parent;
    private Vector2Int _gridPos;
    private Board _board;
    
    public void InitializePiece(Board b, Transform parent, GameObject[] t, Vector2Int pos, int rot)
    {
        Debug.Log(b);
        Debug.Log(parent);
        
        _board = b;
        _parent = parent;
        
        Debug.Log(_board);
        Debug.Log(_parent);
        
        _parent.position = b.GetWorldPos(_gridPos);
        
        for (int i = 0; i < t.Length; i++)
        {
            _tiles.Add(new Tile(this, t[i], tilesPositions[i]));
        }
        
        _gridPos = pos;
        UpdatePositionsForTiles(false);
    }

    //Calling from agent input system to move piece
    public void MoveHorizontal(int dir)
    {
        if(dir == 0)
            return;
        
        if(CanMove(dir == 1 ? Direction.RIGHT: Direction.LEFT))
        {
            // Update grid position for this piece
            _gridPos += new Vector2Int(dir, 0);
            // Update world position
            _parent.position = _board.GetWorldPos(_gridPos);
            // Update new tile positions according to the piece position
            UpdatePositionsForTiles(false);
        }
    }

    public void RemoveTile(Tile t)
    {
        _tiles.Remove(t);
        Destroy(t.go);

        if (_tiles.Count < 1)
        {
            Destroy(_parent.gameObject);
            Destroy(this);
        }
    }

    public void MoveDown()
    {
        // Remove tiles form nodes so we can check without conflicting with our tiles
        RemoveFromNodes();
        
        if (CanMove(Direction.DOWN))
        {
            // Update grid position for this piece
            _gridPos += Vector2Int.up;
            // Update world position
            Debug.Log(_parent);
            Debug.Log(_board);
            _parent.position = _board.GetWorldPos(_gridPos);
            // Update new tile positions according to the piece position
            UpdatePositionsForTiles(false);
        }
        else // If the piece can't be moves - the move of the piece is finished
        {
            // Update tile position and subscribing for current nodes
            UpdatePositionsForTiles(true);
            // Calling on drop finished event 
            OnDropFinished?.Invoke();
        }
    }
    
    // Calculating new local tile positions according to this piece grid position
    private void UpdatePositionsForTiles(bool final)
    {
        foreach (var t in _tiles)
        {
            Vector2Int pos = _gridPos + t.GetLocalPosition();
            t.UpdateWorldPosition(_board.GetWorldPos(pos));
            _board.SetTileToNode(t, pos, final? Status.OCCUPATED : Status.TEMPORARY);
        }
    }

    // Unsubscribing all tile from their current nodes
    private void RemoveFromNodes()
    {
        foreach (var t in _tiles)
        {
            _board.RemoveTileFromNode(_gridPos + t.GetLocalPosition());
        }
    }

    // Check if all tiles are able to move down
    private bool CanMove(Direction dir)
    {
        Vector2Int d;
        switch (dir)
        {
            case Direction.LEFT:
                d = Vector2Int.left;
                break;
            case Direction.RIGHT:
                d = Vector2Int.right;
                break;
            case Direction.DOWN:
                d = Vector2Int.up; // Up when down coz our board nodes goes from top left corner and its y id is increasing 
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }

        return _tiles.All(t => _board.IsTileAvailable(_gridPos + d + t.GetLocalPosition()));
    }

    private bool CanRotate(int rot, Vector2Int[] pos)
    {
        return CalculateComplex(rot, pos).All(t => _board.IsTileAvailable(_gridPos + t));
    }
    
    // Rotate piece 90* counter clockwise/clockwise using complex numbers
    public void RotateComplex(int dir)
    {
        if(dir == 0)
            return;
        
        Vector2Int[] newPos = new Vector2Int[_tiles.Count];
        
        if (CanRotate(dir, newPos))
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                // Converting the result to tile position Vector 2int
                _tiles[i].UpdateLocalPosition(newPos[i]);
            }
        
            // Update new tile positions
            UpdatePositionsForTiles(false);
        }
    }

    private Vector2Int[] CalculateComplex(int dir,  Vector2Int[] pos)
    {
        // Create a complex number with input -1/1 as imaginary number
        Complex m = new Complex(0, dir);
        
        for (int i = 0; i < _tiles.Count; i++)
        {
            Vector2Int p = _tiles[i].GetLocalPosition();
            // Create a complex number with tile position
            Complex v = new Complex(p.x, p.y);
            // Multiplying complex pos to complex rotation
            v *= m;
            // Converting the result to tile position Vector 2int
            pos[i] = new Vector2Int((int)v.Real, (int)v.Imaginary);
        }

        return pos;
    }

    public void DestroyPiece()
    {
        foreach (Tile tile in _tiles)
        {
            tile.go.transform.SetParent(_parent.parent);
        }
        
        Destroy(_parent.gameObject);
    }
}

public enum Direction
{
    LEFT, 
    RIGHT,
    DOWN
}

