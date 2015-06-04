/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Interface providing the "push"-methods that ScoreFlash offers. 
    /// </summary>
    /// <remarks>
    ///     In general, there are three ways of "pushing" messages using ScoreFlash:
   	///     PushLocal(...), PushScreen(...) and PushWorld(...). Each of these can be
   	///     used either with ScoreFlash.Instance (if you are using a single instance
   	///     of ScoreFlash in your scene), or ScoreFlashManager.Get("name of instance")
   	///     if you are using multiple instances of ScoreFlash in a single scene. You
   	///     can also use these methods directly when you have a reference to a ScoreFlash
   	///     instance in the script where you want to push the messages.
   	///     <p />
   	///     Each of these methods can be called with different sets of parameters depending
   	///     on your needs. You can either just use the text as parameter, or pass a color,
   	///     or GUIStyles. PushScreen and PushWorld also require coordinates.
   	///     <p />
   	///     The difference between PushLocal and the other two (Screen/World) is that PushLocal uses the 
   	///     alignment set in the ScoreFlash instance under "Main Layout"; so it either
   	///     shows the message on top, or in the middle, or bottom of the screen. 
   	///     <p />
   	///     PushScreen uses screen coordinates, so you could use this, for instance, to
   	///     easily push messages to the current mouse pointer position or the location
   	///     where a user taps the screen of a mobile device.
   	///     <p />
   	///     PushWorld uses "world coordinates". In other words, coordinates in the 3D space.
   	///     If you are using PushWorld with Vector3, Camera.main must return the correct camera
   	///     (if you're only using one camera this is never a problem, otherwise, the correct tag 
   	///     must be set, and it must be set only on your main camera). This limitation does not 
   	///     apply if you are using ScoreFlashFollow3D (which you have to attach to the game object 
   	///     that the message should be pushed at); there you can assign the camera to be used 
   	///     in ScoreFlashFollow3D.
   	///     <p />
    ///     Having this interface instead of directly using ScoreFlash
    ///     mostly helps with Intellisense because instead of giving you all the
    ///     MonoBehaviour stuff that you won't really be interested in when
    ///     using ScoreFlash, it only gives you the relevant methods. When
    ///     accessing ScoreFlash via <see cref="ScoreFlashManager.Get(string)"/> or via
    ///     <see cref="ScoreFlash.Instance"/> this simplifies working with
    ///     ScoreFlash quite a bit. When using your own references to ScoreFlash,
    ///     you could keep a private variable of type IScoreFlash for this convenience.
    ///     Unfortunately, you cannot have public inspector variables of type
    ///     IScoreFlash, so it's up to you to typecast your reference from the
    ///     bloated ScoreFlash MonoBehaviour to a clean IScoreFlash object.
    /// </remarks>
    public interface IScoreFlash {

        /// <summary>
        ///     Immediately cleans up all messages currently on screen.
        /// </summary>
        void Cleanup();

        #region PushLocal - Shortcuts
        /// <summary>
        ///     Pushes a message to the screen, using the layout settings specified on the
        ///     instance of ScoreFlash you are using this on.
        /// </summary>
        /// <remarks>
        ///     For pushing each message with a different color, see:
        ///     <see cref="PushLocal(object, Color)"/>!
        ///     If you only have a single
        ///     instance of ScoreFlash in your scene, you should call this via
        ///     <c>ScoreFlash.Instance.PushLocal(...)</c>; if you have multiple instances
        ///     of ScoreFlash in your scene, call it using <see cref="ScoreFlashManager.Get(string)"/>:
        ///     <c>ScoreFlashManager.Get("<em>name of ScoreFlash instance</em>").PushLocal(...)</c>.
        ///     You can use the button <em>Copy Ref</em> in the ScoreFlashManager inspector GUI to
        ///     copy the code for the reference (<c>ScoreFlashManager.Get("<em>name of ScoreFlash instance</em>")</c>).
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <returns>the ScoreMessage representing this message</returns>
        ScoreMessage PushLocal(object message);

        /// <summary>
        ///     Pushes a message to the screen, using the layout settings specified on the
        ///     instance of ScoreFlash you are using this on but with a specific color.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <param name="color">
        ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
        ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
        ///     in the ScoreFlash instance that this message was pushed to
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushLocal(object message, Color color);

        /// <summary>
        ///     Pushes a message to the screen, using the layout settings specified on the
        ///     instance of ScoreFlash you are using this on but using a specific GUIStyle.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
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
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushLocal(object message, GUIStyle style);

        /// <summary>
        ///     Pushes a message to the screen, using the layout settings specified on the
        ///     instance of ScoreFlash you are using this on but using specific GUIStyles
        ///     for standard and high densities.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
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
        /// <param name="styleHighDensity">
        ///     a custom style, or <c>null</c> to use the custom style <c>ScoreFlash</c> of
        ///     <see cref="ScoreFlash.skinHighDensity"/> (this parameter is optional!)
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushLocal(object message, GUIStyle style, GUIStyle styleHighDensity);
        #endregion PushLocal - Shortcuts

        /// <summary>
        ///     Pushes a message to the screen, using the layout settings specified on the
        ///     instance of ScoreFlash you are using this on. 
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
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
        /// <param name="styleHighDensity">
        ///     a custom style, or <c>null</c> to use the custom style <c>ScoreFlash</c> of
        ///     <see cref="ScoreFlash.skinHighDensity"/> (this parameter is optional!)
        /// </param>
        /// <param name="messageQueueID">
        ///     An integer that defines which message queue is used for this message. Any
        ///     two messages that have different IDs will be treated completely
        ///     independently; in particular ScoreFlash will not try to make them
        ///     be readable by pushing older messages "up". When the IDs are the same,
        ///     ScoreFlash checks the screen distance of the reference position and
        ///     if that distance is larger than 15 pixels, the messages will also not
        ///     push previous messages up. If you want to use ScoreFlash for objects,
        ///     it's recommended that you use <see cref="ScoreFlashFollow3D"/> which
        ///     automatically uses this. If you follow your own approach, the easiest
        ///     way to make this work is by simply using the game object's instance ID
        ///     (<a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.GetInstanceID.html">Object.GetInstanceID()</a>a>).
        ///     This parameter is optional!
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushLocal(object message, GUIStyle style, GUIStyle styleHighDensity, int messageQueueID);


        /// <summary>
        ///     Pushes a message to the screen, using the layout settings specified on the
        ///     instance of ScoreFlash you are using this on but a specific color.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <param name="color">
        ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
        ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
        ///     in the ScoreFlash instance that this message was pushed to
        /// </param>
        /// <param name="messageQueueID">
        ///     An integer that defines which message queue is used for this message. Any
        ///     two messages that have different IDs will be treated completely
        ///     independently; in particular ScoreFlash will not try to make them
        ///     be readable by pushing older messages "up". When the IDs are the same,
        ///     ScoreFlash checks the screen distance of the reference position and
        ///     if that distance is larger than 15 pixels, the messages will also not
        ///     push previous messages up. If you want to use ScoreFlash for objects,
        ///     it's recommended that you use <see cref="ScoreFlashFollow3D"/> which
        ///     automatically uses this. If you follow your own approach, the easiest
        ///     way to make this work is by simply using the game object's instance ID
        ///     (<a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.GetInstanceID.html">Object.GetInstanceID()</a>a>).
        ///     This parameter is optional!
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushLocal(object message, Color color, int messageQueueID);

        #region PushScreen - Shortcuts

        /// <summary>
        ///     Pushes a message to the screen, at <c>screenPosition</c>. 
        ///     Aside of using <c>screenPosition</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object)"/>.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="screenPosition">where should the message have its reference point on the screen?</param>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushScreen(Vector2 screenPosition, object message);

        /// <summary>
        ///     Pushes a message to the screen, at <c>screenPosition</c> using a specific color. 
        ///     Aside of using <c>screenPosition</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, Color)"/>.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="screenPosition">where should the message have its reference point on the screen?</param>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <param name="color">
        ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
        ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
        ///     in the ScoreFlash instance that this message was pushed to
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushScreen(Vector2 screenPosition, object message, Color color);


        /// <summary>
        ///     Pushes a message to the screen, at <c>screenPosition</c>. 
        ///     Aside of using <c>screenPosition</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, GUIStyle)"/>.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="screenPosition">where should the message have its reference point on the screen?</param>
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
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushScreen(Vector2 screenPosition, object message, GUIStyle style);

        /// <summary>
        ///     Pushes a message to the screen, at <c>screenPosition</c>. 
        ///     Aside of using <c>screenPosition</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, GUIStyle, GUIStyle)"/>.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="screenPosition">where should the message have its reference point on the screen?</param>
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
        /// <param name="styleHighDensity">
        ///     a custom style, or <c>null</c> to use the custom style <c>ScoreFlash</c> of
        ///     <see cref="ScoreFlash.skinHighDensity"/> (this parameter is optional!)
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushScreen(Vector2 screenPosition, object message, GUIStyle style, GUIStyle styleHighDensity);

        #endregion PushScreen - Shortcuts

        /// <summary>
        ///     Pushes a message to the screen, at <c>screenPosition</c>. 
        ///     Aside of using <c>screenPosition</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, GUIStyle, GUIStyle, int)"/>.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="screenPosition">where should the message have its reference point on the screen?</param>
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
        /// <param name="styleHighDensity">
        ///     a custom style, or <c>null</c> to use the custom style <c>ScoreFlash</c> of
        ///     <see cref="ScoreFlash.skinHighDensity"/> (this parameter is optional!)
        /// </param>
        /// <param name="messageQueueID">
        ///     An integer that defines which message queue is used for this message. Any
        ///     two messages that have different IDs will be treated completely
        ///     independently; in particular ScoreFlash will not try to make them
        ///     be readable by pushing older messages "up". When the IDs are the same,
        ///     ScoreFlash checks the screen distance of the reference position and
        ///     if that distance is larger than 15 pixels, the messages will also not
        ///     push previous messages up. If you want to use ScoreFlash for objects,
        ///     it's recommended that you use <see cref="ScoreFlashFollow3D"/> which
        ///     automatically uses this. If you follow your own approach, the easiest
        ///     way to make this work is by simply using the game object's instance ID
        ///     (<a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.GetInstanceID.html">Object.GetInstanceID()</a>a>).
        ///     This parameter is optional!
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushScreen(Vector2 screenPosition, object message, GUIStyle style, GUIStyle styleHighDensity, int messageQueueID);

        /// <summary>
        ///     Pushes a message to the screen, at <c>screenPosition</c> using a specific color. 
        ///     Aside of using <c>screenPosition</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, Color, int)"/>.
        /// </summary>
        /// <remarks>
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="screenPosition">where should the message have its reference point on the screen?</param>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <param name="color">
        ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
        ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
        ///     in the ScoreFlash instance that this message was pushed to
        /// </param>
        /// <param name="messageQueueID">
        ///     An integer that defines which message queue is used for this message. Any
        ///     two messages that have different IDs will be treated completely
        ///     independently; in particular ScoreFlash will not try to make them
        ///     be readable by pushing older messages "up". When the IDs are the same,
        ///     ScoreFlash checks the screen distance of the reference position and
        ///     if that distance is larger than 15 pixels, the messages will also not
        ///     push previous messages up. If you want to use ScoreFlash for objects,
        ///     it's recommended that you use <see cref="ScoreFlashFollow3D"/> which
        ///     automatically uses this. If you follow your own approach, the easiest
        ///     way to make this work is by simply using the game object's instance ID
        ///     (<a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.GetInstanceID.html">Object.GetInstanceID()</a>a>).
        ///     This parameter is optional!
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushScreen(Vector2 screenPosition, object message, Color color, int messageQueueID);

        #region PushWorld(Vector3) - Shortcuts

        /// <summary>
        ///     Pushes a message to the screen, at <c>worldPosition</c>, with
        ///     a screen offset of <c>screenOffset</c>. 
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>worldPosition</c> and <c>screenOffset</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object)"/>.
        ///     <p />
        ///     <strong>IMPORTANT: This only works when Camera.main returns the correct
        ///     camera! If you need to wire up specific cameras, use
        ///     <see cref="PushWorld(ScoreFlashFollow3D, object)"/>
        ///     instead; you can use the property
        ///     <see cref="ScoreFlashFollow3D.keepStatic"/> to disable following the
        ///     messages.
        ///     </strong>
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="worldPosition">where should the message have its reference point on the screen?</param>
        /// <param name="screenOffset">offset in screen coordinates (e.g. to place the message above an object)</param>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message);

        /// <summary>
        ///     Pushes a message to the screen, at <c>worldPosition</c>, with
        ///     a screen offset of <c>screenOffset</c>, using a specific color. 
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>worldPosition</c> and <c>screenOffset</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, Color)"/>.
        ///     <p />
        ///     <strong>IMPORTANT: This only works when Camera.main returns the correct
        ///     camera! If you need to wire up specific cameras, use
        ///     <see cref="PushWorld(ScoreFlashFollow3D, object, Color)"/>
        ///     instead; you can use the property
        ///     <see cref="ScoreFlashFollow3D.keepStatic"/> to disable following the
        ///     messages.
        ///     </strong>
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="worldPosition">where should the message have its reference point on the screen?</param>
        /// <param name="screenOffset">offset in screen coordinates (e.g. to place the message above an object)</param>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <param name="color">
        ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
        ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
        ///     in the ScoreFlash instance that this message was pushed to
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, Color color);

        /// <summary>
        ///     Pushes a message to the screen, at <c>worldPosition</c>, with
        ///     a screen offset of <c>screenOffset</c>. 
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>worldPosition</c> and <c>screenOffset</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, GUIStyle)"/>.
        ///     <p />
        ///     <strong>IMPORTANT: This only works when Camera.main returns the correct
        ///     camera! If you need to wire up specific cameras, use
        ///     <see cref="PushWorld(ScoreFlashFollow3D, object, GUIStyle)"/>
        ///     instead; you can use the property
        ///     <see cref="ScoreFlashFollow3D.keepStatic"/> to disable following the
        ///     messages.
        ///     </strong>
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="worldPosition">where should the message have its reference point on the screen?</param>
        /// <param name="screenOffset">offset in screen coordinates (e.g. to place the message above an object)</param>
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
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, GUIStyle style);

        /// <summary>
        ///     Pushes a message to the screen, at <c>worldPosition</c>, with
        ///     a screen offset of <c>screenOffset</c>. 
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>worldPosition</c> and <c>screenOffset</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, GUIStyle, GUIStyle, int)"/>.
        ///     <p />
        ///     <strong>IMPORTANT: This only works when Camera.main returns the correct
        ///     camera! If you need to wire up specific cameras, use
        ///     <see cref="PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>
        ///     instead; you can use the property
        ///     <see cref="ScoreFlashFollow3D.keepStatic"/> to disable following the
        ///     messages.
        ///     </strong>
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="worldPosition">where should the message have its reference point on the screen?</param>
        /// <param name="screenOffset">offset in screen coordinates (e.g. to place the message above an object)</param>
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
        /// <param name="styleHighDensity">
        ///     a custom style, or <c>null</c> to use the custom style <c>ScoreFlash</c> of
        ///     <see cref="ScoreFlash.skinHighDensity"/> (this parameter is optional!)
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, GUIStyle style, GUIStyle styleHighDensity);

        #endregion PushWorld(Vector3) - Shortcuts

        /// <summary>
        ///     Pushes a message to the screen, at <c>worldPosition</c>, with
        ///     a screen offset of <c>screenOffset</c>. 
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>worldPosition</c> and <c>screenOffset</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, GUIStyle, GUIStyle, int)"/>.
        ///     <p />
        ///     <strong>IMPORTANT: This only works when Camera.main returns the correct
        ///     camera! If you need to wire up specific cameras, use
        ///     <see cref="PushWorld(ScoreFlashFollow3D, object, GUIStyle, GUIStyle)"/>
        ///     instead; you can use the property
        ///     <see cref="ScoreFlashFollow3D.keepStatic"/> to disable following the
        ///     messages.
        ///     </strong>
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="worldPosition">where should the message have its reference point on the screen?</param>
        /// <param name="screenOffset">offset in screen coordinates (e.g. to place the message above an object)</param>
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
        /// <param name="styleHighDensity">
        ///     a custom style, or <c>null</c> to use the custom style <c>ScoreFlash</c> of
        ///     <see cref="ScoreFlash.skinHighDensity"/> (this parameter is optional!)
        /// </param>
        /// <param name="messageQueueID">
        ///     An integer that defines which message queue is used for this message. Any
        ///     two messages that have different IDs will be treated completely
        ///     independently; in particular ScoreFlash will not try to make them
        ///     be readable by pushing older messages "up". When the IDs are the same,
        ///     ScoreFlash checks the screen distance of the reference position and
        ///     if that distance is larger than 15 pixels, the messages will also not
        ///     push previous messages up. If you want to use ScoreFlash for objects,
        ///     it's recommended that you use <see cref="ScoreFlashFollow3D"/> which
        ///     automatically uses this. If you follow your own approach, the easiest
        ///     way to make this work is by simply using the game object's instance ID
        ///     (<a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.GetInstanceID.html">Object.GetInstanceID()</a>a>).
        ///     This parameter is optional!
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, GUIStyle style, GUIStyle styleHighDensity, int messageQueueID);

        /// <summary>
        ///     Pushes a message to the screen, at <c>worldPosition</c>, with
        ///     a screen offset of <c>screenOffset</c>, using a specific color. 
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>worldPosition</c> and <c>screenOffset</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, Color, int)"/>.
        ///     <p />
        ///     <strong>IMPORTANT: This only works when Camera.main returns the correct
        ///     camera! If you need to wire up specific cameras, use
        ///     <see cref="PushWorld(ScoreFlashFollow3D, object, Color)"/>
        ///     instead; you can use the property
        ///     <see cref="ScoreFlashFollow3D.keepStatic"/> to disable following the
        ///     messages.
        ///     </strong>
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="worldPosition">where should the message have its reference point on the screen?</param>
        /// <param name="screenOffset">offset in screen coordinates (e.g. to place the message above an object)</param>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <param name="color">
        ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
        ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
        ///     in the ScoreFlash instance that this message was pushed to
        /// </param>
        /// <param name="messageQueueID">
        ///     An integer that defines which message queue is used for this message. Any
        ///     two messages that have different IDs will be treated completely
        ///     independently; in particular ScoreFlash will not try to make them
        ///     be readable by pushing older messages "up". When the IDs are the same,
        ///     ScoreFlash checks the screen distance of the reference position and
        ///     if that distance is larger than 15 pixels, the messages will also not
        ///     push previous messages up. If you want to use ScoreFlash for objects,
        ///     it's recommended that you use <see cref="ScoreFlashFollow3D"/> which
        ///     automatically uses this. If you follow your own approach, the easiest
        ///     way to make this work is by simply using the game object's instance ID
        ///     (<a href="http://docs.unity3d.com/Documentation/ScriptReference/Object.GetInstanceID.html">Object.GetInstanceID()</a>a>).
        ///     This parameter is optional!
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(Vector3 worldPosition, Vector2 screenOffset, object message, Color color, int messageQueueID);

        #region PushWorld(ScoreFlashFollow3D) - Shortcuts

        /// <summary>
        ///     Pushes a message to the screen that follows <c>follow3D</c>; see also
        ///     <see cref="ScoreFlashFollow3D"/>.
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>follow3D</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object)"/>.
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="follow3D">ScoreFlashFollow3D attached to the object the message should follow</param>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message);

        /// <summary>
        ///     Pushes a message to the screen that follows <c>follow3D</c> and has a specific color;
        ///     see also <see cref="ScoreFlashFollow3D"/>.
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>follow3D</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, Color)"/>.
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="follow3D">ScoreFlashFollow3D attached to the object the message should follow</param>
        /// <param name="message">
        ///     The message to be animated by ScoreFlash. This can be a string, a 
        ///     fully configured custom renderer, an int, long, float, double or 
        ///     any object with a useful ToString() method
        /// </param>
        /// <param name="color">
        ///     Use this to control the color the message is sent with. Uses RGB from <c>color</c>,
        ///     and multiplies <c>color.a</c> with the current alpha value depending on the settings
        ///     in the ScoreFlash instance that this message was pushed to
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message, Color color);

        /// <summary>
        ///     Pushes a message to the screen that follows <c>follow3D</c>; see also
        ///     <see cref="ScoreFlashFollow3D"/>.
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>follow3D</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, GUIStyle)"/>.
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="follow3D">ScoreFlashFollow3D attached to the object the message should follow</param>
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
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message, GUIStyle style);

        #endregion PushWorld(ScoreFlashFollow3D) - Shortcuts

        /// <summary>
        ///     Pushes a message to the screen that follows <c>follow3D</c>; see also
        ///     <see cref="ScoreFlashFollow3D"/>.
        /// </summary>
        /// <remarks>
        ///     Aside of using <c>follow3D</c> instead of the layout 
        ///     settings of the ScoreFlash instance this is called on, this does the
        ///     same as <see cref="PushLocal(object, GUIStyle, GUIStyle)"/>.
        ///     <p />
        ///     See <see cref="ScoreFlash"/> for several usage examples and links to tutorial videos.
        /// </remarks>
        /// <param name="follow3D">ScoreFlashFollow3D attached to the object the message should follow</param>
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
        /// <param name="styleHighDensity">
        ///     a custom style, or <c>null</c> to use the custom style <c>ScoreFlash</c> of
        ///     <see cref="ScoreFlash.skinHighDensity"/> (this parameter is optional!)
        /// </param>
        /// <returns>the ScoreMessage representing this text</returns>
        ScoreMessage PushWorld(ScoreFlashFollow3D follow3D, object message, GUIStyle style, GUIStyle styleHighDensity);

    }
}