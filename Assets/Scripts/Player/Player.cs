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
    /// Данные игрока
    /// </summary>
    private PlayerData Data;

    public delegate void OnValueChanged(int value);
    /// <summary>
    /// Событие при изменении монет
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
    /// Добавить 'value' монет
    /// </summary>
    /// <param name="value"></param>
    public void AddCoins(int value)
    {
        Data.Coins += value;
        onCoinsChanged?.Invoke(Data.Coins);
    }
    /// <summary>
    /// Отнять 'value' монет
    /// </summary>
    /// <param name="price"></param>
    public void MinusCoins(int value)
    {
        Data.Coins -= value;
        onCoinsChanged?.Invoke(Data.Coins);
    }
    /// <summary>
    /// True если у нас достаточно монет
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
    /// Корутина, чтобы обновление UI при старте сработало с задержкой в один кадр
    /// </summary>
    /// <returns></returns>
    private IEnumerator AfterStartRoutine()
    {
        yield return new WaitForEndOfFrame();
        onCoinsChanged?.Invoke(Data.Coins);
    }

    #region Save&Load
    /// <summary>
    /// Сохранить профиль игрока
    /// </summary>
    private void Save()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Возвращает сохраненные данные или новый профиль
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