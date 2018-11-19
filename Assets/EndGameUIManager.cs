using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIManager : MonoBehaviour {

    [SerializeField] GameObject root_canvas;

    [SerializeField] LeaderBoardManager lb_manager;

    [SerializeField] LeaderboardUI lb_ui;

    [SerializeField] Text leaderboard_title, score_text;

    public void ShowEndGameUI(GameInfo info) {
        LoadEndGameUI(info);

        ShowUI(true);
    }

    public void ShowUI(bool show) {
        root_canvas.SetActive(show);
    }

    void LoadEndGameUI(GameInfo info) {
        lb_ui.Clear();
        lb_manager.LoadLeaderboard(info.mode);

        if (info.mode == GameMode.standard) {
            score_text.text = info.score + " pts";

            LoadStandardLeaderboard(info);
        } else if (info.mode == GameMode.word_rush) {
            score_text.text = ((int)info.time_used/60) + ":" + (info.time_used % 60).ToString("00.00");

            LoadWordRushLeaderboard(info);
        } else if (info.mode == GameMode.blitz) {
            score_text.text = info.score + " pts";

            LoadBlitzLeaderboard(info);
        }

        lb_manager.SaveLeaderboard();
    }

    void LoadStandardLeaderboard(GameInfo info) {
        leaderboard_title.text = "Standard Leaderboard";

        int placement = lb_manager.active_leaderboard.UpdateLeaderboard(info.score);

        LoadPointsBasedLeaderboard(placement);
    }

    void LoadBlitzLeaderboard(GameInfo info) {
        leaderboard_title.text = "Blitz Leaderboard";

        int placement = lb_manager.active_leaderboard.UpdateLeaderboard(info.score);

        LoadPointsBasedLeaderboard(placement);
    }

    void LoadWordRushLeaderboard(GameInfo info) {
        leaderboard_title.text = "Word Rush Leaderboard";

        int placement = lb_manager.active_leaderboard.UpdateLeaderboard(info.time_used);

        LoadTimeBasedLeaderboard(placement);
    }

    void LoadPointsBasedLeaderboard(int highlight_number) {
        for (int i = 0; i < lb_manager.active_leaderboard.count; i++) {
            lb_ui.SetScore(i, lb_manager.active_leaderboard.GetScore(i).ToString(), i == highlight_number);
        }
    }

    void LoadTimeBasedLeaderboard(int highlight_number) {
        for (int i = 0; i < lb_manager.active_leaderboard.count; i++) {
            float score = lb_manager.active_leaderboard.GetScore(i);
            lb_ui.SetScore(i, (int)(score / 60) + ":" + (score % 60).ToString("00.000"), i == highlight_number);
        }
    }
}