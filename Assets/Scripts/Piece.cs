using System;
using System.Linq;
using System.Numerics;
using UnityEngine;

[CreateAssetMenu(menuName = "Piece/Piece Shape")]
public class Piece : ScriptableObject
{
    public event Action OnDropFinished;
    public Vector2Int[] tilesPositions;
    
    [NonSerialized]
    private GameObject[] _tiles;
    private Transform _parent;
    private Vector2Int _gridPos;
    private Board _board;
    
    public void InitializePiece(Board b, Transform parent, GameObject[] t, Vector2Int pos, int rot)
    {
        _board = b;
        _parent = parent;
        _tiles = t;
        _gridPos = pos;
        _parent.position = b.GetWorldPos(_gridPos);
        
        UpdatePositionsForTiles();
    }

    public void MoveDown()
    {
        // Remove tiles form nodes so we can check without conflicting with our tiles
        RemoveFromNodes();
        
        if (CanMoveDown())
        {
            // Update grid position for this piece
            _gridPos += Vector2Int.up;
            // Update world position
            _parent.position = _board.GetWorldPos(_gridPos);
            // Update new tile positions according to the piece position
            UpdatePositionsForTiles();
        }
        else // If the piece can;t be moves - the move of the piece is finished
        {
            // Update tile position and subscribing for current nodes
            UpdatePositionsForTiles();
            // Calling on drop finished event 
            OnDropFinished?.Invoke();
        }
    }
    
    // Calculating new local tile positions according to this piece grid position
    private void UpdatePositionsForTiles()
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i].transform.position = _board.GetWorldPos(_gridPos + tilesPositions[i]);
            _board.SetTileToNode(_tiles[i], _gridPos + tilesPositions[i]);
        }
    }

    // Unsubscribing all tile from their current nodes
    private void RemoveFromNodes()
    {
        for (var i = 0; i < _tiles.Length; i++)
        {
            _board.RemoveTileFromNode(_gridPos + tilesPositions[i]);
        }
    }

    // Check if all tiles are able to move down
    private bool CanMoveDown()
    {
        return tilesPositions.All(t => _board.IsTileAvailable(_gridPos + Vector2Int.up + t));
    }
    
    
    // Rotate piece 90* counter clockwise/clockwise using complex numbers
    public void RotateComplex(float input)
    {
        // Create a complex number with input -1/1 as imaginary number
        Complex m = new Complex(0, input);
        
        for (int i = 0; i < tilesPositions.Length; i++)
        {
            // Create a complex number with tile position
            Complex v = new Complex(tilesPositions[i].x, tilesPositions[i].y);
            // Multiplying complex pos to complex rotation
            v *= m;
            // Converting the result to tile position Vector 2int
            tilesPositions[i] = new Vector2Int((int)v.Real, (int)v.Imaginary);
        }
        
        // Update new tile positions
        UpdatePositionsForTiles();
    }
    
    public void RotateMatrix(float input)
    {
        // Update new tile positions
        UpdatePositionsForTiles();
    }
}

