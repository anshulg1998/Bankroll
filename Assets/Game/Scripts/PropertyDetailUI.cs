using UnityEngine;
public class PropertyDetailUI : MonoBehaviour
{
    /// <summary>
    /// Displays the property details for a given tile.
    /// </summary>
    /// <param name="tileData">The tile data to display.</param>
    public  virtual void ShowPropertyDetails(BoardTileData tileData)
    {
        // This method should be overridden in derived classes to implement specific UI logic
        Debug.LogWarning("ShowPropertyDetails not implemented in derived class.");  
    }

    /// <summary>
    /// Hides the property detail UI.
    /// </summary>
    public virtual void HidePropertyDetails()
    {
        // This method should be overridden in derived classes to implement specific UI logic
        Debug.LogWarning("HidePropertyDetails not implemented in derived class.");
    }
}