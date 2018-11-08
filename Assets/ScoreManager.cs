using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    [SerializeField] Text text;

    public float score { get; private set; }

    public void Clear() {
        score = 0;
        UpdateDisplay();
    }

    public void AddPointsForWord(string word) {
        score += 50 * (1 << (word.Length - 2));
        UpdateDisplay();
    }

    void UpdateDisplay() {
        text.text = score + " pts";
    }
}
