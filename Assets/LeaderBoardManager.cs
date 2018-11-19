using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour {

    public Leaderboard active_leaderboard { get; private set; }

    string filepath;
    GameMode active_mode;

    public void LoadLeaderboard(GameMode mode) {
        if (active_mode == mode && active_leaderboard != null) {
            return;
        }

        if (active_leaderboard != null) SaveLeaderboard();

        active_mode = mode;

        filepath = "";

        if (mode == GameMode.blitz) {
            filepath = "Blitz.lb";
        } else if (mode == GameMode.standard){
            filepath = "Standard.lb";
        } else if (mode == GameMode.word_rush) {
            filepath = "WordRush.lb";
        } else {
            active_leaderboard = null;
            return;
        }



        filepath = Path.Combine(Application.persistentDataPath, filepath);

        if (!File.Exists(filepath)) {
            active_leaderboard = CreateLeaderboard(mode);

            SaveLeaderboard();
        } else {
            LoadLeaderboard();
        }
    }

    public void SaveLeaderboard() {
        if (filepath == "") {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(filepath, FileMode.OpenOrCreate);

        bf.Serialize(file, active_leaderboard);

        file.Close();
    }

    void LoadLeaderboard() {
        if (filepath == "") {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(filepath, FileMode.Open);


        active_leaderboard = (Leaderboard)bf.Deserialize(file);
    }

    Leaderboard CreateLeaderboard(GameMode mode) {
        bool ascending = (GameMode.word_rush == mode);

        Leaderboard leaderboard = new Leaderboard(10, ascending);

        return leaderboard;
    }
}
