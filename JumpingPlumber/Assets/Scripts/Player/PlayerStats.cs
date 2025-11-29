using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int gold = 0;
    private int score = 0;

    public void AddGold(int amount)
    {
        gold += amount;
        // Notify UI
        OnGoldChanged?.Invoke(gold);
    }

    public event Action<int> OnGoldChanged;
}
