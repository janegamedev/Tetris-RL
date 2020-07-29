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
        public event Action<int> ActionInputReceived = delegate {  };
        
        public int frequency;
        public KeysHolder keys;
    
        public override void OnEpisodeBegin()
        {
            base.OnEpisodeBegin();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(1);
        }
    
        public void Request()
        {
            RequestDecision();
        }

        public int GetFrequency()
        {
            return frequency;
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
            
            if (Input.GetKey(KeyCode.Space))
                actionsOut[2] = keys.keyHardDrop;
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            HorizontalInputReceived.Invoke((int) vectorAction[0]);
            RotationInputReceived.Invoke((int) vectorAction[1]);
            ActionInputReceived.Invoke((int) vectorAction[2]);
        }

        public void Reset()
        {
            EndEpisode();
        }
    }
}