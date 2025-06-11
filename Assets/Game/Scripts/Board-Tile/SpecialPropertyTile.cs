using UnityEngine;

public class SpecialPropertyTile : BoardTile
{
    public override void OnPlayerLanded(Player player)
    {
        //additional mechanic logic
      EventBus.Publish(new OnPlayerStepCompleteEvent(player));
    }
}
