/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NarayanaGames.ScoreFlashComponent;

/// <summary>
///     Manages several instances of <see cref="ScoreFlash"/>. 
///     For an example how to use this, see <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>.
/// </summary>
[AddComponentMenu("ScoreFlash/ScoreFlashManager")]
public class ScoreFlashManager : MonoBehaviour {
    /// <summary>
    ///     Assures that there is only one instance of ScoreFlashManager available in any
    ///     scene by destroying any new instances that are being created.
    ///     Check this if you want to use the same ScoreFlashManager setup in all scenes;
    ///     uncheck this, if you want to use different setups in different scenes.
    /// </summary>
    public bool ensureSingleton = true;

    #region Unity Singleton Pattern
    // TODO: Consider adding a mode where ScoreFlashManager does not "DontDestroyOnLoad" like ScoreFlash has it!
    private static ScoreFlashManager instance = null;
    void Awake() {
        if (instance != null && instance != this) {
            if (ensureSingleton && !Application.isEditor) {
                Destroy(this.gameObject);
            }
            return;
        } else {
            instance = this;
        }
        if (ensureSingleton && !Application.isEditor) {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    /// <summary>
    ///     Provides access to the one Instance of ScoreFlashManager.
    /// </summary>
    private static ScoreFlashManager Instance {
        get {
            #region Secret Sauce to make "re-compile while playing" work ;-)
            /*
             * Three possible reasons this can be null: 
             * 1) code change while playing => recompile; this can be fixed on the fly
             * 2) you forgot to add the ScoreFlash object to your scene; fixing this is up to you!
             * 3) ScoreFlash.Instance was called from Awake(), which isn't supported
             */
            if (instance == null) {
                instance = (ScoreFlashManager)Object.FindObjectOfType(typeof(ScoreFlashManager));
                if (instance == null) {
                    Debug.LogError("Trying to access ScoreFlashManager.Instance but there "
                        + "is no gameobject with ScoreFlashManager attached in the scene! "
                        + "Please add the object! "
                        + "If you have such an object, you might have tried calling "
                        + "ScoreFlashManager.* from Awake() which is not supported!");
                } else {
                    Debug.Log("Restored ScoreFlashManager.Instance - most likely you did a "
                        + "recompile while playing, all is good, no worries ;-)");
                }
            }
            #endregion Secret Sauce to make "re-compile while playing" work ;-)
            return instance;
        }
    }
    #endregion Unity Singleton Pattern

    private Dictionary<string, IScoreFlash> scoreFlashInstances = new Dictionary<string, IScoreFlash>();

    /// <summary>
    ///     Gets a ScoreFlash instance by its name. 
    /// </summary>
    /// <remarks>
    ///     As this caches all ScoreFlash instances
    ///     in a dictionary, it is very fast (much faster than using 
    ///     <a href="http://docs.unity3d.com/Documentation/ScriptReference/GameObject.Find.html">GameObject.Find(...)</a>
    ///     which is used only when the ScoreFlash instance by <c>name</c> is not
    ///     stored in the cache, yet).
    ///     For an example how to use this, see <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>.
    /// </remarks>
    /// <param name="name">
    ///     The name of a ScoreFlash instance; it's best to use the "Copy Name" button from
    ///     the ScoreFlashManager custom inspector to get this name. This prevents lookups
    ///     failing because of mistyped names.
    /// </param>
    /// <returns>the ScoreFlash instance called <c>name</c></returns>
    public static IScoreFlash Get(string name) {
        if (!Instance.scoreFlashInstances.ContainsKey(name)) {
            GameObject potentialScoreFlash = GameObject.Find(name);
            IScoreFlash scoreFlash = (IScoreFlash) potentialScoreFlash.GetComponent<ScoreFlash>();
            if (scoreFlash != null) {
                Instance.scoreFlashInstances[name] = scoreFlash;
                if (((ScoreFlash)scoreFlash).transform.parent != Instance.transform) {
                    Debug.LogWarning(
                        string.Format(
                        "You should put all ScoreFlash instances including '{0}' as children below the ScoreFlashManager!",
                        name),
                        (ScoreFlash)scoreFlash
                        );
                }
            } else {
                Debug.LogError(
                    string.Format("No ScoreFlash by the name '{0}'; available score flash instances: {1}", name, Instance.ListScoreFlashNames()));
            }
        }

        if (Instance.scoreFlashInstances[name] == null) {
            Debug.LogError("ScoreFlash '{0}' seemingly was destroyed ... did you have it as child of ScoreFlashManager?");
#if !UNITY_FLASH // Unity Flash doesn't support Values.GetEnumerator() => FAIL!
            try {
                Instance.scoreFlashInstances[name] = Instance.scoreFlashInstances.Values.GetEnumerator().Current;
            } catch {
                Debug.LogError("Tried to find an alternative ScoreFlash but that failed ...");
            }
#endif
        }

        return Instance.scoreFlashInstances[name];
    }

    private string ListScoreFlashNames() {
        ScoreFlash[] scoreFlashInstances = (ScoreFlash[])GameObject.FindObjectsOfType(typeof(ScoreFlash));
        List<string> names = new List<string>();
        foreach (ScoreFlash scoreFlash in scoreFlashInstances) {
            names.Add(scoreFlash.name);
        }
        return string.Join(", ", names.ToArray());
    }
}
