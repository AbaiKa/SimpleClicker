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
    /// ����� �������� ������� 'TxtName'
    /// </summary>
    [Header("Settings")]
    [SerializeField]
    [Tooltip("����� �������� ������� 'TxtName'")]
    private TextMeshProUGUI txtName;
    /// <summary>
    /// �������� ��� ������ 'Image'
    /// </summary>
    [SerializeField]
    [Tooltip("�������� ��� ������ 'Image'")]
    private Image imgProgressBar;
    /// <summary>
    /// ����� ������ ������� 'TxtLevel'
    /// </summary>
    [SerializeField]
    [Tooltip("����� ������ 'TxtLevel'")]
    private TextMeshProUGUI txtLevel;
    /// <summary>
    /// ����� ������ �� �������
    /// </summary>
    [SerializeField]
    [Tooltip("����� ������ 'TxtProfit'")]
    private TextMeshProUGUI txtProfit;
    /// <summary>
    /// ������ ��� ��������� ������ 'LevelUp'
    /// </summary>
    [SerializeField]
    [Tooltip("������ ��������� ������ 'BtnLevelUp'")]
    private Button btnLevelUp;
    /// <summary>
    /// ����� �� ������ LevelUp
    /// </summary>
    private TextMeshProUGUI txtLevelUp;
    /// <summary>
    /// ������ ��������� (Booster)
    /// </summary>
    [Header("Booster")]
    [SerializeField]
    [Tooltip("������ ���������")]
    private BoosterAssistant boosterPrefab;
    /// <summary>
    /// ��������� ��� ��������
    /// </summary>
    [SerializeField]
    [Tooltip("��������� ��� ��������")]
    private RectTransform container;
    /// <summary>
    /// ������ ����������
    /// </summary>
    private List<BoosterAssistant> boostersList = new List<BoosterAssistant>();
    /// <summary>
    /// ������-���������
    /// </summary>
    public BusinessSettings Settings { get; set; }
    #endregion

    #region Private
    /// <summary>
    /// ������ ������ �������
    /// </summary>
    private float progressTimer;

    /// <summary>
    /// ������ ������� (�������, ��������� � �.�.)
    /// </summary>
    private BusinessData Data;
    #endregion

    #endregion

    #region Methods

    #region Unity
    private void Update()
    {
        // ���� �� ��������� ������ ������
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
    /// ������������� �������
    /// </summary>
    public void Init()
    {
        txtLevelUp = btnLevelUp.GetComponentInChildren<TextMeshProUGUI>();

        Data = Load();

        txtName.text = Settings.Name;
        imgProgressBar.fillAmount = 0;

        btnLevelUp.onClick.AddListener(LevelUp);

        ProfitBoosterSettings settings = Settings.ProfitBoosters[0];

        // ������� ������� �� ����������� (50% -> 100% -> ... -> 400%)
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
    /// ������� ��������� ���� ������
    /// </summary>
    public void OpenBusiness()
    {
        // ������ ��� ������
        if(Data.Available) return;

        Data.Available = true;
        Data.Level++;
        UpdateInfoPanel();
    }
    #endregion

    #region Private
    /// <summary>
    /// �������� ������ �������
    /// </summary>
    private void Progress()
    {
        progressTimer += Time.deltaTime;

        if (progressTimer >= Settings.ProfitDelay)
        {
            // �������� ������
            // TODO: �������� ��������� ������
            progressTimer = 0;

            Player.Instance.AddCoins(GetProfit());
        }

        imgProgressBar.fillAmount = progressTimer / Settings.ProfitDelay;
    }

    /// <summary>
    /// �������� ������� �������
    /// </summary>
    private void LevelUp()
    {
        int price = GetLevelUpPrice();
        if (Player.Instance.CanBuy(price) == false)
        {
            // TODO: ���������� ������ ������ ����� ��� ���������� �������
            return;
        }

        Player.Instance.MinusCoins(price);
        Data.Available = true;
        Data.Level++;

        // �������� �������� � UI
        UpdateInfoPanel();
    }
    /// <summary>
    /// ������� �� �������
    /// </summary>
    /// <returns></returns>
    private int GetProfit()
    {
        int baseProfit = (int)Data.Level * Settings.Profit;
        int profit = baseProfit;

        // ����� �� ����������
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
    /// ��������� ���������� ������
    /// </summary>
    /// <returns></returns>
    private int GetLevelUpPrice()
    {
        return (int)(Data.Level + 1) * Settings.Price;
    }

    #region UI
    private void UpdateInfoPanel()
    {
        txtLevel.text = $"��: \n{Data.Level}";
        txtProfit.text = $"�����: \n{GetProfit()}";
        txtLevelUp.text = $"LvlUp \n ����: {GetLevelUpPrice()}";
    }
    #endregion

    #region Save&Load
    /// <summary>
    /// ��������� ������� ������
    /// </summary>
    private void Save()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(Settings.Key, json);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// ���������� ����������� ������ ���� ��� ����
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
    /// True ���� ������ ��� �������� 
    /// </summary>
    public bool Available;
    /// <summary>
    /// ������� ������� �������
    /// </summary>
    public uint Level;
}

#endregion