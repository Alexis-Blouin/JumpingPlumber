using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    
    private Label _goldLabel;
    private Label _scoreLabel;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        _goldLabel = root.Q<Label>("GoldLabelCount");
        _scoreLabel = root.Q<Label>("ScoreLabelCount");

        playerStats.OnGoldChanged += UpdateGoldUI;
        playerStats.OnScoreChanged += UpdateScoredUI;
    }

    private void OnDisable()
    {
        playerStats.OnGoldChanged -= UpdateGoldUI;
        playerStats.OnScoreChanged -= UpdateScoredUI;
    }

    private void UpdateGoldUI(int newGold)
    {
        _goldLabel.text = newGold.ToString();
    }
    
    private void UpdateScoredUI(int newGold)
    {
        _scoreLabel.text = newGold.ToString();
    }
}
