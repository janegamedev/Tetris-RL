public class InputRequester : StateActions
{
    private readonly IntVariable _horizontalVariable, _rotationVariable, _actionVariable;
    private readonly IInputGiver _giver;
    
    public InputRequester(IInputGiver giver, IntVariable h, IntVariable r, IntVariable a)
    {
        _giver = giver;
        _horizontalVariable = h;
        _rotationVariable = r;
        _actionVariable = a;
        SubscribeGiver();
    }

    private void SubscribeGiver()
    {
        _giver.HorizontalInputReceived += UpdateHorizontalVariable;
        _giver.RotationInputReceived += UpdateRotationalVariable;
        _giver.ActionInputReceived += UpdateActionVariable;
    }
    
    public override void Execute()
    {
        _giver.Request();
    }

    private void UpdateHorizontalVariable(int value)
    {
        _horizontalVariable.value = value;
    }

    private void UpdateRotationalVariable(int value)
    {
        _rotationVariable.value = value;
    }

    private void UpdateActionVariable(int value)
    {
        _actionVariable.value = value;
    }
}