using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PieceController: MonoBehaviour
{
    public Board board;
    public Piece[] pieces;
    public Vector2Int spawnPosition;
    public GameObject prefab;
    public float deltaNormal, deltaHard, deltaSort;
    
    private Queue<Piece> _nextPieces = new Queue<Piece>();
    private Piece _activePiece;

    private void Start()
    {
        GeneratePiece();
        StartCoroutine(MovePieceDown());
    }

    private void GeneratePiece()
    {
        if (_nextPieces.Count < 2)
        {
            int i = 2 - _nextPieces.Count;

            for (int j = 0; j < i; j++)
            {
                _nextPieces.Enqueue(pieces[Random.Range(0,pieces.Length)]);
            }
        }

        _activePiece = SpawnPiece(_nextPieces.Dequeue());
        _activePiece.OnDropFinished += GeneratePiece;
    }

    IEnumerator MovePieceDown()
    {
        while (_activePiece != null)
        {
            _activePiece.MoveDown();
            
            yield return new WaitForSeconds(deltaNormal);
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