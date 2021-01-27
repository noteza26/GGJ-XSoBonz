using UnityEngine;
using UnityEditor;

/// <summary>
/// Hierarchy Window Group Header
/// http://diegogiacomelli.com.br/unitytips-hierarchy-window-group-header
/// </summary>
#if UNITY_EDITOR
// Editor specific code here
[InitializeOnLoad]


public static class HierarchyWindowGroupHeader
{
    public enum ColorChange
    {
        Red,
        Blue,
    }
    static HierarchyWindowGroupHeader()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null && gameObject.name.StartsWith("---", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, Color.gray);
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("-", "").ToUpperInvariant());
        }
        if (gameObject != null && gameObject.name.StartsWith("---Red", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, Color.red);
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("---Red", "").ToUpperInvariant());

        }
        if (gameObject != null && gameObject.name.StartsWith("---Blue", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, Color.blue);
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("---Blue", "").ToUpperInvariant());

        }
        if (gameObject != null && gameObject.name.StartsWith("---Green", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, Color.green);
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("---Green", "").ToUpperInvariant());
        }
    }
}
#endif