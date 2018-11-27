using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FitBoardToScreen : MonoBehaviour {

    Camera cam;

    [SerializeField] GameObject board;
    [SerializeField] RectTransform ui_canvas;


    [SerializeField] Vector2 board_size;
    [SerializeField] float board_padding, ui_padding;
    [SerializeField] Vector3 offset_from_bottom;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();

        cam.orthographicSize = ((board_size.x * (1/cam.aspect))+ board_padding * 2)/2;

        board.transform.position = cam.ScreenToWorldPoint(new Vector3(cam.pixelRect.width/2f, 0, 10)) + offset_from_bottom;


        float remaining_size = (2 * cam.orthographicSize - (board_size.y + ui_padding));

        ui_canvas.sizeDelta = new Vector2(ui_canvas.sizeDelta.x, (remaining_size / ui_canvas.localScale.y));
    }
}
