/****************************************************
 *  (c) 2013 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;
using NarayanaGames.Common;

namespace NarayanaGames.Common.UI {
    /// <summary>
    ///     NGAlignment is responsible for everything that's related to
    ///     alignments, both on screen and for "inner anchors".
    /// </summary>
    public class NGAlignment {

        /// <summary>
        ///     Alignments and anchors for positioning relative to the screen (or a panel)
        ///     and positioning relative to an anchor.
        /// </summary>
        public enum ScreenAlign : int {
            /// <summary>
            ///     Align to the top left of the screen. Position is interpreted as
            ///     position.x normalized pixels right of left screen border, position.y
            ///     normalized pixels below top screen border.
            /// </summary>
            TopLeft = -3,
            /// <summary>
            ///     Align to the middle left of the screen. Position is interpreted as
            ///     position.x normalized pixels right of left screen border, position.y
            ///     normalized pixels below vertical middle of screen.
            /// </summary>
            MiddleLeft = -2,
            /// <summary>
            ///     Align to the bottom left of the screen. Position is interpreted as
            ///     position.x normalized pixels right of left screen border, position.y
            ///     normalized pixels above bottom screen border.
            /// </summary>
            BottomLeft = -1,
            /// <summary>
            ///     Align to the top center of the screen. Position is interpreted as
            ///     position.x normalized pixels right of horizontal center of screen, position.y
            ///     normalized pixels below top screen border.
            /// </summary>
            TopCenter = 0,
            /// <summary>
            ///     Align to the middle center of the screen. Position is interpreted as
            ///     position.x normalized pixels right of horizontal center of screen, position.y
            ///     normalized pixels below vertical middle of screen.
            /// </summary>
            MiddleCenter = 1,
            /// <summary>
            ///     Align to the bottom center of the screen. Position is interpreted as
            ///     position.x normalized pixels right of horizontal center of screen, position.y
            ///     normalized pixels above bottom screen border.
            /// </summary>
            BottomCenter = 2,
            /// <summary>
            ///     Align to the top right of the screen. Position is interpreted as
            ///     position.x normalized pixels left of right screen border, position.y
            ///     normalized pixels below top screen border.
            /// </summary>
            TopRight = 6,
            /// <summary>
            ///     Align to the middle right of the screen. Position is interpreted as
            ///     position.x normalized pixels left of right screen border, position.y
            ///     normalized pixels below vertical middle of screen.
            /// </summary>
            MiddleRight = 7,
            /// <summary>
            ///     Align to the bottom right of the screen. Position is interpreted as
            ///     position.x normalized pixels left of right screen border, position.y
            ///     normalized pixels above bottom screen border.
            /// </summary>
            BottomRight = 8
        }

        /// <summary>
        ///     Values for vertical alignments.
        /// </summary>
        public enum VerticalAlign : int {
            /// <summary>
            ///     Aligned to or anchored at the top.
            /// </summary>
            Top = -1,
            /// <summary>
            ///     Aligned to or anchored at the middle.
            /// </summary>
            Middle = 0,
            /// <summary>
            ///     Aligned to or anchored at the bottom.
            /// </summary>
            Bottom = 1
        }

        /// <summary>
        ///     Values for horizontal alignments.
        /// </summary>
        public enum HorizontalAlign : int {
            /// <summary>
            ///     Aligned to or anchored left.
            /// </summary>
            Left = -1,
            /// <summary>
            ///     Aligned to or anchored center.
            /// </summary>
            Center = 0,
            /// <summary>
            ///     Aligned to or anchored right.
            /// </summary>
            Right = 1
        }

        /// <summary>
        ///     Returns the horizontal alignment portion of an alignment.
        /// </summary>
        /// <param name="align">the full alignment / anchor</param>
        /// <returns>the horizontal part of align</returns>
        public static HorizontalAlign Horizontal(ScreenAlign align) {
            switch (align) {
                case NGAlignment.ScreenAlign.TopLeft:
                case NGAlignment.ScreenAlign.MiddleLeft:
                case NGAlignment.ScreenAlign.BottomLeft:
                    return HorizontalAlign.Left;
                case NGAlignment.ScreenAlign.TopCenter:
                case NGAlignment.ScreenAlign.MiddleCenter:
                case NGAlignment.ScreenAlign.BottomCenter:
                    return HorizontalAlign.Center;
                case NGAlignment.ScreenAlign.TopRight:
                case NGAlignment.ScreenAlign.MiddleRight:
                case NGAlignment.ScreenAlign.BottomRight:
                    return HorizontalAlign.Right;
            }
            // Fallback, should never be called
            return HorizontalAlign.Center;
        }

        /// <summary>
        ///     Returns the vertical alignment portion of an alignment.
        /// </summary>
        /// <param name="align">the full alignment / anchor</param>
        /// <returns>the vertical part of align</returns>
        public static VerticalAlign Vertical(ScreenAlign align) {
            switch (align) {
                case NGAlignment.ScreenAlign.TopLeft:
                case NGAlignment.ScreenAlign.TopCenter:
                case NGAlignment.ScreenAlign.TopRight:
                    return VerticalAlign.Top;
                case NGAlignment.ScreenAlign.MiddleLeft:
                case NGAlignment.ScreenAlign.MiddleCenter:
                case NGAlignment.ScreenAlign.MiddleRight:
                    return VerticalAlign.Middle;
                case NGAlignment.ScreenAlign.BottomLeft:
                case NGAlignment.ScreenAlign.BottomCenter:
                case NGAlignment.ScreenAlign.BottomRight:
                    return VerticalAlign.Bottom;
            }
            // Fallback, should never be called
            return VerticalAlign.Middle;
        }

        /// <summary>
        ///     Shortcut to quickly determine whether an alignment is right aligned.
        ///     This is useful because usually, right aligned needs to special treatment.
        /// </summary>
        /// <param name="align">the alignment</param>
        /// <returns>whether align is one of TopRight, MiddleRight, BottomRight</returns>
        public static bool IsRight(ScreenAlign align) {
            return Horizontal(align) == HorizontalAlign.Right;
        }

        /// <summary>
        ///     Shortcut to quickly determine whether an alignment is bottom aligned.
        ///     This is useful because usually, bottom aligned needs to special treatment.
        /// </summary>
        /// <param name="align">the alignment</param>
        /// <returns>whether align is one of BottomLeft, BottomCenter, BottomRight</returns>
        public static bool IsBottom(ScreenAlign align) {
            return Vertical(align) == VerticalAlign.Bottom;
        }





        /// <summary>
        ///     Returns an offset to render an item anchored via innerAnchor.
        /// </summary>
        /// <remarks>
        ///     For example, if an item should be right-aligned, this will return
        ///     <c>-pos.width</c> so that the right border of the item is located
        ///     at the position of the item (in other words, it's anchored on its
        ///     right edge).
        /// </remarks>
        /// <param name="innerAnchor">the anchor / alignment</param>
        /// <param name="pos">the full size position of the item to be rendered</param>
        /// <returns>
        ///     an offset to be applied to pos to have the item rendered according to innerAnchor
        /// </returns>
        public static Vector2 GetAlignBasedOffset(NGAlignment.ScreenAlign innerAnchor, Rect pos) {
            float x = 0;
            // determine x-position
            switch (Horizontal(innerAnchor)) {
                case HorizontalAlign.Left: x = 0; break;
                case HorizontalAlign.Center: x = -pos.width * 0.5F; break;
                case HorizontalAlign.Right: x = -pos.width; break;
            }

            // determine y-position and return the complete position vector
            switch (Vertical(innerAnchor)) {
                case VerticalAlign.Top: return new Vector2(x, 0);
                case VerticalAlign.Middle: return new Vector2(x, -pos.height * 0.5F);
                case VerticalAlign.Bottom: return new Vector2(x, -pos.height);
            }

            // fallback - should never be called
            return Vector2.zero;
        }

        /// <summary>
        ///     Converts pos from <em>relative to screen alignment</em> 
        ///     to a screen position that is <em>relative to screen alignment</em> using <c>align</c>.
        ///     This also converts pos to pixels (in case we have a high density display).
        /// </summary>
        /// <param name="align">the screen alignment</param>
        /// <param name="pos">the position relative to screen alignment</param>
        /// <param name="isInSceneView">needed to work around a Unity bug</param>
        /// <returns>the screen position</returns>
        public static Vector2 GetScreenBasedReferencePosition(NGAlignment.ScreenAlign align, Vector2 pos, bool isInSceneView) {
            float x = 0;
            // determine x-position
            switch (Horizontal(align)) {
                case HorizontalAlign.Left:
                    x = NGUtil.Scale(pos.x);
                    break;
                case HorizontalAlign.Center:
                    x = EditorSceneViewGUI.GetScreenWidth(isInSceneView) * 0.5F + NGUtil.Scale(pos.x);
                    break;
                case HorizontalAlign.Right:
                    x = EditorSceneViewGUI.GetScreenWidth(isInSceneView) - NGUtil.Scale(pos.x);
                    break;
            }

            // determine y-position and return the complete position vector
            switch (Vertical(align)) {
                case VerticalAlign.Top:
                    return new Vector2(x, NGUtil.Scale(pos.y));
                case VerticalAlign.Middle:
                    return new Vector2(x, EditorSceneViewGUI.GetScreenHeight(isInSceneView) * 0.5F + NGUtil.Scale(pos.y));
                case VerticalAlign.Bottom:
                    return new Vector2(x, EditorSceneViewGUI.GetScreenHeight(isInSceneView) - NGUtil.Scale(pos.y));
            }

            // fallback - should never be called
            return Vector2.zero;
        }




        /// <summary>
        ///     Various alignment types, coming from different packages (everyone has their
        ///     own way of naming and ordering these ;-) ).
        /// </summary>
        public enum AlignmentType {
            /// <summary>
            ///     The standard TextAnchor alignment from Unity, used by GUIText and UnityGUI.
            /// </summary>
            TextAnchor,
            /// <summary>
            ///     The standard text alignment (left / center / right).
            /// </summary>
            TextAlignment,
            /// <summary>
            ///     UIWidget.Pivot from NGUI. 
            /// </summary>
            NGUI_Pivot,
            /// <summary>
            ///     SpriteText.Alignment_Type from EZ GUI.
            /// </summary>
            EZGUI_Alignment,
            /// <summary>
            ///     SpriteText.Anchor_Pos from EZ GUI.
            /// </summary>
            EZGUI_Anchor
        }

        /// <summary>
        ///     Converts an alignment to another enum (e.g. Unity's built in types, NGUI or EZ GUI).
        /// </summary>
        /// <param name="screenAlign">alignment in our encoding</param>
        /// <param name="alignmentType">target alignment</param>
        /// <returns></returns>
        public static int ConvertAlignment(NGAlignment.ScreenAlign screenAlign, AlignmentType alignmentType) {
            switch (alignmentType) {
                case AlignmentType.TextAnchor:
                    switch (screenAlign) {
                        case NGAlignment.ScreenAlign.TopLeft: return (int)TextAnchor.UpperLeft;
                        case NGAlignment.ScreenAlign.MiddleLeft: return (int)TextAnchor.MiddleLeft;
                        case NGAlignment.ScreenAlign.BottomLeft: return (int)TextAnchor.LowerLeft;
                        case NGAlignment.ScreenAlign.TopCenter: return (int)TextAnchor.UpperCenter;
                        case NGAlignment.ScreenAlign.MiddleCenter: return (int)TextAnchor.MiddleCenter;
                        case NGAlignment.ScreenAlign.BottomCenter: return (int)TextAnchor.LowerCenter;
                        case NGAlignment.ScreenAlign.TopRight: return (int)TextAnchor.UpperRight;
                        case NGAlignment.ScreenAlign.MiddleRight: return (int)TextAnchor.MiddleRight;
                        case NGAlignment.ScreenAlign.BottomRight: return (int)TextAnchor.LowerRight;
                    }
                    break;
                case AlignmentType.TextAlignment:
                    switch (Horizontal(screenAlign)) {
                        case NGAlignment.HorizontalAlign.Left: return (int)TextAlignment.Left;
                        case NGAlignment.HorizontalAlign.Center: return (int)TextAlignment.Center;
                        case NGAlignment.HorizontalAlign.Right: return (int)TextAlignment.Right;
                    }
                    break;
                case AlignmentType.NGUI_Pivot:
                    switch (screenAlign) {
                        //TopLeft     0
                        //Top         1
                        //TopRight    2
                        //Left        3
                        //Center      4
                        //Right       5
                        //BottomLeft  6
                        //Bottom      7
                        //BottomRight 8
                        case NGAlignment.ScreenAlign.TopLeft: return 0; //(int) UIWidget.Pivot.TopLeft;
                        case NGAlignment.ScreenAlign.MiddleLeft: return 3; //(int) UIWidget.Pivot.Left;
                        case NGAlignment.ScreenAlign.BottomLeft: return 6; //(int) UIWidget.Pivot.BottomLeft;
                        case NGAlignment.ScreenAlign.TopCenter: return 1; //(int) UIWidget.Pivot.Top;
                        case NGAlignment.ScreenAlign.MiddleCenter: return 4; //(int) UIWidget.Pivot.Center;
                        case NGAlignment.ScreenAlign.BottomCenter: return 7; //(int) UIWidget.Pivot.Bottom;
                        case NGAlignment.ScreenAlign.TopRight: return 2; //(int) UIWidget.Pivot.TopRight;
                        case NGAlignment.ScreenAlign.MiddleRight: return 5; //(int) UIWidget.Pivot.Right;
                        case NGAlignment.ScreenAlign.BottomRight: return 8; //(int) UIWidget.Pivot.BottomRight;
                    }
                    break;
                case AlignmentType.EZGUI_Alignment:
                    //Left,
                    //Center,
                    //Right
                    switch (Horizontal(screenAlign)) {
                        case NGAlignment.HorizontalAlign.Left: return 0;
                        case NGAlignment.HorizontalAlign.Center: return 1;
                        case NGAlignment.HorizontalAlign.Right: return 2;
                    }
                    break;
                case AlignmentType.EZGUI_Anchor:
                    switch (screenAlign) {
                        //Upper_Left,
                        //Upper_Center,
                        //Upper_Right,
                        //Middle_Left,
                        //Middle_Center,
                        //Middle_Right,
                        //Lower_Left,
                        //Lower_Center,
                        //Lower_Right
                        case NGAlignment.ScreenAlign.TopLeft: return 0;
                        case NGAlignment.ScreenAlign.TopCenter: return 1;
                        case NGAlignment.ScreenAlign.TopRight: return 2;
                        case NGAlignment.ScreenAlign.MiddleLeft: return 3;
                        case NGAlignment.ScreenAlign.MiddleCenter: return 4;
                        case NGAlignment.ScreenAlign.MiddleRight: return 5;
                        case NGAlignment.ScreenAlign.BottomLeft: return 6;
                        case NGAlignment.ScreenAlign.BottomCenter: return 7;
                        case NGAlignment.ScreenAlign.BottomRight: return 8;
                    }
                    break;
            }

            // Fallback, should never be called
            return (int)TextAnchor.UpperLeft;
        }





    }
}