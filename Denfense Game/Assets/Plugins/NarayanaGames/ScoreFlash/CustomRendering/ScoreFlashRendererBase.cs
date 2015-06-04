/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *                                                  *
 *  If you want to support ScoreFlash with your     *
 *  GUI system, feel free to include this with your *
 *  package. However, make sure to use exactly the  *
 *  file that came with the ScoreFlash package so   *
 *  that Unity recognizes the Guid and overwrites   *
 *  it in case it is already there!                 *
 ****************************************************/

using UnityEngine;
using System.Collections;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Implement this adaptor for using custom renderers, like GUIText. This
    ///     cannot be attached to game objects! The custom renderers implementing
    ///     the abstract methods, however, can!
    /// </summary>
    /// <remarks>
    ///     For example, see <see cref="ScoreFlashRendererGUIText"/>,
    ///     <see cref="ScoreFlashRendererUnityGUI"/>, ...
    /// </remarks>
    public abstract class ScoreFlashRendererBase : MonoBehaviour {

        /// <summary>
        ///     If this returns <c>true</c>, slots for GUISkins are shown in
        ///     the ScoreFlash inspector.
        /// </summary>
        public abstract bool UsesGUISkin { get; }

        /// <summary>
        ///     If this returns <c>true</c>, a slot for a custom parent is
        ///     added to the ScoreFlash inspector. This is can be used e.g.
        ///     for NGUI where you should put the generated objects below
        ///     anchors or panels.
        /// </summary>
        public abstract bool RequiresCustomParent { get; }

        /// <summary>
        ///     Creates a new instance of this renderer that will handle rendering
        ///     for a given message. Used internally by ScoreFlash. Default
        ///     implementation simply uses 
        ///     <a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.Instantiate.html">Instantiate()</a>.
        /// </summary>
        /// <param name="parent">the parent transform this should go into</param>
        /// <returns>the new instance</returns>
        public virtual ScoreFlashRendererBase CreateInstance(Transform parent) {
            ScoreFlashRendererBase result = (ScoreFlashRendererBase)Instantiate(this);
            result.transform.parent = parent;
            return result;
        }

        /// <summary>
        ///     Is called when a renderer is put into the renderer pool so that it 
        ///     can do any necessary cleanup work. This is used e.g. for NoesisGUI
        ///     which has its own labels.
        /// </summary>
        public virtual void Cleanup() {
            // intentionally empty
        }

        /// <summary>
        ///     Needs to properly destroy the instance of this renderer. This is
        ///     called when the message has been animated through the complete animation.
        ///     Default implementation simply calls 
        ///     <a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.Destroy.html">Destroy()</a>.
        /// </summary>
        public virtual void DestroyInstance() {
            Cleanup();
            StartCoroutine(DestroyAfterDelay());
        }

        private IEnumerator DestroyAfterDelay() {
            if (this.gameObject != null) {
                if (!Application.isPlaying && Application.isEditor) {
                    yield return new WaitForEndOfFrame();
                    DestroyImmediate(this.gameObject);
                } else {
                    Destroy(this.gameObject);
                }
            }
        }

        /// <summary>
        ///     Can be used to initialize the message renderer.
        /// </summary>
        /// <param name="msg">the initial of the message</param>
        public virtual void Initialize(ScoreMessage msg) { }


        /// <summary>
        ///     The size of the message on screen. Depending on the underlying GUI system
        ///     you are using, there may be different ways of calculating the
        ///     size. This needs to be in screen coordinates.
        /// </summary>
        /// <param name="msg">the current version of the message</param>
        public abstract Vector2 GetSize(ScoreMessage msg);

        /// <summary>
        ///     Update the message. The implementation needs
        ///     to make sure that you update position, scale, rotation color and outline
        ///     color as well as the text ... and whether the message is currently visible!
        /// </summary>
        /// <remarks>
        ///     See also:
        ///     <list type="bullet">
        ///         <item><description><see cref="ScoreMessage.IsVisible"/></description></item>
        ///         <item><description><see cref="ScoreMessage.Text"/></description></item>
        ///         <item><description><see cref="ScoreMessage.position"/></description></item>
        ///         <item><description><see cref="ScoreMessage.rotation"/></description></item>
        ///         <item><description><see cref="ScoreMessage.scale"/></description></item>
        ///         <item><description><see cref="ScoreMessage.CurrentTextColor"/></description></item>
        ///         <item><description><see cref="ScoreMessage.OutlineColor"/></description></item>
        ///     </list>
        /// </remarks>
        /// <param name="msg">the current version of the message</param>
        public abstract void UpdateMessage(ScoreMessage msg);

        /// <summary>
        ///     Multiplies the alpha value of <c>b</c> with the alpha value
        ///     of <c>a</c> and returns <c>a</c>, which basically means that
        ///     you get <c>a</c> with an alpha value that can be animated with
        ///     <c>b</c>. This is used to adjust the alpha value of background
        ///     images that are used together with ScoreFlash prefabs.
        /// </summary>
        /// <param name="a">the color of the background</param>
        /// <param name="b">the color that has the alpha value</param>
        /// <returns><c>a</c> with the alpha multiplied with the alpha of <c>b</c></returns>
        public Color AlphaMultiplyColor(Color a, Color b) {
            a.a *= b.a;
            return a;
        }

    }
}