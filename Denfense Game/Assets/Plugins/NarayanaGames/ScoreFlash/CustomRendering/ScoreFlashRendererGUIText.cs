/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschr�nkt) *
 *  This is just an example, do with it whatever    *
 *  you like ;-)                                    *
 ****************************************************/

using UnityEngine;
using System.Collections;

using NarayanaGames.Common;
using NarayanaGames.Common.UI;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Attach this to a GUIText prefab and assign the prefab to an 
    ///     instance of <see cref="ScoreFlash"/> to have that ScoreFlash
    ///     render its messages using GUIText.
    /// </summary>
    /// <remarks>
    ///     While using GUIText is possible, it is not recommended. It adds
    ///     just as many draw calls as using UnityGUI does and does not
    ///     support scaling and rotating the texts. So consider this just
    ///     another example of how ScoreFlashRendererBase can be implemented.
    ///     One nice thing about GUIText is that it supports some special
    ///     rendering techniques, though ... so maybe there is some use
    ///     case for this ... it's up to you ;-)
    ///     Please let me know if you need a version of this that supports
    ///     high density displays (it's not hard but as I don't really see
    ///     use cases for this ATM, it wouldn't be worth the time to
    ///     implement it).
    /// </remarks>
    [ExecuteInEditMode()]
    [RequireComponent(typeof(GUIText))]
    public class ScoreFlashRendererGUIText : ScoreFlashRendererBase {

        private GUIText myGuiText;
        private Material myMaterial;

        void Awake() {
            myGuiText = this.GetComponent<GUIText>();
            if (Application.isPlaying) {
                myMaterial = myGuiText.material;
            }
        }

        #region Implementation of methods required by ScoreFlashRendererBase

        /// <summary>
        ///     Returns <c>false</c> because GUIText does not make use of GUISkins.
        /// </summary>
        public override bool UsesGUISkin {
            get { return false; }
        }

        /// <summary>
        ///     Returns <c>false</c> because GUIText does not need a custom parent.
        /// </summary>
        public override bool RequiresCustomParent {
            get { return false; }
        }

        /// <summary>
        ///     The size of the message on screen. Depending on the underlying GUI system
        ///     you are using, there may be different ways of calculating the
        ///     size. This needs to be in screen coordinates.
        /// </summary>
        /// <param name="msg">the current version of the message</param>
        public override Vector2 GetSize(ScoreMessage msg) {
            myGuiText.text = msg.Text;
            Rect size = myGuiText.GetScreenRect();
            return new Vector2(size.width, size.height);
        }

        /// <summary>
        ///     Update the message. This only handles text and position because
        ///     that's all that is supported by GUIText (shame on you, GUIText!)
        /// </summary>
        /// <param name="msg">the current version of the message</param>
        public override void UpdateMessage(ScoreMessage msg) {
            myGuiText.enabled = msg.IsVisible;
            myGuiText.text = msg.Text;
            if (myMaterial != null) {
                myMaterial.color = msg.CurrentTextColor;
            }

            myGuiText.anchor = (TextAnchor)NGAlignment.ConvertAlignment(msg.InnerAnchor, NGAlignment.AlignmentType.TextAnchor);
            myGuiText.alignment = (TextAlignment)NGAlignment.ConvertAlignment(msg.InnerAnchor, NGAlignment.AlignmentType.TextAlignment);

            // we need to convert screen position from pixel based to
            // "0-1"-based:
            Vector3 position = new Vector3(
                msg.Position.x / EditorSceneViewGUI.GetScreenWidth(msg.IsSceneView),
                1F - (msg.Position.y / EditorSceneViewGUI.GetScreenHeight(msg.IsSceneView)));
            myGuiText.transform.position = position;
        }

        #endregion Implementation of methods required by ScoreFlashRendererBase
    }
}