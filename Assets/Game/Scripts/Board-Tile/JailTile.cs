using UnityEngine;

public class JailTile : BoardTile
{
    public override void OnPlayerLanded(Player player)
    {
        Debug.Log($"{player.PlayerData.playerName} has landed on Jail Tile.");
        player.SetStrategy(new JailPlayerStrategy(3)); // Example: skip 3 turns
        EventBus.Publish(new OnChanceEndedEvent());
    }
}
