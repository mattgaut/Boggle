using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDictionary : MonoBehaviour, IGameDictionary {

    [SerializeField] TextAsset dictionary;



    List<string> loaded_dictionary;

    public int Count {
        get {
            return loaded_dictionary.Count;
        }
    }

    // Use this for initialization
    void Awake () {
        loaded_dictionary = new List<string>(dictionary.text.Split( new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
    }
	
	public List<string> Search(string search) {
        if (search.Length == 0) {
            return loaded_dictionary;
        }
        search = search.ToUpper();
        int bottom_search = 0, top_search = 0;

        bottom_search = loaded_dictionary.BinarySearch(search);

        if (bottom_search < 0) {
            bottom_search *= -1;
            bottom_search--;
        }

        top_search = bottom_search;
        while (top_search < loaded_dictionary.Count && loaded_dictionary[top_search].StartsWith(search)) {
            top_search++;
        }

        List<string> found_words = loaded_dictionary.GetRange(bottom_search, top_search - bottom_search);


        return found_words;
    }

    public bool Contains(string search) {
        search = search.ToUpper();
        return loaded_dictionary.BinarySearch(search) >= 0;
    }
}

public class GameSubDictionary : IGameDictionary {
    List<string> sub_dictionary;

    public int Count {
        get {
            return sub_dictionary.Count;
        }
    }

    public GameSubDictionary(List<string> words) {
        this.sub_dictionary = new List<string>(words);
    }

    public List<string> Search(string search) {
        if (search.Length == 0) {
            return sub_dictionary;
        }
        search = search.ToUpper();
        int bottom_search = 0, top_search = 0;

        bottom_search = sub_dictionary.BinarySearch(search);

        if (bottom_search < 0) {
            bottom_search *= -1;
            bottom_search--;
        }

        top_search = bottom_search;
        while (top_search < sub_dictionary.Count && sub_dictionary[top_search].StartsWith(search)) {
            top_search++;
        }

        List<string> found_words = sub_dictionary.GetRange(bottom_search, top_search - bottom_search);


        return found_words;
    }

    public bool Contains(string search) {
        search = search.ToUpper();
        return sub_dictionary.BinarySearch(search) >= 0;
    }
}

public interface IGameDictionary {
    List<string> Search(string search);
    bool Contains(string search);
    int Count { get; }
}
