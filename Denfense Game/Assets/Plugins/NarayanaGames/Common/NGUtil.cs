/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;

namespace NarayanaGames.Common {
    /// <summary>
    ///     Provides several static utility methods / properties to make my
    ///     life easier. As it makes my life easier, it hopefully will also
    ///     make your life easier. 
    /// </summary>
    /// <remarks>
    ///     Aside from several special "equal checks" (where objA.Equals(objB) fails)
    ///     and several "make object(s) readable" methods (where obj.ToString() fails)
    ///     this has several methods to handle portrait / landscape and normal density
    ///     / high density displays easy (think of Retina vs. non-Retina).
    /// </remarks>
    public class NGUtil {

        /// <summary>
        ///     Compares two animation curves and returns true, if their keys
        ///     are equal.
        /// </summary>
        /// <param name="a">the first animation curve</param>
        /// <param name="b">the second animation curve</param>
        /// <returns>true, if the keys of <c>a</c> and <c>b</c> are equal</returns>
        public static bool AreEqual(AnimationCurve a, AnimationCurve b) {
            if (a == null || b == null) {
                return false;
            }

            if (a.length != b.length)
                return false;

            for (int i = 0; i < a.length; i++) {
                if (!a.keys[i].Equals(b.keys[i])) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Compares two lists and returns true, if all items are equal.
        /// </summary>
        /// <param name="a">the first list</param>
        /// <param name="b">the second list</param>
        /// <returns>true, if all items of <c>a</c> and <c>b</c> are equal</returns>
        public static bool AreEqual(IList a, IList b) {
            if (a == null || b == null) {
                return false;
            }

            if (a.Count != b.Count)
                return false;

            for (int i = 0; i < a.Count; i++) {
                // null and null is equal (we say ;-) 
                if (a[i] == null && b[i] == null) {
                    continue;
                }
                // if both are not null, but one is => not equal!
                if (a[i] == null || b[i] == null) {
                    return false;
                }

                if (!a[i].Equals(b[i])) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Creates a user readable diff between two objects of same type.
        ///     Currently implements special handling for: IList (arrays and lists)
        ///     and AnimationCurve.
        /// </summary>
        /// <param name="a">an object that should be made readable</param>
        /// <param name="b">an object that should be made readable</param>
        /// <returns>a readable string representation of the changes between <c>a</c> and <c>b</c></returns>
        public static string ToReadableDiff(object a, object b) {
            // fallback: ToString()
            string aString = ToReadableString(a);
            string bString = ToReadableString(b);

            // special handling for arrays and lists
            if (a is IList && b is IList) {
                IList aList = (IList)a;
                IList bList = (IList)b;
                if (aList.Count != bList.Count) {
                    aString = string.Format("Count={0}", aList.Count);
                    bString = string.Format("Count={0}", bList.Count);
                } else { // same number of items => show first item that's different
                    for (int i = 0; i < aList.Count; i++) {
                        if (!aList[i].Equals(bList[i])) {
                            aString = string.Format("{0}:{1}", i, ToReadableString(aList[i]));
                            bString = string.Format("{0}:{1}", i, ToReadableString(bList[i]));
                            break;
                        }
                    }
                }
            }

            // with AnimationCurves, simply go into the keyframes ...
            if (a is AnimationCurve && b is AnimationCurve) {
                return ToReadableDiff(((AnimationCurve)a).keys, ((AnimationCurve)b).keys);
            }

            return string.Format("{0} -> {1}", aString, bString);
        }

        /// <summary>
        ///     Tries to put <c>a</c> into a readable form, if ToString fails
        ///     to do so.
        /// </summary>
        /// <param name="a">some object that should be made readable</param>
        /// <returns>hopefully a readable form of a ;-)</returns>
        public static string ToReadableString(object a) {
            if (a is Keyframe) {
                Keyframe aKeyframe = ((Keyframe)a);
                return string.Format("[{0:0.00}={1:0.00} (in: {2:0.0}, out: {3:0.0}; mode: {4}]",
                    aKeyframe.time, aKeyframe.value, aKeyframe.inTangent, aKeyframe.outTangent, aKeyframe.tangentMode);
            }

            return a == null ? "<None>" : a.ToString();
        }


        /// <summary>
        ///     Returns <c>true</c>, if according to Screen.width and Screen.height, we
        ///     are in portrait mode; and <c>false</c>, if we are more likely in 
        ///     landscape mode (according to Screen.width and Screen.height). A square
        ///     screen will return false (we define square as "still landscape" ;-) ).
        /// </summary>
        public static bool IsPortrait {
            get { return Screen.width < Screen.height; }
        }

        /// <summary>
        ///     Automatically scales your pixel definitions for widths, heights
        ///     or relative screen positions for high density (e.g. Retina)
        ///     displays. This either returns <c>pixels</c>, as it is, or
        ///     <c>pixels * 2</c> if we have a high density display.
        /// </summary>
        /// <param name="pixels">pixels for "normal" screens</param>
        /// <returns>pixels for the current screen density</returns>
        public static float Scale(float pixels) {
            return pixels * (IsHighDensityDisplay ? 2F : 1F);
        }

        /// <summary>
        ///     Automatically scales your pixel definitions for widths, heights
        ///     or relative screen positions for high density (e.g. Retina)
        ///     displays. This either returns <c>pixels</c>, as it is, or
        ///     <c>pixels * 2</c> if we have a high density display.
        /// </summary>
        /// <param name="pixels">position for "normal" screens</param>
        /// <returns>position for the current screen density</returns>
        public static Vector2 Scale(Vector2 pixels) {
            return pixels * (IsHighDensityDisplay ? 2F : 1F);
        }

        /// <summary>
        ///     Automatically scales only the width and height of a rectangle for
        ///     high density (e.g. Retina). This is useful for rectangles that
        ///     already have a correct position but the wrong dimensions.
        ///     Use with caution! ;-)
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Rect ScaleOnlySize(Rect pixels) {
            pixels.width *= (IsHighDensityDisplay ? 2F : 1F);
            pixels.height *= (IsHighDensityDisplay ? 2F : 1F);
            return pixels;
        }

        /// <summary>
        ///     Inverts the scale applied via <see cref="Scale(float)"/>. This 
        ///     is useful if you're grabbing dimensions from objects while we 
        ///     are in high density mode.
        /// </summary>
        /// <param name="scaledPixels">pixels with the current screen density</param>
        /// <returns>pixels for "normal" screens</returns>
        public static float ScaleInverse(float scaledPixels) {
            return scaledPixels * (IsHighDensityDisplay ? 0.5F : 1F);
        }

        /// <summary>
        ///     Inverts the scale applied via <see cref="Scale(float)"/>. This 
        ///     is useful if you're grabbing dimensions from objects while we 
        ///     are in high density mode.
        /// </summary>
        /// <param name="scaledPixels">pixels with the current screen density</param>
        /// <returns>pixels for "normal" screens</returns>
        public static Vector2 ScaleInverse(Vector2 scaledPixels) {
            return scaledPixels * (IsHighDensityDisplay ? 0.5F : 1F);
        }

        /// <summary>
        ///     Set this to true for testing high density screen layouts etc.
        ///     If this is <c>true</c>, IsHighDensityDisplay always returns
        ///     <c>true</c> regardless of whether we actually have a high density
        ///     display. This is useful to test high density GUI layouts in
        ///     the editor (which never has a Retina display ;-) ).
        /// </summary>
        public static bool ForceHighDensity = false;

        /// <summary>
        ///     Are we on a high density screen (e.g. Retina display)?
        /// </summary>
        /// <remarks>
        ///     Starting with
        ///     Unity 3.5 this uses Screen.dpi (which only works on mobile, though!) and
        ///     also does a sanity check for the actual screen resolution to prevent low
        ///     resolution but higher density Android devices to get the Retina stuff
        ///     (which will usually break screen designs that assume "iPhone is minimum").
        ///     As Unity 3.4.2 doesn't support Screen.dpi, we use a very simple heuristic
        ///     based on screen resolution that will only reliably catch iPhone 4, 4S, 5 
        ///     and iPad 3 (and Sharp IS03), and not so reliably (haven't tested) will 
        ///     also catch Galaxy Nexus, LG Optimus LTE, Sony Xperia S, Galaxy Note.
        ///     As this heuristic is based on screen resolutions, you might get high
        ///     density if some vendor chose to provide a display with 960x640 or
        ///     2048x1536 or 1136x640 even though it isn't high density. The Android
        ///     heuristic also checks against device names. If this fails for you,
        ///     please let me know the precise details and I'll fix it; you find my
        ///     contact information on 
        ///     <a href="http://narayana-games.net/Contact.aspx">narayana-games.net</a>.
        ///     <p>
        ///     We could also use the classes recommended by Android (LDPI, MDPI, HDPI, XHDPI)
        ///     but that would make everything much more complex without adding significant
        ///     benefit. To learn more on screen densities see 
        ///     <a href="http://www.teehanlax.com/blog/density-converter/">
        ///         Designing (and converting) for multiple mobile densities</a> and
        ///     <a href="http://developer.android.com/guide/practices/screens_support.html">
        ///         Supporting Multiple Screens</a>
        ///     </p>
        /// </remarks>
        public static bool IsHighDensityDisplay {
            get {
                if (ForceHighDensity) {
                    return true;
                }

                return Screen.dpi > 210 && Mathf.Min(Screen.width, Screen.height) >= 640;
            }
        }

        /// <summary>
        ///     Tests if a given renderer is visible from a specific camera.
        /// </summary>
        /// <param name="camera">the camera to test</param>
        /// <param name="renderer">the renderer to test</param>
        /// <returns><c>true</c>, if renderer is visible from camera</returns>
        public static bool IsRendererVisibleFrom(Camera camera, Renderer renderer) {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Tries to load an asset of type T from path.
        /// </summary>
        /// <typeparam name="T">the type of the asset</typeparam>
        /// <param name="path">the full pasth to the asset starting with <c>Assets/</c></param>
        /// <returns>the asset to be loaded, or null if it cannot be loaded</returns>
        public static T LoadAsset<T>(string path) where T : UnityEngine.Object {
            T result = null;
            try {
                result = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
            } catch {
                // nothing special to do here, result will be null => error is written to log
            }
            if (result == null) {
                Debug.LogError(string.Format("{0} is not assigned, and cannot find at its default path:\n{1}",
                    typeof(T).Name, path));
            }
            return result;
        }
#endif // UNITY_EDITOR


        #region Obsolete stuff
        /// <summary>
        ///     Obsolete - use 
        ///     <see cref="NGEasing.EaseOnCurve(AnimationCurve, float, float, float)"/>
        ///     instead!
        /// </summary>
        /// <param name="curve">the curve to drive easing</param>
        /// <param name="from">the value to start from</param>
        /// <param name="to">the value to ease to</param>
        /// <param name="time">
        ///     the time in the animation (if you use animation curves with 
        ///     time from 0 to 1, this works exactly as Mathf.Lerp)
        /// </param>
        /// <returns>a value between <c>from</c> and <c>to</c> based on <c>time</c></returns>
        [System.Obsolete("Use Easing.EaseOnCurve instead!")]
        public static float EaseOnCurve(AnimationCurve curve, float from, float to, float time) {
            return NGEasing.EaseOnCurve(curve, from, to, time);
        }

        /// <summary>
        ///     Obsolete - use 
        ///     <see cref="NGEasing.EaseOnCurve(AnimationCurve, Vector3, Vector3, float)"/>
        ///     instead!
        /// </summary>
        /// <param name="curve">the curve to drive easing</param>
        /// <param name="from">the value to start from</param>
        /// <param name="to">the value to ease to</param>
        /// <param name="time">
        ///     the time in the animation (if you use animation curves with 
        ///     time from 0 to 1, this works exactly as Mathf.Lerp)
        /// </param>
        /// <returns>a value between <c>from</c> and <c>to</c> based on <c>time</c></returns>
        [System.Obsolete("Use NGEasing.EaseOnCurve instead!")]
        public static Vector3 EaseOnCurve(AnimationCurve curve, Vector3 from, Vector3 to, float time) {
            return NGEasing.EaseOnCurve(curve, from, to, time);
        }

        /// <summary>
        ///     Obsolete - use 
        ///     <see cref="NGEasing.EaseOnCurve(AnimationCurve, Color, Color, float)"/>
        ///     instead!
        /// </summary>
        /// <param name="curve">the curve to drive easing</param>
        /// <param name="from">the value to start from</param>
        /// <param name="to">the value to ease to</param>
        /// <param name="time">
        ///     the time in the animation (if you use animation curves with 
        ///     time from 0 to 1, this works exactly as Mathf.Lerp)
        /// </param>
        /// <returns>a value between <c>from</c> and <c>to</c> based on <c>time</c></returns>
        [System.Obsolete("Use NGEasing.EaseOnCurve instead!")]
        public static Color EaseOnCurve(AnimationCurve curve, Color from, Color to, float time) {
            return NGEasing.EaseOnCurve(curve, from, to, time);
        }
        #endregion Obsolete stuff

    }
}