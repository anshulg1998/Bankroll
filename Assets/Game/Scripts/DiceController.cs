using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DiceController manages dice rolling, animation, and event-driven dice logic.
/// </summary>
public class DiceController : MonoBehaviour
{
    private IDiceRollStrategy diceStrategy = null;
    [SerializeField] private DiceManagerConfig diceManagerConfig;
    [SerializeField] private Transform diceParent;
    [SerializeField] Button rollButton;
    private Dice[] diceArray;

    
#if UNITY_EDITOR
    [ContextMenuItem("roll", nameof(RollDiceCustomOutcome))]
    public int roll;
    public void RollDiceCustomOutcome()
    {
        RollDice(roll); // Example: always rolls 3 in editor context menu
    }
#endif

    private void Awake()
    {
        // Register this DiceController instance with the ServiceLocator for global access
        ServiceLocator.Register(this);
    }

    public void RollDice()
    {
        // Rolls the dice using the current dice strategy and animates the result
        int roll = diceStrategy.Roll();
        rolledEvent = diceArray.Length;
        foreach (var dice in diceArray)
        {
            if (dice != null)
            {
                dice.ShowRoll(roll, () => RollAnimationCallback(roll));
            }
            else
            {
                RollAnimationCallback(roll);
            }
        }
    }
     public void RollDice(int outcome)
    {
        // Rolls the dice with a custom outcome (useful for testing)
        rolledEvent = diceArray.Length;
        foreach (var dice in diceArray)
        {
            if (dice != null)
            {
                dice.ShowRoll(outcome, () => RollAnimationCallback(outcome));
            }
            else
            {
                RollAnimationCallback(outcome);
            }
        }
    }

    private int rolledEvent = 0;

    private void RollAnimationCallback(int roll)
    {
        // Called after each dice finishes its roll animation
        rolledEvent--;
        Debug.Log($"Dice rolled: {roll}");
        if (rolledEvent == 0)
        {
            // All dice have finished animating, publish the result event
            EventBus.Publish(new DiceRolledEvent(roll));
        }
    }


    public void Initialise(IDiceRollStrategy strategy)
    {
        // Sets the dice rolling strategy and hooks up the roll button
        diceStrategy = strategy;
        rollButton.onClick.AddListener(RollDice);
    }

    private void OnEnable()
    {
        // Subscribe to relevant game events
        EventBus.Subscribe<GameStartEvent>(OnGameStarted);
        EventBus.Subscribe<OnPlayerReadyEvent>(OnPlayerReady);
        EventBus.Subscribe<DiceRolledEvent>(OnDiceRolled);
    }

    private void OnDisable()
    {
        // Unsubscribe from game events
        EventBus.Unsubscribe<GameStartEvent>(OnGameStarted);
        EventBus.Unsubscribe<OnPlayerReadyEvent>(OnPlayerReady);
        EventBus.Unsubscribe<DiceRolledEvent>(OnDiceRolled);
    }

    private void OnDiceRolled(DiceRolledEvent evt)
    {
        // Disable dice button after rolling
        DiceButtonInteraction(false);
    }

    private void OnGameStarted(GameStartEvent evt)
    {
        // Initialize dice controller and spawn dice at game start
        Initialise(evt.SessionData.DiceStrategy);
        SpawnDice();
        rollButton.gameObject.SetActive(true);
    }

    private void OnPlayerReady(OnPlayerReadyEvent evt)
    {
        // Enable dice for the current player's turn
        Debug.Log($"Player {evt.Player.PlayerData.playerName} is ready. Preparing dice.");
        GetDiceReady();
    }

    private void GetDiceReady()
    {
        // Enable dice button for interaction
        DiceButtonInteraction(true);
        // Add further logic as needed
    }

    public void DiceButtonInteraction(bool enable)
    {
        // Enable or disable the roll button
        Debug.Log($"Dice button interaction set to: {enable}");
        if (rollButton != null)
            rollButton.interactable = enable;
    }

    private void SpawnDice()
    {
        // Instantiates dice prefabs and sets up the dice array
        if (diceManagerConfig == null || diceManagerConfig.dicePrefab == null)
        {
            Debug.LogError("DiceManagerConfig or dicePrefab is not assigned.");
            return;
        }
        // Clean up old dice
        if (diceArray != null)
        {
            foreach (var d in diceArray)
                if (d != null) Destroy(d.gameObject);
        }
        diceArray = new Dice[diceManagerConfig.diceCount];
        for (int i = 0; i < diceManagerConfig.diceCount; i++)
        {
            var diceObj = Instantiate(diceManagerConfig.dicePrefab, diceParent != null ? diceParent : this.transform);
            diceArray[i] = diceObj.GetComponent<Dice>();
            if (diceArray[i] == null)
                Debug.LogError("Dice prefab missing Dice component!");
        }
    }
}

