/*****************************************************
 *  (c) 2013 narayana games UG (haftungsbeschraenkt) *
 *  This class uses concepts discussed at:           *
 *  http://framebunker.com/blog/unity-singletons/    *
 *  Special credits go to Nicholas, Benoit & Emil    *
 *****************************************************/

using UnityEngine;

namespace NarayanaGames.Common {
    /// <summary>
    ///     A singleton base class for singletons that are only available in
    ///     the scene where they are actually attached to a game object.
    /// </summary>
    /// <typeparam name="T">Your class inheriting from this class</typeparam>
    public class NGSingleton<T> : MonoBehaviour where T : MonoBehaviour {

        private static T instance = null;
        /// <summary>
        ///     Assigns instance when the scene is loaded and logs a warning
        ///     when there already is an instance.
        /// </summary>
        public void Awake() {
            if (instance != null) {
                Debug.LogWarning(string.Format("There already is an instance "
                    +"of {0} in the scene.", TypeName), this);
            }
            instance = (T)(object)this;
        }

        /// <summary>
        ///     Assigns instance when code is compiled while playing.
        /// </summary>
        public void OnEnable() {
            if (instance == null) {
                Debug.Log(string.Format("Restored instance of {0}. Probably "
                    + "did a re-compile; all is good!", TypeName), this);
                instance = (T)(object)this;
            }
        }

        /// <summary>
        ///     Provides access to the one and only Instance of T. If no 
        ///     instance of T can be found, the editor is paused to prevent
        ///     the console being spammed with NullReferenceExceptions.
        /// </summary>
        public static T Instance {
            get {
#if UNITY_EDITOR
                #region Secret Sauce to make "re-compile while playing" work ;-)
                /*
                 * Three possible reasons this can be null: 
                 * 1) code change while playing => recompile; this can be fixed 
                 *    on the fly
                 * 2) you forgot to add the object to your scene; fixing this is 
                 *    up to you!
                 * 3) T.Instance was called from Awake(), which doesn't work
                 */
                if (instance == null) {
                    instance = (T)Object.FindObjectOfType(typeof(T));
                    if (instance == null) {
                        Debug.LogError(string.Format("Trying to access "
                            + "{0}.Instance but there is no gameobject with {0} "
                            + "attached in the scene! Please add the object! "
                            + "If you have such an object, you might have "
                            + "tried calling {0}.Instance from Awake() which is "
                            + "not supported, call from Start() instead!", TypeName));
                        Debug.Break(); 
                    } else {
                        Debug.Log(string.Format("Restored {0}.Instance - most "
                            + "likely you did a code-change while playing, all "
                            + "is good, no worries ;-)", TypeName));
                    }
                }
                #endregion Secret Sauce to make "re-compile while playing" work ;-)
#endif //UNITY_EDITOR
                return instance;
            }
        }

        private static string TypeName {
            get { return typeof(T).Name; }
        }

    }
}