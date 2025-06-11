using UnityEngine;
using UnityEditor;
using System.IO;

public class CityTileDataImporter : MonoBehaviour
{
    [ContextMenu("Import CityTileData From File")]
    public void ImportCityTileDataFromFile()
    {
        string filePath = "Assets/Resources/CityTileDataList.txt";
        string assetFolder = "Assets/Game/ScriptableObjects/CityTiles/";
        if (!Directory.Exists(assetFolder))
            Directory.CreateDirectory(assetFolder);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"CityTileDataList.txt not found at {filePath}");
            return;
        }
        var lines = File.ReadAllLines(filePath);
        int created = 0;
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//") || line.StartsWith("Format"))
                continue;
            var parts = line.Split(',');
            if (parts.Length < 3) continue;
            string cityName = parts[0].Trim();
            int purchasePrice = int.Parse(parts[1].Trim());
            int rentValue = int.Parse(parts[2].Trim());

            var asset = ScriptableObject.CreateInstance<CityTileData>();
            asset.cityName = cityName;
            asset.purchasePrice = purchasePrice;
            asset.rentValue = rentValue;
            AssetDatabase.CreateAsset(asset, $"{assetFolder}{cityName}_TileData.asset");
            created++;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Created {created} CityTileData assets in {assetFolder}");
    }
}
