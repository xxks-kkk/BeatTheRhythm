/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using System;
using System.Collections;
using UnityEngine;

using NarayanaGames.Common;
using NarayanaGames.ScoreFlashComponent;
using NarayanaGames.Common.UI;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Represents an individual message. This is a lightweight C# class;
    ///     no game object attached. So we can have a lot of these!
    /// </summary>
    public class ScoreMessage : IComparable
    {

        /// <summary>
        ///     The animation phase this ScoreMessage currently is in.
        /// </summary>
        public enum Phase {
            /// <summary>The message is fading in.</summary>
            FadeIn,
            /// <summary>The message can be read by the player.</summary>
            Read,
            /// <summary>The message is fading out.</summary>
            FadeOut
        }

        private Phase currentPhase = Phase.FadeIn;
        /// <summary>
        ///     The current phase of this ScoreMessage, either FadeIn, Read or FadeOut.
        /// </summary>
        public Phase CurrentPhase {
            get { return currentPhase; }
            internal set {
                if (currentPhase != value) {
                    currentPhase = value;
                    switch (currentPhase) {
                        case Phase.Read: ReadReached(); break;
                        case Phase.FadeOut: FadeOutReached(); break;
                    }
                }
            }
        }

        // NOTE: this can later be extended to call event handlers
        private void ReadReached() {
            if (freezeOnRead) {
                localTimeScale = 0;
            }
        }

        // NOTE: this can later be extended to call event handlers
        private void FadeOutReached() {

        }

        public string appendToText = "";

        private string text = "";
        /// <summary>
        ///     Get or set the current text of the message. Changing the 
        ///     text when the message is already floating may result in unexpected
        ///     behavior! If <c>isDebug</c> is active, the text is ignored and
        ///     a debug message is shown instead. See <see cref="ScoreFlash.isDebugMode"/>.
        /// </summary>
        public string Text {
            get {
                if (isDebug) {
                    switch (scoreFlash.currentDebugMode) {
                        case ScoreFlash.DebugMode.NormalMessages: 
                            return text;
                        case ScoreFlash.DebugMode.LocalPosition:
                            return string.Format("({0:0}, {1:0})", pos.x, pos.y);
                        case ScoreFlash.DebugMode.ScreenPosition:
                            return string.Format("({0:0}, {1:0})", Position.x, Position.y);
                        case ScoreFlash.DebugMode.FullRect:
                            return string.Format("(x:{0:0}, y:{1:0}, w:{2:0}, h:{3:0})", Position.x, Position.y, Position.width, Position.height);
                        case ScoreFlash.DebugMode.Velocity:
                            return string.Format("({0:0}, {1:0})", velocity.x, velocity.y);
                        case ScoreFlash.DebugMode.Age:
                            return string.Format("Age: {0:0.0}", Age);
                        case ScoreFlash.DebugMode.Alignment:
                            return string.Format("ScreenAlign: {0}, InnerAnchor: {1}", screenAlign, innerAnchor);
                    }

                    //return string.Format("velocity:{0}, pos:{1}, scale:{2:0.0}, fadeQuickly:{3}", velocity, pos, scale, fadeQuickly);
                    //return string.Format("velocity:{0}", velocity, pos, scale, fadeQuickly);
                    //return string.Format("{0}: ({1}x{2}), high density = {3}, age = {4:0.0}", text, position.width, position.height, NGUtil.IsHighDensityDisplay, Age);
                    //return string.Format("{0}, {1} - {2}", screenAlign, style.alignment, position);
                    return string.Format("{0}, {1} - {2}", screenAlign, innerAnchor, position);
                }
                return text + " " + appendToText;
            }
            set { text = value; }
        }

        /// <summary>
        ///     Updates the message currently displayed.
        /// </summary>
        /// <param name="message">
        ///     Either a simple text string, or your custom implementation of
        ///     <see cref="ScoreFlashRendererBase"/>, or any of: int, long,
        ///     float, double, Component (so you can also put in MonoBehaviours),
        ///     GameObject ... or any object that has a useful ToString method ;-)
        /// </param>
        public void UpdateMessage(object message) {
            if (message is ScoreFlashRendererBase) {
                scoreFlashRenderer = (ScoreFlashRendererBase)message;
            } else if (message is int) {
                int val = (int)message;
                this.Text = val > 0 ? string.Format("+{0}", val) : string.Format("{0}", val);
            } else if (message is long) {
                long val = (long)message;
                this.Text = val > 0 ? string.Format("+{0}", val) : string.Format("{0}", val);
            } else if (message is float) {
                float val = (float)message;
                this.Text = val > 0 ? string.Format("+{0:0.00}", val) : string.Format("{0:0.00}", val);
            } else if (message is double) {
                double val = (double)message;
                this.Text = val > 0 ? string.Format("+{0:0.0000}", val) : string.Format("{0:0.0000}", val);
            } else if (message is UnityEngine.Component) {
                UnityEngine.Component val = (UnityEngine.Component)message;
                this.Text = val.name;
            } else if (message is GameObject) {
                GameObject val = (GameObject)message;
                this.Text = val.name;
            } else if (message is string) {
                this.Text = (string)message;
            } else { // we don't know better? well, just use the usual ToString()
                this.Text = message.ToString();
            }
        }

        /// <summary>
        ///     Assigns a new color to this message.
        /// </summary>
        /// <remarks>
        ///     You can use this, for example, to change the color from green 
        ///     to red, when a value goes from "is fine" to "is dangerous".
        /// </remarks>
        /// <param name="color"></param>
        public void UpdateColor(Color color) {
            scoreFlash.AssignAlphaMultipliedColor(this, color);
        }

        /// <summary>
        ///     Returns true if we are currently rendering in the scene view.
        /// </summary>
        public bool IsSceneView {
            get {
                return scoreFlash.IsSceneView;
            }
        }

        private NGAlignment.ScreenAlign screenAlign;
        /// <summary>
        ///     Gets or sets the screen alignment of this message. Usually, you should
        ///     configure this in your ScoreFlash instance - but being able to set this
        ///     from code may help in some situations. You need to check this when
        ///     doing your own custom renderers!
        /// </summary>
        public NGAlignment.ScreenAlign ScreenAlign {
            get { return screenAlign; }
            set { screenAlign = value; }
        }

        private Rect position;
        /// <summary>
        ///     Get the current position of the message (in screen coordinates as used by
        ///     UnityGUI, top-left is (0, 0)!). Read only!
        /// </summary>
        public Rect Position {
            get { return position; }
            set { position = value; }
        }

        private NGAlignment.ScreenAlign innerAnchor;
        /// <summary>
        ///     Gets or sets the anchor of the message itself. Usually, you should
        ///     configure this in your ScoreFlash instance - but being able to set this
        ///     from code may help in some situations. You need to check this when
        ///     doing your own custom renderers!
        /// </summary>
        public NGAlignment.ScreenAlign InnerAnchor {
            get { return innerAnchor; }
            set { innerAnchor = value; }
        }

        private float rotation = 0;
        /// <summary>
        ///     Get the current rotation in degrees. You need to use this in custom renderers. Read only!
        /// </summary>
        public float Rotation {
            get { return rotation; }
            internal set { rotation = value; }
        }

        private bool isWarmUpMessage = false;
        internal void MakeWarmUpMessage() {
            isWarmUpMessage = true;
        }

        private float scale = 1.0F;
        /// <summary>
        ///     Get the current scale - 1 means "original scale". You need to use this in custom renderers. Read only!
        /// </summary>
        public float Scale {
            get { return isWarmUpMessage ? 0.0001F : scale; }
            internal set { scale = value; }
        }

        private Color currentTextColor = Color.white;
        /// <summary>
        ///     Get the color the text should be displayed with. You need to use this in custom renderers. Read only!
        /// </summary>
        public Color CurrentTextColor {
            get { return currentTextColor; }
            internal set { currentTextColor = value; }
        }

        private Color outlineColor = Color.black;
        /// <summary>
        ///     Get the color the outline should currently have. You need to use this in custom renderers. Read only!
        /// </summary>
        public Color OutlineColor {
            get { return outlineColor; }
            internal set { outlineColor = value; }
        }

        /// <summary>
        ///     Get the maximum width this score message is allowed to consume. Read only!
        /// </summary>
        public float MaxWidth {
            get {
                if (followLocation == null) {
                    return Mathf.Min(MaxWidthViaWidth, MaxWidthViaPadding);
                } else {
                    return MaxWidthViaWidth;
                }
            }
        }

        /// <summary>
        ///     Used internally.
        /// </summary>
        internal float MaxWidthViaWidth {
            get { return NGUtil.Scale(scoreFlashLayout.MaxWidth); }
        }

        /// <summary>
        ///     Used internally.
        /// </summary>
        internal float MaxWidthViaPadding {
            get {
                float maxWidthViaPadding = 10000;
                if (scoreFlashLayout.MinPaddingX >= 0) {
                    switch (NGAlignment.Horizontal(innerAnchor)) {
                        case NGAlignment.HorizontalAlign.Left:
                            maxWidthViaPadding = Screen.width - NGUtil.Scale(scoreFlashLayout.MinPaddingX) - ReferencePosition.x;
                            break;
                        case NGAlignment.HorizontalAlign.Center:
                            maxWidthViaPadding = Screen.width - 2F * NGUtil.Scale(scoreFlashLayout.MinPaddingX);
                            break;
                        case NGAlignment.HorizontalAlign.Right:
                            maxWidthViaPadding = ReferencePosition.x - NGUtil.Scale(scoreFlashLayout.MinPaddingX);
                            break;
                    }
                }
                return maxWidthViaPadding;
            }
        }

        internal float localTimeScale = 1;
        /// <summary>
        ///     Get or set the local time scale of this message. Use this to make individual
        ///     messages either move quicker (values above 1) or more slowly
        ///     (values between 0 and 1). If you set this to 0, the message stops
        ///     moving, so that way, you can make it "persistent".
        /// </summary>
        public float LocalTimeScale {
            get { return localTimeScale; }
            set { localTimeScale = Mathf.Clamp(value, 0, float.MaxValue); }
        }

        private bool freezeOnRead = false;
        /// <summary>
        ///     If <c>true</c>, this will set LocalTimeScale to 0 when the message reaches
        ///     its read state, resulting in the message becoming persistent.
        /// </summary>
        public bool FreezeOnRead {
            get { return freezeOnRead; }
            set { freezeOnRead = value; }
        }

        /// <summary>
        ///     If you want to move your message around while it's frozen, you can use this
        ///     to do that. But it only works when you are not following a 3D object and
        ///     generally may cause weird effects. So use this with caution!
        /// </summary>
        /// <param name="offset">how many pixels to move on x and y</param>
        public void Move(Vector2 offset) {
            if (followLocation != null) {
                Debug.LogError("You cannot call Move() when following a 3D object!");
            }
            referencePosition += offset;
        }

        #region Private Stuff! Helper Class to keep state of individual messages
        /// <summary>
        ///     Reference to the instance of ScoreFlash that manages and animates this message.
        /// </summary>
        internal ScoreFlash scoreFlash;
        /// <summary>
        ///     Reference to the instance of an object that controls layout aspects of
        ///     this message. This could either be ScoreFlash or ScoreFlashLayout.
        /// </summary>
        internal IHasVisualDesigner scoreFlashLayout;
        /// <summary>
        ///     An optional ScoreFlashRenderer that can be used instead of the UnityGUI based
        ///     approach. See <see cref="ScoreFlash.scoreFlashRenderer"/>.
        /// </summary>
        internal ScoreFlashRendererBase scoreFlashRenderer = null;
        /// <summary>
        ///     When true (set via ScoreFlash), instead of showing the actual message,
        ///     debug information will be shown ... if you need other information that
        ///     what you currently see, have a look at the code and change it (if you
        ///     have the full package with source code ... when using ScoreFlash.DLL,
        ///     you'll have to use what you have).
        ///     See <see cref="ScoreFlash.isDebugMode"/>.
        /// </summary>
        internal bool isDebug = false;

        //private float receiveTime = 0; // no longer using "receiveTime" as Time.time / Time.realtimeSinceStartup seems to be buggy
        private float age = 0;

        /// <summary>
        ///     Should this message use realtime or "game time" (using Time.timeScale)?
        ///     Set via <see cref="ScoreFlash.timeReference"/>. This cannot be changed
        ///     when the message is floating!
        /// </summary>
        internal bool useRealTime = true;

        private ScoreFlashFollow3DLocation followLocation;
        /// <summary>
        ///     Used to follow a 3D object. Set by ScoreFlash when a message is
        ///     pushed.
        /// </summary>
        /// <param name="follow3D">a helper object used to follow a game object</param>
        internal void SetScoreFlashFollow3D(ScoreFlashFollow3D follow3D) {
            innerAnchor = follow3D.innerAnchor;
            followLocation = follow3D.GetLocation();
        }
        /// <summary>
        ///     Used internally.
        ///     This can be used by 3D renders to get all relevant information.
        /// </summary>
        public ScoreFlashFollow3DLocation FollowLocation {
            get { return followLocation; }
        }
        /// <summary>
        ///     This can be used by 3D renders to get all relevant information.
        /// </summary>
        public ScoreFlashFollow3D Follow3D {
            get { return followLocation.Target; }
        }

        private bool isVisible = true;
        /// <summary>
        ///     Should this message currently be visible? You can set this; however,
        ///     if using ScoreFlashFollow3D, this may override, if invisible (in other
        ///     words: when the renderer of the ScoreFlashFollow3D is not visible,
        ///     the message will never be visible even if you assign true!)
        /// </summary>
        public bool IsVisible {
            set { isVisible = false; }
            get {
                if (FollowLocation != null) {
                    return isVisible && FollowLocation.IsVisible;
                }
                return isVisible;
            }
        }

        private bool isSceneView = false;
        internal void SetSceneView(bool isSceneView) {
            this.isSceneView = isSceneView;
        }

        private Vector2 originalPosition = Vector2.zero;
        /// <summary>
        ///     The original position of the message without applying any translation.
        ///     If using screen align, these are basically the 9 positions on screen
        ///     (top left, top center etc.); when using ScoreFlashFollow3D, this is
        ///     the location after the 3D translation has been applied, before the
        ///     2D translation is applied.
        /// </summary>
        public Vector2 OriginalPosition {
            get {
                if (followLocation == null) {
                    return originalPosition;
                } else {
                    Vector2 screenPosition = followLocation.GetCamera(isSceneView).WorldToScreenPoint(followLocation.CurrentTranslatedPosition);
                    screenPosition.y = EditorSceneViewGUI.GetScreenHeight(scoreFlash.IsSceneView) - screenPosition.y;
                    return screenPosition;
                }
            }
            set { originalPosition = value; }
        }

        private Vector2 referencePosition = Vector2.zero;
        /// <summary>
        ///     The "reference position" of this message. This is where the
        ///     message is located at the beginning of the read state, when
        ///     there is no offset. Used internally.
        /// </summary>
        internal Vector2 ReferencePosition {
            get {
                if (followLocation == null) {
                    return referencePosition;
                } else {
                    Vector2 screenPosition = OriginalPosition;
                    screenPosition += followLocation.CurrentScreenOffset;
                    return screenPosition;
                }
            }
            set { referencePosition = value; }
        }

        /// <summary>
        ///     The current offset. Used internally, during fade in phase.
        /// </summary>
        internal Vector2 pos = Vector2.zero;
        /// <summary>
        ///     The current velocity. Used internally, during reading and fade out phase.
        /// </summary>
        internal Vector2 velocity = Vector2.zero;

        /// <summary>
        ///     The current rotation speed. Used internally.
        /// </summary>
        internal float rotationSpeed = 40;

        // colors cached for each message to implement different colorSelectionModes
        /// <summary>Used internally.</summary>
        internal Color fadeInColor;
        /// <summary>Used internally.</summary>
        internal Color readColorStart;
        /// <summary>Used internally.</summary>
        internal Color readColorEnd;
        /// <summary>Used internally.</summary>
        internal Color fadeOutColor;

        /// <summary>Used internally when using UnityGUI for rendering.</summary>
        internal GUIStyle style;
        /// <summary>Used internally when using UnityGUI for rendering.</summary>
        internal GUIStyle styleOutline;


        /// <summary>Used internally.</summary>
        internal float OutlineColorAlpha {
            set { outlineColor.a = value; }
        }

        /// <summary>Used internally.</summary>
        internal ScoreMessage(ScoreFlash scoreFlash, IHasVisualDesigner scoreFlashLayout, object message) {
            this.scoreFlash = scoreFlash;
            this.scoreFlashLayout = scoreFlashLayout;

            if (scoreFlash.enabled) {
                UpdateMessage(message);
            }

            //this.receiveTime = CurrentTime;
            this.age = 0;
        }

        /// <summary>
        ///     Cleans up renderers after rendering is complete.
        /// </summary>
        internal void DestroyInstance() {
            if (scoreFlashRenderer != null) {
                scoreFlashRenderer.DestroyInstance();
            }
        }

        /// <summary>Used internally.</summary>
        internal float Age {
            get {
                return age;
                //return CurrentTime - receiveTime; 
            }
        }

        /// <summary>Used internally.</summary>
        internal void AddDeltaTimeToAge(float deltaTime) {
            this.age += deltaTime;
        }

        /// <summary>Used internally.</summary>
        internal void UpdateRenderer() {
            if (scoreFlashRenderer != null) {
                scoreFlashRenderer.UpdateMessage(this);
            }
            if (followLocation != null) {
                followLocation.Update();
            }
        }

        #endregion Private Stuff! Helper Class to keep state of individual messages

        /// <summary>
        ///     Compare by age. This is used by ScoreFlash to make sure
        ///     messages are always sorted correctly so that Dequeue() works
        ///     as expected and spreading messages works as expected.
        /// </summary>
        /// <param name="otherScoreMessage">another ScoreMessage to compare with</param>
        /// <returns>this.Age.CompareTo(other.Age) ;-)</returns>
        public int CompareTo(object otherScoreMessage) {
            if (!(otherScoreMessage is ScoreMessage)) {
                return 0;
            }
            ScoreMessage other = (ScoreMessage)otherScoreMessage;
#if UNITY_FLASH
            if (this.Age < other.Age) {
                return -1;
            } else if (this.Age > other.Age) {
                return 1;
            } else {
                return 0;
            }
#else
            return this.Age.CompareTo(other.Age);
#endif
        }
    }
}