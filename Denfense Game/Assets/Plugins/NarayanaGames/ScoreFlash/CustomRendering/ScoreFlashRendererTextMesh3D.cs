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
    ///     Attach this to a Text Mesh prefab and assign the prefab to an 
    ///     instance of <see cref="ScoreFlash"/> to have that ScoreFlash
    ///     render its messages using Text Mesh. This is the 3D space version
    ///     that can also be used in VR environments (e.g. Oculus Rift).
    /// </summary>
    /// <remarks>
    ///     While using Text Mesh is possible, it is not recommended. You
    ///     might consider using 
    ///     <a href="https://www.assetstore.unity3d.com/en/#!/content/17662">Text Mesh Pro</a> 
    ///     instead, which is a lot more powerful. But to prevent creating any
    ///     external dependencies this is included.
    /// </remarks>
    [ExecuteInEditMode()]
    [RequireComponent(typeof(TextMesh))]
    public class ScoreFlashRendererTextMesh3D : ScoreFlashRendererBase {

        public TextMesh textMesh = null;
        public Transform textTransform = null;

        public float positionScale = 0.01F;

        #region Implementation of methods required by ScoreFlashRendererBase

        /// <summary>
        ///     Returns <c>false</c> because Text Mesh does not make use of GUISkins.
        /// </summary>
        public override bool UsesGUISkin {
            get { return false; }
        }

        /// <summary>
        ///     Returns <c>false</c> because we position these absolutely.
        /// </summary>
        public override bool RequiresCustomParent {
            get { return false; }
        }

        /// <summary>
        ///     Creates a new instance of this renderer that will handle rendering
        ///     for a given message. Used internally by ScoreFlash. Default
        ///     implementation simply uses 
        ///     <a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.Instantiate.html">Instantiate()</a>.
        /// </summary>
        /// <param name="parent">the parent transform this should go into</param>
        /// <returns>the new instance</returns>
        public override ScoreFlashRendererBase CreateInstance(Transform parent) {
            this.gameObject.layer = parent.gameObject.layer;

            ScoreFlashRendererBase result = (ScoreFlashRendererBase)Instantiate(this);
            result.transform.parent = parent;
            return result;
        }


        public override void Initialize(ScoreMessage msg) {
            textMesh.text = msg.Text;
            // msg.MaxWidth; // not supported by text mesh

            // We can't use this, yet, because NGAlignment doesn't support Text Mesh Pro, yet
            // I might clean this up in a future version; on the other hand, it's a nice example for adding
            // a custom renderer for a GUI framework that's not supported "out-of-the-box" ;-)
            //NGAlignment.ConvertAlignment(msg.InnerAnchor, NGAlignment.AlignmentType.NGUI_Pivot);

            ApplyScoreFlashAnchorToTextMesh(msg);
        }

        /// <summary>
        ///     The size of the message on screen. This is using UILabel.relativeSize.
        /// </summary>
        /// <param name="msg">the current version of the message</param>
        public override Vector2 GetSize(ScoreMessage msg) {
            textMesh.text = msg.Text;
            return new Vector2(
                textMesh.GetComponent<Renderer>().bounds.size.x,
                textMesh.GetComponent<Renderer>().bounds.size.y);
        }

        /// <summary>
        ///     Update the message.
        /// </summary>
        /// <param name="msg">the current version of the message</param>
        public override void UpdateMessage(ScoreMessage msg) {
            transform.position = msg.FollowLocation.CurrentTranslatedPosition;
            transform.LookAt(msg.Follow3D.referenceCamera.transform.position, msg.Follow3D.referenceCamera.transform.up);

            // are we visible (we may be behind the player when rendering ScoreFlashFollow3D stuff)
            textMesh.GetComponent<Renderer>().enabled = msg.IsVisible;

            //textMeshPro.lineLength = msg.MaxWidth;

            // push text and color to NGUI
            textMesh.text = msg.Text;
            textMesh.color = msg.CurrentTextColor;

            Vector2 pos = new Vector2(msg.Position.x + msg.Position.width * 0.5F, msg.Position.y + msg.Position.height * 0.5F);
            pos -= msg.OriginalPosition;

            // convert our coordinate system into World coordinate system
            Vector3 position = new Vector3(
                pos.x * positionScale,
                -pos.y * positionScale,
                0); // any value > 0 will do ;-)
            textTransform.localPosition = position;

            // ... ok, I guess there's are kind of obvious, aren't they?
            textMesh.transform.localScale = msg.Scale * Vector3.one;
            textTransform.localRotation = Quaternion.Euler(0, 180, -msg.Rotation);
        }

        private void ApplyScoreFlashAnchorToTextMesh(ScoreMessage msg) {
            textMesh.anchor = (TextAnchor)NGAlignment.ConvertAlignment(msg.InnerAnchor, NGAlignment.AlignmentType.TextAnchor);
            textMesh.alignment = (TextAlignment)NGAlignment.ConvertAlignment(msg.InnerAnchor, NGAlignment.AlignmentType.TextAlignment);
        }

        #endregion Implementation of methods required by ScoreFlashRendererBase
    }
}