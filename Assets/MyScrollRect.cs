using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyScrollRect : ScrollRect {

    [SerializeField] float clamp_to_nearest_multiple;
    [SerializeField] int min, max;
    [SerializeField] float time_to_clamp;

    Coroutine lerp;

    public override void OnEndDrag(PointerEventData eventData) {
        if (lerp != null) {
            StopCoroutine(lerp);
        }
        lerp = StartCoroutine(LerpToClamp(time_to_clamp));
    }

    IEnumerator LerpToClamp(float time) {
        Vector2 start = content.anchoredPosition;

        Vector2 goal = new Vector2(RoundToNearestMultiple(content.anchoredPosition.x, clamp_to_nearest_multiple), content.anchoredPosition.y);
        if (goal.x < min * clamp_to_nearest_multiple) {
            goal.x = min * clamp_to_nearest_multiple;
        } else if (goal.y > max * clamp_to_nearest_multiple) {
            goal.y = max *clamp_to_nearest_multiple ;
        }
        float start_time = time;
        while (time > 0) {
            time -= Time.deltaTime;
            content.anchoredPosition = start + ((goal - start) * (1 - time / start_time));
            yield return null;
        }

        content.anchoredPosition = goal;
    }

    float RoundToNearestMultiple(float x, float multiple) {
        if (multiple == 0) {
            return 0;
        }

        float remainder = Mathf.Abs(x) % multiple;

        float sign = Mathf.Sign(x);
        
        if (remainder < (multiple / 2f)) {
            return x -  (sign * remainder);
        } else {
            return x + (sign * (multiple - remainder));
        }
    }
}
