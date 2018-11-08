using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplayController : MonoBehaviour {

    [SerializeField] Text word_text;

    [SerializeField] Color correct_color, used_color, base_color;

    public enum State { correct, incorrect, used }

    public string word { get; private set; }

    public void SetText(string text) {
        SetText(text, base_color);
    }

    public void SetText(string text, State state) {
        if (state == State.correct) {
            SetText(text, correct_color);
        } else if (state == State.incorrect) {
            SetText(text, base_color);
        } else if (state == State.used) {
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
