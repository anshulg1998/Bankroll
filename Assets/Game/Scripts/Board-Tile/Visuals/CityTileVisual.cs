using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Visual component for city tiles. Displays city name, price, and background color.
/// </summary>
public class CityTileVisual : TileVisual
{
    public TextMeshProUGUI cityNameText;
    public TextMeshProUGUI priceText;
    public Image backgroundImage;

    public override void Setup(BoardTileData tileData)
    {
        base.Setup(tileData);
        // Add listener to display property details popup when button is clicked
        displayDetailButton.onClick.AddListener(() => ShowPropertyPopup(tileData));
        if (tileData is CityTileData cityData)
        {
            if (cityNameText != null)
                cityNameText.text = cityData.cityName;
            if (priceText != null)
                priceText.text = cityData.purchasePrice.ToString();
            // Optionally set background color based on cityData (add a color field to CityTileData if needed)
        }
    }

    /// <summary>
    /// Shows a UI popup with the details of the property.
    /// </summary>
    /// <param name="tileData">The tile data to display in the popup.</param>
    private void ShowPropertyPopup(BoardTileData tileData)
    {
       propertyDetalUI.ShowPropertyDetails(tileData);
    }
}
