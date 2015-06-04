using UnityEngine;
using System.Collections;
using NarayanaGames.Common.UI;
#if UNITY_EDITOR
using UnityEditor;
using NarayanaGames.Common;
#endif 

namespace NarayanaGames.ScoreFlashComponent {
#if UNITY_EDITOR
    /// <summary>
    ///     Handles rendering the score flash visual designer into the scene view.
    /// </summary>
    public class ScoreFlashVisualDesigner {

        private IHasVisualDesigner designed = null;
        /// <summary>
        ///     Are we currently in design mode?
        /// </summary>
        public bool isDesignMode = false;

        private bool isSceneView = false;

        private bool changed = false;

        /// <summary>
        ///     Place holder text shown when in design mode.
        /// </summary>
        public string designText = "Hello world!";

        /// <summary>
        ///     Is the instance selected in the hierarchy? Used internally for editor magic.
        /// </summary>
        public bool IsSelected { get; set; }

        private NGDragButton dragButtonPositionDoNotEverAccess = null;
        private NGDragButton DragButtonPosition {
            get {
                if (dragButtonPositionDoNotEverAccess == null) {
                    Rect initialPos = new Rect(0, 0, 12, 12);
                    Vector2 initialMaxPos = new Vector2(EditorSceneViewGUI.GetScreenWidth(isSceneView), EditorSceneViewGUI.GetScreenHeight(isSceneView));
                    dragButtonPositionDoNotEverAccess = new NGDragButton(initialPos, initialMaxPos, designed.DefaultScoreFlash.editorIconSet.moveAround);
                }
                return dragButtonPositionDoNotEverAccess;
            }
        }
        /// <summary>
        ///     Used internally.
        /// </summary>
        public bool IsDraggingPosition {
            get { return DragButtonPosition.IsDragging; }
        }

        private NGDragButton dragButtonWidthDoNotEverAccess = null;
        private NGDragButton DragButtonWidth {
            get {
                if (dragButtonWidthDoNotEverAccess == null) {
                    Rect initialPos = new Rect(0, 0, 12, 12);
                    Vector2 initialMaxPos = new Vector2(EditorSceneViewGUI.GetScreenWidth(isSceneView), EditorSceneViewGUI.GetScreenHeight(isSceneView));
                    dragButtonWidthDoNotEverAccess = new NGDragButton(initialPos, initialMaxPos, designed.DefaultScoreFlash.editorIconSet.moveScaleWidth);
                }
                return dragButtonWidthDoNotEverAccess;
            }
        }
        /// <summary>
        ///     Used internally.
        /// </summary>
        public bool IsDraggingWidth {
            get { return DragButtonWidth.IsDragging; }
        }

        private NGDragButton dragButtonPaddingDoNotEverAccess = null;
        private NGDragButton DragButtonPadding {
            get {
                if (dragButtonPaddingDoNotEverAccess == null) {
                    Rect initialPos = new Rect(0, 0, 12, 12);
                    Vector2 initialMaxPos = new Vector2(EditorSceneViewGUI.GetScreenWidth(isSceneView), EditorSceneViewGUI.GetScreenHeight(isSceneView));
                    dragButtonPaddingDoNotEverAccess = new NGDragButton(initialPos, initialMaxPos, designed.DefaultScoreFlash.editorIconSet.moveScalePadding);
                }
                return dragButtonPaddingDoNotEverAccess;
            }
        }
        /// <summary>
        ///     Used internally.
        /// </summary>
        public bool IsDraggingPadding {
            get { return DragButtonPadding.IsDragging; }
        }

        /// <summary>
        ///     Constructs a new designer.
        /// </summary>
        /// <param name="designed">the object that is being designed</param>
        /// <param name="isDesignMode">are we currently in design mode?</param>
        /// <param name="designText">the text to be shown</param>
        public ScoreFlashVisualDesigner(IHasVisualDesigner designed, bool isDesignMode, string designText) {
            this.designed = designed;
            this.isDesignMode = isDesignMode;
            this.designText = designText;
        }

        #region Advanced Editor Scripting
#if UNITY_EDITOR

        /// <summary>
        ///     Renders the scene view GUI.
        /// </summary>
        /// <param name="sceneView">ignored</param>
        public void OnSceneViewGUI(SceneView sceneView) {
            if (!designed.IsDesignMode) {
                return;
            }

            designed.DefaultScoreFlash.SetIsSceneView(true);

            changed = false;
            //Tools.viewTool = ViewTool.None;

            ScoreMessage msg = null;

            Handles.BeginGUI();
            {
                Rect viewPort = new Rect(0, 0, Screen.width, Screen.height);
                GUI.BeginGroup(viewPort);
                {

                    msg = OnSceneGUI();

                    /*
                     * draw an invisible button so we don't accidentally lose focus 
                     * when clicking next to our buttons / handles (which is really 
                     * annoying when it happens)
                     * NOTE: Disabled this because it's a pretty severe change compared 
                     * to of how Unity usually works and this may be a problem for some 
                     * users.
                     */
                    if (!designed.IsMultiSelect && !(designed is ScoreFlashFollow3D)) { // ScoreFlashFollow3D needs selection in 3D space!
                        Color origColor = GUI.color;
                        Color alpha = GUI.color;
                        alpha.a = 0.1F;
                        GUI.color = alpha;
                        GUI.Button(viewPort, "");

                        GUI.color = origColor;
                    }
                }
                GUI.EndGroup();
                
            }
            Handles.EndGUI();

            if (msg != null && msg.FollowLocation != null && designed.PositionWorld.HasValue) {
                DrawWorldOffsetPositionHandle(msg);
            }

            if (IsDraggingPosition && msg != null) {
                DrawPositionHandles(msg);
            }
            if (IsDraggingPadding && msg != null) {
                DrawPaddingWidthHandles(msg, true);
            }
            if (IsDraggingWidth && msg != null) {
                DrawPaddingWidthHandles(msg, false);
            }

            if (changed || GUI.changed) {
                EditorUtility.SetDirty(((MonoBehaviour)designed));
            }

            if (msg != null) {
                msg.DestroyInstance();
            }

            //Event.current.Use();
        }

        private void DrawWorldOffsetPositionHandle(ScoreMessage msg) {
            if (msg.FollowLocation != null) {
                Handles.color = Color.white;
                Handles.DrawLine(
                    msg.FollowLocation.CurrentOriginalPosition,
                    msg.FollowLocation.CurrentTranslatedPosition);
            }

            Vector3 offsetRelativeToPosition = msg.FollowLocation.CurrentOriginalPosition + designed.PositionWorld.Value;
            Vector3 newOffsetRelativeToPosition = Handles.PositionHandle(offsetRelativeToPosition, Quaternion.identity);
            if (newOffsetRelativeToPosition != offsetRelativeToPosition) {
                designed.PositionWorld = newOffsetRelativeToPosition - msg.FollowLocation.CurrentOriginalPosition;
                changed = true;
            }
        }

        private void DrawPositionHandles(ScoreMessage msg) {
            Vector2 msgPos = FlipY(msg.Position);

            Vector2 refPoint = msg.OriginalPosition;
            Vector2 scrOri = FlipY(refPoint);

            Vector3 pointA = ScreenToWorldPoint(msgPos);
            Vector3 pointB = ScreenToWorldPoint(scrOri);

            Vector3 pointX = ScreenToWorldPoint(new Vector2(scrOri.x, msgPos.y));
            Vector3 pointY = ScreenToWorldPoint(new Vector2(msgPos.x, scrOri.y));

            Handles.color = Color.blue;
            Handles.DrawLine(pointA, pointB);
            Handles.DrawLine(pointB, pointY);
            Handles.DrawLine(pointB, pointX);

            Handles.color = Color.red;
            Handles.DrawLine(pointA, pointX);

            Handles.color = Color.green;
            Handles.DrawLine(pointA, pointY);
        }

        private void DrawPaddingWidthHandles(ScoreMessage msg, bool isPadding) {
            bool isRight = NGAlignment.IsRight(msg.InnerAnchor);
            
            float width = isPadding ? msg.MaxWidthViaPadding : msg.MaxWidthViaWidth;

            Rect dragHandle = GetPaddingWidthPosition(msg, width, isPadding);
            dragHandle.y += dragHandle.height * 0.5F;
            if (!isRight && isPadding || isRight && !isPadding) {
                dragHandle.x += dragHandle.width;
            }
            Vector2 msgPos = FlipY(dragHandle);

            Vector2 scrOri = Vector2.zero;
            if (isPadding) {
                if (!isRight) {
                    scrOri = new Vector2(EditorSceneViewGUI.GetScreenWidth(isSceneView), msgPos.y);
                } else {
                    scrOri = new Vector2(0, msgPos.y);
                }
            } else {
                if (!isRight) {
                    scrOri = new Vector2(msgPos.x - width + dragHandle.width, msgPos.y);
                } else {
                    scrOri = new Vector2(msgPos.x + width - dragHandle.width, msgPos.y);
                }
            }
            Vector3 pointA = ScreenToWorldPoint(msgPos);
            Vector3 pointB = ScreenToWorldPoint(scrOri);

            Handles.color = isPadding ? Color.green : Color.cyan;
            Handles.DrawLine(pointA, pointB);
        }

        private Vector3 ScreenToWorldPoint(Vector2 screenPoint) {
            return Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.current.nearClipPlane + 1));
        }

        private Vector2 FlipY(Rect input) {
            return FlipY(new Vector2(input.x, input.y));
        }

        private Vector2 FlipY(Vector2 input) {
            input.y = EditorSceneViewGUI.GetScreenHeight(isSceneView) - input.y;
            return input;
        }

        private ScoreMessage OnSceneGUI() {
            designed.DefaultScoreFlash.useGUILayout = false;

            ScoreMessage msg = null;

            if (Application.isEditor && IsSelected) {
                GUISkin skin = GUI.skin;
                GUI.skin = designed.DefaultScoreFlash.editorGUISkin;
                if (isDesignMode) {
                    msg = DrawEditorGUIDesign(true);
                }
                GUI.skin = skin;
            }
            return msg;
        }

        /// <summary>
        ///     Draws an in-editor GUI designer.
        /// </summary>
        /// <param name="isSceneView">are we currently in the scene view (or game view)?</param>
        /// <returns>a temporary ScoreMessage for rendering the GUI</returns>
        public ScoreMessage DrawEditorGUIDesign(bool isSceneView) {
            this.isSceneView = isSceneView;
            ScoreMessage msg = null;
            Color origColor = GUI.color;
            {
                // save rendering because for the design mode, we *must* use UnityGUI
                ScoreFlash.RenderingType savedRenderingType = designed.DefaultScoreFlash.rendering;
                if (savedRenderingType == ScoreFlash.RenderingType.CustomRenderer) {
                    designed.DefaultScoreFlash.rendering = ScoreFlash.RenderingType.UnityGUI_Font;
                }

                msg = DrawEditorGUIDesignExampleMessage(isSceneView);

                designed.DefaultScoreFlash.rendering = savedRenderingType;

                if (isSceneView && !designed.IsMultiSelect) {
                    DrawEditorGUIDesignScreenAlignment(msg);
                }


                if (isSceneView) {
                    float size = 12;
                    DrawEditorGUIDesignInnerAnchor(msg, size);

                    size = 14;
                    DrawEditorGUIDesignPosition(msg, size);
                }
            }
            GUI.color = origColor;
            return msg;
        }

        private void DrawEditorGUIDesignScreenAlignment(ScoreMessage msg) {
            Rect buttonPosition = new Rect(0, 0, 120, 40);
            NGAlignment.ScreenAlign oldAlign = designed.ScreenAlign;
            if (designed.SupportsScreenAlign && !designed.LockScreenAlign) {
                foreach (NGAlignment.ScreenAlign align in System.Enum.GetValues(typeof(NGAlignment.ScreenAlign))) {
                    if (align != designed.ScreenAlign) {
                        buttonPosition = DrawAlignmentButton(null, buttonPosition, align, align.ToString(), null);
                    }
                }
            }
            if (designed.Position.sqrMagnitude > 43 * 43) {
                buttonPosition.width = buttonPosition.height = 28;
                DrawAlignmentButton(msg, buttonPosition, designed.ScreenAlign, null, designed.DefaultScoreFlash.editorIconSet.moveReset);
            }
            if (oldAlign != designed.ScreenAlign) {
                changed = true;
            }
        }

        private Rect DrawAlignmentButton(ScoreMessage msg, Rect buttonPosition, NGAlignment.ScreenAlign align, string text, Texture icon) {
            Vector2 clampedPosition = new Vector2(Mathf.Clamp(designed.Position.x, -100F, 100F), Mathf.Clamp(designed.Position.y, -100F, 100F));
            if (!designed.SupportsScreenAlign) {
                align = NGAlignment.ScreenAlign.MiddleCenter;
            }
            if (msg != null) {
                clampedPosition = Vector2.zero;
            }
            Vector2 posForButton = (msg == null)
                ? NGAlignment.GetScreenBasedReferencePosition(align, clampedPosition, isSceneView)
                : msg.OriginalPosition;


            Vector2 offset = NGAlignment.GetAlignBasedOffset(align, buttonPosition);
            buttonPosition.x = Mathf.Clamp(posForButton.x + offset.x, 0F, EditorSceneViewGUI.GetScreenWidth(isSceneView) - buttonPosition.width);
            buttonPosition.y = Mathf.Clamp(posForButton.y + offset.y, 0F, EditorSceneViewGUI.GetScreenHeight(isSceneView) - buttonPosition.height);
            Color col = GUI.color;
            if (IsDraggingPosition) {
                col.a = 0.4F;
            }
            if (clampedPosition != designed.Position) {
                col.a = 0.2F;
            }
            if (msg != null) {
                col.a = 1F;
            }
            GUI.color = col;
            if (msg == null) {
                if (GUI.Button(buttonPosition, text)) {
                    designed.ScreenAlign = align;
                }
            } else { // otherwise: reset position
                if (GUI.Button(buttonPosition, icon)) {
                    designed.Position = Vector2.zero;
                    changed = true;
                }
            }
            return buttonPosition;
        }

        private ScoreMessage DrawEditorGUIDesignExampleMessage(bool isSceneView) {
            ScoreMessage msg = designed.CreateDesignerMessage(designText);
            msg.SetSceneView(isSceneView);

            Rect pos = msg.Position;
            pos.x = msg.ReferencePosition.x; // +msg.pos.x; // we only care for the pure reference here
            pos.y = msg.ReferencePosition.y; // + msg.pos.y;
            msg.Position = pos;

            Rect localPos = msg.Position;
            Vector2 alignBasedOffset = ScoreFlash.GetAlignBasedOffset(msg);
            localPos.x += alignBasedOffset.x;
            localPos.y += alignBasedOffset.y;


            bool oldIsMax = designed.MaxWidth == 4000;
            if (designed is ScoreFlashFollow3D || designed.IsMultiSelect) {
                DrawMaxWidthBox(isSceneView, msg, msg.MaxWidthViaWidth, 0.6F, Color.cyan, false);
            } else {
                if (oldIsMax) { // width is maxed? => only draw padding
                    DrawMaxWidthBox(isSceneView, msg, msg.MaxWidthViaPadding, 0.6F, Color.green, true);
                } else {
                    if (msg.MaxWidthViaPadding > msg.MaxWidthViaWidth) {
                        DrawMaxWidthBox(isSceneView, msg, msg.MaxWidthViaPadding, 0.2F, Color.green, true);
                        DrawMaxWidthBox(isSceneView, msg, msg.MaxWidthViaWidth, 0.6F, Color.cyan, false);
                    } else {
                        DrawMaxWidthBox(isSceneView, msg, msg.MaxWidthViaWidth, 0.2F, Color.cyan, false);
                        DrawMaxWidthBox(isSceneView, msg, msg.MaxWidthViaPadding, 0.6F, Color.green, true);
                    }
                }
            }

            Color currentColor = Color.green;

            if (!Application.isPlaying || isSceneView) {
                currentColor = Color.yellow;
                currentColor.a = IsDraggingPosition && isSceneView ? 0.2F : 0.6F;
                GUI.color = currentColor;
                GUI.Box(localPos, "");

                if (isSceneView) {
                    bool newIsMax = DrawWidthLock(msg, oldIsMax, localPos, localPos);
                    if (oldIsMax != newIsMax) {
                        designed.MaxWidth = newIsMax ? 4000 : localPos.width;
                        changed = true;
                    }
                }
            }

            currentColor = msg.CurrentTextColor;
            currentColor.a = IsDraggingPosition && isSceneView ? 0.3F : 1F;
            GUI.color = currentColor;
            if (!Application.isPlaying || isSceneView) {
                if (msg.scoreFlashRenderer == null) {
                    GUI.Label(localPos, msg.Text, msg.style);
                } else {
                    msg.scoreFlashRenderer.UpdateMessage(msg);
                    if (msg.scoreFlashRenderer is IHasOnGUI) {
                        ((IHasOnGUI)msg.scoreFlashRenderer).OnGUI();
                    }
                }
            }
            GUI.color = Color.white;
            return msg;
        }

        private bool DrawWidthLock(ScoreMessage msg, bool oldIsMax, Rect lockPos, Rect localPos) {
            lockPos.width = designed.DefaultScoreFlash.editorIconSet.lockOpen.width;
            lockPos.height = designed.DefaultScoreFlash.editorIconSet.lockOpen.height;
            // trying to find a position for the lock that makes sense ... and can be used all the time ;-)
            if (!NGAlignment.IsRight(msg.InnerAnchor)) {
                lockPos.x += localPos.width + 3;
            } else {
                lockPos.x -= lockPos.width + 3;
            }
            lockPos.y = localPos.y + localPos.height + 3F;
            float xOnFlip = lockPos.x - lockPos.width;

            GUIContent lockClosed = new GUIContent(designed.DefaultScoreFlash.editorIconSet.lockClosed, "Set Max Width to Current");
            GUIContent lockOpen = new GUIContent(designed.DefaultScoreFlash.editorIconSet.lockOpen, "Set Max Width to Max");
            return EditorSceneViewGUI.ToggleWithToolTip(lockPos, xOnFlip, oldIsMax, lockClosed, lockOpen);
        }


        private void DrawMaxWidthBox(bool isSceneView, ScoreMessage msg, float width, float alpha, Color color, bool isPadding) {
            Rect maxPos = GetMaxRectPosition(msg, width);

            color.a = IsDraggingPosition && isSceneView ? 0.1F : alpha;
            GUI.color = color;
            GUI.Box(maxPos, "");

            if (isSceneView) {
                NGDragButton button = isPadding ? DragButtonPadding : DragButtonWidth;
                button.maxPosition.x = EditorSceneViewGUI.GetScreenWidth(isSceneView);
                button.maxPosition.y = EditorSceneViewGUI.GetScreenHeight(isSceneView);


                color.a = IsDraggingPosition && isSceneView ? 0.1F : (designed.LockScreenAlign ? 0.8F : 0.5F);
                GUI.color = color;
                button.position = GetPaddingWidthPosition(msg, width, isPadding);
                Vector2 diff = button.DrawMe();
                float diffX = diff.x;
                if (diffX != 0) {
                    NGAlignment.HorizontalAlign align = NGAlignment.Horizontal(msg.InnerAnchor);
                    changed = true;

                    if (isPadding) {
                        if (align != NGAlignment.HorizontalAlign.Right) {
                            diffX *= -1F; // invert
                        }
                        designed.MinPaddingX += diffX;
                    } else {
                        if (align == NGAlignment.HorizontalAlign.Right) {
                            diffX *= -1F; // invert
                        }
                        if (align == NGAlignment.HorizontalAlign.Center) {
                            diffX *= 2F; // 1 pixel => left / right => 2 pixels
                        }
                        designed.MaxWidth += diffX;
                    }
                }

                if (isPadding && IsDraggingPadding || !isPadding && IsDraggingWidth) {
                    if (isPadding) {
                        DragButtonPadding.DrawToolTip(isSceneView,
                            "Padding: {0}", designed.MinPaddingX);
                    } else {
                        DragButtonWidth.DrawToolTip(isSceneView,
                            "Max Width: {0}", designed.MaxWidth);
                    }
                    // drawing handles has to be done outside of "Handles.BeginGUI()" and Handles.EndGUI() => OnSceneViewGUI()
                }

            }
        }

        private Rect GetPaddingWidthPosition(ScoreMessage msg, float width, bool isPadding) {
            Rect maxPos = GetMaxRectPosition(msg, width);
            Rect paddingWidthPos = new Rect(maxPos);
            paddingWidthPos.width = paddingWidthPos.height = 28;
            paddingWidthPos.y = isPadding ? paddingWidthPos.y - paddingWidthPos.height : paddingWidthPos.y + maxPos.height;
            if (!NGAlignment.IsRight(msg.InnerAnchor)) {
                paddingWidthPos.x += maxPos.width - paddingWidthPos.width;
            } // otherwise: location is already fine :-)
            return paddingWidthPos;
        }

        private Rect GetMaxRectPosition(ScoreMessage msg, float width) {
            Rect maxPos = new Rect(msg.Position);
            maxPos.width = width;
            Vector2 alignBasedOffsetMax = NGAlignment.GetAlignBasedOffset(msg.InnerAnchor, maxPos);
            maxPos.x += alignBasedOffsetMax.x;
            maxPos.y += alignBasedOffsetMax.y;
            return maxPos;
        }


        private void DrawEditorGUIDesignInnerAnchor(ScoreMessage msg, float size) {
            Rect pos = msg.Position;
            pos.width = pos.height = size;
            if (!designed.LockInnerAnchor && !IsDraggingPosition) {
                pos.x = msg.Position.x + size * 0.5F; pos.y = msg.Position.y + size * 0.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.TopLeft)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.TopLeft;
                    changed = true;
                }
                pos.x = msg.Position.x - size * 0.5F; pos.y = msg.Position.y + size * 0.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.TopCenter)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.TopCenter;
                    changed = true;
                }
                pos.x = msg.Position.x - size * 1.5F; pos.y = msg.Position.y + size * 0.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.TopRight)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.TopRight;
                    changed = true;
                }
                pos.x = msg.Position.x + size * 0.5F; pos.y = msg.Position.y - size * 0.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.MiddleLeft)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.MiddleLeft;
                    changed = true;
                }
                pos.x = msg.Position.x - size * 0.5F; pos.y = msg.Position.y - size * 0.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.MiddleCenter)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.MiddleCenter;
                    changed = true;
                }
                pos.x = msg.Position.x - size * 1.5F; pos.y = msg.Position.y - size * 0.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.MiddleRight)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.MiddleRight;
                    changed = true;
                }
                pos.x = msg.Position.x + size * 0.5F; pos.y = msg.Position.y - size * 1.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.BottomLeft)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.BottomLeft;
                    changed = true;
                }
                pos.x = msg.Position.x - size * 0.5F; pos.y = msg.Position.y - size * 1.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.BottomCenter)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.BottomCenter;
                    changed = true;
                }
                pos.x = msg.Position.x - size * 1.5F; pos.y = msg.Position.y - size * 1.5F;
                if (EditorSceneViewGUI.Toggle(pos, designed.InnerAnchor != NGAlignment.ScreenAlign.BottomRight)) {
                    designed.InnerAnchor = NGAlignment.ScreenAlign.BottomRight;
                    changed = true;
                }
            } else {
                pos.x = msg.Position.x - size * 0.5F; pos.y = msg.Position.y - size * 0.5F;
                GUI.Box(pos, "");
            }


            pos.width = designed.DefaultScoreFlash.editorIconSet.lockOpen.width;
            pos.height = designed.DefaultScoreFlash.editorIconSet.lockOpen.height;

            pos.x = msg.Position.x + size * 1.5F; pos.y = msg.Position.y - size * 0.5F;
            if (pos.x + 2 * pos.width + 3 > Screen.width - 2) {
                pos.x = msg.Position.x - size * 2.5F - 2 * pos.width + 6;
            }

            // when we're going too far right for the tooltip, where should the tool tip start
            float xOnFlip = msg.Position.x - size * 2.5F;
            GUIContent lockClosed;
            GUIContent lockOpen;
            bool newLocked = false;
            if (designed.SupportsScreenAlign && designed.SupportsInnerAnchor) {
                lockClosed = new GUIContent(designed.DefaultScoreFlash.editorIconSet.lockClosed, "Define arbitrary inner anchors");
                lockOpen = new GUIContent(designed.DefaultScoreFlash.editorIconSet.lockOpen, "Lock Inner Anchors to Screen Align");
                newLocked = EditorSceneViewGUI.ToggleWithToolTip(pos, xOnFlip, designed.LockInnerAnchor, lockClosed, lockOpen);
                if (newLocked != designed.LockInnerAnchor) {
                    designed.LockInnerAnchor = newLocked;
                    changed = true;
                }
                pos.x += pos.width + 3;
            }

            if (designed.SupportsScreenAlign) {
                lockClosed = new GUIContent(designed.DefaultScoreFlash.editorIconSet.lockClosed, "Show Buttons for Screen Alignment");
                lockOpen = new GUIContent(designed.DefaultScoreFlash.editorIconSet.lockOpen, "Hide Buttons for Screen Alignment");
                newLocked = EditorSceneViewGUI.ToggleWithToolTip(pos, xOnFlip, designed.LockScreenAlign, lockClosed, lockOpen);
                if (newLocked != designed.LockScreenAlign) {
                    designed.LockScreenAlign = newLocked;
                    changed = true;
                }
            }
        }

        private void DrawEditorGUIDesignPosition(ScoreMessage msg, float size) {
            DragButtonPosition.position.width = DragButtonPosition.position.height = size * 2;

            if (designed.LockInnerAnchor) {
                DragButtonPosition.position.x = msg.Position.x - size;
                DragButtonPosition.position.y = msg.Position.y - size;
            } else {
                DragButtonPosition.position.x = msg.Position.x + size * 2.5F;
                if (NGAlignment.IsRight(designed.ScreenAlign)) {
                    DragButtonPosition.position.x = msg.Position.x - size * 3.5F;
                }
                DragButtonPosition.position.y = msg.Position.y + size * 2.5F;
                if (NGAlignment.IsBottom(designed.ScreenAlign)) {
                    DragButtonPosition.position.y = msg.Position.y - size * 3.5F;
                }
            }

            GUI.color = Color.white;
            DragButtonPosition.maxPosition.x = EditorSceneViewGUI.GetScreenWidth(isSceneView);
            DragButtonPosition.maxPosition.y = EditorSceneViewGUI.GetScreenHeight(isSceneView);
            Vector2 dragDistance = DragButtonPosition.DrawMe();

            Vector2 designedPosition = designed.Position;
            if (NGAlignment.IsRight(designed.ScreenAlign)) {
                designedPosition.x -= dragDistance.x;
            } else {
                designedPosition.x += dragDistance.x;
            }
            if (NGAlignment.IsBottom(designed.ScreenAlign)) {
                designedPosition.y -= dragDistance.y;
            } else {
                designedPosition.y += dragDistance.y;
            }
            if (designed.Position != designedPosition) {
                designed.Position = designedPosition;
                changed = true;
            }

            if (IsDraggingPosition) {
                DragButtonPosition.DrawToolTip(isSceneView,
                    "({0:0}, {1:0})", designed.Position.x, designed.Position.y);
                
                // drawing handles has to be done outside of "Handles.BeginGUI()" and Handles.EndGUI() => OnSceneViewGUI()
            }
        }
#endif // UNITY_EDITOR
        #endregion Advanced Editor Scripting

    }
#endif // UNITY_EDITOR
}