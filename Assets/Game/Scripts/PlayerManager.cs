using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-50)]
/// <summary>
/// PlayerManager listens for GameStartEvent and creates players when the game starts.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerManagerConfig playerConfig;
    [SerializeField] private Transform playerParent;
    private List<Player> players;
    private Player currentPlayer;
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GameStartEvent>(OnGameStarted);
        EventBus.Subscribe<OnPlayerSetEvent>(OnPlayerSet);
        EventBus.Subscribe<DiceRolledEvent>(OnDiceRolled);
        EventBus.Subscribe<EliminatePlayerEvent>(OnEliminatePlayer);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameStartEvent>(OnGameStarted);
        EventBus.Unsubscribe<OnPlayerSetEvent>(OnPlayerSet);
        EventBus.Unsubscribe<DiceRolledEvent>(OnDiceRolled);
        EventBus.Unsubscribe<EliminatePlayerEvent>(OnEliminatePlayer);
    }

    private void OnGameStarted(GameStartEvent evt)
    {
        CreatePlayers();
    }

    private void OnPlayerSet(OnPlayerSetEvent evt)
    {
        if (evt.CurrentPlayerIndex < 0 || evt.CurrentPlayerIndex >= players.Count)
        {
            Debug.LogWarning($"Invalid player index: {evt.CurrentPlayerIndex}. Cannot set current player.");
            return;
        }
        currentPlayer = GetPlayerByIndex(evt.CurrentPlayerIndex);
        currentPlayer.SetLastRoll(0);
        currentPlayer?.CallStrategy();
    }

    private void OnDiceRolled(DiceRolledEvent evt)
    {
        var boardManager = ServiceLocator.Get<BoardManager>();
        if (boardManager != null)
        {
            if (currentPlayer != null)
            {
                int newPosition = currentPlayer.PlayerData.position + evt.RollValue;
                int boardSize = boardManager.BoardSize;
                newPosition = newPosition % boardSize;
                currentPlayer.PlayerData.position = newPosition;
                currentPlayer.SetPawnPosition(boardManager.GetTilePosition(newPosition), () => 
                {
                    // After moving, check if the player landed on a property
                    currentPlayer.SetLastRoll(evt.RollValue);
                    EventBus.Publish(new OnPlayerLandEvent(currentPlayer));
                });
                // Raise OnPlayerLandEvent after moving
            }
        }
    }

    private void OnEliminatePlayer(EliminatePlayerEvent evt)
    {
        if (evt.EliminatedPlayer != null && players != null && players.Contains(evt.EliminatedPlayer))
        {
            players.Remove(evt.EliminatedPlayer);
            Destroy(evt.EliminatedPlayer.gameObject);
            Debug.Log($"Player eliminated: {evt.EliminatedPlayer.PlayerData.playerName}");
            if (players.Count == 1)
            {
                var winner = players[0];
                EventBus.Publish(new GameEndEvent(winner.PlayerData.playerName));
            }
        }
    }

    /// <summary>
    /// Clears existing player objects.
    /// </summary>
    public void ClearPlayers()
    {
        if (players == null) return;
        foreach (var player in players)
        {
            if (player != null)
                Destroy(player.gameObject);
        }
        players.Clear();
    }

    /// <summary>
    /// Creates player objects and initializes them for the game.
    /// </summary>
    public void CreatePlayers()
    {
        if (playerConfig == null || playerConfig.totalPlayers <= 0)
        {
            Debug.LogWarning("PlayerManagerConfig is missing or invalid.");
            return;
        }
        players = new List<Player>();
        var uis = FindObjectsOfType<PlayerUI>();
        for (int i = 0; i < playerConfig.totalPlayers; i++)
        {
            GameObject playerObj = null;
            if (playerConfig.playerPrefab != null)
            {
                playerObj = Instantiate(playerConfig.playerPrefab);
                playerObj.name = $"Player_{i + 1}";
            }
            else
            {
                playerObj = new GameObject($"Player_{i + 1}");
            }
            var player = playerObj.GetComponent<Player>() ?? playerObj.AddComponent<Player>();
            Color color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f);
            player.Initialise($"Player{i + 1}", playerConfig.startingMoney, color, 0);
            player.transform.SetParent(playerParent != null ? playerParent : this.transform);
            players.Add(player);
            uis[i].Initialize(player.PlayerData);
            player.SetPawnPosition(FindObjectOfType<BoardManager>().GetTilePosition(0), null); // Set initial position, can be adjusted later
        }
    }

    public Player GetPlayerByIndex(int index)
    {
        if (players == null || index < 0 || index >= players.Count)
            return null;
        return players[index];
    }

    public int PlayerCount => players != null ? players.Count : 0;
}
