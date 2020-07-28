using System.Collections.Generic;
using UnityEngine;

public class LineChecker : StateActions
{
    private Board _board;
    private Queue<int> _lines;
    private bool _checkNeeded, _nextLine, _needMoveDown;

    private float _destroyDelay = 0.25f, _timeCount;
    private int _currentLine, _currentIndex;
    private StateManager _manager;
    
    public LineChecker(StateManager m, Board b)
    {
        _manager = m;
        _board = b;
        _checkNeeded = true;
    }
    
    public override void Execute()
    {
        if (_checkNeeded)
        {
            _lines = _board.CheckForLines();
            _checkNeeded = false;
            _timeCount = 0;
            _nextLine = true;
            _needMoveDown = false;
        }

        if (_lines.Count < 1)
        {
            _checkNeeded = true;
            if(_needMoveDown)
                MoveTilesDown();
            _manager.SetState("start");
            return;
        }

        if (_nextLine)
        {
            _needMoveDown = true;
            _currentLine = _lines.Dequeue();
            _nextLine = false;
            _currentIndex = 0;
        }
        
        if (_timeCount >= _destroyDelay)
        {
            _timeCount = 0;
            DestroyTile();
        }
        else
        {
            _timeCount += Time.deltaTime;
        }
    }

    private void DestroyTile()
    {
        _board.ClearNode(new Vector2Int(5 + _currentIndex, _currentLine));
        _board.ClearNode(new Vector2Int(5 - (1 + _currentIndex), _currentLine));

        if (_currentIndex + 1 < 6)
            _currentIndex += 1;
        else
            _nextLine = true;
    }

    private void MoveTilesDown()
    {
        _board.MoveTilesDown();
    }

    private void Reset()
    {
        _nextLine = false;
        _currentIndex = 0;
        _timeCount = 0;
    }
}