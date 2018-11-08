using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour {

    [SerializeField] Text text;

    [SerializeField] Collider2D big_collider;

    BoardManager board;

    int id;

    public string value { get; private set; }

    public void SetValue(string value) {
        this.value = value;
        text.text = value;
    }

    public void Init(BoardManager bm, int id) {
        board = bm;
        this.id = id;
    }

    private void OnMouseDown() {
        board.StartWordSearch(id);
    }

    private void OnMouseEnter() {
        board.AddToWordSearch(id);
    }

    private void Update() {
        if (board) big_collider.enabled = !board.is_searching;
    }
}
