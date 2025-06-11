using UnityEngine;

/// <summary>
/// Base class for all board tiles.
/// </summary>
public class BoardTile : MonoBehaviour
{
    public BoardTileData tileData;
    public TileVisual tileVisual;
    public virtual void OnPlayerLanded(Player player) { }

    public virtual void Init(BoardTileData tileData, Transform parent, Vector3 position, int tileIndex = -1, string tileName = "")
    {
        this.tileData = tileData;
        this.transform.SetParent(parent);
        this.transform.position = position;
        tileVisual = GetComponent<TileVisual>();
        if (tileVisual != null)
            tileVisual.Setup(tileData);
        // Optionally set index and name if provided
        // (Override in derived classes for more specific logic)
    }
}
