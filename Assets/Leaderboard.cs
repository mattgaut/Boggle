using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Leaderboard {

    public int count {
        get { return top_scores.Count; }
    }

    [SerializeField] List<float> top_scores;
    [SerializeField] bool ascending_scores;
    [SerializeField] int max_scores;

    public Leaderboard(int size, bool ascending_scores = false) {
        top_scores = new List<float>();
        max_scores = size;
        this.ascending_scores = ascending_scores;
    }

    public int UpdateLeaderboard(float score) {
        int place;
        if (ascending_scores) {
            place = top_scores.BinarySearch(score, Comparer<float>.Default);
        } else {
            place = top_scores.BinarySearch(score, new DescendingComparer());
        }
        if (place < 0) {
            place = -(place + 1);
        }
        
        top_scores.Insert(place, score);

        if (top_scores.Count > max_scores) {
            top_scores.RemoveAt(top_scores.Count - 1);
        }

        return place;
    }

    public float GetScore(int position) {
        return top_scores[position];
    }

    class DescendingComparer : IComparer<float> {
        public int Compare(float x, float y) {
            return Comparer<float>.Default.Compare(y, x);
        }
    }
}
