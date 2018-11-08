using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour, IGameDictionary {

    public bool is_searching { get; private set; }

    [SerializeField] GameDictionary dictionary;

    [SerializeField] WordBankController word_bank;

    [SerializeField] LineDisplayController line_display;

    [SerializeField] WordDisplayController word_display;

    [SerializeField] int seed;

    [SerializeField] bool use_random;

    List<string> alphabet;
    List<int> alphabet_frequency;

    List<string> words_in_play;
    HashSet<string> used_words;

    string current_word;
    int last_block;
    HashSet<int> used_blocks; 

    [SerializeField] Block[] board;

    public List<string> Search(string search) {
        if (search.Length == 0) {
            return words_in_play;
        }
        search = search.ToUpper();
        int bottom_search = 0, top_search = 0;

        bottom_search = words_in_play.BinarySearch(search);

        if (bottom_search < 0) {
            bottom_search *= -1;
            bottom_search--;
        }

        top_search = bottom_search;
        while (top_search < words_in_play.Count && words_in_play[top_search].StartsWith(search)) {
            top_search++;
        }

        List<string> found_words = words_in_play.GetRange(bottom_search, top_search - bottom_search);


        return found_words;
    }

    public bool Contains(string search) {
        search = search.ToUpper();
        return words_in_play.BinarySearch(search) >= 0;
    }

    private void Awake() {
        alphabet = new List<string>()        { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Qu", "R", "S", "T", "U", "V", "X", "Y", "Z" };
        alphabet_frequency = new List<int>() {  10,  4,   4,   8,   12,  4,   6,   4,   10,  2,   2,   8,   4,   12,  8,   4,   1,    12,  8,   12,  6,   4,   2,   4,   2 };

        used_words = new HashSet<string>();

        for (int i = 0; i < board.Length; i++) {
            board[i].Init(this, i);
        }
    }

    private void Start() {
        seed = use_random ? System.DateTime.Now.Millisecond : seed;
        Random.InitState(seed);
        ShuffleBoard();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            seed = System.DateTime.Now.Millisecond;
            Random.InitState(seed);
            ShuffleBoard();
        }

        if (is_searching && Input.GetMouseButtonUp(0)) {
            EndWordSearch();
        }
    }

    public void ShuffleBoard() {
        List<int> alphabet_frequency_copy = new List<int>(alphabet_frequency);
        int sum = 0;
        foreach (int i in alphabet_frequency_copy) {
            sum += i;
        }
        foreach (Block b in board) {
            int value = Random.Range(0, sum - 1);
            while (value > 0) {
                for (int i = 0; i < alphabet_frequency_copy.Count; i++) {
                    value -= alphabet_frequency_copy[i];
                    if (value <= 0) {
                        alphabet_frequency_copy[i]--;
                        sum--;
                        b.SetValue(alphabet[i]);
                        break;
                    }
                }
            }
        }
        SetAvailableWords();
        word_bank.Clear();
        used_words.Clear();
        word_bank.SetWordsLeft(words_in_play.Count);
    }

    public void StartWordSearch(int block_id) {
        current_word = board[block_id].value.ToUpper();
        last_block = block_id;
        word_display.SetText(current_word);
        is_searching = true;

        used_blocks = new HashSet<int>();
        used_blocks.Add(block_id);

        line_display.AddPosition(board[block_id].transform.position);
    }

    public void AddToWordSearch(int block_id) {
        if (is_searching && IsAdjacent(last_block, block_id) && !used_blocks.Contains(block_id)) {
            current_word += board[block_id].value.ToUpper();
            used_blocks.Add(block_id);
            last_block = block_id;

            WordDisplayController.State state = WordDisplayController.State.correct;
            if (!words_in_play.Contains(current_word)) {
                state = WordDisplayController.State.incorrect;
            } else if (used_words.Contains(current_word)) {
                state = WordDisplayController.State.used;
            }
            word_display.SetText(current_word, state);

            line_display.AddPosition(board[block_id].transform.position);
        }
    }

    public void EndWordSearch() {
        is_searching = false;

        word_display.Clear();

        line_display.Clear();

        if (words_in_play.Contains(current_word) && !used_words.Contains(current_word)) {
            word_bank.AddWord(current_word);
            used_words.Add(current_word);
        }
    }

    void SetAvailableWords() {
        words_in_play = new List<string>();
        HashSet<string> all_words = new HashSet<string>();
        for (int i = 0; i < 16; i++) {
            all_words.UnionWith(FindWordsInPlay(i));
        }

        words_in_play = new List<string>(all_words);
        words_in_play.Sort();
    }

    HashSet<string> FindWordsInPlay(int starting_block) {
        HashSet<string> found_words = new HashSet<string>();

        string value = "";

        int visited_blocks = 0;

        FindWordsInPlay(starting_block, found_words, value, visited_blocks);

        return found_words;
    }

    void FindWordsInPlay(int block, HashSet<string> words, string value, int visited_blocks) {
        value += board[block].value;
        visited_blocks += (1 << block);

        bool keep_searching = false;
        if (dictionary.Contains(value)) {
            words.Add(value.ToUpper());
            keep_searching = true;
        } else if (dictionary.Search(value).Count > 0) {
            keep_searching = true;
        }

        if (keep_searching) {
            // Search Above
            if (block + 4 < 16 && (visited_blocks & (1 << block + 4)) == 0) {
                FindWordsInPlay(block + 4, words, value, visited_blocks);
            }

            // Search Below
            if (block - 4 >= 0 && (visited_blocks & (1 << block - 4)) == 0) {
                FindWordsInPlay(block - 4, words, value, visited_blocks);
            }

            // Search Leftward tiles
            if (block % 4 != 0) {
                // Search Left
                if (block - 1 >= 0  && (visited_blocks & (1 << block - 1)) == 0) {
                    FindWordsInPlay(block - 1, words, value, visited_blocks);
                }
                // Search TopLeft
                if (block + 3 < 16 && (visited_blocks & (1 << block + 3)) == 0) {
                    FindWordsInPlay(block + 3, words, value, visited_blocks);
                }

                // Search BottomLeft
                if (block - 5 >= 0 && (visited_blocks & (1 << block - 5)) == 0) {
                    FindWordsInPlay(block - 5, words, value, visited_blocks);
                }
            }

            // Search Rightward Tiles
            if (block % 4 != 3) {
                // Search Right
                if (block + 1 < 16 && (visited_blocks & (1 << block + 1)) == 0) {
                    FindWordsInPlay(block + 1, words, value, visited_blocks);
                }
            
                // Search TopRight
                if (block + 5 < 16  && (visited_blocks & (1 << block + 5)) == 0) {
                    FindWordsInPlay(block + 5, words, value, visited_blocks);
                }

                // Search BottomRight
                if (block - 3 >= 0 && (visited_blocks & (1 << block - 3)) == 0) {
                    FindWordsInPlay(block - 3, words, value, visited_blocks);
                }
            }
        }
    }

    bool IsAdjacent(int a, int b) {
        int value = a - b;

        if (a % 4 == 0) {
            if (value == 1 || value == -3 || value == 5) {
                return false;
            }
        }
        if (b % 4 == 0) {
            if (value == -1 || value == 3 || value == -5) {
                return false;
            }
        }
        if (a % 4 == 3) {
            if (value == -1 || value == 3 || value == -5) {
                return false;
            }
        }
        if (b % 4 == 3) {
            if (value == 1 || value == -3 || value == 5) {
                return false;
            }
        }
        if (Mathf.Abs(value) == 1) {
            return true;
        }
        if (Mathf.Abs(value) == 3) {
            return true;
        }
        if (Mathf.Abs(value) == 4) {
            return true;
        }
        if (Mathf.Abs(value) == 5) {
            return true;
        }

        return false;
    }
}
