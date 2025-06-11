using UnityEngine;

[CreateAssetMenu(fileName = "SpecialPropertyTileData", menuName = "Game/BoardTileData/SpecialPropertyTileData")]
public class SpecialPropertyTileData : BoardTileData
{
    public enum SpecialType { Railway, Airport, Harbor }
    public SpecialType propertyType;
    public int purchasePrice;
    public int rentValue;
    // Add unique mechanics fields as needed
}
