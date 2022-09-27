using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominos;
    public Piece activePiece { get; private set; }
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        this.activePiece = GetComponentInChildren<Piece>();
        this.tilemap = GetComponentInChildren<Tilemap>();

        for(int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Inicialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];

        this.activePiece.Inicialize(this, spawnPosition, data);

        if(IsValidPosition(this.activePiece, this.spawnPosition))
        {
            Set(this.activePiece);
        }
        else
        {
            GameOver();
        }
        
    }

    public void Set(Piece piece)
    {
        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {

        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i ++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if(!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if(this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while(row < bounds.yMax)
        {
            if(IsRowFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;
        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        while(row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }

            row++;
        }
    }

    private bool IsRowFull(int row)
    {
        RectInt bounds = this.Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if(!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
    }
}
