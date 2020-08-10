using System.Collections.Generic;
using Tetris_RL.Core;
using Tetris_RL.FSM;
using Tetris_RL.Interfaces;
using Tetris_RL.RL;
using Tetris_RL.Variables;
using UnityEngine;

namespace Tetris_RL.Actions
{
    public class LineChecker : StateActions
    {
        private readonly Board _board;
        private readonly StateManager _manager;
        private readonly IntVariable _score;
        private readonly IInputGiver _giver;
        
        private Queue<int> _lines;
        private bool _checkNeeded;
        private float _destroyDelay = 0.25f, _timeCount;
        private int _currentLine, _currentIndex;
    
        public LineChecker(StateManager m, Board b, IntVariable s, IInputGiver giver)
        {
            _manager = m;
            _board = b;
            _score = s;
            _giver = giver;
            _checkNeeded = true;
        }
    
        public override void Execute()
        {
            if (_checkNeeded)
            {
                // Receiving queue of line indexes to clear
                _lines = _board.GetLinesToClear();
                // Resetting line clearing delay  
                ResetDelay();
                
                if (_lines.Count < 1)
                {
                    EndClearing();
                    return;
                }
                
                // Setting first current line index to clear
                _currentLine = _lines.Dequeue();
                _checkNeeded = false;
            }
            
            // Kinda coroutine for clearing one line tile by tile 
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
            // Cleaning two tiles symmetrically from the center to sides
            // Where current index is multiplying value from 0 to 4
            _board.ClearNode(new Vector2Int(5 + _currentIndex, _currentLine));          // Remove right side tile
            _board.ClearNode(new Vector2Int(5 - (1 + _currentIndex), _currentLine));    // Remove left side tile

            // Increasing current index if possible
            if (_currentIndex + 1 < 5)        
            {
                _currentIndex += 1;
            }
            else
            {
                // Calling move down tile after each line cleared
                _board.MoveDownAbove(_currentLine);
                
                //Adding agent reward
                _giver.AddBreakLineReward();
                
                // Increase score by constant amount
                _score.Value += 100;
                
                if (_lines.Count > 0)  // If there are any lines left
                {
                    _currentLine = _lines.Dequeue();        // Change current line index
                    ResetDelay();                           // Resetting line clearing delay    
                }
                else
                {
                    EndClearing();                           // End cleaning state
                }
            }
        }

        private void EndClearing()
        {
            _checkNeeded = true;
            // Calling start state in state manager
            _manager.SetState("start");
        }

        private void ResetDelay()
        {
            _currentIndex = 0;
            _timeCount = 0;
        }
        
    }
}