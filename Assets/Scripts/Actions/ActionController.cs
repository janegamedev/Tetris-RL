using Tetris_RL.FSM;
using Tetris_RL.RL;
using Tetris_RL.Variables;

namespace Tetris_RL.Actions
{
    public class ActionController : StateActions
    {
        private readonly PieceVariable _currentPiece;
        private readonly BoolVariable _hardDrop;
        private readonly IInputGiver _giver;
    
        public ActionController(PieceVariable p, IInputGiver g, BoolVariable d)
        {
            _currentPiece = p;
            _giver = g;
            _hardDrop = d;
            SubscribeGiver();
        }
        
        private void SubscribeGiver()
        {
            _giver.HorizontalInputReceived += MovePieceHorizontal;
            _giver.RotationInputReceived += RotatePiece;
            _giver.ActionInputReceived += ActivateDrop;
        }
        
        public override void Execute()
        {
            _giver.Request();
        }
        
        private void ActivateDrop(int value)
        { 
            if(_currentPiece.value == null || value == 0 || _hardDrop.Value)
                return;

            _hardDrop.Value = true;
        }

        private void RotatePiece(int dir)
        {
            if(_currentPiece.value == null || dir == 0 || _hardDrop.Value)
                return;
            
            _currentPiece.value.RotateComplex(dir);
        }

        private void MovePieceHorizontal(int dir)
        {
            if(_currentPiece.value == null || dir == 0 || _hardDrop.Value)
                return;

            _currentPiece.value.MoveHorizontal(dir);
        }
    }

    
}