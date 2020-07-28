using Sirenix.OdinInspector;
using UnityEngine;

public class TetrisController : StateManager
{
    [BoxGroup("Board configs")]
    public Board boardConfigs;
    [BoxGroup("Board configs")] 
    public Transform boardTransform;

    [BoxGroup("Pieces Configs")]
    public Piece[] pieces;
    [BoxGroup("Pieces Configs")] 
    public TileSpawner spawner;
    
    [BoxGroup("Default Configs")] 
    public Type plyingType;
    [BoxGroup("Input References")]
    public IInputGiver agentGiver, playerGiver;

    [BoxGroup("Movement Configs")]
    public SpeedHolder speedConfig;

    private PieceVariable _currentPiece, _nextPiece;
    private IntVariable _horizontalVariable, _rotationVariable, _actionVariable;
    private FloatVariable _currentMoveDelay;
    private IInputGiver _currentGiver;
    private Board _currentBoard;
    
    private void Awake()
    {
        BuildBoard();
        InitVariables();
    }

    public override void Init()
    {
        State start = new State();
        start.actions.Add(new PieceSpawn(this, _currentBoard, spawner, _currentPiece, _nextPiece, pieces));

        State pieceControl = new State();
        pieceControl.actions.Add(new InputRequester(_currentGiver, _horizontalVariable, _rotationVariable, _actionVariable));
        pieceControl.actions.Add(new ActionController(_currentPiece, _horizontalVariable, _rotationVariable, _actionVariable));
        pieceControl.actions.Add(new MoverDown(this, speedConfig, _currentPiece, _currentMoveDelay));

        State lineCheck = new State();
        lineCheck.actions.Add(new LineChecker(this, _currentBoard));
        
        State end = new State();
        
        allStates.Add("start", start);
        allStates.Add("pieceControl", pieceControl);
        allStates.Add("lineCheck", lineCheck);
        allStates.Add("end", end);
        
        startingState = start;
        SetStartingState();
    }

    private void InitVariables()
    {
        _horizontalVariable = ScriptableObject.CreateInstance<IntVariable>();
        _rotationVariable = ScriptableObject.CreateInstance<IntVariable>();
        _actionVariable = ScriptableObject.CreateInstance<IntVariable>();
        _currentPiece = ScriptableObject.CreateInstance<PieceVariable>();
        _nextPiece = ScriptableObject.CreateInstance<PieceVariable>();
        _currentMoveDelay = ScriptableObject.CreateInstance<FloatVariable>();
        _currentGiver = plyingType == Type.PLAYER ? playerGiver : agentGiver;
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
                GameObject go = Instantiate(_currentBoard.nodePrefab, pos, Quaternion.identity, boardTransform);
                go.name = "Node " + x + ":" + y;
                BoardNode n = new BoardNode(go);
                nodes[x, y] = n;
            }
        }
        
        _currentBoard.Init(nodes, offset);
    }
}

public enum Type
{
    PLAYER, 
    AI
}