using UnityEngine;

/// <summary>
/// Manages turn order and tracks the current player index.
/// Listens for GameStartEvent to initialize turn state.
/// </summary>
[DefaultExecutionOrder(0)]

public class TurnManager : MonoBehaviour
{
    public int CurrentPlayerIndex { get; private set; } = 0;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GameStartEvent>(OnGameStart);
        EventBus.Subscribe<OnChanceEndedEvent>(OnChanceEnded);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameStartEvent>(OnGameStart);
        EventBus.Unsubscribe<OnChanceEndedEvent>(OnChanceEnded);
    }

    private void OnGameStart(GameStartEvent evt)
    {
        Debug.Log("TurnManager: Game started, initializing turn state.");
        SetCurrentPlayerIndex(0); // Start with the first player
        // Optionally, raise a TurnChangedEvent here
    }

    private void SetCurrentPlayerIndex(int index)
    {
        CurrentPlayerIndex = index;
        Debug.Log($"TurnManager: Current player index set to {CurrentPlayerIndex}");
        EventBus.Publish(new OnPlayerSetEvent(index));
        // Optionally, raise a TurnChangedEvent here
    }

    /// <summary>
    /// Handles the event when a player's chance/turn ends and advances to the next player.
    /// </summary>
    /// <param name="evt">The OnChanceEndedEvent instance.</param>
    private void OnChanceEnded(OnChanceEndedEvent evt)
    {
        NextPlayer();
    }

    /// <summary>
    /// Advances to the next player in the turn order.
    /// </summary>
    public void NextPlayer()
    {
        // Assuming 2+ players, wrap around to 0
        int playerCount = ServiceLocator.Get<PlayerManager>()?.PlayerCount ?? 0;
        if (playerCount <= 0) return;
        int nextIndex = (CurrentPlayerIndex + 1) % playerCount;
        SetCurrentPlayerIndex(nextIndex);
    }
}
