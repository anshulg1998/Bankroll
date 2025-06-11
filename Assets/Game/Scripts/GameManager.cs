using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton GameManager controls the main game flow, turn management, and event broadcasting.
/// </summary>
public class GameManager : GenericSingleton<GameManager>
{
    /// <summary>
    /// Initializes or resets the game session, preparing all necessary components for a new game.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Game started!");
        EventBus.Publish(new GameStartEvent(PrepareGameSession()));
        // Add further initialization logic as needed (e.g., reset managers, set up turn order, etc.)
    }

    /// <summary>
    /// Prepares the game session data, including player count, names, dice strategy, and board configuration.
    /// </summary>
    /// <returns>GameSessionData containing session configuration.</returns>
    private GameSessionData PrepareGameSession()
    {
        int numPlayers = 2; // Default, or fetch from config/UI
        var playerNames = new List<string> { "Player1", "Player2" };
        TileManagerConfig boardData = null; // Assign from your config or BoardManager
        return new GameSessionData(numPlayers, playerNames, new StandardDiceRoll(), boardData);
    }

    /// <summary>
    /// Subscribes to game end event when enabled.
    /// </summary>
    private void OnEnable()
    {
        EventBus.Subscribe<GameEndEvent>(OnGameEnd);
    }

    /// <summary>
    /// Unsubscribes from game end event when disabled.
    /// </summary>
    private void OnDisable()
    {
        EventBus.Unsubscribe<GameEndEvent>(OnGameEnd);
    }

    /// <summary>
    /// Handles the end of the game, displaying the winner.
    /// </summary>
    /// <param name="evt">The GameEndEvent containing winner information.</param>
    private void OnGameEnd(GameEndEvent evt)
    {
        Debug.Log($"Game Over! Winner: {evt.WinnerName}");
        // Optionally, show a UI popup or handle end-of-game logic here
    }
}

/// <summary>
/// Struct containing configuration data for a game session.
/// </summary>
public struct GameSessionData
{
    /// <summary>
    /// Number of players in the session.
    /// </summary>
    public int NumberOfPlayers;
    /// <summary>
    /// List of player names.
    /// </summary>
    public List<string> PlayerNames;
    /// <summary>
    /// Dice roll strategy for the session.
    /// </summary>
    public IDiceRollStrategy DiceStrategy; // e.g., "SingleRoll", "DoubleRoll"
    /// <summary>
    /// Board configuration data.
    /// </summary>
    public TileManagerConfig BoardData;

    /// <summary>
    /// Constructs a new GameSessionData struct.
    /// </summary>
    /// <param name="numberOfPlayers">Number of players.</param>
    /// <param name="playerNames">List of player names.</param>
    /// <param name="diceStrategy">Dice roll strategy.</param>
    /// <param name="boardData">Board configuration data.</param>
    public GameSessionData(int numberOfPlayers, List<string> playerNames, IDiceRollStrategy diceStrategy, TileManagerConfig boardData)
    {
        NumberOfPlayers = numberOfPlayers;
        PlayerNames = new List<string>(playerNames);
        DiceStrategy = diceStrategy;
        BoardData = boardData;
    }
}