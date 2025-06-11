using UnityEngine;

public class StartTile : BoardTile
{
     public override void OnPlayerLanded(Player player)
    {
         EventBus.Publish(new OnPlayerStepCompleteEvent(player));
    }
}
