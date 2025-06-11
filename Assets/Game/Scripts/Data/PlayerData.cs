using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject data container for player data in the Monopoly-style game.
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public event System.Action<int> OnAmountUpdated;

    public string playerName;
    private int _money;
    public int Money
    {
        get => _money;
        set
        {
            if (_money != value)
            {
                _money = value;
                OnAmountUpdated?.Invoke(_money);
            }
        }
    }
    public int position;
   
   // public List<BoardTile> ownedProperties = new List<BoardTile>();
}
