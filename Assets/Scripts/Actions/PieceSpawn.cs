using System.Collections.Generic;
using System.Linq;
using Tetris_RL.Core;
using Tetris_RL.FSM;
using Tetris_RL.Variables;
using UnityEngine;

namespace Tetris_RL.Actions
{
    public class PieceSpawn : StateActions
    {
        private readonly Piece[] _pieces;
        private readonly PieceVariable _currentPiece, _nextPiece;
        private readonly FloatVariable _currentDelay;
        private readonly StateManager _manager;
        private readonly TileSpawner _spawner;
        private readonly Board _board;
        private readonly Vector2Int _spawnPosition = new Vector2Int(4, 0);
   
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
                _currentPiece.value = _spawner.SpawnTile(_pieces[Random.Range(0,_pieces.Length)], _board, _spawnPosition);
                _nextPiece.value = _pieces[Random.Range(0,_pieces.Length)];
            }
            else
            {
                if (IsPieceFittable(_nextPiece.value))
                {
                    _currentPiece.value = _spawner.SpawnTile(_nextPiece.value, _board, _spawnPosition);
                    _nextPiece.value = _pieces[Random.Range(0,_pieces.Length)];
                }
                else
                {
                    _manager.SetState("end");
                    return;
                }
            }
        
            _manager.SetNextState();
        }

        private bool IsPieceFittable(Piece p)
        {
            return p.tilesPositions.All(tilesPosition => _board.IsTileAvailable(_spawnPosition + tilesPosition));
        }
    }
}