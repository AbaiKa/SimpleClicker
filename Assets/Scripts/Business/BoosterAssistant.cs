using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Button))]
public class BoosterAssistant : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// ��������� ���������
    /// </summary>
    public ProfitBoosterSettings Settings { get; set; }
    /// <summary>
    /// True ���� ��������� ��� �������� 
    /// </summary>
    public bool Available { get; private set; }
    /// <summary>
    /// ������ ������
    /// </summary>
    private Button button;
    /// <summary>
    /// ����� �� ������
    /// </summary>
    private TextMeshProUGUI txtOnBtn;
    public delegate void OnBuy();
    /// <summary>
    /// ������� ��� ������� ���������
    /// </summary>
    public OnBuy OnBuyBooster;
    #endregion

    #region Methods
    /// <summary>
    /// ������������� �������
    /// </summary>
    public void Init()
    {
        button = GetComponent<Button>();
        txtOnBtn = button.GetComponentInChildren<TextMeshProUGUI>();
        Available = PlayerPrefs.GetInt(Settings.Key, -1) == -1 ? false : true;
        button.onClick.AddListener(BuyBooster);
        UpdateText();
    }
    /// <summary>
    /// ������ ���������
    /// </summary>
    private void BuyBooster()
    {
        // �� ��� ������ ���������
        if (Available) return;
        if(Player.Instance.CanBuy(Settings.Price) == false)
        {
            // TODO: ���������� ������ ������ ����� ��� ���������� �������
            return;
        }

        Player.Instance.MinusCoins(Settings.Price);
        Available = true;

        UpdateText();
        OnBuyBooster?.Invoke();

        // ��������� �������
        PlayerPrefs.SetInt(Settings.Key, 0);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// �������� ����� �� ������
    /// </summary>
    /// <param name="msg"></param>
    private void UpdateText()
    {
        txtOnBtn.text = $"{Settings.Name}\n�����: +{Settings.Value * 100}%";
        if (Available) txtOnBtn.text += "\n�������";
        else txtOnBtn.text += $"\n����: {Settings.Price}";
    }
    #endregion
}