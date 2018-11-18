using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WordState { correct, incorrect, used }

public enum GameMode { timed, zen, find_words}

public class GameManager : MonoBehaviour {

    public GameSubDictionary current_game_dictionary { get; private set; }

    [SerializeField] BoardManager board_manager;

    [SerializeField] WordBankController word_bank;

    [SerializeField] ScoreManager score_manager;

    [SerializeField] SoundEffectManager sfx;

    HashSet<string> used_words;

    [SerializeField] int seed;

    [SerializeField] bool use_random;

    [SerializeField] float time_per_board;

    [SerializeField][Range(0,1)] float time_trial_percent_needed;

    [SerializeField] Text time_remaining_text;

    [SerializeField] GameMode mode;

    float timer;

    private void Awake() {
        used_words = new HashSet<string>();
    }

    private void Start() {
        seed = use_random ? System.DateTime.Now.Millisecond : seed;
        Random.InitState(seed);
        ShuffleBoard();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            int seed = System.DateTime.Now.Millisecond;
            Random.InitState(seed);
            ShuffleBoard();
        }


        if (mode == GameMode.find_words) {
            DisplayTime(timer);
            timer += Time.deltaTime;

            if (score_manager.reached_target) {
                // End Game
            }
        } else if (mode == GameMode.timed) {
            DisplayTime(timer);

            if (timer < 0) {
                // End Game
            }

            timer -= Time.deltaTime;
        }
    }

    public void ShuffleBoard() {
        current_game_dictionary = new GameSubDictionary(board_manager.ShuffleBoard());

        word_bank.Clear();
        score_manager.Clear();
        used_words.Clear();
        if (mode == GameMode.zen) {
            word_bank.SetWordsLeft(current_game_dictionary.Count);
        } else if (mode == GameMode.timed){
            timer = time_per_board;
        } else if (mode == GameMode.find_words) {
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
}
