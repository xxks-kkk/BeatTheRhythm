/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;

using NarayanaGames.Common;
using NarayanaGames.ScoreFlashComponent;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Attach this to a simple prefab with just a game object and assign that 
    ///     prefab to an instance of <see cref="ScoreFlash"/> to have that ScoreFlash
    ///     render its messages using GUIText.
    /// </summary>
    /// <remarks>
    ///     This is just an example for how ScoreFlashRendererBase can be implemented.
    ///     As ScoreFlash uses UnityGUI per default, you'll usually not use this
    ///     implementation!
    /// </remarks>
    public class ScoreFlashRendererUnityGUI : ScoreFlashRendererBase, IHasOnGUI {

        private ScoreMessage msg;

        #region Implementation of methods required by ScoreFlashRendererBase

        /// <summary>
        ///     Returns <c>true</c> because UnityGUI does use GUISkins.
        /// </summary>
        public override bool UsesGUISkin {
            get { return true; }
        }

        /// <summary>
        ///     Returns <c>false</c> because UnityGUI does not need a custom parent.
        /// </summary>
        public override bool RequiresCustomParent {
            get { return false; }
        }

        /// <summary>
        ///     Calculates the size using the GUIStyle from the message.
        /// </summary>
        /// <param name="msg">the current version of the message</param>
        public override Vector2 GetSize(ScoreMessage msg) {
            GUIContent msgText = new GUIContent(msg.Text);
            Vector2 size = msg.style.CalcSize(msgText);
            float maxWidth = msg.MaxWidth;
            if (size.x > maxWidth) {
                msg.style.wordWrap = true;
                size.x = maxWidth;
                size.y = msg.style.CalcHeight(msgText, size.x);
            }
            return size;
        }

        /// <summary>
        ///     Update the message. The implementation needs
        ///     to make sure that you update position, scale, color and outline
        ///     color as well as the text.
        /// </summary>
        /// <param name="msg">the current version of the message</param>
        public override void UpdateMessage(ScoreMessage msg) {
            this.msg = msg;
        }

        #endregion Implementation of methods required by ScoreFlashRendererBase

        private bool warningLogged = false;

        /// <summary>
        ///     Renders the message.
        /// </summary>
        public void OnGUI() {
            if (msg == null && !warningLogged) {
                warningLogged = true;
                string warningMsg = "OnGUI called before Initialize(...) was called "
                    + "- did you attach ScoreFlashRendererUnityGUI to an object in "
                    + "the scene? You should not!";
                Debug.LogWarning(warningMsg, this.gameObject);
            }
            if (msg == null || !msg.IsVisible) {
                return;
            }

            Matrix4x4 originalGUIMatrix = GUI.matrix;

            Vector2 alignBasedOffset = ScoreFlash.GetAlignBasedOffset(msg);

            Rect localPos = msg.Position;
            localPos.x += alignBasedOffset.x;
            localPos.y += alignBasedOffset.y;

            Vector2 pivotPoint = new Vector2(localPos.x + localPos.width * 0.5F, localPos.y + localPos.height * 0.5F);
            GUIUtility.ScaleAroundPivot(new Vector2(msg.Scale, msg.Scale), pivotPoint);
            GUIUtility.RotateAroundPivot(msg.Rotation, pivotPoint);

            msg.style.normal.textColor = msg.CurrentTextColor;
            msg.styleOutline.normal.textColor = msg.OutlineColor;

            bool renderOutline = !msg.scoreFlash.disableOutlines;
#if UNITY_IPHONE || UNITY_ANDROID
                renderOutline = false;
#endif //UNITY_IPHONE || UNITY_ANDROID


            if (!msg.scoreFlash.disableOutlines && (msg.scoreFlash.forceOutlineOnMobile || renderOutline)) {
                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {
                        if (x != 0 || y != 0) {
                            Rect posRecOutline = new Rect(localPos.x + x, localPos.y + y, msg.Position.width, msg.Position.height);
                            GUI.Label(posRecOutline, msg.Text, msg.styleOutline);
                        }
                    }
                }
            }

            GUI.Label(localPos, msg.Text, msg.style);

            GUI.matrix = originalGUIMatrix;
        }
    }
}