using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class PieceController: MonoBehaviour
{
    [BoxGroup("Board Config")]
    public Board board;
    [BoxGroup("Pieces Configs")]
    public Piece[] pieces;
    [BoxGroup("Pieces Spawn Point")]
    public Vector2Int spawnPosition;
    [BoxGroup("Pieces Prefab")]
    public GameObject prefab;
   
    [BoxGroup("Speed Settings")]
    public float deltaNormal, deltaHard, deltaSort;
    private float _currentSpeed;
    private bool _isHardDrop;

    private List<Piece> _pieces = new List<Piece>();
    private Queue<Piece> _nextPieces = new Queue<Piece>();
    private Piece _activePiece;

    
    private void Start()
    {
        GeneratePiece();
        StartCoroutine(MovePieceDown());
    }

    private void GeneratePiece()
    {
        board.CheckForLines();
        
        if (_nextPieces.Count < 2)
        {
            int i = 2 - _nextPieces.Count;

            for (int j = 0; j < i; j++)
            {
                _nextPieces.Enqueue(pieces[Random.Range(0,pieces.Length)]);
            }
        }
        
        _isHardDrop = false;
        _currentSpeed = deltaNormal;
        _activePiece = SpawnPiece(_nextPieces.Dequeue());
        _activePiece.OnDropFinished += GeneratePiece;
    }
    

    IEnumerator MovePieceDown()
    {
        while (_activePiece != null)
        {
            _activePiece.MoveDown();
            
            yield return new WaitForSeconds(_currentSpeed);
        }
    }

    public void MovePieceHorizontal(int dir)
    {
        if (dir == 0 || _isHardDrop)
            return;

        _activePiece.MoveHorizontal(dir);
    }

    public void RotatePiece(int dir) 
    {
        if(dir == 0 || _isHardDrop)
            return;
        
        _activePiece.RotateComplex(dir);
    }

    public void Action(int action)
    {
        
        if(action == 0)
            return;

        if (action == 1) // Hold
        {
            
        }
        else  // Hard drop
        {
            _isHardDrop = true;
            _currentSpeed = deltaHard;
        }
       
    }

    private Piece SpawnPiece(Piece piece)
    {
        Piece p = Instantiate(piece);
        GameObject go = new GameObject();
        go.name = p.name;
        go.transform.SetParent(transform);

        GameObject[] ts = new GameObject[p.tilesPositions.Length];
        for (int i = 0; i < p.tilesPositions.Length; i++)
        {
            GameObject t = Instantiate(prefab, go.transform);
            ts[i] = t;
        }
        
        p.InitializePiece(board, go.transform, ts, spawnPosition, Random.Range(0,4));
        return p;
    }

    [ContextMenu("Rotate")]
    public void Rotate()
    {
        _activePiece.RotateComplex(1);
    }
}