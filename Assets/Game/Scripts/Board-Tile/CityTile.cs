using UnityEngine;

public class CityTile : BoardTile
{
    public override void Init(BoardTileData tileData, Transform parent, Vector3 position, int tileIndex = -1, string tileName = "")
    {
        base.Init(tileData, parent, position, tileIndex, tileName);
        this.tileData.TileName = tileData as CityTileData != null ? (tileData as CityTileData).cityName : "City Tile";
    }

    public override void OnPlayerLanded(Player player)
    {
        // Show popup with city info and rent
        int rent = 0;
        var cityData = tileData as CityTileData;
        string cityName = tileData != null ? cityData.cityName : "Unknown City";
        if (cityData != null)
        {
            rent = cityData.rentValue;
        }
        var propertyManager = ServiceLocator.Get<PropertyManager>();
        bool isOwned = propertyManager != null && propertyManager.IsOwned(tileData);
        var ownerData = propertyManager != null ? propertyManager.GetOwner(tileData) : null;
        bool isOwner = ownerData == player.PlayerData;
        if (isOwned && !isOwner)
        {

            player.UpdateMoney(-rent);
            // Find the owner Player instance and pay rent
            var playerManager = ServiceLocator.Get<PlayerManager>();
            if (playerManager != null)
            {
                for (int i = 0; i < playerManager.PlayerCount; i++)
                {
                    var p = playerManager.GetPlayerByIndex(i);
                    if (p != null && p.PlayerData == ownerData)
                    {
                        p.UpdateMoney(rent);
                        break;
                    }
                }
            }

            CityPopup.ShowCityPopup(cityName, rent, cityData.purchasePrice, false, null, OnCancelButtonClicked);
        }
        else if (!isOwned && player.PlayerData.Money >= cityData.purchasePrice)
        {
            // Show buy button if unowned
            CityPopup.ShowCityPopup(cityName, rent, cityData.purchasePrice, true, () =>
            {
                // Raise purchase event
                EventBus.Publish(new PurchaseEvent(tileData, player.PlayerData, cityData.purchasePrice));
            }, OnCancelButtonClicked);
        }
        else
        {
            CityPopup.ShowCityPopup(cityName, rent, cityData.purchasePrice, false, null, OnCancelButtonClicked);
        }

        void OnCancelButtonClicked()
        {
            Debug.Log($"{player.PlayerData.playerName} has cancelled the city popup for {cityName}.");
            EventBus.Publish(new OnPlayerStepCompleteEvent(player));
        }
    }

    
        
}
