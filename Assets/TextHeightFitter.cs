using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class TextHeightFitter : MonoBehaviour {

    Text text;
    RectTransform rect_transform;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
        text.verticalOverflow = VerticalWrapMode.Overflow;
        rect_transform = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        rect_transform.sizeDelta = new Vector2(rect_transform.sizeDelta.x, text.preferredHeight);
	}
}
