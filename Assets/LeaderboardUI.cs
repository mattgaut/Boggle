using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour {

    [SerializeField] Text[] scores;

    public void Clear() {
        foreach (Text t in scores) {
            t.enabled = false;
        }
    }

    public void SetScore(int placement, string score, bool highlight = false) {
        if (placement < 0 || placement >= scores.Length) {
            return;
        }

        scores[placement].enabled = true;

        scores[placement].text = score;
        if (highlight) {
            scores[placement].color = Color.yellow;
        } else {
            scores[placement].color = Color.white;
        }
    }
}
