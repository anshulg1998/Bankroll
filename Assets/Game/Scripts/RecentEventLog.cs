using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// RecentEventLog listens to important game events and logs them as string messages.
/// Provides a popup UI to display the event log in a scrollable view.
/// </summary>
public class RecentEventLog : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button openLogButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private TextMeshProUGUI logEntryPrefab;

    private readonly List<string> eventMessages = new List<string>();
    private const int maxLogEntries = 50;

    private void Awake()
    {
        ServiceLocator.Register(this);
        if (popupPanel != null)
            popupPanel.SetActive(false);
        if (openLogButton != null)
            openLogButton.onClick.AddListener(ShowLogPopup);
        if (closeButton != null)
            closeButton.onClick.AddListener(HideLogPopup);
    }

    private void OnEnable()
    {
        // Subscribe to important game events
        EventBus.Subscribe<GameStartEvent>(OnGameStart);
        EventBus.Subscribe<GameEndEvent>(OnGameEnd);
        EventBus.Subscribe<OnPlayerReadyEvent>(OnPlayerReady);
        EventBus.Subscribe<DiceRolledEvent>(OnDiceRolled);
        EventBus.Subscribe<OnPlayerLandEvent>(OnPlayerLand);
        EventBus.Subscribe<PurchaseEvent>(OnPurchase);
        EventBus.Subscribe<EliminatePlayerEvent>(OnPlayerEliminated);
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        EventBus.Unsubscribe<GameStartEvent>(OnGameStart);
        EventBus.Unsubscribe<GameEndEvent>(OnGameEnd);
        EventBus.Unsubscribe<OnPlayerReadyEvent>(OnPlayerReady);
        EventBus.Unsubscribe<DiceRolledEvent>(OnDiceRolled);
        EventBus.Unsubscribe<OnPlayerLandEvent>(OnPlayerLand);
        EventBus.Unsubscribe<PurchaseEvent>(OnPurchase);
        EventBus.Unsubscribe<EliminatePlayerEvent>(OnPlayerEliminated);
    }

    private void OnGameStart(GameStartEvent evt)
    {
        AddLog("Game started.");
    }
    private void OnGameEnd(GameEndEvent evt)
    {
        AddLog($"Game ended. Winner: {evt.WinnerName}");
    }
    private void OnPlayerReady(OnPlayerReadyEvent evt)
    {
        AddLog($"Player ready: {evt.Player.PlayerData.playerName}");
    }
    private void OnDiceRolled(DiceRolledEvent evt)
    {
        AddLog($"Dice rolled: {evt.RollValue}");
    }
    private void OnPlayerLand(OnPlayerLandEvent evt)
    {
        AddLog($"Player landed: {evt.Player.PlayerData.playerName} on tile {evt.Player.PlayerData.position}");
    }
    private void OnPurchase(PurchaseEvent evt)
    {
        AddLog($"{evt.Buyer.playerName} purchased {evt.Property?.TileName ?? "property"} for {evt.Amount}");
    }
    private void OnPlayerEliminated(EliminatePlayerEvent evt)
    {
        AddLog($"Player eliminated: {evt.EliminatedPlayer.PlayerData.playerName}");
    }

    /// <summary>
    /// Adds a message to the event log and trims if necessary.
    /// </summary>
    private void AddLog(string message)
    {
        if (eventMessages.Count >= maxLogEntries)
            eventMessages.RemoveAt(0);
        eventMessages.Add($"[{System.DateTime.Now:HH:mm:ss}] {message}");
    }

    /// <summary>
    /// Shows the event log popup and populates the scroll view.
    /// </summary>
    public void ShowLogPopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(true);
        if (scrollViewContent != null && logEntryPrefab != null)
        {
            foreach (Transform child in scrollViewContent)
                Destroy(child.gameObject);
            foreach (var msg in eventMessages)
            {
                var entry = Instantiate(logEntryPrefab, scrollViewContent);
                entry.gameObject.SetActive(true);
                entry.text = msg;
            }
        }
    }

    /// <summary>
    /// Hides the event log popup.
    /// </summary>
    public void HideLogPopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}
