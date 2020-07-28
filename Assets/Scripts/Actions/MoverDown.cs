using UnityEngine;

public class MoverDown : StateActions
{
    private readonly PieceVariable _currentPiece;
    private readonly SpeedHolder _speedConfigs;
    private readonly FloatVariable _currentDelay;
    private float _timeCount;

    private bool _needSubscribe;

    private StateManager _manager;
    
    public MoverDown(StateManager m, SpeedHolder s, PieceVariable p, FloatVariable d)
    {
        _manager = m;
        _speedConfigs = s;
        _currentPiece = p;
        _currentDelay = d;
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

    private void MoveFinished()
    {
        _needSubscribe = true;
        _currentPiece.value.DestroyPiece();
        _currentPiece.value = null;
        _currentDelay.value = _speedConfigs.deltaNormal;
        
        _manager.SetNextState();
    }
}