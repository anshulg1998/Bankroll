using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-100)]
/// <summary>
/// BoardManager listens for GameStartEvent and creates the board when the game starts.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [SerializeField] private TileManagerConfig boardConfig; // Board configuration data
    [SerializeField] private Transform boardParent; // Parent transform for board tiles

    private List<BoardTile> boardTiles = new List<BoardTile>(); // List of all board tiles

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GameStartEvent>(OnGameStart);
        EventBus.Subscribe<OnPlayerLandEvent>(OnPlayerLand);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameStartEvent>(OnGameStart);
        EventBus.Unsubscribe<OnPlayerLandEvent>(OnPlayerLand);
    }

    /// <summary>
    /// Handles the GameStartEvent and creates the board.
    /// </summary>
    /// <param name="evt">The GameStartEvent instance.</param>
    private void OnGameStart(GameStartEvent evt)
    {
        CreateBoard();
    }

    /// <summary>
    /// Handles the event when a player lands on a tile.
    /// </summary>
    /// <param name="evt">The OnPlayerLandEvent instance.</param>
    private void OnPlayerLand(OnPlayerLandEvent evt)
    {
        if (evt.Player == null) return;
        int position = evt.Player.PlayerData.position;
        var tile = GetBoardTileAtPosition(position);
        if (tile != null)
        {
            tile.OnPlayerLanded(evt.Player);
        }
    }

    /// <summary>
    /// Gets the board tile at the specified position.
    /// </summary>
    /// <param name="position">The tile index.</param>
    /// <returns>The BoardTile at the given index, or null if out of range.</returns>
    private BoardTile GetBoardTileAtPosition(int position)
    {
        if (boardTiles == null || position < 0 || position >= boardTiles.Count)
            return null;
        return boardTiles[position];
    }

    /// <summary>
    /// Creates the board tiles and sets up the board for gameplay.
    /// </summary>
    public void CreateBoard()
    {
        Debug.Log("BoardManager: Creating board...");
        boardTiles.Clear();
        if (boardConfig == null || boardConfig.boardTiles == null || boardConfig.boardTiles.Count == 0)
        {
            Debug.LogWarning("No board tiles assigned in boardConfig.");
            return;
        }
        int boardSize = boardConfig.boardTiles.Count;
        float tileSpacing = 0.6f;
        Vector3 offset = new Vector3(-2f, 0, -2f);
        for (int i = 0; i < boardSize; i++)
        {
            var data = boardConfig.boardTiles[i];
            if (data == null)
            {
                Debug.LogWarning($"Tile data is null at index {i}. Skipping.");
                boardTiles.Add(null);
                continue;
            }
            GameObject prefab = data.TilePrefab;
            GameObject tileObj = prefab ? Instantiate(prefab) : new GameObject($"Tile_{i}");
            var tile = tileObj.GetComponent<BoardTile>() ?? tileObj.AddComponent<BoardTile>();
            Vector3 pos = GetTilePosition(i, boardSize, tileSpacing, offset);
            tile.Init(data, boardParent != null ? boardParent : this.transform, pos, i, $"tile{i}");
            boardTiles.Add(tile);
        }
    }

    /// <summary>
    /// Calculates the world position for a tile based on its index.
    /// </summary>
    /// <param name="index">Tile index.</param>
    /// <param name="boardSize">Total number of tiles.</param>
    /// <param name="tileSpacing">Spacing between tiles.</param>
    /// <param name="off">Offset for the board position.</param>
    /// <returns>World position for the tile.</returns>
    private Vector3 GetTilePosition(int index, int boardSize, float tileSpacing, Vector3 off)
    {
        // 4 corners, 6 tiles per side (excluding corners)
        int tilesPerSide = (boardSize - 4) / 4;
        // Corner indices: 0, tilesPerSide+1, 2*(tilesPerSide+1), 3*(tilesPerSide+1)
        int corner0 = 0;
        int corner1 = tilesPerSide + 1;
        int corner2 = 2 * (tilesPerSide + 1);
        int corner3 = 3 * (tilesPerSide + 1);

        if (index == corner0) // Bottom-left
            return off;
        if (index == corner1) // Bottom-right
            return off + new Vector3((tilesPerSide + 1) * tileSpacing, 0, 0);
        if (index == corner2) // Top-right
            return off + new Vector3((tilesPerSide + 1) * tileSpacing, 0, (tilesPerSide + 1) * tileSpacing);
        if (index == corner3) // Top-left
            return off + new Vector3(0, 0, (tilesPerSide + 1) * tileSpacing);

        if (index > corner0 && index < corner1) // Bottom side
            return off + new Vector3(index * tileSpacing, 0, 0);
        if (index > corner1 && index < corner2) // Right side
            return off + new Vector3((tilesPerSide + 1) * tileSpacing, 0, (index - corner1) * tileSpacing);
        if (index > corner2 && index < corner3) // Top side
            return off + new Vector3((tilesPerSide + 1 - (index - corner2)) * tileSpacing, 0, (tilesPerSide + 1) * tileSpacing);
        if (index > corner3 && index < boardSize) // Left side
            return off + new Vector3(0, 0, (tilesPerSide + 1 - (index - corner3)) * tileSpacing);

        return off;
    }

    /// <summary>
    /// Gets the world position for a tile by index using default board settings.
    /// </summary>
    /// <param name="index">Tile index.</param>
    /// <returns>World position for the tile.</returns>
    public Vector3 GetTilePosition(int index)
    {
        int boardSize = boardConfig.boardTiles.Count;
        float tileSpacing = 0.6f;
        Vector3 off = new Vector3(-2f, 0, -2f);
        return GetTilePosition(index, boardSize, tileSpacing, off);
    }

    /// <summary>
    /// Gets the total number of tiles on the board.
    /// </summary>
    public int BoardSize => boardConfig != null && boardConfig.boardTiles != null ? boardConfig.boardTiles.Count : 0;
}
