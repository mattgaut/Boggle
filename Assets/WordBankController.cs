using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordBankController : MonoBehaviour {

    [SerializeField] Text words_left;

    [SerializeField] Transform parent_transform;

    [SerializeField] Text word_prefab;

    List<Text> words_in_bank;
    HashSet<string> used_words;

    int words_left_count;

    void Awake() {
        words_in_bank = new List<Text>();
        used_words = new HashSet<string>();
    }

	public void SetWordsLeft(int count) {
        words_left.text = count + " Words Left";
        words_left_count = count;
    }

    public void AddWord(string word) {
        if (used_words.Contains(word)) return;

        Text new_word = Instantiate(word_prefab);
        new_word.text = word;
        int position = words_in_bank.BinarySearch(new_word, new TextComparer());
        if (position < 0) {
            position = -(position + 1);
        }
        words_in_bank.Insert(position, new_word);
        used_words.Add(word);

        new_word.transform.SetParent(parent_transform);
        new_word.transform.SetSiblingIndex(position);

        words_left_count--;
        words_left.text = words_left_count + " Words Left";
    }

    public void Clear() {
        foreach (Text t in words_in_bank) {
            Destroy(t.gameObject);
        }
        words_in_bank.Clear();
        used_words.Clear();
        words_left.text = "";
    }

    class TextComparer : IComparer<Text> {
        public int Compare(Text x, Text y) {
            int length = y.text.Length - x.text.Length;
            if (length == 0) {
                return x.text.CompareTo(y.text);
            } else {
                return length;
            }            
        }
    }
}
