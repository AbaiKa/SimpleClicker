using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessManager : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// Префаб бизнеса
    /// </summary>
    [SerializeField]
    [Tooltip("Префаб бизнеса")]
    private BusinessAssistant businessPrefab;
    /// <summary>
    /// Контейнер для бизнесов
    /// </summary>
    [SerializeField]
    [Tooltip("Контейнер для бизнесов")]
    private RectTransform container;
    /// <summary>
    /// Высота префаба бизнеса + расстояние между ними
    /// </summary>
    [SerializeField]
    [Tooltip("Высота префаба бизнеса + расстояние между ними")]
    private float defaultPrefabSize;
    /// <summary>
    /// Список бизнесов
    /// </summary>
    [SerializeField]
    [Tooltip("Список бизнесов")]
    private List<BusinessProperties> businessSettings = new List<BusinessProperties>();
    #endregion

    #region Methods
    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// Инициализация бизнесов
    /// </summary>
    private void Init()
    {
        // Задаем новый размер для контейнера
        float startSizeY = container.sizeDelta.y;
        container.sizeDelta = new Vector2(0, startSizeY + (defaultPrefabSize * businessSettings.Count));

        BusinessSettings settings = businessSettings[0].Business;

        // Спавним бизнесы по возрастанию (склад -> дом -> ... -> банк)
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
    /// Настройки бизнеса
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("Настройки бизнеса")]
    public BusinessSettings Business { get; private set; }
    /// <summary>
    /// True если доступен вначале игры
    /// </summary>
    [field: SerializeField]
    [field: Tooltip("True если доступен вначале игры")]
    public bool Available { get; private set; }
}
#endregion