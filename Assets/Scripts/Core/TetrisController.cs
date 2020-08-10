using Sirenix.OdinInspector;
using Tetris_RL.Actions;
using Tetris_RL.FSM;
using Tetris_RL.Interfaces;
using Tetris_RL.Variables;
using TMPro;
using UnityEngine;

namespace Tetris_RL.Core
{
    public class TetrisController : StateManager, IResetter
    {
        [BoxGroup("Board configs")]
        public Board boardConfigs;
        [BoxGroup("Board configs")] 
        public Transform boardTransform;

        [BoxGroup("Pieces Configs")]
        public Piece[] pieces;
        [BoxGroup("Pieces Configs")] 
        public TileSpawner spawner;
        
        [BoxGroup("Agent")]
        public IInputGiver currentGiver;
        [BoxGroup("Resetters")] 
        public IResetter[] resetters;

        [BoxGroup("Movement Configs")]
        public SpeedHolder speedConfig;

        [BoxGroup("Canvas Settings")] 
        public TextMeshProUGUI scoreText;

        private PieceVariable _currentPiece, _nextPiece;
        private IntVariable _scoreVariable;
        private FloatVariable _currentMoveDelay;
        private BoolVariable _hardDrop;
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
            pieceControl.actions.Add(new ActionController(_currentPiece, currentGiver, _hardDrop));
            pieceControl.actions.Add(new MoverDown(this, speedConfig, _currentPiece, _currentMoveDelay, _hardDrop));

            State lineCheck = new State();
            lineCheck.actions.Add(new LineChecker(this, _currentBoard, _scoreVariable, currentGiver));
            lineCheck.actions.Add(new ScoreDisplayer(_scoreVariable, scoreText));
        
            State end = new State();
            end.actions.Add(new EndGamer(resetters));
        
            allStates.Add("start", start);
            allStates.Add("pieceControl", pieceControl);
            allStates.Add("lineCheck", lineCheck);
            allStates.Add("end", end);
        
            startingState = start;
            SetStartingState();
        }

        private void InitVariables()
        {
            _scoreVariable = ScriptableObject.CreateInstance<IntVariable>();
            _currentPiece = ScriptableObject.CreateInstance<PieceVariable>();
            _nextPiece = ScriptableObject.CreateInstance<PieceVariable>();
            _currentMoveDelay = ScriptableObject.CreateInstance<FloatVariable>();
            _hardDrop = ScriptableObject.CreateInstance<BoolVariable>();
            currentGiver.SetBoard(_currentBoard);
        }
    
        private void BuildBoard()
        {
            _currentBoard = Instantiate(boardConfigs);
            BoardNode[,] nodes = new BoardNode[_currentBoard.gridSize.x, _currentBoard.gridSize.y];
            Vector2 offset = new Vector2(_currentBoard.gridSize.x / 2f - _currentBoard.nodeSize / 2f - transform.position.x, _currentBoard.gridSize.y / 2f - _currentBoard.nodeSize / 2f + transform.position.y);
        
            for (int y = 0; y < _currentBoard.gridSize.y; y++)
            {
                for (int x = 0; x < _currentBoard.gridSize.x; x++)
                {
                    Vector3 pos = new Vector3(x * _currentBoard.nodeSize - offset.x, -y * _currentBoard.nodeSize + offset.y, 0);
                    GameObject go = Instantiate(_currentBoard.nodePrefab, pos, Quaternion.identity, boardTransform);
                    go.name = "Node " + x + ":" + y;
                    Node node = go.AddComponent<Node>();
                    BoardNode n = new BoardNode();
                    node.node = n;
                    nodes[x, y] = n;
                }
            }
        
            _currentBoard.Init(nodes, offset);
        }

        public void Reset()
        {
            _currentPiece.value = null;
            _nextPiece.value = null;
            _scoreVariable.Value = 0;
            _currentBoard.ResetBoard();
            SetStartingState();
        }
    }
}