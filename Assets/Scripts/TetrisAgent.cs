using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class TetrisAgent : Agent, IInputGiver
{
    public PieceController controller;
    public event Action<int> HorizontalInputReceived = delegate{}; 
    public event Action<int> RotationInputReceived = delegate{};
    public event Action<int> ActionInputReceived = delegate {  };

    private float _keyNothing = 0, _keyLeft = -1, _keyRight = 1, _keyHold = 1, _keyHardDrop = 2;
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void Initialize()
    {
        HorizontalInputReceived += controller.MovePieceHorizontal;
        RotationInputReceived += controller.RotatePiece;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(1);
    }
    
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = _keyNothing;
        if (Input.GetKey(KeyCode.LeftArrow))
            actionsOut[0] = _keyLeft;
        if (Input.GetKey(KeyCode.RightArrow))
            actionsOut[0] = _keyRight;
        
        actionsOut[1] = _keyNothing;
        if (Input.GetKey(KeyCode.Z))
            actionsOut[1] = _keyLeft;
        if (Input.GetKey(KeyCode.X))
            actionsOut[1] = _keyRight;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        HorizontalInputReceived.Invoke((int) vectorAction[0]);
        RotationInputReceived.Invoke((int) vectorAction[1]);
    }
}

public interface IInputGiver
{
    event Action<int> HorizontalInputReceived; 
    event Action<int> RotationInputReceived;
    event Action<int> ActionInputReceived;
}
