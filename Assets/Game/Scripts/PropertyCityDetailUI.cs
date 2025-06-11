using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// PropertyDetailUI displays property details in a popup UI. Call Show to display and Hide to close.
/// </summary>
public class PropertyCityDetailUI : PropertyDetailUI
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI propertyNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI rentText;
    [SerializeField] private TextMeshProUGUI propertyOwnerText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
        if (closeButton != null)
            closeButton.onClick.AddListener(HidePropertyDetails);
    }

    /// <summary>
    /// Shows the property detail popup with the given data.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="propertyType">Type of the property (e.g., City, Utility).</param>
    /// <param name="price">Purchase price.</param>
    /// <param name="rent">Rent value.</param>
    public  override void ShowPropertyDetails(BoardTileData data)
    {
        popupPanel.SetActive(true);
        CityTileData cityData = data as CityTileData;
        if (propertyNameText != null)
            propertyNameText.text = $"propertyName: {cityData.cityName}";

        if (priceText != null)
            priceText.text = $"Price: {cityData.purchasePrice}";
        if (rentText != null)
            rentText.text = $"Rent: {cityData.rentValue}";

        if (propertyOwnerText != null)
            propertyOwnerText.text = $"Owner: {ServiceLocator.Get<PropertyManager>()?.GetOwner(data)?.playerName  ?? "None"}";
    }

    /// <summary>
    /// Hides the property detail popup.
    /// </summary>
    public  override void HidePropertyDetails()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}
