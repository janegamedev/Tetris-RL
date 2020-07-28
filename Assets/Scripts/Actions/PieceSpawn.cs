using UnityEngine;

public class PieceSpawn : StateActions
{
    private readonly Piece[] _pieces;
    private readonly PieceVariable _currentPiece, _nextPiece;
    private readonly FloatVariable _currentDelay;
    private readonly StateManager _manager;
    private readonly TileSpawner _spawner;
    private readonly Board _board;
   
    public PieceSpawn(StateManager m, Board b, TileSpawner s, PieceVariable c, PieceVariable n, Piece[] ps)
    {
        _manager = m;
        _board = b;
        _spawner = s;
        _pieces = ps;
        _currentPiece = c;
        _nextPiece = n;
    }
    
    public override void Execute()
    {
        if (_currentPiece.value == null && _nextPiece.value == null)
        {
            _currentPiece.value = _spawner.SpawnTile(_pieces[Random.Range(0,_pieces.Length)], _board);
            _nextPiece.value = _pieces[Random.Range(0,_pieces.Length)];
        }
        else
        {
            _currentPiece.value = _spawner.SpawnTile(_nextPiece.value, _board);
            _nextPiece.value = _pieces[Random.Range(0,_pieces.Length)];
        }
        
        _manager.SetNextState();
    }
}