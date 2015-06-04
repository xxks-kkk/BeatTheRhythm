using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NarayanaGames.ScoreFlashComponent;
using NarayanaGames.Common.UI;

/// <summary>
///     ScoreFlashLayout can be used to have specific screen based locations 
///     for different messages. For example, you could have the players score
///     shown oriented at the top left of the screen, and the current task of
///     the player shown oriented at the top right of the screen; both using
///     the same instance of ScoreFlash.
/// </summary>
[ExecuteInEditMode()]
[AddComponentMenu("ScoreFlash/ScoreFlashLayout")]
public class ScoreFlashLayout : ScoreFlashBase, IScoreFlashLayout {

    #region Update Mechanism

    /// <summary>
    ///     Performs a couple of checks to ensure that everything is set 
    ///     up correctly after an upgrade to a new version.
    /// </summary>
    /// <returns><c>true</c>, if this instance was updated and changes need to be applied</returns>
    public override bool UpgradeCheck() {
        return base.UpgradeCheck();
    }

    #endregion Update Mechanism

    #region Properties for testing

    /// <summary>
    ///     Use this to show random messages.
    /// </summary>
    public bool isTestAutogenerateMessages = false;

    /// <summary>
    ///     The usual delay between two messages in seconds.
    ///     Use this to increase or decrease the frequency in which test messages
    ///     are being sent to ScoreFlash. NOTE: This does not
    ///     have an effect on messages sent via includeMessageSpam (these are
    ///     always sent in a burst without any delay.
    ///     Also be aware that this is based on real time (so Time.timeScale
    ///     has no effect).
    /// </summary>
    public float testMessageDelaySeconds = 3F;

    #endregion Properties for testing

    #region Main Layout Properties

    /// <summary>
    ///     Design mode allows you to use the scene view to set up the positioning
    ///     of this ScoreFlashLayout. This works both while playing and while not playing,
    ///     however, if you have moving options, you might want to pause the game to set 
    ///     things up (otherwise you might chase your messages around ;-) ).
    /// </summary>
    public bool isDesignMode = true;

    /// <summary>
    ///     Unity doesn't properly support updating the GameView when an object is
    ///     deselected, so if you disable this, the design will stay in the game
    ///     view until it is updated once more. I have a hack for this - but 
    ///     unfortunately, it's causing trouble when the object is being deleted 
    ///     (there's an exception that I cannot catch). Both is not a real problem,
    ///     it's just a little annoying.
    /// </summary>
    public bool enableDeselectHack = false;

    /// <summary>
    ///     Message that is shown while in design mode.
    /// </summary>
    public string designText = "+1024";

    /// <summary>
    ///     Locks the screen alignment. This is only used by the visual designer.
    /// </summary>
    public bool lockScreenAlign = false;

    /// <summary>
    ///     Controls whether the message is aligned to top, middle or bottom
    ///     of the screen. Defines how <see cref="position"/> is applied.
    ///     Possible values are defined by <see cref="NGAlignment.ScreenAlign"/>.
    /// </summary>
    public NGAlignment.ScreenAlign screenAlign = NGAlignment.ScreenAlign.MiddleCenter;

    /// <summary>
    ///     Position for messages pushed with <see cref="IScoreFlash.PushLocal(object)"/> 
    ///     in normalized pixels relative to <see cref="NGAlignment.ScreenAlign"/>.
    /// </summary>
    /// <remarks>
    ///     For example, if <see cref="screenAlign"/> is <see cref="NGAlignment.ScreenAlign.BottomRight"/>,
    ///     <c>(15, 10)</c> means 15 pixels left of the right border of the viewport, and
    ///     10 pixels above the bottom border. If a high density (Retina) display is
    ///     recognized, these values are automatically multiplied with 2!
    /// </remarks>
    public Vector2 position = Vector2.zero;

    /// <summary>
    ///     Locks the <see cref="innerAnchor"/> to <see cref="screenAlign"/>.
    /// </summary>
    /// <remarks>
    ///     Most of the time, you'll want to have the set to true because most of the time,
    ///     it just makes sense and is easier to configure. However, if you want to, for instance,
    ///     have numbers right aligned but you want to position them relative to the left
    ///     screen border, you'd set this to <c>true</c> and use screenAlign = *Left and
    ///     innerAnchor = *Right (e.g. TopLeft, TopRight).
    /// </remarks>
    public bool lockInnerAnchor = true;

    /// <summary>
    ///     Use this to control the anchor / alignment of the messages sent
    ///     using this instance of ScoreFlashLayout. 
    /// </summary>
    /// <remarks>
    ///     Most of the time, 
    ///     you'll want to keep this *Center (TopCenter, MiddleCenter or BottomCenter)
    ///     but you could also right align scores, or left align chat messages,
    ///     if you need to.
    /// </remarks>
    public NGAlignment.ScreenAlign innerAnchor = NGAlignment.ScreenAlign.MiddleCenter;


    /// <summary>
    ///     Minimum distance between the message and the left/right screen 
    ///     borders before the message is wrapped in two (or more) lines.
    ///     This works together with <see cref="maxWidth"/>.
    /// </summary>
    /// <remarks>
    ///     When center aligned (TopCenter, MiddleCenter, BottomCenter),
    ///     this uses both screen edges; note that this may not give you
    ///     perfectly correct results if using X-offsets! When left or
    ///     right align (all the others), this uses the actual x-position
    ///     and <see cref="innerAnchor"/> to find out where the message is
    ///     going and then assures that it doesn't get closer to the 
    ///     screen border the message is point to (i.e. left align results
    ///     in padding against the right screen border, right align
    ///     results in padding against the left screen boder).
    ///     Default is 20F. This assumes scale = 1; with scale &gt; 1,
    ///     the message might clip at the screen borders. Obviously, this
    ///     only has an effect with long messages. MinPaddingX is used,
    ///     when the resulting width is smaller than the one defined in
    ///     <see cref="maxWidth"/>. Set to a negative value if you only
    ///     want to use maxWidth (padding will then be ignored).
    ///     This uses normalized coordinates (i.e. if you are on a
    ///     high density display, e.g. Retina, the value will be
    ///     multiplied with 2).
    /// </remarks>
    public float minPaddingX = 20F;

    /// <summary>
    ///     Maximum width of the message before it's wrapped. This
    ///     works together with <see cref="minPaddingX"/>.
    /// </summary>
    /// <remarks>
    ///     Default is 260F. This assumes scale = 1; with scale &gt; 1,
    ///     the message might clip at the screen borders. Obviously, this
    ///     only has an effect with long messages. MaxWidth is used when
    ///     it is smaller than the maximum width resulting from
    ///     <see cref="minPaddingX"/>. If you want to not use maxWidth,
    ///     just set it to a very high value (e.g. 4000, which will be
    ///     larger than any relevant screen ;-) ).
    ///     This uses normalized coordinates (i.e. if you are on a
    ///     high density display, e.g. Retina, the value will be
    ///     multiplied with 2).
    /// </remarks>
    public float maxWidth = 600F;


    /// <summary>
    ///     Freeze any message pushed directly to this ScoreFlashLayout at its
    ///     read state.
    /// </summary>
    /// <remarks>
    ///     If checked, this will make any message pushed to this ScoreFlashLayout
    ///     via <see cref="Push(object)"/> or <see cref="Push(object, Color)"/>
    ///     (directly on the ScoreFlashLayout instance) freeze at the beginning
    ///     of their read phase, essentially making them persistent. You can use
    ///     this for name tags, HUDs or whatever you can think of that requires
    ///     being located relative to world objects. When pushing further messages, 
    ///     the current message will move on, so you can either change the ScoreMessage
    ///     or generate a new one. See <see cref="ScoreMessage.FreezeOnRead"/>.
    /// </remarks>
    public bool freezeOnRead = false;

    /// <summary>
    ///     If assigned, instead of using ScoreFlash.Instance, this ScoreFlashLayout
    ///     will use the assigned ScoreFlash instance when messages are pushed directly
    ///     via <see cref="Push(object)"/>.
    /// </summary>
    public ScoreFlash defaultScoreFlash = null;
    #endregion Main Layout Properties

    private ScoreMessage currentMessage = null;
    /// <summary>
    ///     Provides access to the current message, which is always the last message
    ///     that was sent through <see cref="Push(object)"/> or <see cref="Push(object, Color)"/>.
    ///     You can use this to change the message without going through the animations.
    ///     This is usually used together with <see cref="freezeOnRead"/>.
    /// </summary>
    public ScoreMessage CurrentMessage {
        get { return currentMessage; }
    }

    /// <summary>
    ///     Shortcut for pushing messages directly using this ScoreFlashLayout instance.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IScoreFlash.PushLocal(object)"/> on <see cref="defaultScoreFlash"/>
    ///     (or using the single ScoreFlash instance if defaultScoreFlash wasn't assigned).
    /// </remarks>
    /// <param name="message">the message to send, could also be a custom renderer (or any object)</param>
    /// <returns>return the ScoreMessage representing message</returns>
    public ScoreMessage Push(object message) {
        HandlePreviousMessage();
        messageForScoreFlash = message;
        currentMessage = DefaultScoreFlash.PushLocal(this, null, null, this.GetInstanceID());
        ApplyMessageSettings(currentMessage);
        return currentMessage;
    }

    private object messageForScoreFlash = null;
    internal object MessageForScoreFlash {
        get { return messageForScoreFlash; }
    }

    /// <summary>
    ///     Shortcut for pushing messages in a specific color directly using this ScoreFlashLayout instance.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IScoreFlash.PushLocal(object, Color)"/> on <see cref="defaultScoreFlash"/>
    ///     (or using the single ScoreFlash instance if defaultScoreFlash wasn't assigned).
    /// </remarks>
    /// <param name="message">the message to send, could also be a custom renderer (or any object)</param>
    /// <param name="color">
    ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
    ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
    ///     in the ScoreFlash instance that this message was pushed to
    /// </param>
    /// <returns>return the ScoreMessage representing msg</returns>
    public ScoreMessage Push(object message, Color color) {
        HandlePreviousMessage();
        messageForScoreFlash = message;
        currentMessage = DefaultScoreFlash.PushLocal(this, color, this.GetInstanceID());
        ApplyMessageSettings(currentMessage);
        return currentMessage;
    }

    private void HandlePreviousMessage() {
        if (currentMessage != null && currentMessage.FreezeOnRead) {
            currentMessage.FreezeOnRead = false; // make sure we don't "re-freeze" on accident
            currentMessage.LocalTimeScale = 1F;
        }
    }

    private void ApplyMessageSettings(ScoreMessage msg) {
        msg.FreezeOnRead = freezeOnRead;
    }





    #region HandleStuffForTesting
    void Update() {
        HandleStuffForTesting();
    }

    private Coroutine showTestMessages = null;

    private void HandleStuffForTesting() {
        // start or stop our testing coroutine
        if (isTestAutogenerateMessages && showTestMessages == null) {
            showTestMessages = StartCoroutine(ShowTestMessages());
        }
        if (!isTestAutogenerateMessages && showTestMessages != null) {
            showTestMessages = null;
        }
    }

    private float NormalizedTestMessageDelaySeconds {
        get { return testMessageDelaySeconds * (Time.timeScale + 0.00001F); }
    }

    private IEnumerator ShowTestMessages() {
        yield return new WaitForSeconds((0.2F + Random.Range(0F, 0.5F)) * (Time.timeScale + 0.00001F));
        while (showTestMessages != null) { // we can end this coroutine any time by setting it to null
            Push("+1000");

            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;

            Push("-11");

            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;

            Push("+888");

            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;

            Push("I could be sayin' somethin'!");

            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;
        }
    }

    #endregion HandleStuffForTesting





    #region Abstract Methods from ScoreFlashBase
    /// <summary>
    ///     The text that should be displayed as placeholder in design mode.
    /// </summary>
    protected override string DesignText {
        get { return designText; }
    }
    #endregion Abstract Methods from ScoreFlashBase

    #region Implementation of IHasVisualDesigner interface
    /// <summary>
    ///     Is the designer actually switched on? See <see cref="isDesignMode"/>.
    /// </summary>
    public override bool IsDesignMode {
        get { return isDesignMode; }
    }

    /// <summary>
    ///     Publicly accessibly ScoreFlash instance that this ScoreFlashLayout uses.
    ///     If it is not assigned, simply returns <c>ScoreFlash.Instance</c>.
    /// </summary>
    public override ScoreFlash DefaultScoreFlash {
        get {
            return defaultScoreFlash == null ? (ScoreFlash)ScoreFlash.Instance : defaultScoreFlash;
        }
    }

    /// <summary>
    ///     The position, see <see cref="position"/>.
    /// </summary>
    public override Vector2 Position {
        get { return position; }
        set { position = value; }
    }

    /// <summary>
    ///     Always return null.
    /// </summary>
    public override Vector3? PositionWorld {
        get { return null; }
        set { ; }
    }

    /// <summary>
    ///     Returns true.
    /// </summary>
    public override bool SupportsScreenAlign {
        get { return true; }
    }

    /// <summary>
    ///     The screen align for this layout instance. See <see cref="screenAlign"/>.
    /// </summary>
    public override NGAlignment.ScreenAlign ScreenAlign {
        get { return screenAlign; }
        set { screenAlign = value; }
    }

    /// <summary>
    ///     Shall we lock screen align? See <see cref="lockScreenAlign"/>.
    /// </summary>
    public override bool LockScreenAlign {
        get { return lockScreenAlign; }
        set { lockScreenAlign = value; }
    }

    /// <summary>
    ///     Returns true.
    /// </summary>
    public override bool SupportsInnerAnchor {
        get { return true; }
    }

    /// <summary>
    ///     The inner anchor for this layout instance. See <see cref="innerAnchor"/>.
    /// </summary>
    public override NGAlignment.ScreenAlign InnerAnchor {
        get { return innerAnchor; }
        set { innerAnchor = value; }
    }

    /// <summary>
    ///     Is the inner anchor currently locked to the screen alignment?
    ///     See <see cref="lockInnerAnchor"/>.
    /// </summary>
    public override bool LockInnerAnchor {
        get { return lockInnerAnchor; }
        set { lockInnerAnchor = value; }
    }

    /// <summary>
    ///     The maximum width of the items. See <see cref="maxWidth"/>.
    /// </summary>
    public override float MaxWidth {
        get { return maxWidth; }
        set { maxWidth = Mathf.Clamp(value, 0, float.MaxValue); }
    }

    /// <summary>
    ///     The minimum padding of the items. See <see cref="minPaddingX"/>.
    /// </summary>
    public override float MinPaddingX {
        get { return minPaddingX; }
        set { minPaddingX = Mathf.Clamp(value, 0, float.MaxValue); }
    }

    /// <summary>
    ///     Creates a ScoreMessage to be used in the designer.
    /// </summary>
    /// <param name="text">the text for the message</param>
    /// <returns>the ScoreMessage</returns>
    public override ScoreMessage CreateDesignerMessage(string text) {
        messageForScoreFlash = text;
        ScoreMessage msg = DefaultScoreFlash.PushLocal(this, null, null, null, null, null);
        msg.CurrentTextColor = DefaultScoreFlash.readColorStart;
        return msg;
    }

    #endregion Implementation of IHasVisualDesigner interface


}
