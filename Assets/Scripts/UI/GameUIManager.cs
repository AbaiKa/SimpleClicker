using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// �������� �������� ����������
/// </summary>
public class GameUIManager : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// ����� ����� 'txtCoins'
    /// </summary>
    [SerializeField]
    [Tooltip("����� ����� 'txtCoins'")]
    private TextMeshProUGUI txtCoins;
    #endregion

    #region Methods
    private void Start()
    {
        Player.Instance.onCoinsChanged += (int value) => txtCoins.text = $"������: {value}$";
    }
    #endregion
}
