using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPopup : MonoBehaviour
{
    public static CityPopup Instance;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMPro.TextMeshProUGUI cityNameText;
    [SerializeField] private TMPro.TextMeshProUGUI rentText;
    [SerializeField] private TMPro.TextMeshProUGUI buyAmountText;
    [SerializeField] private UnityEngine.UI.Button buyButton;
    [SerializeField] private UnityEngine.UI.Button closeButton;
    private void Awake()
    {
        Instance = this;
        if (popupPanel != null)
            popupPanel.SetActive(false);
            
        if (closeButton != null)
            closeButton.onClick.AddListener(Hide);
    }

    private System.Action oncancelAction;
    public static void ShowCityPopup(string cityName, int rent, int buyAmount, bool showBuyButton, System.Action onBuy, System.Action onCancel)
    {
        if (Instance == null) return;
        Instance.popupPanel.SetActive(true);
        Instance.cityNameText.text = cityName;
        Instance.rentText.text = $"Rent: {rent}";
        Instance.buyAmountText.text = $"Buy: {buyAmount}";
        Instance.buyButton.gameObject.SetActive(showBuyButton);
        Instance.buyButton.onClick.RemoveAllListeners();
        Instance.oncancelAction = onCancel;
        if (showBuyButton && onBuy != null)
        {
            Instance.buyButton.onClick.AddListener(() =>
            {
                onBuy.Invoke();
                Instance.Hide();
            });
        }
    }

    public void Hide()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);

        }
        if(oncancelAction != null)
        {
            oncancelAction.Invoke();
            oncancelAction = null;
        }
    }
}
