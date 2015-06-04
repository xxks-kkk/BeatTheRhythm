/****************************************************
 *  (c) 2013 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;

namespace NarayanaGames.Common.UI {
    /// <summary>
    ///     A simple implementation of a button that can be dragged around.
    /// </summary>
    public class NGDragButton {

        /// <summary>
        ///     The current position of this DragButton. Can be directly
        ///     modified (because it is a public variable and not a public
        ///     property as usually in Unity).
        /// </summary>
        public Rect position = new Rect(10, 10, 20, 20);

        /// <summary>
        ///     The current max position - usually, this will be Screen.width
        ///     and Screen.height (but in editor scripting, we need to fix
        ///     some Unity bugs on the fly, so we're 
        /// </summary>
        public Vector2 maxPosition = Vector2.zero;

        // we don't need to see those outside ... for now ;-)
        private Vector2 lastMousePos = Vector2.zero;
        private Texture icon;

        private bool isDragging = false;
        /// <summary>
        ///     Is the button currently involved in a drag operation?
        /// </summary>
        public bool IsDragging {
            get { return isDragging; }
        }

        /// <summary>
        ///     Constructs a new drag button that keeps its state.
        /// </summary>
        /// <param name="position">the initial position</param>
        /// <param name="maxPosition">the maximum position to which this button can be dragged</param>
        /// <param name="icon">an icon to be drawn on the button</param>
        public NGDragButton(Rect position, Vector2 maxPosition, Texture icon) {
            this.position = position;
            this.maxPosition = maxPosition;
            this.icon = icon;
        }

        /// <summary>
        ///     Called from OnGUI to draw this DragButton.
        /// </summary>
        /// <returns>the distance dragged (if currently dragging), or Vector2.zero</returns>
        public Vector2 DrawMe() {
            return NGDragButton.DrawDragButton(ref lastMousePos, ref position, maxPosition, ref isDragging, icon);
        }

        /// <summary>
        ///     Draws a tooltip for this button, usually with the current coordinates.
        /// </summary>
        /// <param name="isSceneView">needed for a workaround in Unity 3.5</param>
        /// <param name="format">the text to be displayed, like with string.Format(...)</param>
        /// <param name="args">the arguments for the text</param>
        public void DrawToolTip(bool isSceneView, string format, params object[] args) {
            string coordinates = string.Format(format, args);
            Vector2 labelSize = GUI.skin.box.CalcSize(new GUIContent(coordinates));
            float origX = position.x;
            position.x = position.x + position.width + 10F;

            float newWidth = labelSize.x + 10F;

            if (position.x + newWidth > EditorSceneViewGUI.GetScreenWidth(isSceneView) - 20F) {
                position.x = origX - newWidth - 10F;
            }

            position.width = newWidth;
            //dragLabel.height = size * 2;


            GUI.color = Color.black;
            GUI.Box(position, "");
            position.x += 5F;
            position.y += (position.height - labelSize.y) * 0.5F;
            GUI.color = Color.white;
            GUI.Label(position, coordinates);
        }

        /// <summary>
        ///     This method can be called from anywhere using the usual "UnityGUI" style,
        ///     where you need to keep state in your own classes.
        /// </summary>
        /// <param name="lastMousePos">the last mouse position</param>
        /// <param name="position">the current position of the drag button</param>
        /// <param name="maxPosition">the maximum position on screen for dragging</param>
        /// <param name="isDragging">are we currently in a dragging operation</param>
        /// <param name="icon">icon on the button</param>
        /// <returns>the offset from the dragging operation - up to you to handle appropriately ;-)</returns>
        public static Vector2 DrawDragButton(ref Vector2 lastMousePos, ref Rect position, Vector2 maxPosition, ref bool isDragging, Texture icon) {
            Vector2 result = Vector2.zero;

            if (Event.current.isMouse) {
                switch (Event.current.type) {
                    case EventType.MouseDown:
                        // is this an actual click into the button?
                        isDragging = position.Contains(Event.current.mousePosition);
                        break;
                    case EventType.MouseUp: 
                        // NOTE: Sometimes, this got stuck - "return result" fixes that ... for some reason ;-)
                        isDragging = false;
                        return result; // se
                }
            }
            if (isDragging) {
                GUIStyle activeButton = GUI.skin.FindStyle("ActiveButton");
                GUI.Button(position, icon, activeButton);
            } else {
                GUI.Button(position, icon);
            }

            // when we drag out of the view port - stop dragging
            Vector2 m = Event.current.mousePosition;
            if (!(m.x >= 0 && m.y >= 0 && m.x < maxPosition.x && m.y < maxPosition.y)) {
                isDragging = false;
            }
            if (isDragging) {
                result = m - lastMousePos;
            }
            lastMousePos = Event.current.mousePosition;
            return result;
        }
    }
}