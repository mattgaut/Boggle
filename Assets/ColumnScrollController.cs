using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnScrollController : MonoBehaviour {

    [SerializeField] RectTransform grid;

    [SerializeField] Button scroll_left, scroll_right;

    bool need_children_update = true;

    int column_position = 0;

    int children_per_column;
    int number_of_columns;

    float child_width;

    private void OnEnable() {
        if (need_children_update && grid.childCount > 0) {
            StartCoroutine(UpdateNextFrame());
        }
    }

    public void AddObject(GameObject obj, int position = -1) {
        obj.transform.SetParent(grid);
        if (position >= 0) obj.transform.SetSiblingIndex(position);
        obj.transform.localScale = grid.localScale;

        if (need_children_update && isActiveAndEnabled)
            StartCoroutine(UpdateNextFrame());
        else
            UpdateButtons();
    }

    IEnumerator UpdateNextFrame() {
        yield return null;

        RectTransform child_transform = grid.GetChild(0).GetComponent<RectTransform>();

        child_width = child_transform.rect.width;

        children_per_column = (int)(grid.rect.height / child_transform.rect.height);
        number_of_columns = (int)(grid.rect.width / child_width);

        need_children_update = false;

        UpdateButtons();
    }

    void UpdateButtons() {
        scroll_left.interactable = column_position > 0;

        scroll_right.interactable = (float)grid.childCount / children_per_column > column_position + number_of_columns;       
    }

    public void ScrollRight() {
        if (grid.childCount == 0) {
            return;
        }

        if ((float)grid.childCount / children_per_column > column_position + number_of_columns) {
            column_position++;

            grid.localPosition -= Vector3.right * child_width;

            UpdateButtons();
        }       
    }

    public void ScrollLeft() {
        if (column_position == 0 || grid.childCount == 0) {
            return;
        }
        column_position--;

        grid.localPosition += Vector3.right * child_width;

        UpdateButtons();
    }

    public void Clear() {

        for (int i = grid.childCount - 1; i >= 0; i--) {
            Destroy(grid.GetChild(i).gameObject);
        }
    }
}
