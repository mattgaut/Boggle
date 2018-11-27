using UnityEditor;

[CustomEditor(typeof(MyScrollRect))]
public class MyScrollViewEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}