using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tetris_RL.FSM
{
    public abstract class StateManager : SerializedMonoBehaviour
    {
        private protected State startingState, currentState;
        public bool forceExit;
    
        protected readonly Dictionary<string, State> allStates = new Dictionary<string, State>();

        private void Start()
        {
            Init();
        }

        public abstract void Init();

        public void FixedUpdate()
        {
            if (currentState != null)
            {
                currentState.Tick(this);
            }

            forceExit = false;
        }

        public void SetNextState()
        {
            int ind = 0;
            for (int i = 0; i < allStates.Count; i++)
            {
                if (allStates.ElementAt(i).Value == currentState)
                    ind = i;
            }

            if (ind + 1 >= allStates.Count)
                ind = 0;
            SetState(allStates.ElementAt(ind + 1).Key);
        }

        public void SetState(string id)
        {
            State targetState = GetState(id);

            if (targetState == null)
            {
                Debug.LogError("State with id: " + id + " cannot be found!");
            }

            currentState = targetState;
        }

        public void SetStartingState()
        {
            currentState = startingState;
        }

        private State GetState(string id)
        {
            allStates.TryGetValue(id, out State result);
            return result;
        }
    }
}