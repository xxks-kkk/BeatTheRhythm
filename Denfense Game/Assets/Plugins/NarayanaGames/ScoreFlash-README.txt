ScoreFlash V4.6.1
This file was last updated: 2014-12-27

Thank you for buying ScoreFlash - I really appreciate it and hope you will 
enjoy using ScoreFlash just as much as I enjoy creating and maintaining it!!!

NOTE: If you need to use ScoreFlash for a project that's on a Unity version 
prior to Unity 4.3.4, please use the package 
ScoreFlash_3_2_3_ForUnity3.5.7-4.3.3.unitypackage
which you find in: (Assets /) Plugins / NarayanaGames


ScoreFlash is for scores what a news flash is for news: Let your players know 
what they've achieved - be it scores or collected power ups or achievements. 
Whatever you want to put "in their face": ScoreFlash lets you do it in a fancy 
way. And the best thing about it: It's so easy! All you need is grab the prefab 
from Plugins/NarayanaGames/ScoreFlash, put it into your scene and where ever 
you want the player to know they've done something awesome, just write:

ScoreFlash.Instance.PushLocal("You just did something awesome!");

Yes. It's that easy!

That said: You can still tweak all kinds of settings to make this look exactly 
the way you want it to. ScoreFlash comes with a custom inspector that not only 
lets you tweak everything with the detail level you feel comfortable with (just 
fold out what you can handle and close what feels "too much") - but it also 
lets you store the changes you made during play mode automatically, if you wish.

For a written tutorial, check out ScoreFlashBasics. You find this at:
/Assets/Plugins/NarayanaGames/ScoreFlashBasics.pdf

If you haven't done so, yet - please check out the videos on the 

Score Flash Product Page
http://scoreflash.narayana-games.net

To get a quick overview, check out the score flash class documentation
(which includes several code examples):
http://narayana-games.net/static/docs/assetstore/ScoreFlash/html/T_ScoreFlash.htm

For all the details, check the full ScoreFlash API Documentation:
http://narayana-games.net/static/docs/assetstore/ScoreFlash/index.html

If you just want to see the documentation of the fields (aka exposed properties), 
go directly to ScoreFlash Fields:
http://narayana-games.net/static/docs/assetstore/ScoreFlash/html/Fields_T_ScoreFlash.htm

Much easier than reading all that stuff is watching the Introduction Video 
which includes a quick tutorial to get you started within just a few minutes:
http://www.youtube.com/watch?v=JFmnFguX8mU

There are quite a few more tutorial videos that you can find in the
Official ScoreFlash Unity Forum Thread:
http://bit.ly/ScoreFlashUnityForum


I highly recommend checking out all of the tutorials and examples to get the 
most out of ScoreFlash. It's very easy to get started with ScoreFlash - but 
if you want to tap into its full power, it's good to take a little time to learn!

Score Flash Product Page
http://scoreflash.narayana-games.net


Supported 3rd party Packages:
You can use ScoreFlash without any 3rd party packages. However, if you have and
of these 3rd party packages ...
- Daikon Forge GUI
- TextMesh Pro
- NoesisGUI
- NGUI
- EZ GUI
- Sprite Manager 2
- Text Box 
- PlayMaker
... there's support for these. As the support files only compile when the packages 
are there, please be sure to follow these guidelines:

1. Make sure the 3rd party package you want to use is imported into your project.
2. Get the specific ScoreFlash package related to the 3rd party package from the 
   Asset Store. You can easily navigate to them from the Unity menu:
   Help / narayana games / Score Flash Addons
3. ENJOY!!!

It is very important that you update these packages after each update of 
ScoreFlash (if you use them). Failure to do so may result in compilation errors!

UPDATING FROM AN EARLIER VERSION TO V4.0.0:
IMPORTANT: The integration packages are no longer part of ScoreFlash. Grab
them for free on the Asset Store while you can (eventually, I'll reduce the
price of ScoreFlash and increase the price of the integrations).
Code change: Everything except ScoreFlash, ScoreFlashFollow3D, ScoreFlashLayout 
and ScoreFlashManager is now in its appropriate namespace. Most likely this
won't make any difference but in case you're using some of my more hidden
classes from your code, you might have to add
using NarayanaGames.ScoreFlashComponent;
to the top of your (C#) scripts.

UPDATING FROM AN EARLIER VERSION TO V3.1.0:
- Make sure to have a backup of your project (unless it's under version control)
- Read Plugins/NarayanaGames/ScoreFlash-UpdateTo31.pdf - for most people, the 
  upgrade will be very simple but if you have your own custom renderers, you 
  need to understand the changes you'll have to make

UPDATING FROM AN EARLIER VERSION TO V2.2.2:
Util.cs and Easing.cs have been replaced with NGUtil.cs and NGEasing.cs. 
To properly upgrade to V2.2.2, I recommend first fixing all warnings (you 
might get warnings that Util and Easing are obsolete if you have been using 
any of the provided methods in your own code - if there are no warnings, all 
the better, just go ahead with the next step). Once you do not get any more 
warning from ScoreFlash, first make a back up of your project. Then delete 
the two files Util.cs and Easing.cs that you find either in 
Plugins/NarayanaGames/Common or in Plugins/NarayanaGames/Common/Deprecated 
(this is where they actually would belong). You can also delete the folder 
Plugins/NarayanaGames/Common/Deprecated. DONE. 
In case you get any error messages now, you'll have to replace any occurence 
of Util with NGUtil and Easing with NGEasing (unless you have other packages 
that also provide these - in that case, you have to be very careful to not 
break anything).



Folders and files that come with this package:

Plugins/NarayanaGames
- ScoreFlash-README.txt - this file, it seems you've already found it ;-)
- ScoreFlashBasics.pdf - a little written introduction to ScoreFlash
- ScoreFlash-UpdateTo31.pdf - important information for people upgrading from any version before 3.1 to 3.1 or later
- ScoreFlash_3_2_3_ForUnity3.5.7-4.3.3.unitypackage - Version 3.2.3 of ScoreFlash to provide backwards compatibility with
  Unity 3.5.7, 4.0, 4.1, 4.2 ... if you need to keep your project on a Unity version prior to 4.3.4, this is what you
  want to import instead of the main ScoreFlash package. This does fix several issues with ScoreFlash 3.2.2 which was
  the last version published via the Asset Store before ScoreFlash 4

Plugins/NarayanaGames/ScoreFlash has the following files:
- ScoreFlash.prefab - a simple drag and drop prefab (easiest way to get started)
- ScoreFlash.cs - the main implementation of ScoreFlash (a script to attach to a game object)
- ScoreFlashManager.cs - use this if you want to have multiple instances of ScoreFlash in a scene (a script to attach to a game object)
- ScoreFlashLayout.cs - use this if you want to use different layout settings (screen alignment, offset etc.) with one instance of ScoreFlash
- ScoreFlashFollow3D.cs - use this to conveniently have messages follow in-game objects (a script to attach to a game object)

Plugins/NarayanaGames/Component has the following files:
- IScoreFlash.cs - interface that ScoreFlash implements which offers the various Push-methods
- IHasVisualDesigner.cs - interface for all MonoBehaviours that have a visual designer
- IScoreFlashLayout.cs - interface for all classes defining a score flash layout
- ScoreFlashBase.cs - base class for Score Flash
- ScoreFlashVisualDesigner.cs - implements most of the logic for visual designers
- ScoreMessage.cs - this class represents a single message used in the system
- ScoreFlashFollow3DLocation.cs - helper used by ScoreFlashFollow3D.cs

- DELETED as of 4.0.0: PlayMaker-Actions-Package 
  This is now a separate package on
  the Unity Asset Store. You find it in the Unity menu:
  Help / narayana games / Score Flash Addons


Plugins/NarayanaGames/CustomRendering has these files:
- ScoreFlashRendererBase.cs - base class used to implement custom renderers; this cannot be attached to a game object!!!
- ScoreFlashRendererGUIText.cs - an example implementation using GUIText for rendering
- ScoreFlashRendererGUIText prefab - an example prefab using ScoreFlashRendererGUIText.cs
- ScoreFlashRendererTextMesh.cs - an example implementation using TextMesh for rendering
- ScoreFlashRendererTextMesh prefab - an example prefab using ScoreFlashRendererTextMesh.cs
- ScoreFlashRendererTextMesh3D.cs - an example implementation using TextMesh for rendering in 3d space (for VR, e.g. Oculus Rift)
- ScoreFlashRendererTextMesh3D prefab - an example prefab using ScoreFlashRendererTextMesh3D.cs
- ScoreFlashRendererUnityGUI.cs - an example implementation using UnityGUI for rendering
- ScoreFlashRendererUnityGUI prefab - an example prefab using ScoreFlashRendererUnityGUI.cs
- IHasOnGUI.cs - interface that custom renderers using UnityGUI can implement so they work properly with the visual designer

DELETED as of 4.0.0:
- ScoreFlashRendererNGUI-Package - if you have NGUI in your project, you can double-click and import this to get a custom renderer that uses NGUI :-)
- ScoreFlashRendererEZGUI-Package - if you have EZ GUI in your project, you can double-click and import  this to get a custom renderer that uses EZ GUI :-)
- ScoreFlashRendererSM2-Package - if you have Sprite Manager in your project, you can double-click and import  this to get a custom renderer that uses Sprite Manager 2 :-)
- ScoreFlashRendererTextBox-Package - if you have Text Box in your project, you can double-click and import  this to get a custom renderer that uses Text Box :-)
These are all now separate packages on the Unity Asset Store. You find them in the Unity menu:
Help / narayana games / Score Flash Addons

Plugins/NarayanaGames/BaseConfiguration has 4 files:
- ScoreFlashSkin - default skin used by ScoreFlash.prefab
- ScoreFlashSkin_Retina - default skin used by ScoreFlash.prefab in Retina mode
- Vanilla50 - a font used by ScoreFlashSkin (50pt)
- Vanilla100 - a font used by ScoreFlashSkin_Retina (100pt)

Plugins/NarayanaGames/Common has 3 files:
- NGEasing.cs - provides a couple of convenient easing methods
- NGQueue.cs - a simple Queue implementation (that prevents importing System.dll)
- NGUtil.cs - provides a couple of generic utility methods

Plugins/NarayanaGames/Common/Deprecated had 2 files in earlier versions (these are now deleted):
- DELETED: Easing.cs - obsolete, kept for backwards compatibility - you can delete this
- DELETED: Util.cs - obsolete, kept for backwards compatibility - you can delete this


Editor/NarayanaGames/ScoreFlash has these files:
- ScoreFlashEditor.cs - provides the custom inspector GUI for ScoreFlash
- ScoreFlashManagerEditor.cs - provides the custom inspector GUI for ScoreFlashManager and adds menus:
  + GameObject/Create Other/Score Flash Manager - creates a new ScoreFlashManager in a scene that doesn't have one
  + GameObject/Create Other/Score Flash - creates an instance of ScoreFlash
- ExtentionInfoScoreFlash.cs - adds help menus (in the Unity help menu) to documentation and contact information:
  + Help/narayana games/Score Flash Documentation - link to full documentation
  + Help/narayana games/Score Flash on Unity Forums - link to Unity forums, thread "Score Flash"
  + Help/narayana games/About Score Flash - version information on this plugin
  + Help/narayana games/Report a Problem - how to contact us to report issues

Editor/NarayanaGames/Common has these files:
- EditorGUIExtensions.cs - a few useful methods for EditorGUIs
- ExtensionInfo.cs - a generic class for plugins
- PlayModeChangesHelper.cs - provides a very easy way to add persisting playmode changes to a custom inspector


There's also a few examples coming with the package; you can simply delete 
those after importing ScoreFlash to a project (once you are familiar with how 
to use it).

Xamples-ScoreFlash which has a few code examples:
- ExampleBoo.boo - a simple example of how to use ScoreFlash from Boo
- ExampleBooMultipleInstances.cs - a more complex examples how to use multiple instances of ScoreFlash from Boo
- ExampleCSharp.cs - a simple example of how to use ScoreFlash from C#
- ExampleCSharpMultipleInstances.cs - a more complex examples how to use multiple instances of ScoreFlash from C#
- ExampleJavaScript.js - a simple example of how to use ScoreFlash from JavaScript
- ExampleJavaScriptMultipleInstances.cs - a more complex examples how to use multiple instances of ScoreFlash from JavaScript

And several examples scenes:
- Xample_Default - a minimum scene to become familiar with ScoreFlash, with default settings
- Xample_Random_Top - an example illustrating how the "random colors" feature and top alignment can be used
- Xample_Sequence_Bottom - an example illustrating how the "color sequence" feature and bottom alignment can be used

More examples can be found in Xamples-ScoreFlash/Advanced:
- AchievementsExample - illustrates how you can use custom renderers to create your own achievements
- MultipleInstancesOfScoreFlash - two scenes and a script illustrating how multiple instances of ScoreFlash can be used
- ScoreFlashAsContextUI - shows you how you can use ScoreFlash to create a context menu for a game object
- ScreenSpaceMessages - how to make messages appear on mouse clicks (at the mouse pointer location)
- WorldSpaceMessages - how to make messages appear using world space coordinates and ScoreFlashFollow3D
- Xample_XCustomRenderers - a scene illustrating how custom renderers can be used
- Xample_XMouseClickMessages - a scene using MouseClickMessagesCSharp.cs
- Xample_XWorldSpaceMessages - a scene illustrating how world space coordinates can be used (i.e. having messages appear above game objects)

Xamples-ScoreFlash/Advanced/Xample_XDemoScoreFlash
- XDemoScoreFlash - the Web player demo you can see at http://narayana-games.net/Products/ScoreFlashDemo.aspx
- ScoreFlashDemoGUI.cs - the code for the demo ... this a mess, don't look at it! DO NOT LOOK AT THIS! ;-)

- XDocsWebPlayer/XAwesomeScoreFlash - a little mini Web player I used for the original product page

... and a couple of example fonts and GUISkins for you to play with and get you started.

Enjoy!!!

Sunny regards,
Jashan Chittesh, 2014-12-27
narayana games - enlivening your true nature
http://narayana-games.net
