/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

// if you are building for servers, define SERVER to automatically have this disabled
//#define SERVER //I

// if you have narayana games LoggingFacade, define LOGGING_FACADE to make ScoreFlash use it
//#define LOGGING_FACADE //I

//#define SCOREFLASH_FREE_DEMO

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using NarayanaGames.Common;
using NarayanaGames.Common.UI;
using NarayanaGames.ScoreFlashComponent;


#region Extended Documentation
/// <summary>
///     ScoreFlash is gives you an easy way to let scores "flash up" on the 
///     screen, like a news flash. For detailed documentation of the various 
///     methods to push methods on screen, see <see cref="IScoreFlash"/>.
/// </summary>    
/// <remarks>
///     Simply add the prefab to your scene and use 
///     <c>ScoreFlash.Push(object msg)</c> (<see cref="Push(object)"/>) from anywhere in 
///     your project to push messages on screen.
///     <p />
///         For a PDF file explaining all about setting up score flash, 
///         see <a href="http://narayana-games.net/static/docs/assetstore/ScoreFlash/ScoreFlashBasics.pdf">
///             PDF: First Steps with Score Flash</a>.
///     <p />
///         For a basic introduction to ScoreFlash,
///         see <a href="http://www.youtube.com/watch?v=01ppsNz77iI">
///             ScoreFlash on the Unity Asset Store - Product Video</a>. Note that
///         instead of using ScoreFlash.Instance.Show(...) as shown in the video, you
///         now use <c>ScoreFlash.Push</c> or <c>ScoreFlash.Instance.PushLocal</c>!
///     <p />
///         To learn how to work with GUISkins, GUIStyles and fonts (only applicable
///         when you're not using a custom renderer),
///         see <a href="http://www.youtube.com/watch?v=qM9dSNWVWvc">
///             ScoreFlash-Tutorial: GUISkins, GUIStyles and Fonts</a>. It's now also possible
///         to directly assign a font using <see cref="rendering"/> (under <em>Main Layout</em>) 
///         set to <see cref="RenderingType.UnityGUI_Font"/> and assigning a font to
///         <em>Main Layout / Font</em> (<see cref="font"/>), and if you want to use ScoreFlash
///         with high density screens (e.g. Retina), you should assign the same font with
///         twice the size to <em>Font High Density</em> (<see cref="fontHighDensity"/>.
///     <p />
///         For a <strong>tutorial</strong> on working with colors in ScoreFlash,
///         see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
///             ScoreFlash-Tutorial: Working with Colors</a>.
///         There's another option now, and that is passing the color as parameter to the
///         push-method (see examples below).
///     <p />
///         ScoreFlash has a very nice feature that allows you to conveniently test and
///         set up how messages look with a test mode (<em>Testing / Autogenerate messages?</em>,
///         see <see cref="isTestAutogenerateMessages"/>) and you can keep any changes you
///         made to ScoreFlash or <see cref="ScoreFlashFollow3D"/> in their respective test
///         modes after playing by using the checkboxes <em>Keep Changes after Play?</em>
///         and <em>Store immediately?</em> which are visible at the bottom of the inspector
///         while playing.
///         For a <strong>tutorial</strong> on persisting your changes made in play mode
///         see <a href="http://www.youtube.com/watch?v=7zAE6K2rdQE">
///             ScoreFlash-Tutorial: Persisting Changes after Playing</a>
///     <p />
///         To be able to use <see cref="ScoreFlash.Push(object)"/> you need to assign
///         a GUI skin with custom style: <c>ScoreFlash</c> which needs to define
///         the correct font (usually a very large font, like 96pt) and Alignment:
///         MiddleCenter (with wrong alignment, messages don't appear in the center
///         of the screen which looks weird). Optionally, you can also add a skin for
///         high density displays (e.g. Retina display).
///         You can change the name of the custom style being used with 
///         <see cref="guiStyleName"/> (<em>Main Layout / Name of GUIStyle</em>).
///     <p />
///         Alternatively, you can use <see cref="ScoreFlash.Push(object, GUIStyle)"/>
///         or <see cref="ScoreFlash.Push(object, GUIStyle, GUIStyle)"/> when you
///         want to control everything related to GUISkins yourself. This gives you
///         more flexibility but also requires more coding on your side.
///     <p />
///         By default, ScoreFlash renders everything using Unity's built-in GUI
///         system. So there are no dependencies on other packages. 
///         However, you can also use ScoreFlash with so-called "custom renderers",
///         for example for NGUI and EZ GUI (included), or you could write your own
///         custom renderer using <see cref="ScoreFlashRendererBase"/> as base class
///         (instead of MonoBehaviour).
///         To use those, unpack the
///         packages you find in <em>Plugins/NarayanaGames/ScoreFlash/CustomRendering</em>,
///         and pull the prefabs into the slot <em>Score Flash Renderer</em> under
///         <em>Main Layout</em>, which becomes available as soon as you switch
///         <em>Rendering</em> to <em>CustomRenderer</em> (see <see cref="rendering"/>). 
///         For a <strong>tutorial</strong> on using ScoreFlash with NGUI,
///         see <a href="http://www.youtube.com/watch?v=guQ2pB-XMR4">
///             ScoreFlash with NGUI - Single Drawcall Messages</a>
///     <p />
///     <example>
///         There's several ways of using ScoreFlash, here's the most simple example 
///         in C# (but ScoreFlash also can be called from JavaScript and Boo):
///         <code>
///         ScoreFlash.Push("Hello World!");
///         </code>
///     </example>
///     <example>
///         
///         If, instead of using the colors provided by the ScoreFlash instance, you
///         want each message to have a specific color, you can use another method
///         that also has a Color parameter. You could use this, for example, to
///         easily have red messages when a player loses score or health, and green 
///         messages, when a player gains score or health, and white messages for
///         chat messages. When using this, the alpha value is multiplied with the
///         current value from the fade animation of the message (so you could make
///         messages appear very "light" without having to change the ScoreFlash
///         configuration - and yet you can still use the fade in / keep / fade out
///         animation of ScoreFlash):
///         <code>
///         Color colorRed = new Color(1F, 0F, 0F, 1F);
///         ScoreFlash.Push("Hello World!", colorRed);
///         </code>
///     </example>
///     <example>
/// 
///         For a much more powerful and clean interface, when you only want to work
///         with a <strong>single instance of ScoreFlash</strong>, use <c>ScoreFlash.Instance</c>.
///         This gives you all the methods available through <see cref="IScoreFlash"/>.
///         One great advantage of using this is that with Intellisense, you only see 
///         the methods available through ScoreFlash (instead of all "stuff" that 
///         ScoreFlash inherits from MonoBehaviour).
///         For example if you want to draw a message at the current location of
///         the object the script with the following code is attached to:
///         <code>
///         Vector2 offset = Vector2.zero;
///         ScoreFlash.Instance.PushWorld(transform.position, offset, "I am here, now!");
///         </code>
///         
///         Or, if you have <see cref="ScoreFlashFollow3D"/> attached to the same object,
///         you can also do:
///         <code>
///         ScoreFlashFollow3D follow3D = GetComponent&lt;ScoreFlashFollow3D&gt;();
///         ScoreFlash.Instance.PushWorld(follow3D, "Some message");
///         </code>
///         
///         If, on the other hand, you want to put a message on a specific location on
///         screen, you can use <see cref="IScoreFlash.PushScreen(Vector2, object)"/>, 
///         e.g. to put a message with the coordinates of where a use clicked with the 
///         mouse at that location on screen:
///         <code>
///         void Update() {
///             if (Input.GetMouseButtonDown(0)) {
///                 Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
///                 string text = string.Format("({0:000}, {1:000})", screenPosition.x, screenPosition.y);
///                     
///                 ScoreFlash.Instance.PushScreen(screenPosition, text);
///             }
///         }
///         </code>
///     </example>
///     <example>
///         
///         Finally, if you want to use <strong>multiple instances of ScoreFlash</strong>, use the
///         <see cref="ScoreFlashManager"/> instead of going through 
///         <see cref="ScoreFlash.Instance"/>.
///         That way, you could easily use a completely different style for each message
///         and you still have all the methods available that <see cref="IScoreFlash"/> offers,
///         and the inspector of <see cref="ScoreFlashManager"/> provides a button <em>Copy Ref</em>
///         which lets you easily copy the full reference to any ScoreFlash instance controlled by
///         a ScoreFlashManager into your code.
///         The final example illustrates how to use multiple instances of ScoreFlash.
///         You'd use this in a scene with a <see cref="ScoreFlashManager"/> that
///         has three instances of ScoreFlash as children, named 
///         <c>SF_Amadeus</c>, <c>SF_Destroy</c> and <c>SF_Eraser</c>.
///         <code>
///using UnityEngine;
///using System.Collections;
///
///public class ExampleCSharpMultipleInstances : MonoBehaviour {
///
///    public void OnGUI() {
///        if (GUI.Button(new Rect(Screen.width - 180F, 10F, 170F, 30F), "Push to Amadeus")) {
///            ScoreFlashManager.Get("SF_Amadeus").PushLocal("Amadeus, yay!");
///        }
///
///        if (GUI.Button(new Rect(Screen.width - 180F, 50F, 170F, 30F), "Push to Destroy")) {
///            ScoreFlashManager.Get("SF_Destroy").PushLocal("Destroy, you see?");
///        }
///
///        if (GUI.Button(new Rect(Screen.width - 180F, 90F, 170F, 30F), "Push to Eraser")) {
///            ScoreFlashManager.Get("SF_Eraser").PushLocal("Eraser, play with me!");
///        }
///
///     }
/// }
///         </code>
///     </example>
/// </remarks>
#endregion Extended Documentation
[AddComponentMenu("ScoreFlash/ScoreFlash")]
[ExecuteInEditMode()]
public class ScoreFlash : ScoreFlashBase, IScoreFlash, IScoreFlashLayout {

#if LOGGING_FACADE // if narayana games Logging Facade is available, we add some logging
    private static readonly LoggingFacade log = new LoggingFacade(typeof(ScoreFlash));
#endif

    #region Public inspector "properties" ;-)

    #region Update Mechanism

    /// <summary>
    ///     Performs a couple of checks to ensure that everything is set 
    ///     up correctly after an upgrade to a new version.
    /// </summary>
    /// <returns><c>true</c>, if this instance was updated and changes need to be applied</returns>
    public override bool UpgradeCheck() {
#if UNITY_EDITOR
        // when these are not assigned - auto-assign them
        if (editorGUISkin == null) {
            Debug.Log("editorGUISkin was not assigned - fixing it for you ;-)");
            editorGUISkin = NGUtil.LoadAsset<GUISkin>("Assets/Plugins/NarayanaGames/Common/Resources/EditorGUISkin.guiskin");
        }
        if (editorIconSet == null) {
            Debug.Log("editorIconSet was not assigned - fixing it for you ;-)");
            editorIconSet = NGUtil.LoadAsset<EditorIconSet>("Assets/Plugins/NarayanaGames/Common/Resources/EditorIconSet.asset");
        }
#endif

        if (base.UpgradeCheck()) {
            if (this.version.Equals("3.0.0") || this.version.Equals("3.1.0")) {
                if (screenAlign == NGAlignment.ScreenAlign.MiddleCenter) {
                    position = Vector2.zero; // make sure to ignore paddingY, if it was set!
                } else {
                    // initialize position just using paddingY
                    position = new Vector2(0, paddingY);
                }
            }

            return true; // we did update
        } else {
            return false; // we did not update
        }
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
    ///     are being sent to ScoreFlash. 
    /// </summary>
    /// <remarks>
    ///     NOTE: This does not
    ///     have an effect on messages sent via includeMessageSpam (these are
    ///     always sent in a burst without any delay.
    ///     Also be aware that this is based on real time (so Time.timeScale
    ///     has no effect).
    /// </remarks>
    public float testMessageDelaySeconds = 3F;

    /// <summary>
    ///     This will send a burst of messages with no delay every once in a
    ///     while (depends on whether includeVeryLongMessages is active).
    ///     It ignores messageFrequencyMultiplier.
    /// </summary>
    public bool includeMessageSpam = true;

    /// <summary>
    ///     Includes a few messages with very long text to see how that is handled.
    /// </summary>
    public bool includeVeryLongMessages = true;

    /// <summary>
    ///     Use this to test high density screen layouts.
    /// </summary>
    public bool isTestForceHighDensity = false;

    /// <summary>
    ///     Design mode allows you to use the scene view to set up the main layout
    ///     of ScoreFlash. This works both while playing and while not playing.
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
    public bool enableDeselectHack = true;

    /// <summary>
    ///     Message that is shown while in design mode.
    /// </summary>
    public string designText = "You can change this message";

    /// <summary>
    ///     Renders debugging information instead of the actual messages. When
    ///     using the default, UnityGUI based rendering, this also renders a box
    ///     around the messages.
    /// </summary>
    public bool isDebugMode = false;

    /// <summary>
    ///     Various options of what should be shown when debug mode is activated.
    /// </summary>
    public enum DebugMode {
        /// <summary>
        ///     Show the message as usual.
        /// </summary>
        NormalMessages,
        /// <summary>
        ///     Show the local (animated) position of the message.
        /// </summary>
        LocalPosition,
        /// <summary>
        ///     Show the screen position of the message.
        /// </summary>
        ScreenPosition,
        /// <summary>
        ///     Show the screen position, width and height of the message.
        /// </summary>
        FullRect,
        /// <summary>
        ///     Show the current velocity of the message (read / fade out phases).
        /// </summary>
        Velocity,
        /// <summary>
        ///     Show the age of the message.
        /// </summary>
        Age,
        /// <summary>
        ///     Show the alignments of the message.
        /// </summary>
        Alignment
    }

    /// <summary>
    ///     What should be shown  in the message when debug mode is activated.
    /// </summary>
    public DebugMode currentDebugMode = DebugMode.NormalMessages;

    #endregion Properties for testing

    #region Main Layout Properties

    /// <summary>
    ///     Select how ScoreFlash renders messages. 
    ///     See <see cref="RenderingType"/>.
    /// </summary>
    public RenderingType rendering = RenderingType.UnityGUI_GUISkin;

    /// <summary>
    ///     Different ways of how ScoreFlash renders messages.
    ///     See <see cref="rendering"/>.
    /// </summary>
    public enum RenderingType : int {
        /// <summary>
        ///     The default method, using a GUISkin with a custom style.
        /// </summary>
        /// <remarks>
        ///     See: <see cref="skin"/>, <see cref="skinHighDensity"/>,
        ///     <see cref="guiStyleName"/>.
        /// </remarks>
        UnityGUI_GUISkin = 0,
        /// <summary>
        ///     Directly assign the font(s) to ScoreFlash.
        /// </summary>
        /// <remarks>
        ///     See: <see cref="font"/>, <see cref="fontHighDensity"/>.
        /// </remarks>
        UnityGUI_Font = 1,
        /// <summary>
        ///     Custom renderers, e.g. used for using NGUI, EZ GUI or eDriven.Gui
        ///     to handle rendering of messages.
        /// </summary>
        /// <remarks>
        ///     See: <see cref="scoreFlashRenderer"/>, <see cref="customParent"/>,
        ///     <see cref="ScoreFlashRendererBase.RequiresCustomParent"/>.
        /// </remarks>
        CustomRenderer = 2
    }

    /// <summary>
    ///     Assign a skin that has the custom style <c>ScoreFlash</c>
    /// </summary>
    /// <remarks>
    ///     <p />
    ///     For a <strong>tutorial</strong> on using GUISkins, GUIStyles and fonts with ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=qM9dSNWVWvc">
    ///         ScoreFlash-Tutorial: GUISkins, GUIStyles and Fonts</a>
    ///     <p />
    ///     See <see cref="RenderingType.UnityGUI_GUISkin"/>.
    /// </remarks>
    public GUISkin skin;
    /// <summary>
    ///     Optional skin for retina displays. 
    /// </summary>
    /// <remarks>
    ///     Simply make a copy of the skin
    ///     you are using for normal densities and use a font that's double
    ///     the size in your custom style <c>ScoreFlash</c>.
    ///     <p />
    ///     For a <strong>tutorial</strong> on using GUISkins, GUIStyles and fonts with ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=qM9dSNWVWvc">
    ///         ScoreFlash-Tutorial: GUISkins, GUIStyles and Fonts</a>
    ///     <p />
    ///     See <see cref="RenderingType.UnityGUI_GUISkin"/>.
    /// </remarks>
    public GUISkin skinHighDensity;

    /// <summary>
    ///     The name of either a default style of a custom style in 
    ///     <see cref="skin"/> (and <see cref="skinHighDensity"/> if you also
    ///     use that) that ScoreFlash uses to get font and style information.
    /// </summary>
    /// <remarks>
    ///     Default is <c>ScoreFlash</c> and the example skins that come with
    ///     ScoreFlash all use this, so if you change the value, they will
    ///     no longer work with your ScoreFlash instance! Changing this is
    ///     very useful if you have multiple instances of ScoreFlash in one
    ///     scene and want to use different fonts but don't want to create
    ///     several GUISkins just to be able to handle those different fonts.
    ///     It's recommended that you use the same name for the custom GUIStyle
    ///     that you are also using for the game objects with the ScoreFlash
    ///     instances.
    ///     <p />
    ///     For a <strong>tutorial</strong> on using GUISkins, GUIStyles and fonts with ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=qM9dSNWVWvc">
    ///         ScoreFlash-Tutorial: GUISkins, GUIStyles and Fonts</a>
    ///     <p />
    ///     See <see cref="RenderingType.UnityGUI_GUISkin"/>.
    /// </remarks>
    public string guiStyleName = "ScoreFlash";

    /// <summary>
    ///     The font to be used by ScoreFlash.
    /// </summary>
    /// <remarks>
    ///     See <see cref="RenderingType.UnityGUI_Font"/>.
    /// </remarks>
    public Font font;

    /// <summary>
    ///     The (optional) font to be used by ScoreFlash when running on a device
    ///     with a high density display (like Retina displays).
    /// </summary>
    /// <remarks>
    ///     See <see cref="RenderingType.UnityGUI_Font"/>.
    /// </remarks>
    public Font fontHighDensity;

    /// <summary>
    ///     Assign this if you want custom rendering, e.g. using GUIText or 
    ///     NGUI or EZ GUI. 
    /// </summary>
    /// <remarks>
    ///     Custom renderers for GUIText and UnityGUI are
    ///     already implemented (UnityGUI is just implement to give you an
    ///     example of how this can be done).
    ///     See <see cref="ScoreFlashRendererBase"/>, 
    ///     <see cref="ScoreFlashRendererUnityGUI"/> and
    ///     <see cref="ScoreFlashRendererGUIText"/>.
    ///     <br />
    ///     See <see cref="RenderingType.CustomRenderer"/>.
    /// </remarks>
    public ScoreFlashRendererBase scoreFlashRenderer;

    /// <summary>
    ///     When using rendering via prefabs, this allows you to instantiate
    ///     a number of these prefabs immediately on startup to prevent any
    ///     hiccups later in the game.
    /// </summary>
    /// <remarks>
    ///     While this can prevent hiccups while playing the game, it does
    ///     add a little time to scene loading.
    /// </remarks>
    public int warmUpCount = 0;

    /// <summary>
    ///     When using a ScoreFlashRenderer that returns <c>true</c> for
    ///     <see cref="ScoreFlashRendererBase.RequiresCustomParent"/>, this
    ///     can be assigned to make sure that the instances of ScoreFlashRenderer
    ///     are parented to this parent. 
    /// </summary>
    /// <remarks>
    ///     This is needed for certain GUI systems
    ///     (e.g. NGUI).
    ///     <br />
    ///     See <see cref="RenderingType.CustomRenderer"/>.
    /// </remarks>
    public Transform customParent;

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
    ///     Controls the anchor of the message. Use this for left/center/right aligning
    ///     the text messages. Possible values are defined by <see cref="NGAlignment.ScreenAlign"/>.
    /// </summary>
    public NGAlignment.ScreenAlign innerAnchor = NGAlignment.ScreenAlign.MiddleCenter;


    /// <summary>
    ///     Obsolete (but I can't officially mark it obsolete because I have to use it
    ///     myself to grab the value on updates when people have older versions of
    ///     ScoreFlash. Some day, I'll make this obsolete and eventually remove it.
    ///     Until then, it's best ignored. Thank you for reading this (you must be
    ///     really interested if you came here ;-) ).
    /// </summary>
    //[System.Obsolete("Replaced with position")]
    public float paddingY = 30F;

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
    #endregion Main Layout Properties

    #region Readability and Performance Tweaks

    /// <summary>
    ///     TimeReference controls whether real time or game time
    ///     (using <c>Time.timeScale</c>) is used. Used by <see cref="timeReference"/>.
    /// </summary>
    public enum TimeReference : int {
        /// <summary>
        ///     Uses real time, so changing Time.timeScale will not have an
        ///     effect on messages displayed by ScoreFlash. This is important
        ///     if you, for example, use ScoreFlash in a pause-menu.
        /// </summary>
        UseRealTime = 0,
        /// <summary>
        ///     Uses game time, so change Time.scale will have an effect on
        ///     message displayed by ScoreFlash. Use this if you want to
        ///     make the animations coming from ScoreFlash fit the current
        ///     Time.timeScale (for example, when in bullet time everything,
        ///     including ScoreFlash messages shall slow down).
        /// </summary>
        UseGameTime = 1
    }

    /// <summary>
    ///     Which time reference shall ScoreFlash use - game time or real time?
    ///     In other words: Should Time.timeScale have an effect (UseGameTime),
    ///     or not (UseRealTime). Possible values are defined by
    ///     <see cref="TimeReference"/>.
    /// </summary>
    public TimeReference timeReference = TimeReference.UseRealTime;

    /// <summary>
    ///     Maximum number of simultanuous messages before we start the 
    ///     "quick mode" which makes older messages age 8 times quicker 
    ///     than usual to avoid information (and drawcall) overflow.
    /// </summary>
    /// <remarks>
    ///     The number of messages can still be larger than this for a
    ///     short moment (and depending on your other settings, even for
    ///     a longer moment as this only increases how quickly the message
    ///     ages).
    /// </remarks>
    public int maxSimultanuousMessages = 5;

    /// <summary>
    ///     When messages arrive quickly, there may be multiple shown simultanuously. 
    ///     This values controls how much distance ScoreFlash tries to keep between
    ///     two messages. If the distance cannot be kept, old messages age quicker
    ///     so they fade out sooner so there's more room for the new messages.
    /// </summary>
    public float minDistanceBetweenMessages = 10.0F;

    /// <summary>
    ///     How quickly should messages be spread to meet the minimum distance?
    ///     Positive values make message move upwards, negative values make message
    ///     spread downwards (it's important to make this fit the other velocities!)
    /// </summary>
    public float spreadSpeed = 3F;

    /// <summary>
    ///     If <c>true</c>, instead of waiting for the fade in phase to finish,
    ///     messages immediately start pushing other messages. In most cases, 
    ///     that should be <c>true</c> but I kept "false" the default to not
    ///     break existing code.
    /// </summary>
    public bool spreadImmediately = true;

    /// <summary>
    ///     Usually, outlines are disabled on mobile devices to reduce draw calls.
    ///     Check this to also render outlines on mobile devices.
    /// </summary>
    public bool forceOutlineOnMobile = false;

    /// <summary>
    ///     If you don't want outlines to be rendered (which adds a few drawcalls),
    ///     check this. This is the default on mobile devices. Readability is highly
    ///     increased with outlines so it is recommended to read this to false
    ///     unless you use this on a background that makes the texts easy to read!
    /// </summary>
    public bool disableOutlines = false;

    /// <summary>
    ///     Color of the outline - should usually be black. Alpha is used
    ///     to multiply with current text color.
    /// </summary>
    public Color colorOutline = new Color(0F, 0F, 0F, 0.8F);
    #endregion Readability and performance Tweaks

    #region Parameters for Color Control

    /// <summary>
    ///     Different ways of how colors are selected. Used by
    ///     <see cref="colorSelectionMode"/>.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public enum ColorControl : int {
        /// <summary>
        ///     Uses one colors for each animation step: Fade In,
        ///     Read Start, Read End, Fade out.
        /// </summary>
        FadePhases = 0,
        /// <summary>
        ///     Uses the color from the skin. This is especially
        ///     useful if you're using 
        ///     <see cref="IScoreFlash.PushLocal(object, GUIStyle)"/> 
        ///     or 
        ///     <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle)"/> 
        ///     because it won't overwrite the colors from your style.
        /// </summary>
        UseColorFromSkin = 1, // let user control alpha values
        /// <summary>
        ///     One color after the other in the order you define them.
        /// </summary>
        Sequence = 2,
        /// <summary>
        ///     Colors get randomly picked from your list. When there
        ///     are more than three colors defined we make sure that
        ///     colors are not repeated until all have been picked
        ///     once.
        /// </summary>
        Random = 3
    }

    /// <summary>
    ///     How should ScoreFlash handle colors? Default is FadePhases.
    ///     Possible values are defined by <see cref="ColorControl"/>.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public ColorControl colorSelectionMode = 0;

    /// <summary>
    ///     Colors used for <see cref="colorSelectionMode"/>:
    ///     <see cref="ColorControl.Sequence"/> and
    ///     <see cref="ColorControl.Random"/>.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public List<Color> colors = new List<Color>() {
        new Color(1F, 1F, 0F, 1F),
        new Color(1F, 0F, 1F, 1F),
        new Color(0F, 1F, 1F, 1F),
        new Color(1F, 0F, 0F, 1F),
        new Color(0F, 1F, 0F, 1F),
        new Color(0F, 0F, 1F, 1F)
    };

    /// <summary>
    ///     Initial alpha value. This is ignored for <see cref="ColorControl.FadePhases"/>
    ///     and used as multiplier for the value from the current color in the other
    ///     colorSelectionMode modes.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public float alphaFadeInMultiplier = 0;
    /// <summary>
    ///     Alpha value for Read Start. This is ignored for <see cref="ColorControl.FadePhases"/>
    ///     and used as multiplier for with the value from the current color in the other
    ///     colorSelectionMode modes.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public float alphaReadStartMultiplier = 1F;
    /// <summary>
    ///     Alpha value for Read End. This is ignored for <see cref="ColorControl.FadePhases"/>
    ///     and used as multiplier for with the value from the current color in the other
    ///     colorSelectionMode modes.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public float alphaReadEndMultiplier = 1F;
    /// <summary>
    ///     Alpha value for Fade Out. This is ignored for <see cref="ColorControl.FadePhases"/>
    ///     and used as multiplier for with the value from the current color in the other
    ///     colorSelectionMode modes.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public float alphaFadeOutMultiplier = 0;

    #endregion Parameters for Color Control


    #region Parameters for the Fade In Phase
    /// <summary>
    ///     The time in seconds that it takes to fade in.
    /// </summary>
    public float fadeInTimeSeconds = 0.5F;

    /// <summary>
    ///     Initial color.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public Color fadeInColor = new Color(0.0F, 1.0F, 0.2F, 0.0F);

    /// <summary>
    ///     The animation curve used to drive the color from colorFadeIn to colorReadStart.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public AnimationCurve fadeInColorCurve = AnimationCurve.Linear(0, 0, 1, 1);



    /// <summary>
    ///     The offset the message starts with relative to its main screen position.
    ///     Positive values in X make the message come from right, negative messages
    ///     make the message come from left.
    /// </summary>
    /// <remarks>
    ///     When using Alignment.ScreenAlign.Middle, both
    ///     positive and negative values for Y do make sense. When using Alignment.ScreenAlign.Top,
    ///     negative values make the message "drop" in from above (outside),
    ///     with ScreenAlighn.Bottom and positive values, the message "rises" in
    ///     from below (outside screen).
    /// </remarks>
    public float fadeInOffsetX = 0F;

    /// <summary>
    ///     The animation curve used to drive fadeInOffset.y while fading in.
    /// </summary>
    public AnimationCurve fadeInOffsetXCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     The offset the message starts with relative to its main screen position.
    ///     Positive values in Y make the message come from below, negative values make
    ///     the message come from above.
    /// </summary>
    /// <remarks>
    ///     When using Alignment.ScreenAlign.Middle, both
    ///     positive and negative values for Y do make sense. When using Alignment.ScreenAlign.Top,
    ///     negative values make the message "drop" in from above (outside),
    ///     with ScreenAlighn.Bottom and positive values, the message "rises" in
    ///     from below (outside screen).
    /// </remarks>
    public float fadeInOffsetY = -150.0F;

    /// <summary>
    ///     The animation curve used to drive fadeInOffset.y while fading in.
    /// </summary>
    public AnimationCurve fadeInOffsetYCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     The initial scale when the message appears.
    /// </summary>
    public float fadeInScale = 0.001F;

    /// <summary>
    ///     The animation curve used to drive the scale from <c>fadeInScale</c>
    ///     to it's read start value (this is always <c>1</c>).
    /// </summary>
    public AnimationCurve fadeInScaleCurve = AnimationCurve.Linear(0, 0, 1, 1);
    #endregion Parameters for the Fade In Phase

    #region Parameters for the Reading Phase
    /// <summary>
    ///     The time the message is kept for the player to read (max to read),
    /// </summary>
    public float readTimeSeconds = 2.5F;

    /// <summary>
    ///     This is the maximum length in characters before readTimeAdd is added to a message.
    /// </summary>
    public int readMinLengthCharsToAddTime = 10;
    /// <summary>
    ///     To be able to read long texts, the player needs more time. This defines how much
    ///     longer long messages remain visible if the message in question has more than
    ///     <c>minLengthCharsToAddTime</c> characters.
    /// </summary>
    public float readAddTimeSeconds = 2.0F;

    /// <summary>
    ///     Color faded to from initial color in first phase.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public Color readColorStart = new Color(1.0F, 1.0F, 0.0F, 1.0F);

    /// <summary>
    ///     The animation curve used to drive the color from colorReadStart to colorReadEnd.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public AnimationCurve readColorCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
    /// <summary>
    ///     Color faded to from max color in "read phase".
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public Color readColorEnd = new Color(0.0F, 0.7F, 0.7F, 0.3F);

    /// <summary>
    ///     The animation curve used to drive the velocity on the x-axis from <c>0</c>
    ///     to it's read end value <c>readTimeFloatUpVelocity</c>.
    /// </summary>
    public AnimationCurve readVelocityXCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     Speed with which the message floats right while it fades out. Use negative
    ///     values to make it float left. 
    ///     WARNING: Values between than -15 and 15 (but not 0) may generate stuttering!
    /// </summary>
    public float readFloatRightVelocity = 0.0F;


    /// <summary>
    ///     The animation curve used to drive the velocity on the y-axis from <c>0</c>
    ///     to it's read end value <c>readTimeFloatUpVelocity</c>.
    /// </summary>
    public AnimationCurve readVelocityCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     Speed with which the message floats up while it fades out. Use negative
    ///     values to make it float down. When using negative values here, you
    ///     should most likely also use negative values for <see cref="spreadSpeed"/>!!!
    ///     WARNING: Values between than -15 and 15 (but not 0) may generate stuttering!
    /// </summary>
    public float readFloatUpVelocity = 20.0F;

    /// <summary>
    ///     The animation curve used to drive the scale from <c>1</c>
    ///     to it's read end value <c>readTimeScale</c>.
    /// </summary>
    public AnimationCurve readScaleCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     Scale at the end of the read time. The message scales from 1
    ///     to this value while it is kept to be read.
    /// </summary>
    public float readScale = 1.5F;

    #endregion Parameters for the Reading Phase

    #region Parameters for the Fade Out Phase

    /// <summary>
    ///     The time the message takes to fade out.
    /// </summary>
    public float fadeOutTimeSeconds = 1.0F;

    /// <summary>
    ///     The animation curve used to drive the color from colorReadEnd to colorFadeOut.
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public AnimationCurve fadeOutColorCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     Color faded to from read color in fade out phase. Should
    ///     have alpha = 0!
    ///     WARNING: If "alpha != 0" => text disappears abruptly!
    /// </summary>
    /// <remarks>
    ///     For a <strong>tutorial</strong> on working with colors in ScoreFlash,
    ///     see <a href="http://www.youtube.com/watch?v=fy-Oo6dg6kA">
    ///         ScoreFlash-Tutorial: Working with Colors</a>
    /// </remarks>
    public Color fadeOutColor = new Color(1.0F, 0.0F, 0.8F, 0.0F);

    /// <summary>
    ///     The animation curve used to drive the velocity on x from <c>readTimeFloatUpVelocity</c>
    ///     to it's fade out end value <c>fadeOutTimeFloatUpVelocity</c>.
    /// </summary>
    public AnimationCurve fadeOutVelocityXCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     Speed with which the message floats right while it fades out. Use negative
    ///     values to make it float left.
    ///     WARNING: Values between -15 and 15 (but not 0) may generate stuttering!
    /// </summary>
    public float fadeOutFloatRightVelocity = 0.0F;


    /// <summary>
    ///     The animation curve used to drive the velocity on y from <c>readTimeFloatUpVelocity</c>
    ///     to it's fade out end value <c>fadeOutTimeFloatUpVelocity</c>.
    /// </summary>
    public AnimationCurve fadeOutVelocityCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     Speed with which the message floats up while it fades out. Use negative
    ///     values to make it float down. When using negative values here, you
    ///     should most likely also use negative values for <see cref="spreadSpeed"/>!!!
    ///     WARNING: Values between -15 and 15 (but not 0) may generate stuttering!
    /// </summary>
    public float fadeOutFloatUpVelocity = 80.0F;

    /// <summary>
    ///     The animation curve used to drive the scale from <c>readTimeScale</c>
    ///     to it's fade out end value <c>fadeOutTimeScale</c>.
    /// </summary>
    public AnimationCurve fadeOutScaleCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    ///     Scale at the end of the fade out time. The message scales from midScale
    ///     to this value while it is kept to be read.
    /// </summary>
    public float fadeOutScale = 2.0F;


    /// <summary>
    ///     This is the initial rotation speed the message gets immediately 
    ///     when moving into fading out. Use rotationAccelleration to make 
    ///     this a little smoother. Set to 0 to not have the message rotate 
    ///     on fading out (rotationAccelleration must also be 0).
    /// </summary>
    public float fadeOutInitialRotationSpeed = 0.0F;

    /// <summary>
    ///     Increases the rotation speed while the message fades out.
    ///     Set to 0 to not have the message rotate on fading out 
    ///     (initialRotationSpeed must also be 0).
    /// </summary>
    public float fadeOutRotationAcceleration = 10.0F;


    #endregion Parameters for the Fade Out Phase

    /// <summary>
    ///     Assures that there is only one instance of ScoreFlash available in any
    ///     scene by destroying any new instances that are being created.
    /// </summary>
    /// <remarks>
    ///     This needs to be true if you want to use ScoreFlash in the "usual way",
    ///     i.e. by calling <see cref="ScoreFlash.Push(object)"/>. If you want to
    ///     work with multiple configurations (e.g. for different timings), set this
    ///     to false and make sure to have a reference to the actual instance.
    ///     See also <see cref="Instance"/>.
    /// </remarks>
    public bool ensureSingleton = true;

    /// <summary>
    ///     GUI skin for rendering the layout designer in the editor (scene view). 
    ///     Do not touch this!
    /// </summary>
    //[HideInInspector()] // it's not so bad if this is visible when in "Default Inspector"
    public GUISkin editorGUISkin;

    /// <summary>
    ///     Icon set with a couple of icons useful for the editor GUI.
    ///     Do not touch this!
    /// </summary>
    //[HideInInspector()] // it's not so bad if this is visible when in "Default Inspector"
    public EditorIconSet editorIconSet;

    #endregion Public inspector "properties" ;-)

    #region Internal player variables and properties

    /// <summary>
    ///     This is used for renaming ScoreFlash instances conveniently 
    ///     from the ScoreFlashManager (if you're using this).
    /// </summary>
    public string TemporaryName { get; set; }

    private Dictionary<int, NGQueue<ScoreMessage>> scoreMessages = new Dictionary<int, NGQueue<ScoreMessage>>();

    /// <summary>
    ///     Used internally!
    /// </summary>
    internal enum DefaultMessageQueueID : int {
        Default = -1,
        DefaultUserPosition = -2
    }

    // helper variable for naming scoreFlashRenderers
    private int rendererIndex = 0;

    private bool hasWarnedMissingCustomStyle = false;
    #endregion Internal player variables and properties

    #region Unity Singleton Pattern
    private static IScoreFlash instance = null;
    void Awake() {
        UpgradeCheck(); // if necessary during runtime: do the upgrade "on-the-fly"

        // if we are a child of a ScoreFlashManager, that will handle singleton stuff for us :-)
        if (this.transform.parent != null && this.transform.parent.GetComponent<ScoreFlashManager>() != null) {
            ensureSingleton = false;
        }
        if (instance != null && ((ScoreFlash)instance) != this) {
            if (ensureSingleton && Application.isPlaying) { // don't do this in edit mode!
                Destroy(this.gameObject);
            }
            return;
        } else {
            instance = (IScoreFlash) this;
        }
        if (ensureSingleton && Application.isPlaying) { // don't do this in edit mode!
            DontDestroyOnLoad(this.gameObject);
        }
#if SERVER
        this.enabled = false;
#endif // SERVER
        if (skin == null && scoreFlashRenderer == null && font == null) {
            Debug.LogWarning(
                "You have not assigned a GUISkin or Score Flash Renderer or Font - you either need to assign one "
                +"or use a custom renderer or pass GUIStyles when pushing messages!",
                this);
        }
        if (rendering == RenderingType.UnityGUI_GUISkin) {
            if (skin != null && skin.FindStyle(guiStyleName) == null) {
                Debug.LogWarning(
                    string.Format("Your Skin does not have a custom style named '{0}'!", guiStyleName),
                    this.skin);
            }
            if (skinHighDensity != null && skinHighDensity.FindStyle(guiStyleName) == null) {
                Debug.LogWarning(
                    string.Format("Your Skin for High Density does not have a custom style named '{0}'!", guiStyleName),
                    this.skinHighDensity);
            }
        }
        if (Application.isPlaying && warmUpCount > 0 && rendering == RenderingType.CustomRenderer) {
            ScoreMessage msg = null;
            for (int i = 0; i < warmUpCount; i++) {
                msg = PushLocal(".");
                msg.MakeWarmUpMessage();
            }
            StartCoroutine(RemoveWarmupRenderers());
        }
    }

    private IEnumerator RemoveWarmupRenderers() {
        yield return new WaitForEndOfFrame();
        Cleanup();
    }

    /// <summary>
    ///     When we are not in "Singleton-Mode" => clean up instance!
    /// </summary>
    void OnDestroy() {
        if (!ensureSingleton) {
            instance = null;
        }
    }

    #endregion Unity Singleton Pattern

    /// <summary>
    ///     Provides access to the one Instance of ScoreFlash. This only works
    ///     reliably if <see cref="ensureSingleton"/> is <c>true</c>! If you
    ///     have multiple instances of ScoreFlash, using this will have
    ///     unpredictable results, use <see cref="ScoreFlashManager.Get(string)"/>
    ///     in that case instead!
    /// </summary>
    public static IScoreFlash Instance {
        get {
            #region Secret Sauce to make "re-compile while playing" work ;-)
            /*
             * Three possible reasons this can be null: 
             * 1) code change while playing => recompile; this can be fixed on the fly
             * 2) you forgot to add the ScoreFlash object to your scene; fixing this is up to you!
             * 3) ScoreFlash.Instance was called from Awake(), which isn't supported
             */
            if (instance == null || ((ScoreFlash)instance) == null) {
                instance = (IScoreFlash) Object.FindObjectOfType(typeof(ScoreFlash));
                if (instance == null) {
                    Object[] candidates = Object.FindObjectsOfType(typeof(ScoreFlash));
                    foreach (Object candidate in candidates) {
                        if (candidate != null
                            && ((ScoreFlash)candidate) != null
                            && ((ScoreFlash)candidate).gameObject != null) {
                            instance = (ScoreFlash) candidate;
                        }
                    }
                }

                if (instance == null) {
                    Debug.LogError("Trying to access ScoreFlash.Instance but there "
                        + "is no gameobject with ScoreFlash attached in the scene! "
                        + "Please add the object, you can use the Prefab that comes "
                        + "with ScoreFlash, or create your own. "
                        + "If you have such an object, you might have tried calling "
                        + "ScoreFlash.Instance from Awake() which is not supported!");
                } else {
                    if (Application.isPlaying) { // only show this when we're actually playing, not in edit mode!
                        Debug.Log("Restored ScoreFlash.Instance - most likely you loaded a new scene or did a "
                            + "recompile while playing, all is good, no worries ;-)");
                    }
                }
            }
            #endregion Secret Sauce to make "re-compile while playing" work ;-)
            return instance; 
        }
    }

    #region Object Pooling
    /// <summary>
    ///     Pooling score flash renderers prevents quite a bit of object creation when 
    ///     you need to render lots of messages.
    /// </summary>
    private List<ScoreFlashRendererBase> rendererPool = new List<ScoreFlashRendererBase>();
    private ScoreFlashRendererBase GetRendererFromPool() {
        ScoreFlashRendererBase objectFromPool = null;
        if (rendererPool.Count > 0) {
            int indexFromPool = rendererPool.Count - 1;
            objectFromPool = rendererPool[indexFromPool];
            rendererPool.RemoveAt(indexFromPool);
            objectFromPool.gameObject.SetActive(true);
            objectFromPool.SendMessage("InitializeRenderer", SendMessageOptions.DontRequireReceiver);
        }
        return objectFromPool;
    }

    private void PushRendererToPool(ScoreFlashRendererBase objectForPool) {
        objectForPool.Cleanup();
        objectForPool.gameObject.SetActive(false);
        rendererPool.Add(objectForPool);
    }

    /// <summary>
    ///     Pooling gui styles prevents quite a bit of object creation when using
    ///     custom renders that do not provide their own GUIStyle.
    /// </summary>
    private List<GUIStyle> guiStylePool = new List<GUIStyle>();
    private GUIStyle GetStyleFromPool() {
        GUIStyle objectFromPool = null;
        if (guiStylePool.Count > 0) {
            int indexFromPool = guiStylePool.Count - 1;
            objectFromPool = guiStylePool[indexFromPool];
            guiStylePool.RemoveAt(indexFromPool);
        } else {
            objectFromPool = new GUIStyle();
        }
        return objectFromPool;
    }
    private GUIStyle GetStyleFromPool(GUIStyle style) {
        GUIStyle objectFromPool = GetStyleFromPool();
        if (objectFromPool != null) {
            objectFromPool.name = style.name;
            objectFromPool.border = style.border;
            objectFromPool.margin = style.margin;
            objectFromPool.padding = style.padding;
            objectFromPool.overflow = style.overflow;
            objectFromPool.imagePosition = style.imagePosition;
            objectFromPool.alignment = style.alignment;
            objectFromPool.wordWrap = style.wordWrap;
            objectFromPool.clipping = style.clipping;
            objectFromPool.contentOffset = style.contentOffset;
            objectFromPool.fixedWidth = style.fixedWidth;
            objectFromPool.fixedHeight = style.fixedHeight;
            objectFromPool.font = style.font;
            objectFromPool.fontSize = style.fontSize;
            objectFromPool.fontStyle = style.fontStyle;
            objectFromPool.richText = style.richText;

            objectFromPool.normal = new GUIStyleState();
            objectFromPool.normal.textColor = style.normal.textColor;
            objectFromPool.normal.background = style.normal.background;

        }
        return objectFromPool;
    }
    private void PushStyleToPool(GUIStyle objectForPool) {
        guiStylePool.Add(objectForPool);
    }
    #endregion Object Pooling


    #region Obsolete push methods (it is recommended to use ScoreFlash.Instance.PushLocal instead)
    /// <summary>
    ///     Shows <c>msg</c> nicely. This is the one method you need to know.
    ///     Usually, you'll use <c>ScoreFlash.Push(object msg)</c>  
    ///     from anywhere in your code.
    /// </summary>
    /// <remarks>
    ///     Internally, this uses <see cref="Instance"/>, so it only works
    ///     when <see cref="ensureSingleton"/> is <c>true</c> and/or there is
    ///     only one instance of ScoreFlash in the current scene.
    /// </remarks>
    /// <param name="message">
    ///     The message to be animated by ScoreFlash. This can be a string, a 
    ///     fully configured custom renderer, an int, long, float, double or 
    ///     any object with a useful ToString() method
    /// </param>
    /// <returns>the ScoreMessage representing the message</returns>
    public static ScoreMessage Push(object message) {
        return ScoreFlash.Instance.PushLocal(message, null, null);
    }

    /// <summary>
    ///     Shows <c>msg</c> nicely in the given <c>color</c>!
    /// </summary>
    /// <param name="message">
    ///     The message to be animated by ScoreFlash. This can be a string, a 
    ///     fully configured custom renderer, an int, long, float, double or 
    ///     any object with a useful ToString() method
    /// </param>
    /// <param name="color">the color to be used for this message</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public static ScoreMessage Push(object message, Color color) {
        return ScoreFlash.Instance.PushLocal(message, color);
    }

    #region Advanced Push Methods with custom GUIStyles
    /// <summary>
    ///     Shows <c>msg</c> using <c>style</c> regardless of normal density or 
    ///     high density screen. It's up to you to make sure that <c>style</c>
    ///     works correctly. It is recommended that it has alignment set to
    ///     UpperCenter and wordWrap set to <c>false</c> (word wrap is 
    ///     automatically activated if the message is too long to be properly
    ///     displayed on screen with <see cref="minPaddingX"/>.
    /// </summary>
    /// <remarks>
    ///     Internally, this uses <see cref="Instance"/>, so it only works
    ///     when <see cref="ensureSingleton"/> is <c>true</c> and/or there is
    ///     only one instance of ScoreFlash in the current scene.
    /// </remarks>
    /// <param name="message">
    ///     The message to be animated by ScoreFlash. This can be a string, a 
    ///     fully configured custom renderer, an int, long, float, double or 
    ///     any object with a useful ToString() method
    /// </param>
    /// <param name="style">
    ///     a custom style, or <c>null</c> to use the custom style <c>ScoreFlash</c> of
    ///     <see cref="ScoreFlash.skin"/> (this parameter is optional!)
    ///     If you have a specific color assigned to that style that you want to be used, 
    ///     you need to have "Colors / Color Selection Mode" set to UseColorFromSkin
    ///     (see <see cref="ScoreFlash.colorSelectionMode"/>, 
    ///     <see cref="ScoreFlash.ColorControl"/>).
    /// </param>
    /// <returns>the ScoreMessage representing the message</returns>
    public static ScoreMessage Push(object message, GUIStyle style) {
        return Push(message, style, style);
    }

    /// <summary>
    ///     Shows <c>msg</c> using <c>style</c> or <c>styleHighDensity</c>
    ///     depending on whether we are on a normal density or high density
    ///     screen (determined using <see cref="NGUtil.IsHighDensityDisplay"/>.
    ///     It's up to you to make sure that <c>style</c>
    ///     works correctly. It is recommended that it has alignment set to
    ///     UpperCenter and wordWrap set to <c>false</c> (word wrap is 
    ///     automatically activated if the message is too long to be properly
    ///     displayed on screen with <see cref="minPaddingX"/>.
    /// </summary>
    /// <remarks>
    ///     Internally, this uses <see cref="Instance"/>, so it only works
    ///     when <see cref="ensureSingleton"/> is <c>true</c> and/or there is
    ///     only one instance of ScoreFlash in the current scene.
    /// </remarks>
    /// <param name="message">
    ///     The message to be animated by ScoreFlash. This can be a string, a 
    ///     fully configured custom renderer, an int, long, float, double or 
    ///     any object with a useful ToString() method
    /// </param>
    /// <param name="style">
    ///     a custom style to be used on normal density screens, or <c>null</c> 
    ///     to use the custom style <c>ScoreFlash</c> of
    ///     <see cref="ScoreFlash.skin"/> (this parameter is optional!)
    ///     If you have a specific color assigned to that style that you want to be used, 
    ///     you need to have "Colors / Color Selection Mode" set to UseColorFromSkin
    ///     (see <see cref="ScoreFlash.colorSelectionMode"/>, 
    ///     <see cref="ScoreFlash.ColorControl"/>).
    /// </param>
    /// <param name="styleHighDensity">the style to be used for the message on high density screens</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public static ScoreMessage Push(object message, GUIStyle style, GUIStyle styleHighDensity) {
        return ScoreFlash.Instance.PushLocal(message, style, styleHighDensity);
    }
    #endregion Advanced Push Methods with custom GUIStyles

    #endregion Obsolete push methods (it is recommended to use ScoreFlash.Instance.PushLocal instead)


    #region Implementation of IScoreFlash interface (see IScoreFlash for detailed documentation)
    /// <summary>
    ///     See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</returns>
    public ScoreMessage PushLocal(object message) {
        return PushLocal(message, null, null);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="color">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <returns>See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</returns>
    public ScoreMessage PushLocal(object message, Color color) {
        return PushLocal(message, color, null, null, (int)ScoreFlash.DefaultMessageQueueID.Default, null);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</returns>
    public ScoreMessage PushLocal(object message, GUIStyle style) {
        return PushLocal(message, style, style);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="styleHighDensity">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</returns>
    public ScoreMessage PushLocal(object message, GUIStyle style, GUIStyle styleHighDensity) {
        return PushLocal(message, null, style, styleHighDensity, (int)ScoreFlash.DefaultMessageQueueID.Default, null);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="styleHighDensity">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="messageQueueID">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</returns>
    public ScoreMessage PushLocal(object message,
            GUIStyle style, GUIStyle styleHighDensity,
            int messageQueueID) { // Unity fails on compiling: (int)ScoreFlash.DefaultMessageQueueID.Default

        return PushLocal(message, null, style, styleHighDensity, messageQueueID, null);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="color">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <param name="messageQueueID">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</returns>
    public ScoreMessage PushLocal(object message, Color color, int messageQueueID) {
        return PushLocal(message, color, null, null, messageQueueID, null);
    }


    /// <summary>
    ///     See <see cref="IScoreFlash.PushScreen(Vector2, object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="screenPosition">See <see cref="IScoreFlash.PushScreen(Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushScreen(Vector2 screenPosition, object message) {
        return PushScreen(screenPosition, message, null, null, (int)ScoreFlash.DefaultMessageQueueID.DefaultUserPosition);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushScreen(Vector2, object, Color, int)"/>!
    /// </summary>
    /// <param name="screenPosition">See <see cref="IScoreFlash.PushScreen(Vector2, object, Color, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <param name="color">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushScreen(Vector2 screenPosition, object message, Color color) {
        return PushScreen(screenPosition, message, color, (int)ScoreFlash.DefaultMessageQueueID.DefaultUserPosition);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushScreen(Vector2, object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="screenPosition">See <see cref="IScoreFlash.PushScreen(Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushScreen(Vector2 screenPosition, object message, GUIStyle style) {
        return PushScreen(screenPosition, message, style, style, (int)ScoreFlash.DefaultMessageQueueID.DefaultUserPosition);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushScreen(Vector2, object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="screenPosition">See <see cref="IScoreFlash.PushScreen(Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="styleHighDensity">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushScreen(Vector2 screenPosition, object message, GUIStyle style, GUIStyle styleHighDensity) {
        return PushScreen(screenPosition, message, style, styleHighDensity, (int)ScoreFlash.DefaultMessageQueueID.DefaultUserPosition);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushScreen(Vector2, object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="screenPosition">See <see cref="IScoreFlash.PushScreen(Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="styleHighDensity">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="messageQueueID">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushScreen(Vector2 screenPosition, object message, GUIStyle style, GUIStyle styleHighDensity, int messageQueueID) {
        return PushScreen(screenPosition, message, null, style, styleHighDensity, messageQueueID);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushScreen(Vector2, object, Color, int)"/>!
    /// </summary>
    /// <param name="screenPosition">See <see cref="IScoreFlash.PushScreen(Vector2, object, Color, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <param name="color">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <param name="messageQueueID">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushScreen(Vector2 screenPosition, object message, Color color, int messageQueueID) {
        return PushScreen(screenPosition, message, color, null, null, messageQueueID);
    }

    private ScoreMessage PushScreen(Vector2 screenPosition, object message, Color? color, GUIStyle style, GUIStyle styleHighDensity, int messageQueueID) {
        ScoreMessage msg = PushLocal(message, color, null, null, messageQueueID, null);
        screenPosition.y = Screen.height - screenPosition.y; // UnityGUI goes this way, Mouse goes that way ... YAY :-/
        // this offset is no longer valid!
        // screenPosition.y -= msg.Position.height * 0.5F;
        msg.ReferencePosition = screenPosition;
        msg.OriginalPosition = screenPosition; // in this case, it's always the same
        return msg;
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="worldPosition">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="screenOffset">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message) {
        return PushWorld(worldPosition, screenOffset, message, null);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, Color, int)"/>!
    /// </summary>
    /// <param name="worldPosition">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, Color, int)"/>!</param>
    /// <param name="screenOffset">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, Color, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <param name="color">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, Color color) {
        return PushWorld(worldPosition, screenOffset, message, color, (int)ScoreFlash.DefaultMessageQueueID.DefaultUserPosition);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="worldPosition">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="screenOffset">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, GUIStyle style) {
        return PushWorld(worldPosition, screenOffset, message, style, style);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="worldPosition">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="screenOffset">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="styleHighDensity">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, GUIStyle style, GUIStyle styleHighDensity) {
        return PushWorld(worldPosition, screenOffset, message, style, styleHighDensity, (int)ScoreFlash.DefaultMessageQueueID.DefaultUserPosition);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!
    /// </summary>
    /// <param name="worldPosition">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="screenOffset">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="styleHighDensity">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="messageQueueID">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, GUIStyle style, GUIStyle styleHighDensity, int messageQueueID) {
        return PushWorld(worldPosition, screenOffset, message, null, style, styleHighDensity, messageQueueID);
    }


    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, Color, int)"/>!
    /// </summary>
    /// <param name="worldPosition">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, Color, int)"/>!</param>
    /// <param name="screenOffset">See <see cref="IScoreFlash.PushWorld(Vector3, Vector2, object, Color, int)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <param name="color">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <param name="messageQueueID">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, Color color, int messageQueueID) {
        return PushWorld(worldPosition, screenOffset, message, color, null, null, messageQueueID);
    }

    private ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, Color? color, GUIStyle style, GUIStyle styleHighDensity, int messageQueueID) {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition -= screenOffset;
        return PushScreen(screenPosition, message, color, style, styleHighDensity, messageQueueID);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>!
    /// </summary>
    /// <param name="follow3D">See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message) {
        return PushWorld(follow3D, message, null);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, Color)"/>!
    /// </summary>
    /// <param name="follow3D">See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, Color)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, Color)"/>!</param>
    /// <param name="color">See <see cref="IScoreFlash.PushLocal(object, Color, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message, Color color) {
        return PushWorld(follow3D, message, color, null, null);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>!
    /// </summary>
    /// <param name="follow3D">See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message, GUIStyle style) {
        return PushWorld(follow3D, message, style, style);
    }

    /// <summary>
    ///     See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>!
    /// </summary>
    /// <param name="follow3D">See <see cref="IScoreFlash.PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>!</param>
    /// <param name="message">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="style">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <param name="styleHighDensity">See <see cref="IScoreFlash.PushLocal(object, GUIStyle, GUIStyle, int)"/>!</param>
    /// <returns>the ScoreMessage representing the message</returns>
    public ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message, GUIStyle style, GUIStyle styleHighDensity) {
        return PushWorld(follow3D, message, null, style, styleHighDensity);
    }

    private ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message, Color? color, GUIStyle style, GUIStyle styleHighDensity) {
        if (!follow3D.keepStatic) {
            return PushLocal(message, color, style, styleHighDensity, follow3D.GetInstanceID(), follow3D);
        } else {
            Vector3 worldPosition = follow3D.GetLocation().CurrentTranslatedPosition;
            Vector2 screenOffset = follow3D.GetLocation().CurrentScreenOffset;
            ScoreMessage msg = PushWorld(worldPosition, screenOffset, message, color, style, styleHighDensity, follow3D.GetInstanceID());
            return msg;
        }
    }

    internal ScoreMessage PushWorldInternal(ScoreFlashFollow3D follow3D, object message, bool enqueueMessage) {
        // follow3D.keepStatic is ignored in the specific case
        return PushLocal(message, null, null, null, enqueueMessage ? (int?) follow3D.GetInstanceID() : null, follow3D);
    }

    #endregion Implementation of IScoreFlash interface (see IScoreFlash for detailed documentation)


    #region PushLocal - registering messages to be shown by ScoreFlash
    internal ScoreMessage PushLocal(object message,
            Color? color, GUIStyle style, GUIStyle styleHighDensity,
            int? messageQueueID, ScoreFlashFollow3D follow3D) { // Unity fails on compiling: (int)ScoreFlash.DefaultMessageQueueID.Default

        // if this instance of ScoreFlash is not enabled - there's no point in registering the message
        if (!this.enabled) {
            return new ScoreMessage(this, this, message);
        }

        #region Initializing the Message

        // note: messageQueueID null => do not put the message in the queue (used internally)
        // if we have a value here, we're never in the scene view => false
        if (messageQueueID.HasValue) {
            SetIsSceneView(false);
        }

        IHasVisualDesigner scoreFlashLayout = this;
        if (message is ScoreFlashLayout) {
            ScoreFlashLayout sender = (ScoreFlashLayout)message;
            scoreFlashLayout = sender;
            message = sender.MessageForScoreFlash;
        }

        ScoreMessage msg = new ScoreMessage(this, scoreFlashLayout, message);

        if (follow3D != null) {
            msg.SetScoreFlashFollow3D(follow3D);
            if (msg.FollowLocation != null && !allFollow3Ds.Contains(msg.FollowLocation)) {
                allFollow3Ds.Add(msg.FollowLocation);
            }
        }

        msg.isDebug = isDebugMode;
        msg.pos = NGUtil.Scale(new Vector2(fadeInOffsetX, fadeInOffsetY));
        msg.rotationSpeed = fadeOutInitialRotationSpeed;
        msg.useRealTime = timeReference == TimeReference.UseRealTime;

        if (msg.scoreFlashRenderer == null) {
            if (rendering == RenderingType.CustomRenderer && scoreFlashRenderer != null) {
                // do we still have objects in the pool? if so => take on from there ;-)
                ScoreFlashRendererBase objectFromPool = GetRendererFromPool();
                if (objectFromPool != null) {
                    msg.scoreFlashRenderer = objectFromPool;
                } else {
                    Transform parent = scoreFlashRenderer.RequiresCustomParent && customParent != null
                        ? customParent : this.transform;
                    msg.scoreFlashRenderer = scoreFlashRenderer.CreateInstance(parent);
                    msg.scoreFlashRenderer.name = string.Format("Renderer_{0:0000}", rendererIndex++);
                }
            }
        } else { // if we have a renderer "delivered" with the message, do the parent assignment now
            if (scoreFlashRenderer.RequiresCustomParent && customParent != null) {
                msg.scoreFlashRenderer.transform.SetParent(customParent);
            } else {
                msg.scoreFlashRenderer.transform.SetParent(this.transform);
            }
        }

        // if a style is passed to the method, this overwrites the font definition!
        if (style == null && rendering == RenderingType.UnityGUI_Font) {
            style = GetStyleFromPool();
            style.font = CurrentFont;
            style.normal.textColor = Color.white;
            style.alignment = (TextAnchor)NGAlignment.ConvertAlignment(msg.InnerAnchor, NGAlignment.AlignmentType.TextAnchor);
            style.wordWrap = false;
        }

        // if we are using UnityGUI, the following is relevant ...
        if (style == null) {
            if (CurrentSkin == null && msg.scoreFlashRenderer == null) {
                Debug.LogError(
                    string.Format(
                    "You have not assigned a GUISkin and called PushLocal without a style - this won't work! "
                    + "Either assign a GUISkin to ScoreFlash, or use one of the Push-methods that take a "
                    + "GUIStyle as parameter! Message '{0}' will not be shown!", message), this);
                return msg;
            }

            if (CurrentSkin != null) {
                if (CurrentSkin.FindStyle(guiStyleName) == null) {
                    if (!hasWarnedMissingCustomStyle) {
                        Debug.LogWarning(
                            string.Format("Your GUISkin does not have custom style '{0}' - please add that custom style "
                            + "and configure it with your desired font. Using label style instead!", guiStyleName),
                            CurrentSkin);
                        hasWarnedMissingCustomStyle = true;
                    }
                    style = GetStyleFromPool(CurrentSkin.label);
                } else {
                    style = GetStyleFromPool(CurrentSkin.FindStyle(guiStyleName));
                }
                // we need these two and people (including myself) forget to assign them => hardcode it!
                //style.alignment = TextAnchor.UpperCenter; // this is now overridden with innerAnchor

                // this may be overridden when the message exceeds maxWidth
                style.wordWrap = false;
            }
        }
        if (styleHighDensity == null) {
            styleHighDensity = style;
        }


        // this is just an initial reference position - it may be overwritten later!
        if (follow3D == null) {
            msg.ScreenAlign = scoreFlashLayout.ScreenAlign;
            msg.InnerAnchor = scoreFlashLayout.LockInnerAnchor ? msg.ScreenAlign : scoreFlashLayout.InnerAnchor;
            msg.OriginalPosition = NGAlignment.GetScreenBasedReferencePosition(msg.ScreenAlign, Vector2.zero, IsSceneView);
            msg.ReferencePosition = NGAlignment.GetScreenBasedReferencePosition(msg.ScreenAlign, scoreFlashLayout.Position, IsSceneView);
        }

        if (style != null) {
            AssignStyle(msg, style, styleHighDensity);
        }

        if (!color.HasValue) {
            AssignColors(msg);
        } else {
            AssignAlphaMultipliedColor(msg, color.Value);
        }

        // when passing a custom renderer, don't use the one ScoreFlash might define
        if (msg.scoreFlashRenderer != null) {
            // this is put down here because ScoreFlashRendererUnityGUI needs the style to calculate the size
            msg.scoreFlashRenderer.Initialize(msg);
            Rect pos = msg.Position;
            Vector2 size = msg.scoreFlashRenderer.GetSize(msg);
            pos.width = size.x;
            pos.height = size.y;
            msg.Position = pos;
        }

        if (messageQueueID.HasValue) {
            if (!scoreMessages.ContainsKey(messageQueueID.Value)) {
                scoreMessages[messageQueueID.Value] = new NGQueue<ScoreMessage>();
            }
            NGQueue<ScoreMessage> messageQueue = scoreMessages[messageQueueID.Value];

            messageQueue.Enqueue(msg);
            if (messageQueue.Count > maxSimultanuousMessages) {
                messageQueue.QueueOrderedList[messageQueue.Count - maxSimultanuousMessages - 1].LocalTimeScale = 8;
            }

            AnimateMessages(); // make sure everything is immediately initialized
#if LOGGING_FACADE // if narayana games Logging Facade is available, we add some logging
        log.DebugFormat("Received score message: '{0}'", msg.Text);
#endif
        }

        return msg;
        #endregion Initializing the Message
    }


    private void AssignStyle(ScoreMessage msg, GUIStyle style, GUIStyle styleHighDensity) {
        msg.style = NGUtil.IsHighDensityDisplay ? styleHighDensity : style;

        msg.style.alignment = (TextAnchor)NGAlignment.ConvertAlignment(msg.InnerAnchor, NGAlignment.AlignmentType.TextAnchor);
        //msg.style.alignment = TextAnchor.UpperLeft;

        // initialize position (y will be assigned in Update)
        GUIContent msgText = new GUIContent(msg.Text);
        Vector2 size = msg.style.CalcSize(msgText);
        float maxWidth = msg.MaxWidth;
        if (size.x > maxWidth) {
            msg.style.wordWrap = true;
#if LOGGING_FACADE
            log.DebugFormat("Applying width fix for '{0}' (word wrapping activated!)", msg.Text);
#endif
            size.x = maxWidth;
            size.y = msg.style.CalcHeight(msgText, size.x);
        }
        Rect position = msg.Position;
        position.width = size.x;
        position.height = size.y;
        msg.Position = position;

        msg.styleOutline = GetStyleFromPool(msg.style);
    }

    // color index for ColorControl.Sequence
    private int colorIndex = 0;
    // previous color index to prevent same color being shown twice
    private int previousIndex = -1;
    // used color indices for ColorControl.Random
    private List<int> usedColorIndices = new List<int>();

    private void AssignColors(ScoreMessage msg) {
        switch (colorSelectionMode) {
            case ColorControl.FadePhases:
                msg.fadeInColor = fadeInColor;
                msg.readColorStart = readColorStart;
                msg.readColorEnd = readColorEnd;
                msg.fadeOutColor = fadeOutColor;
                break;
            case ColorControl.UseColorFromSkin:
                if (msg.style == null) {
                    Debug.LogError(
                        "Trying to use ColorControl.UseColorFromSkin without a skin assigned or a GUIStyle passed! "
                        + " You either need to assign a skin, or pass a GUIStyle in the push-method, or choose another "
                        + " Color Selection Mode!");
                } else {
                    AssignAlphaMultipliedColor(msg, msg.style.normal.textColor);
                }
                break;
            case ColorControl.Sequence:
                if (CheckForColors(msg)) {
                    AssignAlphaMultipliedColor(msg, colors[colorIndex++ % colors.Count]);
                }
                break;
            case ColorControl.Random:
                if (CheckForColors(msg)) {
                    int index = Random.Range(0, colors.Count);
                    int counter = 0;
                    /*
                     * we check for colors.Count > 2 because with only
                     * two colors to pick from, our "use previous color prevention"
                     * would lead to the two colors simply alterating; so in
                     * that case, to achieve a bit of randomness, being able
                     * to show the same color twice is actually better
                     */
                    while (colors.Count > 2 && (previousIndex == index || usedColorIndices.Contains(index))) {
                        index = Random.Range(0, colors.Count);
                        /*
                         * we only try twice the number of available colors
                         * if that fails, either we had very bad luck or the
                         * number of colors has meanwhile changed and we might
                         * never find an index from the available numbers that
                         * is not yet used; colors.Count * 2 is really just
                         * a heuristic; you could put any number in here
                         */
                        if (usedColorIndices.Count >= colors.Count || counter > colors.Count * 2) {
#if LOGGING_FACADE
                            log.DebugFormat(
                                "Cleared used indices usedColorIndices.Count={0} >= colors.Count={1} || counter={2} > colors.Count * 2={3}",
                                usedColorIndices.Count, colors.Count, counter, colors.Count * 2);
#endif
                            usedColorIndices.Clear();
                            counter++;
                        }
                    }
#if LOGGING_FACADE
                    log.DebugFormat("Using color: {0}", index);
#endif
                    previousIndex = index;
                    usedColorIndices.Add(index);
                    AssignAlphaMultipliedColor(msg, colors[index]);
                }
                break;

        }
    }

    private bool CheckForColors(ScoreMessage msg) {
        if (colors == null || colors.Count == 0) {
            if (msg.style == null) {
                Debug.LogError(
                    "No colors assigned, no skin assigned - please set up ScoreFlash correctly: "
                    +"Either assign colors, or a skin, or select Color Selection Mode: FadePhases!");
            } else {
                Debug.LogWarning(
                    string.Format("No Colors assigned - cannot use Color Selection Mode: {0}! Using skin color as fallback!",
                    colorSelectionMode));
                AssignAlphaMultipliedColor(msg, msg.style.normal.textColor);
            }
            return false;
        }
        return true;
    }

    internal void AssignAlphaMultipliedColor(ScoreMessage msg, Color color) {
        msg.fadeInColor = color;
        msg.readColorStart = color;
        msg.readColorEnd = color;
        msg.fadeOutColor = color;
        /*
         * apply multipliers on all colorSelectionModes except FadePhases
         * FadePhases has the alpha values assigned through the colors
         * for each phase, so we don't need this here
         */
        msg.fadeInColor.a *= alphaFadeInMultiplier;
        msg.readColorStart.a *= alphaReadStartMultiplier;
        msg.readColorEnd.a *= alphaReadEndMultiplier;
        msg.fadeOutColor.a *= alphaFadeOutMultiplier;
    }

    #endregion PushLocal - registering messages to be shown by ScoreFlash


    #region Legacy Method - please do not use anymore! Will be removed in some later version!
    /// <summary>
    ///     Legacy method - no longer recommended. Use <c>ScoreFlash.Instance.PushLocal(object)</c>
    ///     (<see cref="IScoreFlash.PushLocal(object)" />)
    ///     instead. 
    /// </summary>
    /// <remarks>
    ///     This is still called
    ///     "Show" because you can't have the same method name static and
    ///     non-static (kind of obvious but I didn't want to introduce a
    ///     breaking change ... and Push is kind of better for this, anyways
    ///     because "Show" is too generic).
    /// </remarks>
    /// <param name="text">The message to be shown</param>
    [System.Obsolete("Use ScoreFlash.Instance.PushLocal(object) instead!", true)]
    public void Show(string text) {
        PushLocal(text);
    }
    #endregion Legacy Method - please do not use anymore! Will be removed in some later version!

    #region If messages are pushed directly from the editor, Screen.width/height are off
#if UNITY_EDITOR
    private string messageFromEditor = null;
    /// <summary>
    ///     This is just a helper to be able to send messages directly from the
    ///     inspector. If calling ScoreFlash.Push(...) directly from there, the Screen.width
    ///     is taken from the inspector window which is not exactly what we want ;-)
    /// </summary>
    /// <param name="text"></param>
    public void PushFromInspector(string text) {
        messageFromEditor = text;
    }
#endif
    #endregion If messages are pushed directly from the editor, Screen.width/height are off

    /// <summary>
    ///     Handles the animation of the messages.
    /// </summary>
    void Update() {
#if UNITY_EDITOR
        // pick up message that was pushed from editor script
        if (Application.isPlaying && messageFromEditor != null) {
            try {
                this.PushLocal(messageFromEditor);
            } catch {
                Debug.LogError(string.Format("Could not push editor message: '{0}'!", messageFromEditor));
            }
            messageFromEditor = null;
        }
#endif
        SetIsSceneView(false);
        HandleStuffForTesting();

        if (scoreMessages.Count > 0) {
            AnimateMessages();
        }
    }

    #region FixedUpdate for ScoreFlashFollow3D
    private List<ScoreFlashFollow3DLocation> allFollow3Ds = new List<ScoreFlashFollow3DLocation>();
    void FixedUpdate() {
        foreach (ScoreFlashFollow3DLocation follow3D in allFollow3Ds) {
            follow3D.FixedUpdate();
        }
    }
    #endregion FixedUpdate for ScoreFlashFollow3D

    #region HandleStuffForTesting
    private Coroutine showTestMessages = null;

    private void HandleStuffForTesting() {
        // write value of forceHighDensity to Util-class
        NGUtil.ForceHighDensity = isTestForceHighDensity;

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
        while (showTestMessages != null) { // we can end this coroutine by setting it to null
            this.PushLocal("+1000");
            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;
            this.PushLocal("+11");
            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;
            this.PushLocal("+22");
            yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            if (showTestMessages == null)
                break;
            if (includeMessageSpam) {
                this.PushLocal("Here comes a burst of 10 messages");
                yield return new WaitForSeconds(0.5F * (Time.timeScale + 0.00001F));
                this.PushLocal("+333");
                this.PushLocal("+11");
                this.PushLocal("+108");
                this.PushLocal("+888");
                this.PushLocal("And now a long message");
                if (includeVeryLongMessages) {
                    this.PushLocal(
                        "And now a very long message to see how that is handled in the middle of a burst!");
                }
                this.PushLocal("+108");
                this.PushLocal("+333");
                this.PushLocal("+11");
                yield return new WaitForSeconds(0.1F * (Time.timeScale + 0.00001F));
                this.PushLocal("+2000");
                yield return new WaitForSeconds(0.2F * (Time.timeScale + 0.00001F));
                this.PushLocal("+108");
                yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            }
            if (includeVeryLongMessages) {
                this.PushLocal(
                    "I really wanted to mention that I appreciate you checking "
                    + "out this awesome tool and hope you enjoy using it as much "
                    + "as I did!!!");
                yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds); // make these precise!
                if (showTestMessages == null)
                    break;
                this.PushLocal(
                    "Also, I want to say THANK YOU to Unity for creating such an "
                    + "awesome game engine that makes it possible for so many "
                    + "people to make their visions come true!!!");
                yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
                if (showTestMessages == null)
                    break;
                this.PushLocal(
                    "And finally: NOW is a really good time to go to the asset "
                    + "store and give me a good rating and positive comment ... "
                    + "at least, if that's how you feel about this. Otherwise, be "
                    + "sure to drop me a message to jashan@narayana-games.net so I "
                    + "can fix whatever you're struggling with!");
                yield return new WaitForSeconds(NormalizedTestMessageDelaySeconds);
            }
            if (showTestMessages == null)
                break;
        }
    }
    #endregion HandleStuffForTesting

    /// <summary>
    ///     Immediately cleans up all messages currently on screen.
    /// </summary>
    public void Cleanup() {
        // animate all currently active messages
        completedMessagesQueues.Clear();
        foreach (int messageQueueID in scoreMessages.Keys) {
            while (scoreMessages[messageQueueID].Count > 0) {
                CleanupMessage(messageQueueID);
            }
        }
        foreach (int messageQueueID in completedMessagesQueues) {
            scoreMessages.Remove(messageQueueID);
        }
    }
    #region Animating the Messages

    private List<int> completedMessagesQueues = new List<int>();

    private void AnimateMessages() {
        // animate all currently active messages
        completedMessagesQueues.Clear();
        foreach (int messageQueueID in scoreMessages.Keys) {
            // Sort messages by age to make sure Dequeue() will work as expected, and "spreading" the messages will also work
#if !UNITY_FLASH // this breaks everything in Flash :-/ => frozen messages are not handled properly in Flash
            scoreMessages[messageQueueID].Sort();
#endif
            List<ScoreMessage> messages = scoreMessages[messageQueueID]; // new List<ScoreMessage>(scoreMessages[messageQueueID]);

            for (int i = messages.Count - 1; i >= 0; i--) { // oldest one last...
                ScoreMessage msg = messages[i];
                float myDeltaTime = msg.useRealTime ? Time.unscaledDeltaTime : Time.deltaTime;

                // when we have too many messages: let old messages disappear quickly (LocalTimeScale is usually 1 or 8)
                msg.AddDeltaTimeToAge(myDeltaTime * msg.LocalTimeScale);

                // read long messages longer so they can be read by the player
                float addToRead = CalcAddToRead(msg);

                AnimatePhases(myDeltaTime * msg.LocalTimeScale, msg, addToRead);

                msg.SetSceneView(false);
                Rect position = msg.Position;
                position.x = msg.ReferencePosition.x + msg.pos.x;
                position.y = msg.ReferencePosition.y + msg.pos.y;
                msg.Position = position;

                // spread messages vertically when there are too many too quickly
                if (msg.LocalTimeScale >= 0.1F) {
                    for (int k = i + 1; k < messages.Count; k++) {
                        ScoreMessage otherMsg = messages[k];
                        if (otherMsg.LocalTimeScale < 0.1F) { // ignore frozen messages
                            continue;
                        }
                        ScoreMessage nextMsg = msg;
                        //for (int nextID = k - 1; nextID >= 0; nextID--) {
                        //    nextMsg = messages[nextID]; // starts with k - 1
                        //    if (nextMsg.LocalTimeScale >= 0.1F) { // found next that is not frozen
                        //        break;
                        //    }
                        //}
                        /* NOTE: This currently doesn't handle messages in different locations too well.
                         * For best results, make sure to use different messagesQueues for different
                         * locations!!!
                         */
                        if (Vector2.Distance(otherMsg.ReferencePosition, nextMsg.ReferencePosition) < 20F) { // did we start "close to each other"?
                            if (spreadImmediately || nextMsg.Age > fadeInTimeSeconds) {
                                SpreadMessages(myDeltaTime, otherMsg, nextMsg);
                            }
                        }
                    }
                }

                msg.UpdateRenderer();

                // when we're done, we're done :-)
                if (msg.Age > fadeInTimeSeconds + readTimeSeconds + addToRead + fadeOutTimeSeconds) {
                    CleanupMessage(messageQueueID);
                }
            }
        }
        foreach (int messageQueueID in completedMessagesQueues) {
            scoreMessages.Remove(messageQueueID);
        }
    }

    private void AnimatePhases(float myDeltaTime, ScoreMessage msg, float addToRead) {
        if (msg.Age < fadeInTimeSeconds) { // phase 1: fade in
            float diffFrac = msg.Age / fadeInTimeSeconds;

            msg.CurrentTextColor = NGEasing.EaseOnCurve(fadeInColorCurve, msg.fadeInColor, msg.readColorStart, diffFrac);
            msg.pos.x = NGEasing.EaseOnCurve(fadeInOffsetXCurve, NGUtil.Scale(fadeInOffsetX), 0, diffFrac);
            msg.pos.y = NGEasing.EaseOnCurve(fadeInOffsetYCurve, NGUtil.Scale(fadeInOffsetY), 0, diffFrac);
            msg.Scale = NGEasing.EaseOnCurve(fadeInScaleCurve, fadeInScale, 1, diffFrac);
        } else if (msg.Age < (fadeInTimeSeconds + readTimeSeconds + addToRead)) { // phase 2: read
            float diffFrac = (msg.Age - fadeInTimeSeconds) / (readTimeSeconds + addToRead);
            msg.CurrentPhase = ScoreMessage.Phase.Read;

            msg.CurrentTextColor = NGEasing.EaseOnCurve(readColorCurve, msg.readColorStart, msg.readColorEnd, diffFrac);
            msg.Scale = NGEasing.EaseOnCurve(readScaleCurve, 1, readScale, diffFrac);
            msg.velocity.x = NGEasing.EaseOnCurve(readVelocityXCurve, 0, NGUtil.Scale(readFloatRightVelocity), diffFrac);
            msg.velocity.y = NGEasing.EaseOnCurve(readVelocityCurve, 0, NGUtil.Scale(readFloatUpVelocity), diffFrac);

            msg.pos.x += msg.velocity.x * myDeltaTime;
            msg.pos.y -= msg.velocity.y * myDeltaTime;
        } else { // phase 3: fade out
            float diffFrac = (msg.Age - (fadeInTimeSeconds + readTimeSeconds + addToRead)) / fadeOutTimeSeconds;
            msg.CurrentPhase = ScoreMessage.Phase.FadeOut;

            msg.CurrentTextColor = NGEasing.EaseOnCurve(fadeOutColorCurve, msg.readColorEnd, msg.fadeOutColor, diffFrac);
            msg.Scale = NGEasing.EaseOnCurve(fadeOutScaleCurve, readScale, fadeOutScale, diffFrac);
            msg.velocity.x = NGEasing.EaseOnCurve(fadeOutVelocityXCurve, NGUtil.Scale(readFloatRightVelocity), NGUtil.Scale(fadeOutFloatRightVelocity), diffFrac);
            msg.velocity.y = NGEasing.EaseOnCurve(fadeOutVelocityCurve, NGUtil.Scale(readFloatUpVelocity), NGUtil.Scale(fadeOutFloatUpVelocity), diffFrac);

            msg.pos.x += msg.velocity.x * myDeltaTime;
            msg.pos.y -= msg.velocity.y * myDeltaTime;

            msg.rotationSpeed += fadeOutRotationAcceleration * myDeltaTime;
            msg.Rotation += msg.rotationSpeed * myDeltaTime;
        }
        // assign outline color with alpha based on current value from phase
        msg.OutlineColor = colorOutline;
        msg.OutlineColorAlpha = msg.CurrentTextColor.a * colorOutline.a;
    }

    private void SpreadMessages(float myDeltaTime, ScoreMessage msg, ScoreMessage nextMsg) {
        float distance = Mathf.Abs(Mathf.Abs(msg.pos.y) - Mathf.Abs(nextMsg.pos.y));
        float requiredHeight = NGUtil.Scale(minDistanceBetweenMessages) + nextMsg.Position.height;
        if (distance < requiredHeight) {
            msg.pos.y -= myDeltaTime * (requiredHeight - distance) * spreadSpeed;

            float addToRead = CalcAddToRead(msg);

            if (msg.Age < fadeInTimeSeconds + readTimeSeconds + addToRead) {
                msg.AddDeltaTimeToAge(myDeltaTime);
            }
            msg.UpdateRenderer();
        }
    }

    private float CalcAddToRead(ScoreMessage msg) {
        float addToRead = 0;
        if (msg.Text.Length > readMinLengthCharsToAddTime) {
            addToRead = readAddTimeSeconds;
            // this should usually not happen; for "ultra-long texts"; if that's still not sufficient: simply ignore ...
            if (msg.Text.Length > 2 * readMinLengthCharsToAddTime) {
                addToRead += readAddTimeSeconds;
            }
        }
        return addToRead;
    }

    private void CleanupMessage(int messageQueueID) {
        ScoreMessage msg = scoreMessages[messageQueueID].Dequeue();
        if (msg.FollowLocation != null && !allFollow3Ds.Contains(msg.FollowLocation)) {
            allFollow3Ds.Remove(msg.FollowLocation);
        }
        if (scoreMessages[messageQueueID].Count == 0) {
            completedMessagesQueues.Add(messageQueueID);
        }
        if (Application.isPlaying) {
            if (msg.scoreFlashRenderer != null) {
                PushRendererToPool(msg.scoreFlashRenderer);
            }
            PushStyleToPool(msg.style);
            PushStyleToPool(msg.styleOutline);
        } else {
            msg.DestroyInstance();
        }
    }

    #endregion Animating the Messages

#if SCOREFLASH_FREE_DEMO
    private Rect demoRect = new Rect(20, 20, 180, 30);
    private float startTime = 0;

    public void Start() {
        startTime = Time.realtimeSinceStartup;
    }

    public void OnEnable() {
        startTime = Time.realtimeSinceStartup;
    }
#endif

    /// <summary>
    ///     Draws UnityGUI based messages ... and the scene view visual designer.
    /// </summary>
    void OnGUI() {
        useGUILayout = false;
        SetIsSceneView(false);

#if SCOREFLASH_FREE_DEMO
        float time = 15 - (Time.realtimeSinceStartup - startTime);
        if (Application.isPlaying && time > 0) {
            demoRect.x = Screen.width - demoRect.width - 20;
            demoRect.y = Screen.height - demoRect.height - 20;
            if (GUI.Button(demoRect, string.Format("Buy ScoreFlash Full ({0})", (int)(time)))) {
                Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/4476");
            }
        }
#endif

        #region Editor stuff
#if UNITY_EDITOR
        if (Application.isEditor && IsSelected) {
            GUISkin skin = GUI.skin;
            GUI.skin = editorGUISkin;
            if (isDebugMode) {
                DrawEditorStatistics();
            }
            if (isDesignMode) {
                // immediately destroy the message after it was used for rendering in the designer
                Designer.DrawEditorGUIDesign(false).DestroyInstance();
            }
            GUI.skin = skin;
            if (!Application.isPlaying) {
                scoreMessages.Clear(); // make sure this is always clean!
                return;
            }
        }
#endif
        #endregion Editor stuff

        // only if we have not assigned a custom renderer
        if (rendering != RenderingType.CustomRenderer) {
            if (scoreMessages.Count > 0) {
                RenderScoreMessage();
            }
        }
	}

    #region Advanced Editor Scripting
#if UNITY_EDITOR
    // this should never be accessed directly ... hence the name ;-)
    private ScoreFlashVisualDesigner designerDoNotEverAccess = null;
    // that's the way to do it
    private ScoreFlashVisualDesigner Designer {
        get {
            if (designerDoNotEverAccess == null) {
                designerDoNotEverAccess = new ScoreFlashVisualDesigner(this, this.isDesignMode, designText);
            }
            // make sure that data is updated
            designerDoNotEverAccess.isDesignMode = isDesignMode;
            designerDoNotEverAccess.designText = designText;
            return designerDoNotEverAccess;
        }
    }

    private void DrawEditorStatistics() {
        GUI.Label(new Rect(10, 10, 400, 20), string.Format("{0}: {1} queues", this.name, scoreMessages.Count));
        float y = 0;
        foreach (int key in scoreMessages.Keys) {
            y += 15;
            GUI.Label(new Rect(10, 10 + y, 400, 20), string.Format("Queue {0}: {1} messages", key, scoreMessages[key].Count));
        }
    }

#endif // UNITY_EDITOR
    #endregion Advanced Editor Scripting


    #region Rendering using UnityGUI
    private GUISkin CurrentSkin {
        get {
            return (NGUtil.IsHighDensityDisplay && skinHighDensity != null)
                ? skinHighDensity
                : skin;
        }
    }

    private Font CurrentFont {
        get {
            return (NGUtil.IsHighDensityDisplay && skinHighDensity != null)
                ? fontHighDensity
                : font;
        }
    }

    private void RenderScoreMessage() {
        foreach (int messageQueueID in scoreMessages.Keys) {
            List<ScoreMessage> messages = scoreMessages[messageQueueID];
            for (int i = messages.Count - 1; i >= 0; i--) { // oldest one last...
                Matrix4x4 originalGUIMatrix = GUI.matrix;

                ScoreMessage msg = messages[i];
                if (!msg.IsVisible) {
                    continue;
                }

                if (msg.style == null) {
                    Debug.LogError(
                        "Message has no style - something went wrong, look previous for error messages! "
                        + "Most likely, you either need to assign a Skin to ScoreFlash (recommended), or "
                        + "pass a GUIStyle-parameter to the push-method you're using (advanced)!");
                }

                Vector2 alignBasedOffset = ScoreFlash.GetAlignBasedOffset(msg);

                Rect localPos = msg.Position;
                localPos.x += alignBasedOffset.x;
                localPos.y += alignBasedOffset.y;

                Vector2 pivotPoint = new Vector2(localPos.x + localPos.width * 0.5F, localPos.y + localPos.height * 0.5F);
                GUIUtility.ScaleAroundPivot(new Vector2(msg.Scale, msg.Scale), pivotPoint);
                GUIUtility.RotateAroundPivot(msg.Rotation, pivotPoint);

                if (msg.style != null) {
                    // this is only called when we have no custom renderer => using UnityGUI => style must be assigned!
                    msg.style.normal.textColor = msg.CurrentTextColor;
                    msg.styleOutline.normal.textColor = msg.OutlineColor;
                }

                bool renderOutline = !disableOutlines;
#if UNITY_IPHONE || UNITY_ANDROID
                renderOutline = false;
#endif //UNITY_IPHONE || UNITY_ANDROID

                // paint a box so we see how large the area precisely is
#if UNITY_EDITOR
                if (msg.isDebug && IsSelected) {
                    GUISkin skin = GUI.skin;
                    GUI.skin = editorGUISkin;
                    Color origColor = GUI.color;

                    Color currentColor = Color.yellow;
                    currentColor.a = 0.6F;
                    GUI.color = currentColor;
                    GUI.Box(localPos, "");

                    currentColor = Color.white;
                    currentColor.a = 1F;
                    GUI.color = currentColor;

                    float size = NGUtil.Scale(10);
                    GUI.Box(new Rect(msg.Position.x - size * 0.5F, msg.Position.y - size * 0.5F, size, size), "");

                    GUI.skin = skin;
                    GUI.color = origColor;
                }
#endif

                if (!disableOutlines && (forceOutlineOnMobile || renderOutline) && msg.style != null) {
                    for (int x = -1; x <= 1; x++) {
                        for (int y = -1; y <= 1; y++) {
                            if (x != 0 || y != 0) {
                                Rect posRecOutline = new Rect(localPos.x + x, localPos.y + y, msg.Position.width, msg.Position.height);
                                GUI.Label(posRecOutline, msg.Text, msg.styleOutline);
                            }
                        }
                    }
                }

                if (msg.style != null) {
                    GUI.Label(localPos, msg.Text, msg.style);
                } else { // fallback to at least show *something* when someone completely f'ed up ScoreFlash ;-)
                    GUI.Label(localPos, msg.Text);
                }

                GUI.matrix = originalGUIMatrix;
            }
        }
    }
    #endregion Rendering using UnityGUI

    
    /// <summary>
    ///     Calculates a pixel offset based on msg.Alignment.ScreenAlign. 
    /// </summary>
    /// <remarks>
    ///     You'll want to use this when your GUI system only supports top left
    ///     positioning (like UnityGUI does, for example). It gives you an offset
    ///     vector that you can use in your implementation of 
    ///     <see cref="ScoreFlashRendererBase.UpdateMessage(ScoreMessage)"/> 
    ///     to calculate an offset, or in your rendering code (e.g. when using
    ///     UnityGUI). This only works, when your renderer returns correct and
    ///     reliable values for width and height, so if your GUI system supports
    ///     it, it's preferable to use its positioning system instead. See
    ///     <see cref="NGAlignment.ConvertAlignment(NGAlignment.ScreenAlign, NGAlignment.AlignmentType)"/> for a
    ///     convenient way to convert ScoreFlashs alignment enumeration
    ///     to GUIText.TextAnchors, NGUI style and EZ GUI style.
    /// </remarks>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static Vector2 GetAlignBasedOffset(ScoreMessage msg) {
        return NGAlignment.GetAlignBasedOffset(msg.InnerAnchor, msg.Position);
    }

    internal override void SetIsSceneView(bool isSceneView) {
        this.isSceneView = isSceneView;
    }

    internal override GUISkin EditorGUISkin {
        get { return editorGUISkin; }
    }

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
    ///     Returns <c>this</c>. Used for the designer mode.
    /// </summary>
    public override ScoreFlash DefaultScoreFlash {
        get { return this; }
    }

    /// <summary>
    ///     The position offset. See <see cref="position"/>.
    /// </summary>
    public override Vector2 Position {
        get { return position; }
        set { position = value; }
    }

    /// <summary>
    ///     Always returns <c>null</c>.
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
    ///     The screen alignment. See <see cref="screenAlign"/>.
    /// </summary>
    public override NGAlignment.ScreenAlign ScreenAlign {
        get { return screenAlign; }
        set { screenAlign = value; }
    }

    /// <summary>
    ///     Shall we lock screen align? See <see cref="lockScreenAlign"/>.
    /// </summary>
    public override bool LockScreenAlign {
        get { return lockScreenAlign;  }
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
    ///     The maximum width of the messages. See <see cref="maxWidth"/>.
    /// </summary>
    public override float MaxWidth {
        get { return maxWidth; }
        set { maxWidth = Mathf.Clamp(value, 0, float.MaxValue); }
    }

    /// <summary>
    ///     The minimum padding of the items. See <see cref="minPaddingX"/>.
    /// </summary>
    public override float MinPaddingX {
        get { return minPaddingX;  }
        set { minPaddingX = Mathf.Clamp(value, 0, float.MaxValue);  }
    }

    /// <summary>
    ///     Creates a ScoreMessage to be used in the designer.
    /// </summary>
    /// <param name="text">the text for the message</param>
    /// <returns>the ScoreMessage</returns>
    public override ScoreMessage CreateDesignerMessage(string text) {
        ScoreMessage msg = PushLocal(text, null, null, null, null, null);
        msg.CurrentTextColor = readColorStart;
        return msg;
    }

    #endregion Implementation of IHasVisualDesigner interface
}
