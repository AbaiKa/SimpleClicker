using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Button))]
public class BoosterAssistant : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// Настройки улучшения
    /// </summary>
    public ProfitBoosterSettings Settings { get; set; }
    /// <summary>
    /// True если улучшение уже имееться 
    /// </summary>
    public bool Available { get; private set; }
    /// <summary>
    /// Кнопка купить
    /// </summary>
    private Button button;
    /// <summary>
    /// Текст на кнопке
    /// </summary>
    private TextMeshProUGUI txtOnBtn;
    public delegate void OnBuy();
    /// <summary>
    /// Событие при покупке улучшения
    /// </summary>
    public OnBuy OnBuyBooster;
    #endregion

    #region Methods
    /// <summary>
    /// Инициализация бустера
    /// </summary>
    public void Init()
    {
        button = GetComponent<Button>();
        txtOnBtn = button.GetComponentInChildren<TextMeshProUGUI>();
        Available = PlayerPrefs.GetInt(Settings.Key, -1) == -1 ? false : true;
        button.onClick.AddListener(BuyBooster);
        UpdateText();
    }
    /// <summary>
    /// Купить улучшение
    /// </summary>
    private void BuyBooster()
    {
        // Мы уже купили улучшение
        if (Available) return;
        if(Player.Instance.CanBuy(Settings.Price) == false)
        {
            // TODO: Предложить игроку купить монет или посмотреть рекламу
            return;
        }

        Player.Instance.MinusCoins(Settings.Price);
        Available = true;

        UpdateText();
        OnBuyBooster?.Invoke();

        // Сохранить покупку
        PlayerPrefs.SetInt(Settings.Key, 0);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Обновить текст на кнопке
    /// </summary>
    /// <param name="msg"></param>
    private void UpdateText()
    {
        txtOnBtn.text = $"{Settings.Name}\nДоход: +{Settings.Value * 100}%";
        if (Available) txtOnBtn.text += "\nКуплено";
        else txtOnBtn.text += $"\nЦена: {Settings.Price}";
    }
    #endregion
}