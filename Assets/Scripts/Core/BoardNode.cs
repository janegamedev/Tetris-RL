using JetBrains.Annotations;

namespace Tetris_RL.Core
{
    [System.Serializable]
    public class BoardNode
    {
        private Status _nodeStatus;
        private Tile _occupatedTile;

        public BoardNode()
        {
            _nodeStatus = Status.AVAILABLE;
        }

        public bool IsNodeOccupated()
        {
            return _nodeStatus == Status.OCCUPATED;
        }

        public Tile GetTile()
        {
            return _occupatedTile;
        }

        public void SetTile([CanBeNull] Tile t, Status status)
        {
            _occupatedTile = t;
            _nodeStatus = status;
        }
    }

    public enum Status
    {
        AVAILABLE,
        TEMPORARY,
        OCCUPATED
    }
}