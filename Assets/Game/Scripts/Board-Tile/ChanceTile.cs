using UnityEngine;

public class ChanceTile : BoardTile
{
    public override void OnPlayerLanded(Player player)
    {
        float chance = UnityEngine.Random.value;
        int money = player.PlayerData.Money;
        if (chance <= 0.1f)
        {
            int reward = Mathf.CeilToInt(money * 0.5f);
            player.UpdateMoney(reward);
            // If BonusAwardedEvent is not defined, use OnChanceEndedEvent or similar
            EventBus.Publish(new OnPlayerStepCompleteEvent(player));
        }
        else
        {
            int penalty = Mathf.CeilToInt(money * UnityEngine.Random.Range(0.1f, 0.3f));
            player.UpdateMoney(-penalty);
            // If PenaltyEvent is not defined, use OnChanceEndedEvent or similar
             EventBus.Publish(new OnPlayerStepCompleteEvent(player));
        }
    }
}
