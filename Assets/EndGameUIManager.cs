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
        lb_manager.GetLeaderboard(info.mode);

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
    }

    void LoadStandardLeaderboard(GameInfo info) {
        leaderboard_title.text = "Standard Leaderboard";

        int placement = lb_manager.GetLeaderboard(info.mode).UpdateLeaderboard(info.score);
        lb_manager.SaveAllLeaderboards();

        LoadPointsBasedLeaderboard(lb_manager.GetLeaderboard(info.mode), placement);
    }

    void LoadBlitzLeaderboard(GameInfo info) {
        leaderboard_title.text = "Blitz Leaderboard";

        int placement = lb_manager.GetLeaderboard(info.mode).UpdateLeaderboard(info.score);
        lb_manager.SaveAllLeaderboards();

        LoadPointsBasedLeaderboard(lb_manager.GetLeaderboard(info.mode), placement);
    }

    void LoadWordRushLeaderboard(GameInfo info) {
        leaderboard_title.text = "Word Rush Leaderboard";

        int placement = lb_manager.GetLeaderboard(info.mode).UpdateLeaderboard(info.time_used);
        lb_manager.SaveAllLeaderboards();

        LoadTimeBasedLeaderboard(lb_manager.GetLeaderboard(info.mode), placement);
    }

    void LoadPointsBasedLeaderboard(Leaderboard leaderboard, int highlight_number) {
        for (int i = 0; i < leaderboard.count; i++) {
            lb_ui.SetScore(i, leaderboard.GetScore(i).ToString(), i == highlight_number);
        }
    }

    void LoadTimeBasedLeaderboard(Leaderboard leaderboard, int highlight_number) {
        for (int i = 0; i < leaderboard.count; i++) {
            float score = leaderboard.GetScore(i);
            lb_ui.SetScore(i, (int)(score / 60) + ":" + (score % 60).ToString("00.000"), i == highlight_number);
        }
    }
}