using System.Collections;
using TMPro;
using UnityEngine;

public class CoinsAmountText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;

    void Start()
    {
        SetBalance(CoinsBalanceManager.Instance.Balance);

        CoinsBalanceManager.Instance.OnBalanceChanged += SetBalance;
    }

    void SetBalance(int amount)
    {
        _text.text = amount.ToString();
    }

    private void OnDestroy()
    {
        if (CoinsBalanceManager.Instance != null)
        {
            CoinsBalanceManager.Instance.OnBalanceChanged -= SetBalance;
        }
    }
}