using UnityEngine;

public class UtilityTile : BoardTile
{
    public override void OnPlayerLanded(Player player)
    {
        //additional mechanic logic
         EventBus.Publish(new OnPlayerStepCompleteEvent(player));
    }
}
