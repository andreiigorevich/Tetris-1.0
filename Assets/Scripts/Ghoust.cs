using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghoust : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPieace;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }
    private void Clear()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = this.trackingPieace.cells[i];
        }
    }

    private void Drop()
    {
        Vector3Int position = this.trackingPieace.position;

        int currentRow = position.y;
        int button = -this.board.boardSize.y / 2 - 1;

        this.board.Clear(this.trackingPieace);

        for(int row = currentRow; currentRow >= button; row--)
        {
            position.y = row;

            if(this.board.IsValidPosition(this.trackingPieace, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }
        this.board.Set(trackingPieace);
    }

    private void Set()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}
