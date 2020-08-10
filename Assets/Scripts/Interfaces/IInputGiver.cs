using System;
using Tetris_RL.Core;

namespace Tetris_RL.Interfaces
{
    public interface IInputGiver
    {
        event Action<int> HorizontalInputReceived; 
        event Action<int> RotationInputReceived;
        event Action<int> ActionInputReceived;

        void Request();
        void SetBoard(Board b);
        void AddBreakLineReward();
    }
}