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
    
    public void AddScore(int amount)
    {
        score += amount;
        // Notify UI
        OnScoreChanged?.Invoke(gold);
    }

    public event Action<int> OnGoldChanged;
    public event Action<int> OnScoreChanged;
}
