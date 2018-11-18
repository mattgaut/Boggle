using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIManager : MonoBehaviour {

    [SerializeField] GameObject root_canvas;

    [SerializeField] Text leaderboard_title, score_text;

    public void ShowEndGameUI(GameInfo info) {
        LoadEndGameUI(info);

        ShowUI(true);
    }

    public void ShowUI(bool show) {
        root_canvas.SetActive(show);
    }

    void LoadEndGameUI(GameInfo info) {
        if (info.mode == GameMode.standard) {
            score_text.text = info.score + " pts";

            LoadStandardLeaderboard(info);
        } else if (info.mode == GameMode.word_rush) {
            score_text.text = ((int)info.time_used/60) + ":" + (info.time_used % 60).ToString("00.00");

            LoadWordRushLeaderboard(info);
        }
    }

    void LoadStandardLeaderboard(GameInfo info) {
        leaderboard_title.text = "Standard Leaderboard";
    }

    void LoadWordRushLeaderboard(GameInfo info) {
        leaderboard_title.text = "Word Rush Leaderboard";
    }
}
