using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WordState { correct, incorrect, used }

public enum GameMode { standard, blitz, zen, word_rush}

public class GameManager : MonoBehaviour {

    public GameSubDictionary current_game_dictionary { get; private set; }

    public bool game_running { get; private set; }

    [SerializeField] BoardManager board_manager;

    [SerializeField] WordBankController word_bank;

    [SerializeField] ScoreManager score_manager;

    [SerializeField] SoundEffectManager sfx;

    [SerializeField] LeaderBoardManager lb_manager;

    [SerializeField] EndGameUIManager end_game_ui;

    HashSet<string> used_words;

    [SerializeField] int seed;

    [SerializeField] bool use_random;

    [SerializeField] float time_per_standard_board, time_per_blitz_board;

    [SerializeField][Range(0,1)] float time_trial_percent_needed;

    [SerializeField] Text time_remaining_text;

    [SerializeField] GameObject board_object;

    [SerializeField] GameMode mode;

    System.DateTime start_time;
    float time_limit;
    float timer;

    private void Awake() {
        used_words = new HashSet<string>();
    }

    private void Start() {
        seed = use_random ? System.DateTime.Now.Millisecond : seed;
        Random.InitState(seed);
        ShuffleBoard();
        SetGameRunning(false);
    }

    void Update() {

        board_object.SetActive(game_running);

        if (game_running) {
            if (mode == GameMode.word_rush) {
                DisplayTime(timer);
                timer = (float)(System.DateTime.Now - start_time).TotalSeconds;

                if (score_manager.reached_target) {
                    EndGame();
                }
            } else if (mode == GameMode.standard || mode == GameMode.blitz) {
                DisplayTime(timer);

                if (timer < 0) {
                    EndGame();
                }

                timer = (float)(start_time - System.DateTime.Now).TotalSeconds + time_limit;
            }
        }
    }

    public void ShuffleBoard() {
        current_game_dictionary = new GameSubDictionary(board_manager.ShuffleBoard());

        word_bank.Clear();
        score_manager.Clear();
        used_words.Clear();

        if (mode == GameMode.zen) {
            word_bank.SetWordsLeft(current_game_dictionary.Count);
        } else if (mode == GameMode.standard) {
            timer = time_per_standard_board;
        } else if (mode == GameMode.blitz) {
            timer = time_per_blitz_board;
        } else if (mode == GameMode.word_rush) {
            timer = 0;
            score_manager.SetTargetScore(GetTotalPointsAvailable(), time_trial_percent_needed);
        }
    }

    public WordState ProcessWordHover(string word) {
        WordState state = WordState.correct;
        if (!current_game_dictionary.Contains(word)) {
            state = WordState.incorrect;
        } else if (used_words.Contains(word)) {
            state = WordState.used;
        }

        sfx.PlayClick();

        return state;
    }

    public void ProcessWordSubmission(string word) {
        if (current_game_dictionary.Contains(word) && !used_words.Contains(word)) {
            word_bank.AddWord(word, mode == GameMode.zen);
            used_words.Add(word);
            score_manager.AddPointsForWord(word);

            sfx.PlayAccept();
        } else {
            sfx.PlayBuzz();
        }
    }

    public void StartBlitzGame() {
        time_limit = time_per_blitz_board;
        StartGame(GameMode.blitz);
    }

    public void StartStandardGame() {
        time_limit = time_per_standard_board;
        StartGame(GameMode.standard);
    }

    public void StartWordRushGame() {
        StartGame(GameMode.word_rush);
    }

    public void StartZenGame() {
        StartGame(GameMode.zen);
    }

    public void PlayAgain() {
        StartGame(mode);
    }

    public void StartGame(GameMode mode) {
        this.mode = mode;

        start_time = System.DateTime.Now;

        ShuffleBoard();

        SetGameRunning(true);
    }

    public void CancelGame() {
        SetGameRunning(false);
    }

    void SetGameRunning(bool running) {
        game_running = running;

        board_object.SetActive(running);
    }

    void DisplayTime(float time) {
        if (time < 0) {
            time_remaining_text.text = "0:00";
        } else {
            time_remaining_text.text = ((int)time / 60).ToString("00") + ":" + ((time % 60).ToString("00.00"));
        }
    }

    int GetTotalPointsAvailable() {
        int to_ret = 0;
        foreach (string word in current_game_dictionary.Search("")) {
            to_ret += score_manager.GetPointsForWord(word);
        }
        return to_ret;
    }

    void EndGame() {
        if (!game_running) {
            return;
        }

        GameInfo info = null;
        if (mode == GameMode.word_rush) {
            info = new GameInfo(mode, (float)timer);
        } else if (mode == GameMode.standard || mode == GameMode.blitz) {
            info = new GameInfo(mode, score_manager.score);
        }

        end_game_ui.ShowEndGameUI(info);

        SetGameRunning(false);
    }
}

public class GameInfo {

    public GameMode mode { get; private set; }

    public float time_used { get; private set; }
    public int score { get; private set; }

    public GameInfo(GameMode mode, int score) {
        this.mode = mode;
        this.score = score;
        time_used = 0f;
    }

    public GameInfo(GameMode mode, float time_used) {
        this.mode = mode;
        score = 0;
        this.time_used = time_used;
    }
}
