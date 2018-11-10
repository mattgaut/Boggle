using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WordState { correct, incorrect, used }

public class GameManager : MonoBehaviour {

    public GameSubDictionary current_game_dictionary { get; private set; }

    [SerializeField] BoardManager board_manager;

    [SerializeField] WordBankController word_bank;

    [SerializeField] ScoreManager score_manager;

    [SerializeField] SoundEffectManager sfx;

    HashSet<string> used_words;

    [SerializeField] int seed;

    [SerializeField] bool use_random;

    private void Start() {
        used_words = new HashSet<string>();

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
    }

    public void ShuffleBoard() {
        current_game_dictionary = new GameSubDictionary(board_manager.ShuffleBoard());

        word_bank.Clear();
        score_manager.Clear();
        used_words.Clear();
        word_bank.SetWordsLeft(current_game_dictionary.Count);
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
            word_bank.AddWord(word);
            used_words.Add(word);
            score_manager.AddPointsForWord(word);

            sfx.PlayAccept();
        } else {
            sfx.PlayBuzz();
        }
    }
}
