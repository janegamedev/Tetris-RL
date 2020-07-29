using System;
using Tetris_RL.Actions;
using Tetris_RL.Variables;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Tetris_RL.RL
{
    public class TetrisAgent : Agent, IInputGiver, IResetter
    {
        public event Action<int> HorizontalInputReceived = delegate{}; 
        public event Action<int> RotationInputReceived = delegate{};
        public event Action<int> ActionInputReceived = delegate {};
        
        public int frequency;
        public KeysHolder keys;

        private int _currentCall;
        private Board _board;
        
        public override void CollectObservations(VectorSensor sensor)
        {
            BoardNode[,] nodes = _board.GetNodes();
            
            for (int y = 0; y < _board.gridSize.y; y++)
            {
                for (int x = 0; x < _board.gridSize.x; x++)
                {
                    sensor.AddObservation(nodes[x, y].GetTile() != null);
                }
            }
        }

        public void SetBoard(Board b)
        {
            _board = b;
        }
    
        public void Request()
        {
            if (_currentCall >= frequency)
            {
                RequestDecision();
                _currentCall = 0;
            }
            else
            {
                _currentCall++;
            }
        }

        public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = keys.keyNothing;
            if (Input.GetKey(KeyCode.LeftArrow))
                actionsOut[0] = keys.keyLeft;
            if (Input.GetKey(KeyCode.RightArrow))
                actionsOut[0] = keys.keyRight;

            actionsOut[1] = keys.keyNothing;
            if (Input.GetKey(KeyCode.Z))
                actionsOut[1] = keys.keyLeft;
            if (Input.GetKey(KeyCode.X))
                actionsOut[1] = keys.keyRight;
            
            actionsOut[2] = keys.keyNothing;
            if (Input.GetKey(KeyCode.Space))
                actionsOut[2] = keys.keyHardDrop;
        }

        public void AddBreakLineReward()
        {
            AddReward(1f);
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            Debug.Log("HEURISTIC");
            HorizontalInputReceived.Invoke((int) vectorAction[0]);
            RotationInputReceived.Invoke((int) vectorAction[1]);
            ActionInputReceived.Invoke((int) vectorAction[2]);
        }

        public void Reset()
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}