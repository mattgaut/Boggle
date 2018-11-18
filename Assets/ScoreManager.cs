using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    [SerializeField] Text text;

    public int score { get; private set; }

    public int target_score { get; private set; }

    public bool reached_target { get { return target_score > 0 && score >= target_score; } }

    public void Clear() {
        score = 0;
        target_score = 0;
        UpdateDisplay();
    }

    public void SetTargetScore(int total_score_available, float target_percent) {
        target_score = (int)(total_score_available * target_percent);
        target_score = target_score - (target_score % 50);

        UpdateDisplay();
    }

    public void AddPointsForWord(string word) {
        score += GetPointsForWord(word);

        UpdateDisplay();
    }

    public int GetPointsForWord(string word) {
        return 50 * (1 << (word.Length - 2));
    }

    void UpdateDisplay() {
        if (target_score > 0) {
            text.text = (target_score - score) < 0 ? "0 pts remaining" : (target_score - score) + " pts remaining";
        } else {
            text.text = score + " pts";
        }
    }
}
