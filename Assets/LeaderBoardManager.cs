using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour {
    Leaderboard blitz, word_rush, standard;

    string filepath;
    GameMode active_mode;

    void Awake() {
        blitz = LoadOrCreateLeaderboard(Path.Combine(Application.persistentDataPath, "Blitz.lb"), GameMode.blitz);
        word_rush = LoadOrCreateLeaderboard(Path.Combine(Application.persistentDataPath, "WordRush.lb"), GameMode.word_rush);
        standard = LoadOrCreateLeaderboard(Path.Combine(Application.persistentDataPath, "Standard.lb"), GameMode.standard);
    }

    private void OnApplicationQuit() {
        SaveAllLeaderboards();
    }

    public Leaderboard GetLeaderboard(GameMode mode) {
        if (mode == GameMode.blitz) {
            return blitz;
        } else if (mode == GameMode.standard) {
            return standard;
        } else if (mode == GameMode.word_rush) {
            return word_rush;
        }
        return null;
    }

    public void ClearAllScores() {
        blitz = CreateLeaderboard(GameMode.blitz);
        standard = CreateLeaderboard(GameMode.standard);
        word_rush = CreateLeaderboard(GameMode.word_rush);
        SaveAllLeaderboards();
    }

    public void SaveAllLeaderboards() {
        SaveLeaderboard(Path.Combine(Application.persistentDataPath, "Blitz.lb"), blitz);
        SaveLeaderboard(Path.Combine(Application.persistentDataPath, "WordRush.lb"), word_rush);
        SaveLeaderboard(Path.Combine(Application.persistentDataPath, "Standard.lb"), standard);
    }

    void SaveLeaderboard(string filepath, Leaderboard to_save) {
        if (to_save == null) {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(filepath, FileMode.OpenOrCreate);

        bf.Serialize(file, to_save);

        file.Close();
    }

    Leaderboard LoadOrCreateLeaderboard(string filepath, GameMode mode) {
        if (File.Exists(filepath)) {
            return LoadLeaderboard(filepath);
        } else {
            return CreateLeaderboard(mode);
        }
    }

    Leaderboard LoadLeaderboard(string filepath) {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(filepath, FileMode.Open);

        return (Leaderboard)bf.Deserialize(file);
    }

    Leaderboard CreateLeaderboard(GameMode mode) {
        bool ascending = (GameMode.word_rush == mode);

        Leaderboard leaderboard = new Leaderboard(50, ascending);

        return leaderboard;
    }
}
