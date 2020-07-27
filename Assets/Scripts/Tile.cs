using UnityEngine;

public class Tile
{
      public GameObject go;
      private Piece _piece;
      private Vector2Int _localPosition;
      
      public Tile(Piece p, GameObject tile, Vector2Int pos)
      {
            _piece = p;
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

      public void DestroyTile()
      { 
            _piece.RemoveTile(this);
      }

      public Piece GetParent()
      {
            return _piece;
      }
}
