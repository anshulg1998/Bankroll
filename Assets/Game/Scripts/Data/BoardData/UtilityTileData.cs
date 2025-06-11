using UnityEngine;

[CreateAssetMenu(fileName = "UtilityTileData", menuName = "Game/BoardTileData/UtilityTileData")]
public class UtilityTileData : BoardTileData
{
    public enum UtilityType { Electricity }
    public UtilityType utilityType;
    public int purchasePrice;
    public int rentValue;
}
