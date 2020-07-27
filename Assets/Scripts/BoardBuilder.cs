using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class BoardBuilder : MonoBehaviour
{
    public Board board;
    
    public void Awake()
    {
        BuildBoard();
    }

    private void BuildBoard()
    {
        BoardNode[,] nodes = new BoardNode[board.gridSize.x, board.gridSize.y];
        Vector2 offset = new Vector2(board.gridSize.x / 2f - board.nodeSize / 2f, board.gridSize.y / 2f - board.nodeSize / 2f);
        
        for (int y = 0; y < board.gridSize.y; y++)
        {
            for (int x = 0; x < board.gridSize.x; x++)
            {
                Vector3 pos = new Vector3(x * board.nodeSize - offset.x, -y * board.nodeSize + offset.y, 0);
                GameObject go = Instantiate(board.nodePrefab, pos, Quaternion.identity, transform);
                go.name = "Node " + x + ":" + y;
                BoardNode n = new BoardNode(go);
                nodes[x, y] = n;
            }
        }
        
        board.Init(nodes, offset);
    }
}

