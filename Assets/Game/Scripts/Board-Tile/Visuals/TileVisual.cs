using UnityEngine;

/// <summary>
/// Base class for all tile visuals. Attach a derived visual script to the tile prefab and reference it from BoardTile.
/// </summary>
public class TileVisual : MonoBehaviour
{
    [SerializeField] protected UnityEngine.UI.Button displayDetailButton;
    [SerializeField] protected PropertyDetailUI propertyDetalUI;
    /// <summary>
    /// Called to update the visual based on the tile data.
    /// Override in derived classes for custom visuals.
    /// </summary>
    public virtual void Setup(BoardTileData tileData)
    {
        // Default: do nothing. Derived classes implement their own visuals.
    }
}
