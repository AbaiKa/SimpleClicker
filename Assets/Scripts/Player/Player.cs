using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// Singleton
    /// </summary>
    public static Player Instance { get; private set; }
    /// <summary>
    /// ������ ������
    /// </summary>
    private PlayerData Data;

    public delegate void OnValueChanged(int value);
    /// <summary>
    /// ������� ��� ��������� �����
    /// </summary>
    public event OnValueChanged onCoinsChanged;
    #endregion

    #region Methods

    #region Unity
    private void Awake()
    {
        Instance = this;
        Data = Load();
    }
    private void Start()
    {
        StartCoroutine(AfterStartRoutine());    
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
    /// �������� 'value' �����
    /// </summary>
    /// <param name="value"></param>
    public void AddCoins(int value)
    {
        Data.Coins += value;
        onCoinsChanged?.Invoke(Data.Coins);
    }
    /// <summary>
    /// ������ 'value' �����
    /// </summary>
    /// <param name="price"></param>
    public void MinusCoins(int value)
    {
        Data.Coins -= value;
        onCoinsChanged?.Invoke(Data.Coins);
    }
    /// <summary>
    /// True ���� � ��� ���������� �����
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool CanBuy(int price)
    {
        return Data.Coins >= price;
    }
    #endregion

    #region Private
    /// <summary>
    /// ��������, ����� ���������� UI ��� ������ ��������� � ��������� � ���� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator AfterStartRoutine()
    {
        yield return new WaitForEndOfFrame();
        onCoinsChanged?.Invoke(Data.Coins);
    }

    #region Save&Load
    /// <summary>
    /// ��������� ������� ������
    /// </summary>
    private void Save()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// ���������� ����������� ������ ��� ����� �������
    /// </summary>
    /// <returns></returns>
    private PlayerData Load()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
            return JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
        else
            return new PlayerData();
    }
    #endregion

    #endregion

    #endregion
}

#region Data
[Serializable]
public class PlayerData
{
    public int Coins = 0;
}
#endregion