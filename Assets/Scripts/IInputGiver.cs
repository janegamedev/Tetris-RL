using System;

namespace Tetris_RL.RL
{
    public interface IInputGiver
    {
        event Action<int> HorizontalInputReceived; 
        event Action<int> RotationInputReceived;
        event Action<int> ActionInputReceived;

        void Request();
    }
}