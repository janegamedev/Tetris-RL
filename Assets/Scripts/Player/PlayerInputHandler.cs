using System;
using Tetris_RL.RL;
using UnityEngine;

namespace Tetris_RL.Player
{
    public class PlayerInputHandler: MonoBehaviour, IInputGiver
    {
        public event Action<int> HorizontalInputReceived = delegate {  };
        public event Action<int> RotationInputReceived = delegate {  };
        public event Action<int> ActionInputReceived = delegate {  };
    
        public void Request()
        {
            GetInput();
        }

        private float _keyNothing = 0, _keyLeft = -1, _keyRight = 1, _keyHold = 1, _keyHardDrop = 2;
        private float[] _actions = new float[3];

        private void GetInput()
        {
            _actions[0] = _keyNothing;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _actions[0] = _keyLeft;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                _actions[0] = _keyRight;
        
            _actions[1] = _keyNothing;
            if (Input.GetKeyDown(KeyCode.Z))
                _actions[1] = _keyLeft;
            if (Input.GetKeyDown(KeyCode.X))
                _actions[1] = _keyRight;
        
            _actions[2] = _keyNothing;
            if (Input.GetKeyDown(KeyCode.C))
                _actions[2] = _keyHold;
            if (Input.GetKeyDown(KeyCode.Space))
                _actions[2] = _keyHardDrop;
        
            HorizontalInputReceived.Invoke((int) _actions[0]);
            RotationInputReceived.Invoke((int) _actions[1]);
            ActionInputReceived.Invoke((int) _actions[2]);
        }
    }
}