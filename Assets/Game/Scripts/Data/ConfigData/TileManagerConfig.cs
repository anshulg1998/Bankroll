using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileManagerConfig", menuName = "Game/TileManagerConfig")]
public class TileManagerConfig : ScriptableObject
{
    public int boardSize;
    public List<BoardTileData> boardTiles;
}
