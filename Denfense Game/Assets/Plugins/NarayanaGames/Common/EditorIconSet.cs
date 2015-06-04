/****************************************************
 *  (c) 2013 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using System.IO;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
///     Class with several useful icons for editor GUIs.
/// </summary>
public class EditorIconSet : ScriptableObject {

#if UNITY_EDITOR && AT_HOME // this isn't really useful for anyone but me ;-)
    [MenuItem("Assets/Create/narayana games/Editor Icon Set", priority = 10000)]
    public static void CreateEditorIconSetInstance() {
        EditorIconSet newEditorIconSet = CreateInstance<EditorIconSet>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path)) {
            path = "Assets/";
        } else if (Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(path), "");
        }

        string fullAssetPath = AssetDatabase.GenerateUniqueAssetPath(path + "EditorIconSet.asset");
        AssetDatabase.CreateAsset(newEditorIconSet, fullAssetPath);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newEditorIconSet;
    }
#endif

    /// <summary>
    ///     Icon for the open lock.
    /// </summary>
    public Texture lockOpen;
    /// <summary>
    ///     Icon for the closed lock.
    /// </summary>
    public Texture lockClosed;
    /// <summary>
    ///     Icon for moving around.
    /// </summary>
    public Texture moveAround;
    /// <summary>
    ///     Icon to reset the location.
    /// </summary>
    public Texture moveReset;
    /// <summary>
    ///     Icon for changing the scale.
    /// </summary>
    public Texture moveScale;
    /// <summary>
    ///     Icon for changing the scale padding.
    /// </summary>
    public Texture moveScalePadding;
    /// <summary>
    ///     Icon for changing the scale width.
    /// </summary>
    public Texture moveScaleWidth;

}
