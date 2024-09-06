using System.Collections;
using UnityEngine;

public delegate void GameActionInt(int value);

public class CoinsBalanceManager : Singleton<CoinsBalanceManager>
{
    [SerializeField] private bool addCoinsOnStart = false;

    public int Balance { get; private set; }

    public event GameActionInt OnBalanceChanged;

    protected override void Awake()
    {
        base.Awake();

        Balance = SavesManager.Instance.GetInt("Balance", 0);

        if (addCoinsOnStart)
        {
            AddCoins(5000);
        }
    }

    public void AddCoins(int amount)
    {
        Balance += amount;
        SavesManager.Instance.SetInt("Balance", Balance);

        OnBalanceChanged?.Invoke(Balance);
    }

    public void SubtractCoins(int amount)
    {
        AddCoins(-amount);
    }

    public bool HasCoins(int amount)
    {
        return amount <= Balance;
    }
}