using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Менеджер игрового интерфейса
/// </summary>
public class GameUIManager : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// Текст денег 'txtCoins'
    /// </summary>
    [SerializeField]
    [Tooltip("Текст денег 'txtCoins'")]
    private TextMeshProUGUI txtCoins;
    #endregion

    #region Methods
    private void Start()
    {
        Player.Instance.onCoinsChanged += (int value) => txtCoins.text = $"Баланс: {value}$";
    }
    #endregion
}
