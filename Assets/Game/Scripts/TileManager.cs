using System;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Data.Common; // For exception handling
/// <summary>
/// Manages all board tiles and their logic for the Monopoly-style game.
/// </summary>
public class TileManager : MonoBehaviour
{
    [SerializeField] private List<BoardTile> tiles;
    [SerializeField] private TileManagerConfig tileManagerConfig;

    // Add tile management logic here

    /// <summary>
    /// Initializes the board with a given list of tiles or creates default tiles if none provided.
    /// </summary>
    public void InitTiles()
    {
        if (tileManagerConfig == null || tileManagerConfig.boardTiles == null || tileManagerConfig.boardTiles.Count == 0)
        {
            return;
        }
        int boardSize = tileManagerConfig.boardTiles.Count;
        if (tiles == null)
        {
            tiles = new List<BoardTile>();
        }
        else
        {
            ResetComponent(); // Clear existing tiles if any
        }
        for (int i = 0; i < boardSize; i++)
        {
            var data = tileManagerConfig.boardTiles[i];
            GameObject tileObj = null;
            if (data == null)
            {
                Debug.LogWarning($"Tile data is null at index {i}. Skipping.");
                continue; // Skip this iteration if data is null
            }
            if (data.TilePrefab == null)
            {
                Debug.LogWarning($"Tile prefab missing for tile {data.TileName} at index {i}. Skipping.");
                tileObj = new GameObject($"Tile_{data.TileIndex}");
            }
            else
            {
                tileObj = UnityEngine.Object.Instantiate(data.TilePrefab);
            }
            var tile = tileObj.GetComponent<BoardTile>();
            if (tile == null)
            {
                tile = tileObj.AddComponent<BoardTile>();
            }
            tile.Init(data, transform,  GetTilePosition(i, boardSize, .57f, new Vector3(-2f, 0, -2f)), i, $"tile{i}");
            tiles.Add(tile);
        }
    }

    private Vector3 GetTilePosition(int index, int boardSize, float tileSpacing, Vector3 off)
    {
        int sideLength = boardSize / 4;
        int side = index / sideLength;
        int offset = index % sideLength;
        Vector3 startPos = off;
        return side switch
        {
            0 => startPos + new Vector3(offset * tileSpacing, 0, 0), // Bottom row (left to right)
            1 => startPos + new Vector3((sideLength - 1) * tileSpacing, 0, offset * tileSpacing), // Right column (bottom to top)
            2 => startPos + new Vector3((sideLength - 1 - offset) * tileSpacing, 0, (sideLength - 1) * tileSpacing), // Top row (right to left)
            3 => startPos + new Vector3(0, 0, (sideLength - 1 - offset) * tileSpacing), // Left column (top to bottom)
            _ => startPos
        };
    }
    
    public Vector3 GetTilePosition(int index)
    {
        if (index < 0 || index >= tiles.Count)
        {
            Debug.LogError($"Index {index} is out of bounds for tile list.");
            return Vector3.zero;
        }
        return tiles[index].transform.position;
    }

    public void ResetComponent()
    {
        
        foreach (var tile in tiles)
        {
            if (tile != null)
            {
                Destroy(tile.gameObject);
            }
        }
        tiles.Clear(); // Clear the list of tiles
    }

    public int GetBoardSize()
    {
        return tiles.Count;
    }
}
