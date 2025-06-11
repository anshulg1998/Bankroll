using System.Collections.Generic;

/// <summary>
/// Marker interface for all event types used with the EventBus.
/// </summary>
public interface IEvent { }

/// <summary>
/// Event raised when the game starts.
/// </summary>
public class GameStartEvent : IEvent
{
    public GameSessionData SessionData { get; }

    public GameStartEvent(GameSessionData sessionData)
    {
        SessionData = sessionData;
    }
}

public class DiceRolledEvent : IEvent
{
    /// <summary>
    /// The value rolled on the dice.
    /// </summary>
    public int RollValue { get; }
    public DiceRolledEvent(int rollValue) => RollValue = rollValue;
}

/// <summary>
/// Event raised when the current player is set or changed.
/// </summary>
public class OnPlayerSetEvent : IEvent
{
    public int  CurrentPlayerIndex { get; }
    public OnPlayerSetEvent(int index)
    {
        CurrentPlayerIndex = index;
    }
}

/// <summary>
/// Event raised when a player lands on a board tile after moving.
/// </summary>
public class OnPlayerLandEvent : IEvent
{
    public Player Player { get; }
    public OnPlayerLandEvent(Player player)
    {
        Player = player;
    }
}

/// <summary>
/// Event raised when a player's chance/turn ends.
/// </summary>
public class OnChanceEndedEvent : IEvent
{
    public Player Player { get; }
    public OnChanceEndedEvent(Player player)
    {
        Player = player;
    }
 }

/// <summary>
/// Event raised when a property is purchased.
/// </summary>
public class PurchaseEvent : IEvent
{
    public BoardTileData Property { get; }
    public PlayerData Buyer { get; }
    public int Amount { get; }
    public PurchaseEvent(BoardTileData property, PlayerData buyer, int amount)
    {
        Property = property;
        Buyer = buyer;
        Amount = amount;
    }
}

/// <summary>
/// Event raised when a player is ready for their turn.
/// </summary>
public class OnPlayerReadyEvent : IEvent
{
    public Player Player { get; }
    public OnPlayerReadyEvent(Player player)
    {
        Player = player;
    }
}

/// <summary>
/// Event raised when a player is eliminated (money reaches zero).
/// </summary>
public class EliminatePlayerEvent : IEvent
{
    public Player EliminatedPlayer { get; }
    public EliminatePlayerEvent(Player player)
    {
        EliminatedPlayer = player;
    }
}

/// <summary>
/// Event raised when the game ends and a winner is determined.
/// </summary>
public class GameEndEvent : IEvent
{
    public string WinnerName { get; }
    public GameEndEvent(string winnerName)
    {
        WinnerName = winnerName;
    }
}

/// <summary>
/// Event raised when a player completes their step/movement.
/// </summary>
public class OnPlayerStepCompleteEvent : IEvent
{
    /// <summary>
    /// The player who completed their step/movement.
    /// </summary>
    public Player Player { get; }
    public OnPlayerStepCompleteEvent(Player player)
    {
        Player = player;
    }
}


