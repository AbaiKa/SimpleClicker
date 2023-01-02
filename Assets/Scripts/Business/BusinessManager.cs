using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessManager : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// ������ �������
    /// </summary>
    [SerializeField]
    [Tooltip("������ �������")]
    private BusinessAssistant businessPrefab;
    /// <summary>
    /// ��������� ��� ��������
    /// </summary>
    [SerializeField]
    [Tooltip("��������� ��� ��������")]
    private RectTransform container;
    /// <summary>
    /// ������ ������� ������� + ���������� ����� ����
    /// </summary>
    [SerializeField]
    [Tooltip("������ ������� ������� + ���������� ����� ����")]
    private float defaultPrefabSize;
    /// <summary>
    /// ������ ��������
    /// </summary>
    [SerializeField]
    [Tooltip("������ ��������")]
    private List<BusinessProperties> businessSettings = new List<BusinessProperties>();
    #endregion

    #region Methods
    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// ������������� ��������
    /// </summary>
    private void Init()
    {
        // ������ ����� ������ ��� ����������
        float startSizeY = container.sizeDelta.y;
        container.sizeDelta = new Vector2(0, startSizeY + (defaultPrefabSize * businessSettings.Count));

        BusinessSettings settings = businessSettings[0].Business;

        // ������� ������� �� ����������� (����� -> ��� -> ... -> ����)
        for (int i = 0; i < businessSettings.Count; i++)
        {
            for (int j = 0; j < businessSettings.Count; j++)
            {
                if (businessSettings[i].Business.Profit >= businessSettings[j].Business.Profit)
                {
                    settings = businessSettings[j].Business;
                }
            }
            BusinessAssistant b = Instantiate(businessPrefab, container);
            b.Settings = settings;
            b.Init();
         
            if (businessSettings[i].Available) b.OpenBusiness();
        }
    }
    #endregion
}

#region BusinessProperties
[System.Serializable]
public struct BusinessProperties
{
    /// <summary>
    /// ��������� �������
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("��������� �������")]
    public BusinessSettings Business { get; private set; }
    /// <summary>
    /// True ���� �������� ������� ����
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("True ���� �������� ������� ����")]
    public bool Available { get; private set; }
}
#endregion