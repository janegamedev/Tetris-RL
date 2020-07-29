using Tetris_RL.FSM;
using Tetris_RL.RL;
using Tetris_RL.Variables;

namespace Tetris_RL.Actions
{
    public class InputRequester : StateActions
    {
        private readonly IntVariable _horizontalVariable, _rotationVariable, _actionVariable;
        private readonly IInputGiver _giver;
        private int _frequency, _current;
        
        
        public InputRequester(IInputGiver giver, IntVariable h, IntVariable r, IntVariable a)
        {
            _giver = giver;
            _horizontalVariable = h;
            _rotationVariable = r;
            _actionVariable = a;
            _frequency = _giver.GetFrequency();
            SubscribeGiver();
            _current = 0;
        }

        private void SubscribeGiver()
        {
            _giver.HorizontalInputReceived += UpdateHorizontalVariable;
            _giver.RotationInputReceived += UpdateRotationalVariable;
            _giver.ActionInputReceived += UpdateActionVariable;
        }
    
        public override void Execute()
        {
            if (_current >= _frequency)
            {
                _giver.Request();
                _current = 0;
            }
            else
            {
                _current++;
            }
        }

        private void UpdateHorizontalVariable(int value)
        {
            _horizontalVariable.Value = value;
        }

        private void UpdateRotationalVariable(int value)
        {
            _rotationVariable.Value = value;
        }

        private void UpdateActionVariable(int value)
        {
            _actionVariable.Value = value;
        }
    }
}