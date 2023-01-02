using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Business", menuName = "Scriptable Objects/Business Settings")]
public class BusinessSettings : ScriptableObject
{
    #region Properties
    /// <summary>
    /// Название ключа для сохранения данных
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Название ключа для сохранения данных (на английском)")]
    public string Key { get; private set; }

    /// <summary>
    /// Название бизнеса
    /// </summary>
    [field: Header("Settings")]
    [field: SerializeField]
    [field: Tooltip("Название бизнеса")]
    public string Name { get; private set; }

    /// <summary>
    /// Базовая стоимость бизнеса
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Базовая стоимость бизнеса")]
    public int Price { get; private set; }

    /// <summary>
    /// Базовый доход от бизнеса
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Базовый доход от бизнеса")]
    public int Profit { get; private set; }

    /// <summary>
    /// Задержка дохода (в секундах)
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Задержка дохода (в секундах)")]
    [field: Range(0, 50)]
    public uint ProfitDelay { get; private set; }

    /// <summary>
    /// Множитель дохода 1
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Список множителей дохода")]
    public List<ProfitBoosterSettings> ProfitBoosters { get; private set; }
    #endregion
}
