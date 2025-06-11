using UnityEngine;

[CreateAssetMenu(fileName = "CityTileData", menuName = "Game/BoardTileData/CityTileData")]
public class CityTileData : BoardTileData
{
    public int purchasePrice;
    public int rentValue;
    public string cityName;
}
