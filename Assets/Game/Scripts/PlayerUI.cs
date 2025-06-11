using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image turnImage;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerMoneyText;
    [SerializeField] private TMP_Dropdown propertyDropdown;
    private PlayerData playerData;

    public void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        this.playerData.OnAmountUpdated += UpdateMoneyUI;

        UpdateUI();
    }

    void OnEnable()
    {
        EventBus.Subscribe<OnPlayerReadyEvent>(SetTurn);
        EventBus.Subscribe<OnChanceEndedEvent>(SetUnTurn);
        EventBus.Subscribe<PurchaseEvent>(AddPropery);
    }

   

    void OnDisable()
    {
        playerData.OnAmountUpdated -= UpdateMoneyUI;
        EventBus.Unsubscribe<OnPlayerReadyEvent>(SetTurn);
        EventBus.Unsubscribe<OnChanceEndedEvent>(SetUnTurn);
        EventBus.Unsubscribe<PurchaseEvent>(AddPropery);


    }
    private void AddPropery(PurchaseEvent @event)
    {
        if (@event.Buyer == playerData)
        {
            Debug.Log($" update property in player ui");
           propertyDropdown.AddOptions(new List<string> { @event.Property.TileName });
        }
    }
    
    private void SetUnTurn(OnChanceEndedEvent @event)
    {
        if (turnImage == null)
        {
            Debug.LogWarning("TurnImage is not assigned in the inspector.");
            return;
        }
        turnImage.color = Color.black;
    }

    private void SetTurn(OnPlayerReadyEvent @event)
    {
        if(@event.Player == null || @event.Player.PlayerData != playerData)
        {
            return; // Ignore events from other players
        }
        if(@event.Player.PlayerData == playerData)
        {
             if (turnImage == null)
            {
                Debug.LogWarning("TurnImage is not assigned in the inspector.");
                return;
            }
            turnImage.color = Color.green;
        }
       
    }

    private void UpdateMoneyUI(int obj)
    {
        UpdateUI();
    }

   
    public void UpdateUI()
    {
        if (playerData == null)
        {
            Debug.LogWarning("PlayerData is not initialized.");
            return;
        }

        if (playerNameText != null)
        {
            playerNameText.text = playerData.playerName;
        }
        else
        {
            Debug.LogWarning("PlayerNameText is not assigned in the inspector.");
        }

        if (playerMoneyText != null)
        {
            playerMoneyText.text = $"${playerData.Money}";
        }
        else
        {
            Debug.LogWarning("PlayerMoneyText is not assigned in the inspector.");
        }
    }
}
