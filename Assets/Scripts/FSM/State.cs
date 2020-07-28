using System.Collections.Generic;

public class State
{
    public List<StateActions> actions = new List<StateActions>();

    public void Tick(StateManager state)
    {
        if(state.forceExit)
            return;

        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].Execute();
        }
    }
}