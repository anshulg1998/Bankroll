using UnityEngine;

/// <summary>
/// ScriptableObject config for PlayerManager settings.
/// </summary>
[CreateAssetMenu(fileName = "PlayerManagerConfig", menuName = "Game/PlayerManagerConfig", order = 1)]
public class PlayerManagerConfig : ScriptableObject
{
    public int totalPlayers = 2;
    public int startingMoney = 1500;
    public GameObject playerPrefab;
}
