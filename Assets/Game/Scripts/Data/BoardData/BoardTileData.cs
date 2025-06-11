using UnityEngine;

[CreateAssetMenu(fileName = "BoardTileData", menuName = "Game/BoardTileData")]
public class BoardTileData : ScriptableObject
{
    public string TileName;
    public int TileIndex;
    public GameObject TilePrefab;
    // Add more tile-specific data as needed (e.g., type, cost, etc.)
}
