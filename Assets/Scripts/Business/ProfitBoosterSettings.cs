using UnityEngine;

[CreateAssetMenu(fileName = "Booster", menuName = "Scriptable Objects/Profit Booster Settings")]
public class ProfitBoosterSettings : ScriptableObject
{
    #region Properties
    /// <summary>
    /// �������� ����� ��� ���������� ������
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("�������� ����� ��� ���������� ������ (�� ����������)")]
    public string Key { get; private set; }

    /// <summary>
    /// �������� ���������
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("�������� ���������")]
    public string Name { get; private set; }

    /// <summary>
    /// ��������� ������ (� ���������) 0.5 = 50%
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("��������� ������ (� ���������) 5 = 500%")]
    [field: Range(0, 5)]
    public float Value { get; private set; }

    /// <summary>
    /// ��������� ���������
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("������� ��������� �������")]
    public int Price { get; private set; }
    #endregion

    #region Methods
    private void OnValidate()
    {
        Value = (float)System.Math.Round(Value, 2);
    }
    #endregion
}
