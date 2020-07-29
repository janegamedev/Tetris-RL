using System;
using Tetris_RL.FSM;
using UnityEngine;

namespace Tetris_RL.Actions
{
    public class EndGamer : StateActions
    {
        public event Action OnEpisodeEnd = delegate {  };
        private readonly IResetter[] _ender;
        
        public EndGamer(IResetter[] e)
        {
            _ender = e;
            foreach (IResetter episodeEnder in _ender)
            {
                OnEpisodeEnd += episodeEnder.Reset;
            }
        }
        
        public override void Execute()
        {
            OnEpisodeEnd?.Invoke();
        }
    }
}