using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TetrisController : StateManager
{
    public Board boardConfigs;

    private Board _currentBoard;
    
    private void Awake()
    {
        BuildBoard();
    }

    private void BuildBoard()
    {
        _currentBoard = Instantiate(boardConfigs);
        BoardNode[,] nodes = new BoardNode[_currentBoard.gridSize.x, _currentBoard.gridSize.y];
        Vector2 offset = new Vector2(_currentBoard.gridSize.x / 2f - _currentBoard.nodeSize / 2f, _currentBoard.gridSize.y / 2f - _currentBoard.nodeSize / 2f);
        
        for (int y = 0; y < _currentBoard.gridSize.y; y++)
        {
            for (int x = 0; x < _currentBoard.gridSize.x; x++)
            {
                Vector3 pos = new Vector3(x * _currentBoard.nodeSize - offset.x, -y * _currentBoard.nodeSize + offset.y, 0);
                GameObject go = Instantiate(_currentBoard.nodePrefab, pos, Quaternion.identity, transform);
                go.name = "Node " + x + ":" + y;
                BoardNode n = new BoardNode(go);
                nodes[x, y] = n;
            }
        }
        
        _currentBoard.Init(nodes, offset);
    }
    
    public override void Init()
    {
        State pieceControl = new State();
        
        State lineCheck = new State();
        
        State endState = new State();
        
        allStates.Add("pieceControl", pieceControl);
        allStates.Add("lineCheck", lineCheck);
        allStates.Add("endState", endState);
        
        startingState = pieceControl;
        SetStartingState();
    }
}

public abstract class StateManager : MonoBehaviour
{
    public State startingState, currentState;
    public bool forceExit;
    
    protected readonly Dictionary<string, State> allStates = new Dictionary<string, State>();

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    public void Tick()
    {
        if (currentState != null)
        {
            currentState.Tick(this);
        }

        forceExit = false;
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

public abstract class StateActions
{
    public abstract void Execute();
}