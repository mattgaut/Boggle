using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour {

    [SerializeField] Text[] scores;

    public void Clear() {
        foreach (Text t in scores) {
            t.enabled = false;
            foreach (Text t2 in t.GetComponentsInChildren<Text>()) {
                t2.enabled = false;
            }
        }
    }

    public void SetScore(int placement, string score, bool highlight = false) {
        if (placement < 0 || placement >= scores.Length) {
            return;
        }

        scores[placement].enabled = true;
        foreach (Text t in scores[placement].GetComponentsInChildren<Text>()) {
            t.enabled = true;
        }

        scores[placement].text = score;
        if (highlight) {
            scores[placement].color = Color.yellow;
        } else {
            scores[placement].color = Color.white;
        }
    }

    public void LoadScoreLeaderboard(Leaderboard board) {
        for (int i = 0; i < board.count && i < scores.Length; i++) {
            SetScore(i, board.GetScore(i).ToString());
        }
    }

    public void LoadTimeLeaderboard(Leaderboard board) {
        for (int i = 0; i < board.count && i < scores.Length; i++) {
            float time = board.GetScore(i);
            SetScore(i, (time / 60).ToString() + ":" + (time % 60).ToString("00.000"));
        }
    }
}
