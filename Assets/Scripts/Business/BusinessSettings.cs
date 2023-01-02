using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Business", menuName = "Scriptable Objects/Business Settings")]
public class BusinessSettings : ScriptableObject
{
    #region Properties
    /// <summary>
    /// �������� ����� ��� ���������� ������
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("�������� ����� ��� ���������� ������ (�� ����������)")]
    public string Key { get; private set; }

    /// <summary>
    /// �������� �������
    /// </summary>
    [field: Header("Settings")]
    [field: SerializeField]
    [field: Tooltip("�������� �������")]
    public string Name { get; private set; }

    /// <summary>
    /// ������� ��������� �������
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("������� ��������� �������")]
    public int Price { get; private set; }

    /// <summary>
    /// ������� ����� �� �������
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("������� ����� �� �������")]
    public int Profit { get; private set; }

    /// <summary>
    /// �������� ������ (� ��������)
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("�������� ������ (� ��������)")]
    [field: Range(0, 50)]
    public uint ProfitDelay { get; private set; }

    /// <summary>
    /// ��������� ������ 1
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("������ ���������� ������")]
    public List<ProfitBoosterSettings> ProfitBoosters { get; private set; }
    #endregion
}
