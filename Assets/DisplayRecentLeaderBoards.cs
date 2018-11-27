using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRecentLeaderBoards : MonoBehaviour {

    [SerializeField] LeaderboardUI standard, blitz, word_rush;

    [SerializeField] LeaderBoardManager manager;

    public void DisplayLeaderboard() {
        standard.Clear();
        standard.LoadScoreLeaderboard(manager.GetLeaderboard(GameMode.standard));

        blitz.Clear();
        blitz.LoadScoreLeaderboard(manager.GetLeaderboard(GameMode.blitz));

        word_rush.Clear();
        word_rush.LoadTimeLeaderboard(manager.GetLeaderboard(GameMode.word_rush));
    }
}
