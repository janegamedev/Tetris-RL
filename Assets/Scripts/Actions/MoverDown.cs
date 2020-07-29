using Tetris_RL.FSM;
using Tetris_RL.Variables;
using Unity.MLAgents;
using UnityEngine;

namespace Tetris_RL.Actions
{
    public class MoverDown : StateActions
    {
        private readonly PieceVariable _currentPiece;
        private readonly SpeedHolder _speedConfigs;
        private readonly FloatVariable _currentDelay;
        private readonly BoolVariable _hardDrop;
        
        private float _timeCount;
        private bool _needSubscribe;

        private StateManager _manager;
    
        public MoverDown(StateManager m, SpeedHolder s, PieceVariable p, FloatVariable d, BoolVariable h)
        {
            _manager = m;
            _speedConfigs = s;
            _currentPiece = p;
            _currentDelay = d;
            _hardDrop = h;
            _hardDrop.OnValueChanged += ChangeSpeed;
            
            _currentDelay.value = _speedConfigs.deltaNormal;
            _needSubscribe = true;
        }

        public override void Execute()
        {
            if (_needSubscribe)
            {
                _needSubscribe = false;
                _currentPiece.value.OnDropFinished += MoveFinished;
            }

            if (_timeCount >= _currentDelay.value)
            {
                _timeCount = 0;
                if(_currentPiece.value != null)
                    _currentPiece.value.MoveDown();
            }
            else
            {
                _timeCount += Time.deltaTime;
            }
        }

        private void ChangeSpeed(bool drop)
        {
            _currentDelay.value = drop == true ? _speedConfigs.deltaHard : _speedConfigs.deltaNormal;
        }

        private void MoveFinished()
        {
            _needSubscribe = true;
            _hardDrop.Value = false;
                       
            if (_currentPiece.value.GetGridPosition().y < 1)
            {
                _manager.SetState("end");
            }
            else
            {
                _manager.SetNextState();
            }
         
            _currentPiece.value.DestroyPiece();
            _currentPiece.value = null;
        }
    }
}