using Tetris_RL.FSM;
using Tetris_RL.Variables;

namespace Tetris_RL.Actions
{
    public class ActionController : StateActions
    {
        private readonly IntVariable _horizontalVariable, _rotationVariable, _actionVariable;
        private readonly PieceVariable _currentPiece;
        private readonly BoolVariable _hardDrop;
    
        public ActionController(PieceVariable p, IntVariable h, IntVariable r, IntVariable a, BoolVariable d)
        {
            _currentPiece = p;
            _horizontalVariable = h;
            _rotationVariable = r;
            _actionVariable = a;
            _hardDrop = d;
        }
  
        public override void Execute()
        {
            if (_hardDrop.Value) return;
            
            if (_horizontalVariable.Value != 0)
                MovePieceHorizontal(_horizontalVariable.Value);
            if(_rotationVariable.Value != 0)
                RotatePiece(_rotationVariable.Value);
       
            if (_actionVariable.Value == 1)
            {
                _hardDrop.Value = true;
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

    
}