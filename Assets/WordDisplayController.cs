using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplayController : MonoBehaviour {

    [SerializeField] Text word_text;

    [SerializeField] Color correct_color, used_color, base_color;

    public string word { get; private set; }

    public void SetText(string text) {
        SetText(text, base_color);
    }

    public void SetText(string text, WordState state) {
        if (state == WordState.correct) {
            SetText(text, correct_color);
        } else if (state == WordState.incorrect) {
            SetText(text, base_color);
        } else if (state == WordState.used) {
            SetText(text, used_color);
        }
    }

    public void Clear() {
        word = "";
        word_text.text = "";
    }

    void SetText(string text, Color color) {
        word_text.text = text;
        word = text;

        word_text.color = color;
    }
}
