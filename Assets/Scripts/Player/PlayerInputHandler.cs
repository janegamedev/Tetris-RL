using System;
using Tetris_RL.Core;
using Tetris_RL.Interfaces;
using Tetris_RL.RL;
using Tetris_RL.Variables;
using UnityEngine;

namespace Tetris_RL.Player
{
    public class PlayerInputHandler: MonoBehaviour, IInputGiver
    {
        public event Action<int> HorizontalInputReceived = delegate {  };
        public event Action<int> RotationInputReceived = delegate {  };
        public event Action<int> ActionInputReceived = delegate {  };
    
        public KeysHolder keys;
        private readonly float[] _actions = new float[3];
        
        public void Request()
        {
            GetInput();
        }

        public void SetBoard(Board b){}

        public void AddBreakLineReward(){}

        private void GetInput()
        {
            _actions[0] = keys.keyNothing;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _actions[0] = keys.keyLeft;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                _actions[0] = keys.keyRight;
        
            _actions[1] = keys.keyNothing;
            if (Input.GetKeyDown(KeyCode.Z))
                _actions[1] = keys.keyLeft;
            if (Input.GetKeyDown(KeyCode.X))
                _actions[1] = keys.keyRight;
        
            _actions[2] = keys.keyNothing;
            if (Input.GetKeyDown(KeyCode.Space))
                _actions[2] = keys.keyHardDrop;
        
            HorizontalInputReceived.Invoke((int) _actions[0]);
            RotationInputReceived.Invoke((int) _actions[1]);
            ActionInputReceived.Invoke((int) _actions[2]);
        }
    }
}