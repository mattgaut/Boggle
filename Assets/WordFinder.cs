﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordFinder : MonoBehaviour {

    [SerializeField] GameManager gm;
    [SerializeField] Text to_display;

	public void FindWord(string search) {
        List<string> words = gm.current_game_dictionary.Search(search);

        string all_words = "";

        foreach (string word in words) {
            all_words += word + "\n";
        }

        to_display.text = all_words;
    }
}
