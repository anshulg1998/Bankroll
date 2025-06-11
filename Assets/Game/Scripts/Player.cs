using UnityEngine;
using DG.Tweening;
using System;
/// <summary>
/// Represents a player in the Monopoly-style board game. Handles player data, movement, strategy, and event-driven actions.
/// </summary>
public class Player : MonoBehaviour
{
    private PlayerData playerData;
    /// <summary>
    /// Gets the PlayerData associated with this player.
    /// </summary>
    public PlayerData PlayerData => playerData;

    [SerializeField] private Transform pawnTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private IPlayerStrategy currentStrategy;
    private int lastRoll = 0;

    /// <summary>
    /// Initializes the player with the specified parameters.
    /// </summary>
    /// <param name="playerName">The name of the player.</param>
    /// <param name="startingMoney">The starting money for the player.</param>
    /// <param name="color">The color of the player pawn.</param>
    /// <param name="position">The starting board position.</param>
    public void Initialise(string playerName, int startingMoney, Color color, int position)
    {
        playerData = ScriptableObject.CreateInstance<PlayerData>();
        SetStrategy(new DefaultPlayerStrategy());
        playerData.playerName = playerName;
        playerData.Money = startingMoney;
        playerData.position = position;
        if (spriteRenderer != null) spriteRenderer.color = color;
        if (playerData != null)
            playerData.OnAmountUpdated += OnAmountUpdated;
    }

    /// <summary>
    /// Moves the player's pawn to the specified position with animation.
    /// </summary>
    /// <param name="position">The target position.</param>
    /// <param name="callback">Callback to invoke after movement completes.</param>
    public void SetPawnPosition(Vector3 position, Action callback)
    {
        if (pawnTransform != null)
        {
            pawnTransform.DOMove(position + Vector3.up * 0.3f, .05f).OnComplete(() => callback?.Invoke());
        }
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PurchaseEvent>(OnPurchaseEvent);
        EventBus.Subscribe<OnPlayerStepCompleteEvent>(OnPlayerStepCompleted);
        
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PurchaseEvent>(OnPurchaseEvent);
        EventBus.Unsubscribe<OnPlayerStepCompleteEvent>(OnPlayerStepCompleted);
        if (playerData != null)
            playerData.OnAmountUpdated -= OnAmountUpdated;
    }

    /// <summary>
    /// Sets the current strategy for the player.
    /// </summary>
    /// <param name="strategy">The strategy to set.</param>
    public void SetStrategy(IPlayerStrategy strategy)
    {
        currentStrategy = strategy;
    }

    /// <summary>
    /// Executes the current player strategy.
    /// </summary>
    public void CallStrategy()
    {
        Debug.Log($"Executing strategy for player {playerData.playerName}: {currentStrategy?.GetType().Name}");
        currentStrategy?.Execute(this);
    }

    /// <summary>
    /// Sets the value of the last dice roll for this player.
    /// </summary>
    /// <param name="roll">The value of the last roll.</param>
    public void SetLastRoll(int roll)
    {
        lastRoll = roll;
    }

    private void OnPlayerStepCompleted(OnPlayerStepCompleteEvent @event)
    {
        if (@event.Player != this) return;
        if (lastRoll == 6)
        {
            EventBus.Publish(new OnPlayerReadyEvent(this));
        }
        else
        {
            EventBus.Publish(new OnChanceEndedEvent(this));
        }
    }

    private void OnPurchaseEvent(PurchaseEvent evt)
    {
        if (playerData != null && evt.Buyer == playerData)
        {
            playerData.Money = (int)Mathf.Clamp(playerData.Money - evt.Amount, 0, Mathf.Infinity);
        }
    }

    /// <summary>
    /// Updates the player's money by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to add or subtract.</param>
    public void UpdateMoney(int amount)
    {
        if (playerData != null)
        {
            playerData.Money = (int)Mathf.Clamp(playerData.Money + amount, 0, Mathf.Infinity);
        }
    }

    private void OnAmountUpdated(int newAmount)
    {
        if (newAmount == 0)
        {
            // Raise EliminatePlayerEvent when player is bankrupt
            EventBus.Publish(new EliminatePlayerEvent(this));
        }
        // Handle money update (e.g., update UI)
        Debug.Log($"Player {playerData.playerName} money updated: {newAmount}");
    }
}

/// <summary>
/// Interface for player strategy pattern.
/// </summary>
public interface IPlayerStrategy
{
    /// <summary>
    /// Executes the strategy for the given player.
    /// </summary>
    /// <param name="player">The player to execute the strategy for.</param>
    void Execute(Player player);
}

/// <summary>
/// Default player strategy for normal turns.
/// </summary>
public class DefaultPlayerStrategy : IPlayerStrategy
{
    public void Execute(Player player)
    {
        EventBus.Publish(new OnPlayerReadyEvent(player));
    }
}

/// <summary>
/// Jail player strategy for handling jail turns.
/// </summary>
public class JailPlayerStrategy : IPlayerStrategy
{
    private int jailSkipTurnCount = 3;
    /// <summary>
    /// Initializes a new instance of the JailPlayerStrategy class.
    /// </summary>
    /// <param name="skipTurns">The number of turns to skip while in jail.</param>
    public JailPlayerStrategy(int skipTurns)
    {
        jailSkipTurnCount = skipTurns;
    }
    public void Execute(Player player)
    {
        jailSkipTurnCount--;
        if (jailSkipTurnCount > 0)
        {
            EventBus.Publish(new OnChanceEndedEvent(player));
        }
        else
        {
            // Out of jail, switch to default strategy
            player.SetStrategy(new DefaultPlayerStrategy());
            player.CallStrategy();
        }
    }
}
