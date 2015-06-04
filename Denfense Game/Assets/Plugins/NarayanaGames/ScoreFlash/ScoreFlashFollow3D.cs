/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschr�nkt) *
 *  All rights reserved                             *
 ****************************************************/

//#define DEBUG_PERSIST_PLAYMODE_CHANGES

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using NarayanaGames.Common;
using NarayanaGames.Common.UI;
using NarayanaGames.ScoreFlashComponent;

/// <summary>
///     Attach this to any game object you'd like to push score messages
///     to using its world position. 
/// </summary>
/// <remarks>
///     You can then pass this to 
///     <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>
///     to make the messages automatically follow your game object.
///     If you do not use physics to move your game object around, be
///     sure to do any position changes in 
///     <a href="http://docs.unity3d.com/Documentation/ScriptReference/MonoBehaviour.FixedUpdate.html">
///         MonoBehaviour.FixedUpdate()</a> - because if you don't, the
///     velocity calculation of <c>ScoreFlashFollow3D</c> might be off!
/// </remarks>
[ExecuteInEditMode()]
[AddComponentMenu("ScoreFlash/ScoreFlashFollow3D")]
public class ScoreFlashFollow3D : ScoreFlashBase, IHasVisualDesigner, IHasTooltips {


    #region Update Mechanism

    /// <summary>
    ///     Performs a couple of checks to ensure that everything is set 
    ///     up correctly after an upgrade to a new version.
    /// </summary>
    /// <returns></returns>
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
    ///     Should test messages be directly pushed on this 
    ///     ScoreFlashFollow3D using <see cref="Push(object)"/>?
    ///     If this is checked, you can also test the behavior of 
    ///     <see cref="freezeOnRead"/>.
    /// </summary>
    public bool pushLocally = true;

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

    /// <summary>
    ///     References to one or more instances of ScoreFlash for testing.
    ///     If you do not assign any here, ScoreFlash.Instance is used instead;
    ///     if you do, one instance after the other gets messages.
    /// </summary>
    public List<ScoreFlash> scoreFlashTestInstances = new List<ScoreFlash>(0);

    #endregion Properties for testing

    /// <summary>
    ///     Renderer of the game object that this ScoreFlashFollow3D follows. This
    ///     is very important because it is needed to determine whether the object
    ///     is actually visible. Like, if it's behind the camera, for instance,
    ///     you don't want those messages to show in front of you (and they will,
    ///     if you forget to set this up correctly ;-) ). For this to fully work
    ///     reliably, make sure to also assign the proper <see cref="referenceCamera" />.
    /// </summary>
    public Renderer targetRenderer = null;

    private bool hasSentTargetRendererError = false;

    /// <summary>
    ///     Renderer of the game object that this ScoreFlashFollow3D follows. This
    ///     is very important because it is needed to determine whether the object
    ///     is actually visible. Like, if it's behind the camera, for instance,
    ///     you don't want those messages to show in front of you (and they will,
    ///     if you forget to set this up correctly ;-) ).
    /// </summary>
    public Renderer TargetRenderer {
        #region Implements a fallback mechanism in case the user forget to assign targetRenderer
        get {
            if (targetRenderer == null) {
                targetRenderer = GetComponentInChildren<Renderer>();
                if (targetRenderer == null && !hasSentTargetRendererError) {
                    hasSentTargetRendererError = true;
                    Debug.LogWarning(
                        string.Format(
                            "ScoreFlashFollow3D in {0} has no target renderer assigned - "
                            + "objects behind camera may show messages! Please assign the "
                            + "renderer of the object for this ScoreFlashFollow3D instance!", 
                            this.name), 
                            this);
                }
                if (targetRenderer != null && !hasSentTargetRendererError) {
                    hasSentTargetRendererError = true;
                    Debug.LogWarning(
                        string.Format(
                            "ScoreFlashFollow3D in {0} has no target renderer assigned - "
                            + "falling back to {1}! To avoid this message, please assign the "
                            + "renderer of the object for this ScoreFlashFollow3D instance!",
                            this.name, targetRenderer.name),
                            this);
                }
            }
            return targetRenderer;
        }
        set { targetRenderer = value; }
        #endregion Implements a fallback mechanism in case the user forget to assign targetRenderer
    }

    /// <summary>
    ///     Design mode allows you to use the scene view to set up the positioning
    ///     of this ScoreFlashFollow3D. This works both while playing and while not playing,
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
    ///     If this is set to <c>true</c>, ScoreFlash will actually not
    ///     follow the object anymore but just use the settings to determine
    ///     the position at the time the message is sent. 
    /// </summary>
    /// <remarks>
    ///     This is useful
    ///     if you want to use ScoreFlashFollow3D for configuration or to
    ///     conveniently ensure ScoreFlash uses the correct message queues 
    ///     but don't want the messages to follow the objects.
    /// </remarks>
    public bool keepStatic = false;

    /// <summary>
    ///     Should the game object "leave" the messages behind or should the
    ///     messages immediately follow the game object?
    /// </summary>
    /// <remarks>
    ///     Value between 0 and 1: 0 means messages immediately follow object,
    ///     1 means messages almost do not follow object. This
    ///     has the strongest effect when <see cref="loseMomentum"/> is 0.
    ///     With higher values in <see cref="loseMomentum"/>, the effect of
    ///     <c>leaveBehind</c> becomes less and less. Only has an effect
    ///     as long as the object this is attached to is alive.
    /// </remarks>
    public float leaveBehind = 0;

    /// <summary>
    ///     When the game objects is destroyed, should the messages lose the
    ///     momentum or not?
    /// </summary>
    /// <remarks>
    ///     Value between 0 and 1: 0 means, the messages keep the velocity the
    ///     game object last had. 1 means, the messages immediately stop at the position
    ///     they had when the object was destroyed. Any value in between will
    ///     make the messages slow down and stop.
    /// </remarks>
    public float loseMomentum = 1;

    /// <summary>
    ///     2D offset in screen coordinates.
    /// </summary>
    public Vector2 screenPositionOffset = Vector2.zero;

    /// <summary>
    ///     3D offset in world coordinates.
    /// </summary>
    public Vector3 worldPositionOffset = Vector3.zero;

    /// <summary>
    ///     Use this to control the anchor / alignment of the messages sent
    ///     using this instance of ScoreFlashFollow3D. 
    /// </summary>
    /// <remarks>
    ///     Most of the time, 
    ///     you'll want to keep this *Center (TopCenter, MiddleCenter or BottomCenter)
    ///     but you could also right align scores, or left align chat messages,
    ///     if you need to.
    /// </remarks>
    public NGAlignment.ScreenAlign innerAnchor = NGAlignment.ScreenAlign.MiddleCenter;

    /// <summary>
    ///     Freeze any message pushed directly to this ScoreFlashFollow3D at its
    ///     read state.
    /// </summary>
    /// <remarks>
    ///     If checked, this will make any message pushed to this ScoreFlashFollow3D
    ///     via <see cref="Push(object)"/> or <see cref="Push(object, Color)"/>
    ///     (directly on the ScoreFlashFollow3D instance) freeze at the beginning
    ///     of their read phase, essentially making them persistent. You can use
    ///     this for name tags, HUDs or whatever you can think of that requires
    ///     being located relative to world objects. When pushing further messages, 
    ///     the current message will move on, so you can either change the ScoreMessage
    ///     or generate a new one. See <see cref="ScoreMessage.FreezeOnRead"/>.
    /// </remarks>
    public bool freezeOnRead = false;

    /// <summary>
    ///     The camera that shows this game object to the user. If this is
    ///     not assigned,
    ///     <a href="http://docs.unity3d.com/Documentation/ScriptReference/Camera-main.html">Camera.main</a>
    ///     is used, which should be fine in most cases; but if you have
    ///     a weird scene setup, you can use this. If you want to use
    ///     ScoreFlash in a split screen set up ... please contact me ;-)
    /// </summary>
    public Camera referenceCamera = null;

    /// <summary>
    ///     If assigned, instead of using ScoreFlash.Instance, this ScoreFlashFollow3D
    ///     will use the assigned ScoreFlash instance when messages are pushed directly
    ///     via <see cref="Push(object)"/>.
    /// </summary>
    public ScoreFlash defaultScoreFlash = null;

    private Rigidbody cachedRigidbody = null;
    public Rigidbody CachedRigidbody {
        get { return cachedRigidbody; }
    }

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
    ///     Shortcut for pushing messages directly using this ScoreFlashFollow3D instance.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object)"/> on <see cref="defaultScoreFlash"/>
    ///     (or using the single ScoreFlash instance if defaultScoreFlash wasn't assigned).
    /// </remarks>
    /// <param name="message">the message to send, could also be a custom renderer (or any object)</param>
    /// <returns>return the ScoreMessage representing msg during </returns>
    public ScoreMessage Push(object message) {
        HandlePreviousMessage();
        currentMessage = DefaultScoreFlash.PushWorld(this, message);
        ApplyMessageSettings(currentMessage);
        return currentMessage;
    }

    /// <summary>
    ///     Shortcut for pushing messages in a specific color directly using this ScoreFlashFollow3D instance.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, Color)"/> on <see cref="defaultScoreFlash"/>
    ///     (or using the single ScoreFlash instance if defaultScoreFlash wasn't assigned).
    /// </remarks>
    /// <param name="message">the message to send, could also be a custom renderer (or any object)</param>
    /// <param name="color">
    ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
    ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
    ///     in the ScoreFlash instance that this message was pushed to
    /// </param>
    /// <returns>return the ScoreMessage representing msg during </returns>
    public ScoreMessage Push(object message, Color color) {
        HandlePreviousMessage();
        currentMessage = DefaultScoreFlash.PushWorld(this, message, color);
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

    private ScoreFlashFollow3DLocation location;

    internal ScoreFlashFollow3DLocation GetLocation() {
        if (location == null) {
            location = new ScoreFlashFollow3DLocation(this);
        }
        location.Init();
        location.UpdateScreenPositionOffset();
        return location;
    }

    void Awake() {
        cachedRigidbody = this.GetComponent<Rigidbody>();
    }

    void Update() {
        leaveBehind = Mathf.Clamp(leaveBehind, 0F, 1F);
        loseMomentum = Mathf.Clamp(loseMomentum, 0F, 1F);
        
        HandleStuffForTesting();
    }

    private bool isFrozen = false;
    private int frozenCounter = 0;
    /// <summary>
    ///     Returns <c>true</c> while the location is frozen.
    ///     <see cref="FreezeLocation(float)"/>.
    /// </summary>
    public bool IsFrozen {
        get { return isFrozen; }
    }

    /// <summary>
    ///     Freezes the location of the message for <c>timeSeconds</c> seconds. 
    /// </summary>
    /// <remarks>
    ///     In other
    ///     words, the positions are no longer updated for this amount of
    ///     time. This is useful, for example, if you have some action on
    ///     an object that still moves, and the score should appear at the
    ///     "impact location", however, you still need to check some
    ///     conditions (possibly over a couple of frames) before the message
    ///     should actually be shown.
    /// </remarks>
    /// <param name="timeSeconds">the time to keep the location frozen</param>
    public void FreezeLocation(float timeSeconds) {
        // make sure we have an instance of the location first!
        GetLocation();
        isFrozen = true;
        frozenCounter++;
        StartCoroutine(UnFreeze(timeSeconds));
    }

    private IEnumerator UnFreeze(float timeSeconds) {
        yield return new WaitForSeconds(timeSeconds);
        frozenCounter--;
        if (frozenCounter == 0) {
            isFrozen = false;
        }
    }

    #region HandleStuffForTesting
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
            PushForTesting("+1000");
            
            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;

            PushForTesting("-11");

            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;

            PushForTesting("+888");

            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;

            PushForTesting("I could be sayin' somethin'!");

            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;
        }
    }

    private void PushForTesting(string msg) {
        if (pushLocally) {
            Push(msg);
        } else {
            NextFlashInstanceForTesting();
            nextFlashInstanceForTestMessage.PushWorld(this, msg);
        }
    }

    private IScoreFlash nextFlashInstanceForTestMessage = null;
    /// <summary>
    ///     For internal use only; this is used by the custom inspector GUI
    ///     to show the next ScoreFlash in testing mode.
    /// </summary>
    public ScoreFlash NextFlashInstanceForTestMessage {
        get {
            if (nextFlashInstanceForTestMessage == null) {
                nextFlashInstanceForTestMessage = ScoreFlash.Instance;
            }
            return (ScoreFlash)nextFlashInstanceForTestMessage; 
        }
    }

    private int flashInstanceIndex = 0;
    /// <summary>
    ///     For internal use only; this is used by the custom inspector GUI
    ///     to show the next ScoreFlash in testing mode.
    /// </summary>
    public void NextFlashInstanceForTesting() {
        if (scoreFlashTestInstances.Count == 0 || scoreFlashTestInstances[flashInstanceIndex] == null) {
            nextFlashInstanceForTestMessage = ScoreFlash.Instance;
            return;
        }
        nextFlashInstanceForTestMessage = null;
        for (int attempt = 0; attempt < scoreFlashTestInstances.Count && nextFlashInstanceForTestMessage == null; attempt++) {
            flashInstanceIndex++;
            flashInstanceIndex %= scoreFlashTestInstances.Count;
            nextFlashInstanceForTestMessage = scoreFlashTestInstances[flashInstanceIndex];
        }
        if (nextFlashInstanceForTestMessage == null) {
            nextFlashInstanceForTestMessage = ScoreFlash.Instance;
        }
        if (nextFlashInstanceForTestMessage == null) {
            Debug.LogError("You need at least one ScoreFlash instance in your scene!");
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
    ///     Is the designer actually switched on?
    /// </summary>
    public override bool IsDesignMode {
        get { return isDesignMode; }
    }

    /// <summary>
    ///     Publicly accessibly ScoreFlash instance that this ScoreFlashFollow3D uses.
    ///     If it is not assigned, simply returns <c>ScoreFlash.Instance</c>.
    /// </summary>
    public override ScoreFlash DefaultScoreFlash {
        get {
            return defaultScoreFlash == null ? (ScoreFlash)ScoreFlash.Instance : defaultScoreFlash;
        }
    }

    /// <summary>
    ///     Screen offset position. See <see cref="screenPositionOffset"/>.
    /// </summary>
    public override Vector2 Position {
        get { return screenPositionOffset; }
        set { screenPositionOffset = value; }
    }

    /// <summary>
    ///     World position. See <see cref="worldPositionOffset"/>.
    /// </summary>
    public override Vector3? PositionWorld {
        get { return worldPositionOffset; }
        set {
            if (value.HasValue) {
                worldPositionOffset = value.Value;
            }
        }
    }

    /// <summary>
    ///     Returns false. ScoreFlashFollow3D is always relative to objects
    ///     in 3D space, so screen alignment doesn't make sense.
    /// </summary>
    public override bool SupportsScreenAlign {
        get { return false; }
    }

    /// <summary>
    ///     Always returns BottomCenter. ScoreFlashFollow3D is always relative to objects
    ///     in 3D space, so screen alignment doesn't make sense.
    /// </summary>
    public override NGAlignment.ScreenAlign ScreenAlign {
        get { return NGAlignment.ScreenAlign.BottomCenter; }
        set { ; } // does nothing
    }

    /// <summary>
    ///     Always returns true.
    /// </summary>
    public override bool LockScreenAlign {
        get { return true; }
        set { ; } // does nothing
    }

    /// <summary>
    ///     Returns true.
    /// </summary>
    public override bool SupportsInnerAnchor {
        get { return true; }
    }

    /// <summary>
    ///     The inner anchor for this ScoreFlashFollow3D instance.
    /// </summary>
    public override NGAlignment.ScreenAlign InnerAnchor {
        get { return innerAnchor; }
        set { innerAnchor = value; }
    }

    /// <summary>
    ///     Returns false. As there is no screen alignment, locking the
    ///     inner anchor wouldn't make sense.
    /// </summary>
    public override bool LockInnerAnchor {
        get { return false; }
        set { ; }
    }

    /// <summary>
    ///     The maximum width of the items.
    /// </summary>
    public override float MaxWidth {
        get { return DefaultScoreFlash.maxWidth; }
        set { DefaultScoreFlash.maxWidth = Mathf.Clamp(value, 0, float.MaxValue); }
    }

    /// <summary>
    ///     The minimum padding of the items.
    /// </summary>
    public override float MinPaddingX {
        get { return DefaultScoreFlash.minPaddingX; }
        set { DefaultScoreFlash.minPaddingX = Mathf.Clamp(value, 0, float.MaxValue); }
    }

    /// <summary>
    ///     Creates a ScoreMessage to be used in the designer.
    /// </summary>
    /// <param name="text">the text for the message</param>
    /// <returns>the ScoreMessage</returns>
    public override ScoreMessage CreateDesignerMessage(string text) {
        ScoreMessage msg = DefaultScoreFlash.PushWorldInternal(this, text, false);
        msg.CurrentTextColor = DefaultScoreFlash.readColorStart;
        ScoreFlashFollow3DLocation follow3DLoc = msg.FollowLocation;
        if (follow3DLoc != null) {
            follow3DLoc.Init();
            follow3DLoc.UpdateScreenPositionOffset();
        }
        return msg;
    }

    #endregion Implementation of IHasVisualDesigner interface
}