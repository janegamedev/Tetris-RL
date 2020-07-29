using Sirenix.OdinInspector;
using UnityEngine;

namespace Tetris_RL.Managers
{
    public class TileSpawner : MonoBehaviour
    {

        [BoxGroup("Pieces Prefab")]
        public GameObject prefab;
    
        public Piece SpawnTile(Piece p, Board b, Vector2Int spawnPosition)
        {
            Piece piece = Instantiate(p);
            GameObject go = new GameObject();
            go.name = p.name;
            go.transform.SetParent(transform);
        
            GameObject[] ts = new GameObject[p.tilesPositions.Length];
            for (int i = 0; i < p.tilesPositions.Length; i++)
            {
                GameObject tiles = Instantiate(prefab, go.transform);
                ts[i] = tiles;
            }
        
            piece.InitializePiece(b, go.transform, ts, spawnPosition, Random.Range(0, 4));
            return piece;
        }
    }
}