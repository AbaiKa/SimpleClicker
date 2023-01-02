using UnityEngine;

[CreateAssetMenu(fileName = "Booster", menuName = "Scriptable Objects/Profit Booster Settings")]
public class ProfitBoosterSettings : ScriptableObject
{
    #region Properties
    /// <summary>
    /// Название ключа для сохранения данных
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Название ключа для сохранения данных (на английском)")]
    public string Key { get; private set; }

    /// <summary>
    /// Название улучшения
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Название улучшения")]
    public string Name { get; private set; }

    /// <summary>
    /// Множитель дохода (в процентах) 0.5 = 50%
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Множитель дохода (в процентах) 5 = 500%")]
    [field: Range(0, 5)]
    public float Value { get; private set; }

    /// <summary>
    /// Стоимость улучшения
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Базовая стоимость бизнеса")]
    public int Price { get; private set; }
    #endregion

    #region Methods
    private void OnValidate()
    {
        Value = (float)System.Math.Round(Value, 2);
    }
    #endregion
}
