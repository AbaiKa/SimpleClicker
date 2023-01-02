using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BusinessAssistant : MonoBehaviour
{
    #region Properties

    #region Editor
    /// <summary>
    /// Текст название бизнеса 'TxtName'
    /// </summary>
    [Header("Settings")]
    [SerializeField]
    [Tooltip("Текст название бизнеса 'TxtName'")]
    private TextMeshProUGUI txtName;
    /// <summary>
    /// Прогресс бар дохода 'Image'
    /// </summary>
    [SerializeField]
    [Tooltip("Прогресс бар дохода 'Image'")]
    private Image imgProgressBar;
    /// <summary>
    /// Текст уровня бизнеса 'TxtLevel'
    /// </summary>
    [SerializeField]
    [Tooltip("Текст уровня 'TxtLevel'")]
    private TextMeshProUGUI txtLevel;
    /// <summary>
    /// Текст дохода от бизнеса
    /// </summary>
    [SerializeField]
    [Tooltip("Текст дохода 'TxtProfit'")]
    private TextMeshProUGUI txtProfit;
    /// <summary>
    /// Кнопка для повышения уровня 'LevelUp'
    /// </summary>
    [SerializeField]
    [Tooltip("Кнопка повышения уровня 'BtnLevelUp'")]
    private Button btnLevelUp;
    /// <summary>
    /// Текст на кнопке LevelUp
    /// </summary>
    private TextMeshProUGUI txtLevelUp;
    /// <summary>
    /// Префаб улучшения (Booster)
    /// </summary>
    [Header("Booster")]
    [SerializeField]
    [Tooltip("Префаб улучшения")]
    private BoosterAssistant boosterPrefab;
    /// <summary>
    /// Контейнер для бустеров
    /// </summary>
    [SerializeField]
    [Tooltip("Контейнер для бустеров")]
    private RectTransform container;
    /// <summary>
    /// Список множителей
    /// </summary>
    private List<BoosterAssistant> boostersList = new List<BoosterAssistant>();
    /// <summary>
    /// Бизнес-настройки
    /// </summary>
    public BusinessSettings Settings { get; set; }
    #endregion

    #region Private
    /// <summary>
    /// Таймер работы бизнеса
    /// </summary>
    private float progressTimer;

    /// <summary>
    /// Данные бизнеса (Уровень, улучшения и т.д.)
    /// </summary>
    private BusinessData Data;
    #endregion

    #endregion

    #region Methods

    #region Unity
    private void Update()
    {
        // Пока не приобрели данный бизнес
        if (Data.Available == false) return;

        Progress();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause) Save();
    }
    private void OnApplicationQuit()
    {
        Save();
    }

    #endregion

    #region Public
    /// <summary>
    /// Инициализация бизнеса
    /// </summary>
    public void Init()
    {
        txtLevelUp = btnLevelUp.GetComponentInChildren<TextMeshProUGUI>();

        Data = Load();

        txtName.text = Settings.Name;
        imgProgressBar.fillAmount = 0;

        btnLevelUp.onClick.AddListener(LevelUp);

        ProfitBoosterSettings settings = Settings.ProfitBoosters[0];

        // Спавним бустеоы по возрастанию (50% -> 100% -> ... -> 400%)
        for (int i = 0; i < Settings.ProfitBoosters.Count; i++)
        {
            for (int j = 0; j < Settings.ProfitBoosters.Count; j++)
            {
                if (Settings.ProfitBoosters[i].Value >= Settings.ProfitBoosters[j].Value)
                {
                    settings = Settings.ProfitBoosters[j];
                }
            }
            BoosterAssistant b = Instantiate(boosterPrefab, container);
            b.Settings = settings;
            b.OnBuyBooster += UpdateInfoPanel;
            b.Init();

            if (boostersList.Contains(b) == false)
                boostersList.Add(b);
        }

        UpdateInfoPanel();
    }
    /// <summary>
    /// Сделать доступным этот бизнес
    /// </summary>
    public void OpenBusiness()
    {
        // Бизнес уже открыт
        if(Data.Available) return;

        Data.Available = true;
        Data.Level++;
        UpdateInfoPanel();
    }
    #endregion

    #region Private
    /// <summary>
    /// Прогресс работы бизнеса
    /// </summary>
    private void Progress()
    {
        progressTimer += Time.deltaTime;

        if (progressTimer >= Settings.ProfitDelay)
        {
            // Обнуляем таймер
            // TODO: Анимация получения дохода
            progressTimer = 0;

            Player.Instance.AddCoins(GetProfit());
        }

        imgProgressBar.fillAmount = progressTimer / Settings.ProfitDelay;
    }

    /// <summary>
    /// Улучшить уровень бизнеса
    /// </summary>
    private void LevelUp()
    {
        int price = GetLevelUpPrice();
        if (Player.Instance.CanBuy(price) == false)
        {
            // TODO: Предложить игроку купить монет или посмотреть рекламу
            return;
        }

        Player.Instance.MinusCoins(price);
        Data.Available = true;
        Data.Level++;

        // Обновить значения в UI
        UpdateInfoPanel();
    }
    /// <summary>
    /// Прибыль от бизнеса
    /// </summary>
    /// <returns></returns>
    private int GetProfit()
    {
        int baseProfit = (int)Data.Level * Settings.Profit;
        int profit = baseProfit;

        // Доход от множителей
        float booster = 0;
        for (int i = 0; i < boostersList.Count; i++)
        {
            if (boostersList[i].Available == false) continue;

            booster += boostersList[i].Settings.Value;
        }

        profit += (int)(booster * baseProfit);
        return profit != 0 ? profit : Settings.Profit;
    }
    /// <summary>
    /// Стоимость следующего уровня
    /// </summary>
    /// <returns></returns>
    private int GetLevelUpPrice()
    {
        return (int)(Data.Level + 1) * Settings.Price;
    }

    #region UI
    private void UpdateInfoPanel()
    {
        txtLevel.text = $"Ур: \n{Data.Level}";
        txtProfit.text = $"Доход: \n{GetProfit()}";
        txtLevelUp.text = $"LvlUp \n Цена: {GetLevelUpPrice()}";
    }
    #endregion

    #region Save&Load
    /// <summary>
    /// Сохранить профиль игрока
    /// </summary>
    private void Save()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(Settings.Key, json);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Возвращает сохраненные данные если они есть
    /// </summary>
    /// <returns></returns>
    private BusinessData Load()
    {
        if (PlayerPrefs.HasKey(Settings.Key))
            return JsonUtility.FromJson<BusinessData>(PlayerPrefs.GetString(Settings.Key));
        else
            return new BusinessData();
    }
    #endregion

    #endregion

    #endregion
}

#region Data

[Serializable]
public class BusinessData
{
    /// <summary>
    /// True если бизнес уже имееться 
    /// </summary>
    public bool Available;
    /// <summary>
    /// Текущий уровень бизнеса
    /// </summary>
    public uint Level;
}

#endregion