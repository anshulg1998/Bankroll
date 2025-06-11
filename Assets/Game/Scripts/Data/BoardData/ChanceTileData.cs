using UnityEngine;

[CreateAssetMenu(fileName = "ChanceTileData", menuName = "Game/BoardTileData/ChanceTileData")]
public class ChanceTileData : BoardTileData
{
    [TextArea]
    public string eventDescription;
    // You can add more fields for event logic if needed
}
