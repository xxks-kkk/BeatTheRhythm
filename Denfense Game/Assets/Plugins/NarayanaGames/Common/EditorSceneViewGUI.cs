/****************************************************
 *  (c) 2013 narayana games UG (haftungsbeschr�nkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;

namespace NarayanaGames.Common {
    /// <summary>
    ///     Provides helper methods for scene view GUIs in the editor.
    /// </summary>
    public class EditorSceneViewGUI {

        /// <summary>
        ///     The scene view pretends to be about 2 pixels wider than it really
        ///     is (at least in Unity 3.5.7).
        /// </summary>
        public const float SceneViewWidthError = 2;

        /// <summary>
        ///     The scene view pretends to be 40 pixels higher than it really
        ///     is (at least in Unity 3.5.7).
        /// </summary>
        public const float SceneViewHeightError = 40;

        /// <summary>
        ///     Toggle that alterates between being a button and a box.
        /// </summary>
        /// <param name="position">location of the toggle</param>
        /// <param name="active">is the toggle active</param>
        /// <returns>was the toggle clicked</returns>
        public static bool Toggle(Rect position, bool active) {
            if (active) {
                return GUI.Button(position, "");
            } else {
                GUI.Box(position, "");
                return false;
            }
        }

        /// <summary>
        ///     A toggle with two icons representing the different states.
        /// </summary>
        /// <param name="pos">the position of the toggle</param>
        /// <param name="active">whether this toggle is toggled ;-)</param>
        /// <param name="iconActive">icon for active state</param>
        /// <param name="iconInactive">icon for inactive state</param>
        /// <returns>whether this toggle is toggled ;-)</returns>
        public static bool Toggle(Rect pos, bool active, GUIContent iconActive, GUIContent iconInactive) {
            return GUI.Toggle(pos, active, active ? iconActive : iconInactive);
        }

        /// <summary>
        ///     A toggle with two icons representing the different states, and tooltips for both states.
        /// </summary>
        /// <param name="pos">the position of the toggle</param>
        /// <param name="xOnFlip">when we're going too far right for the tooltip, where should the tool tip start</param>
        /// <param name="active">whether this toggle is toggled ;-)</param>
        /// <param name="contentActive">icon for active state, with tooltip text</param>
        /// <param name="contentInactive">icon for inactive state, with tooltip text</param>
        /// <returns>whether this toggle is toggled ;-)</returns>
        public static bool ToggleWithToolTip(Rect pos, float xOnFlip, bool active, GUIContent contentActive, GUIContent contentInactive) {
            active = Toggle(pos, active, contentActive, contentInactive);
            Rect tooltipPos = pos;
            Vector2 tooltipSize;
            if (!string.IsNullOrEmpty(GUI.tooltip)) {
                tooltipSize = GUI.skin.label.CalcSize(new GUIContent(GUI.tooltip));
                tooltipPos.width = tooltipSize.x + 10;
                tooltipPos.height = tooltipSize.y + 2;
                tooltipPos.y += 20;
                if (tooltipPos.x + tooltipPos.width > Screen.width - 10) {
                    tooltipPos.x = xOnFlip - tooltipPos.width;
                }
                GUI.Box(tooltipPos, GUI.tooltip);
            }
            GUI.tooltip = null;
            return active;
        }


        /// <summary>
        ///     Returns Screen.width or Screen.width minus <see cref="SceneViewWidthError"/>,
        ///     depending on whether we're in the scene view or not (this is to work around an offset
        ///     bug of Unity).
        /// </summary>
        /// <param name="isInSceneView">are we currently in the scene view?</param>
        /// <returns>Screen.width or Screen.width minus <see cref="SceneViewWidthError"/></returns>
        public static float GetScreenWidth(bool isInSceneView) {
            GameObject sceneCamera = GameObject.Find("SceneCamera");
            if (sceneCamera != null && isInSceneView) {
                return sceneCamera.GetComponent<Camera>().pixelRect.width;
            } else {
                return Screen.width;
            }

            //return isInSceneView ? Screen.width - SceneViewWidthError : Screen.width;
        }

        /// <summary>
        ///     Returns Screen.height or Screen.height minus <see cref="SceneViewHeightError"/>,
        ///     depending on whether we're in the scene view or not (this is to work around an offset
        ///     bug of Unity).
        /// </summary>
        /// <param name="isInSceneView">are we currently in the scene view?</param>
        /// <returns>Screen.height or Screen.height minus <see cref="SceneViewHeightError"/></returns>
        public static float GetScreenHeight(bool isInSceneView) {
            /*
             * Unfortunately, Unity has a pretty annoying bug that messes up 
             * Screen.height in SceneView; this workaround is only applied in 
             * the editor. Once UT has fixed this issue, this whole section
             * can be removed ...
             */
#if UNITY_EDITOR 
            if (isInSceneView) {
                GameObject sceneCamera = GameObject.Find("SceneCamera");
                if (sceneCamera != null) {
                    return sceneCamera.GetComponent<Camera>().pixelRect.height;
                }

            }
#endif
            return Screen.height;
        }
    }
}