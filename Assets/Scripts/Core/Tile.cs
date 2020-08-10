using UnityEngine;

namespace Tetris_RL.Core
{
      public class Tile
      {
            public readonly GameObject go;
            private Vector2Int _localPosition;
      
            public Tile(Piece p, GameObject tile, Vector2Int pos)
            {
                  go = tile;
                  _localPosition = pos;
                  go.GetComponent<SpriteRenderer>().color = p.color;
            }

            public Vector2Int GetLocalPosition()
            {
                  return _localPosition;
            }

            public void UpdateWorldPosition(Vector3 pos)
            {
                  go.transform.position = pos;
            }

            public void UpdateLocalPosition(Vector2Int pos)
            {
                  _localPosition = pos;
            }
      }
}
