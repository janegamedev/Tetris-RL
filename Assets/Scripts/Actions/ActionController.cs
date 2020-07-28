public class ActionController : StateActions
{
    private readonly IntVariable _horizontalVariable, _rotationVariable, _actionVariable;
    private readonly PieceVariable _currentPiece;
    
    public ActionController(PieceVariable p, IntVariable h, IntVariable r, IntVariable a)
    {
        _currentPiece = p;
        _horizontalVariable = h;
        _rotationVariable = r;
        _actionVariable = a;
    }
  
    public override void Execute()
    {
        if (_horizontalVariable.value != 0 /*_isHardDrop*/)
            MovePieceHorizontal(_horizontalVariable.value);
        if(_rotationVariable.value != 0)
            RotatePiece(_rotationVariable.value);
       
        if (_actionVariable.value == 1)
        {
            //Hold
        }
        else if (_actionVariable.value == 2)
        {
            //Hard drop
        }
    }

    private void RotatePiece(int dir)
    {
        if(_currentPiece.value != null)
            _currentPiece.value.RotateComplex(dir);
    }

    private void MovePieceHorizontal(int dir)
    {
        if(_currentPiece.value != null)
            _currentPiece.value.MoveHorizontal(dir);
    }
}