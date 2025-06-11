using System.Collections.Generic;
using UnityEngine;

public class PropertyManager : MonoBehaviour
{
    private Dictionary<BoardTileData, PlayerData> propertyOwners = new Dictionary<BoardTileData, PlayerData>();

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnEnable()
    {
        ServiceLocator.Register(this);
        EventBus.Subscribe<PurchaseEvent>(OnPurchaseEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PurchaseEvent>(OnPurchaseEvent);
    }

    private void OnPurchaseEvent(PurchaseEvent evt)
    {
        if (evt.Property != null && evt.Buyer != null)
        {
            SetOwner(evt.Property, evt.Buyer);
        }
    }

    public void SetOwner(BoardTileData property, PlayerData owner)
    {
        if (property == null) return;
        propertyOwners[property] = owner;
        
    }

    public PlayerData GetOwner(BoardTileData property)
    {
        if (property == null) return null;
        propertyOwners.TryGetValue(property, out var owner);
        return owner;
    }

    public bool IsOwned(BoardTileData property)
    {
        return GetOwner(property) != null;
    }
    public void ClearAllProperties()
    {
        propertyOwners.Clear();
    }
}
