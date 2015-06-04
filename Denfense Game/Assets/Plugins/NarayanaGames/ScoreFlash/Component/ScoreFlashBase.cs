/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using NarayanaGames.Common.UI;
using System.Collections.Generic;
using NarayanaGames.Common;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Base class for MonoBehaviours implementing the main ScoreFlash functionalities.
    /// </summary>
    public abstract class ScoreFlashBase : MonoBehaviour, IHasTooltips, IHasVisualDesigner {


        #region Update Mechanism

        /// <summary>
        ///     The current version of ScoreFlash: 4.6.1
        /// </summary>
        public const string VERSION = "4.6.1";
        /// <summary>
        ///     The build date of this version: 2014-12-27
        /// </summary>
        public const string BUILT = "2014-12-27";

        /// <summary>
        ///     Keeps the version of this instance of ScoreFlash*. 
        /// </summary>
        /// <remarks>
        ///     This updates
        ///     persistently when this instance is opened in the inspector in the
        ///     editor, however it also makes it possible to make changes on the
        ///     fly when loading instances of score flash components that were
        ///     created in a much earlier version but never touched.
        /// </remarks>
        //[HideInInspector()]
        public string version = VERSION;

        /// <summary>
        ///     Performs a couple of checks to ensure that everything is set 
        ///     up correctly after an upgrade to a new version.
        /// </summary>
        /// <returns><c>true</c>, if this instance was updated and changes need to be applied</returns>
        public virtual bool UpgradeCheck() {
            bool needsUpgrade = false;
#if UNITY_EDITOR
            // when these are not assigned - auto-assign them
            if (tooltips == null) {
                Debug.Log("tooltips was not assigned - fixing it for you ;-)");
                tooltips = NGUtil.LoadAsset<TextAsset>("Assets/Editor/NarayanaGames/ScoreFlash/Tooltips.xml");
                needsUpgrade |= true;
            }
#endif
            if (!ScoreFlashBase.VERSION.Equals(this.version)) {
                this.version = ScoreFlashBase.VERSION;
                needsUpgrade |= true;
            }

            return needsUpgrade;
        }

        #endregion Update Mechanism

        /// <summary>
        ///     Text asset with tooltips for the custom inspector.
        /// </summary>
        //[HideInInspector()] // it's not so bad if this is visible when in "Default Inspector"
        public TextAsset tooltips;

#if UNITY_EDITOR
        /// <summary>
        ///     Text asset with tooltips for the custom inspector.
        /// </summary>
        public TextAsset Tooltips { get { return tooltips; } }
#endif //UNITY_EDITOR



        void Awake() {
            UpgradeCheck(); // if necessary during runtime: do the upgrade "on-the-fly"
        }

        #region Advanced Editor Scripting

        /// <summary>
        ///     Used for the designer
        /// </summary>
        /// <summary>
        ///     Draws UnityGUI based messages ... and the scene view visual designer.
        /// </summary>
        void OnGUI() {
            useGUILayout = false;
            SetIsSceneView(false);

            #region Editor stuff
#if UNITY_EDITOR
            if (Application.isEditor && IsSelected) {
                GUISkin skin = GUI.skin;
                GUI.skin = EditorGUISkin;
                if (IsDesignMode) {
                    // immediately destroy the message after it was used for rendering in the designer
                    Designer.DrawEditorGUIDesign(false).DestroyInstance();
                }
                GUI.skin = skin;
            }
#endif
            #endregion Editor stuff
        }

#if UNITY_EDITOR
        // this should never be accessed directly ... hence the name ;-)
        private ScoreFlashVisualDesigner designerDoNotEverAccess = null;
        // that's the way to do it
        private ScoreFlashVisualDesigner Designer {
            get {
                if (designerDoNotEverAccess == null) {
                    designerDoNotEverAccess = new ScoreFlashVisualDesigner(this, IsDesignMode, DesignText);
                }
                // make sure that data is updated
                designerDoNotEverAccess.isDesignMode = IsDesignMode;
                designerDoNotEverAccess.designText = DesignText;
                return designerDoNotEverAccess;
            }
        }

        /// <summary>
        ///     Is the instance selected in the hierarchy? Used internally for editor magic.
        /// </summary>
        public bool IsSelected {
            get { return Designer.IsSelected; }
            set { Designer.IsSelected = value; }
        }

        /// <summary>
        ///     Are multiple instances of that object currently selected?
        /// </summary>
        public bool IsMultiSelect { get; set; }

        /// <summary>
        ///     Used internally.
        /// </summary>
        /// <param name="sceneView">irrelevant</param>
        public void OnSceneViewGUI(SceneView sceneView) {
            SetIsSceneView(true);
            Designer.OnSceneViewGUI(sceneView);
        }

        /// <summary>
        ///     Used internally.
        /// </summary>
        public bool IsDraggingPosition {
            get { return Designer.IsDraggingPosition; }
        }

#endif // UNITY_EDITOR
        #endregion Advanced Editor Scripting

        /// <summary>
        ///     Are we currently in the scene view?
        /// </summary>
        protected bool isSceneView = false;
        internal bool IsSceneView { get { return isSceneView; } }
        internal virtual void SetIsSceneView(bool isSceneView) {
            this.isSceneView = isSceneView;
            DefaultScoreFlash.SetIsSceneView(isSceneView);
        }

        internal virtual GUISkin EditorGUISkin {
            get { return DefaultScoreFlash.EditorGUISkin; }
        }


        #region Abstract Methods from ScoreFlashBase
        /// <summary>
        ///     The text that should be displayed as placeholder.
        /// </summary>
        protected abstract string DesignText { get; }
        #endregion Abstract Methods from ScoreFlashBase


        #region Implementation of IHasVisualDesigner interface
        /// <summary>
        ///     Is the designer actually switched on?
        /// </summary>
        public abstract bool IsDesignMode { get; }

        /// <summary>
        ///     The score flash instance that is used to render messages, or the score
        ///     flash instance itself.
        /// </summary>
        public abstract ScoreFlash DefaultScoreFlash { get; }

        /// <summary>
        ///     The relative, normalized position.
        /// </summary>
        public abstract Vector2 Position { get; set; }

        /// <summary>
        ///     An optional world position / offset. If you're not using this,
        ///     just return null (not Vector3.zero but the real null ;-) ).
        /// </summary>
        public abstract Vector3? PositionWorld { get; set; }

        /// <summary>
        ///     Return <c>true</c>, if you need the visual designer of your component
        ///     to support setting the screen alignment.
        /// </summary>
        public abstract bool SupportsScreenAlign { get; }

        /// <summary>
        ///     The screen alignment of this component. Will only be changed by designer
        ///     if <see cref="SupportsScreenAlign"/> returns true.
        /// </summary>
        public abstract NGAlignment.ScreenAlign ScreenAlign { get; set; }

        /// <summary>
        ///     Should the screen alignment be locked? Will only be changed by designer
        ///     if <see cref="SupportsScreenAlign"/> returns true.
        /// </summary>
        public abstract bool LockScreenAlign { get; set; }

        /// <summary>
        ///     Return <c>true</c>, if you need the visual designer of your component
        ///     to support setting the inner anchor.
        /// </summary>
        public abstract bool SupportsInnerAnchor { get; }

        /// <summary>
        ///     The inner anchor of this component. Will only be changed by designer
        ///     if <see cref="SupportsInnerAnchor"/> returns true.
        /// </summary>
        public abstract NGAlignment.ScreenAlign InnerAnchor { get; set; }

        /// <summary>
        ///     Should the inner anchor be locked? Will only be changed by designer
        ///     if <see cref="SupportsInnerAnchor"/> returns true.
        /// </summary>
        public abstract bool LockInnerAnchor { get; set; }

        /// <summary>
        ///     The maximum width of the items.
        /// </summary>
        public abstract float MaxWidth { get; set; }

        /// <summary>
        ///     The minimum padding of the items.
        /// </summary>
        public abstract float MinPaddingX { get; set; }

        /// <summary>
        ///     Creates a ScoreMessage to be used in the designer.
        /// </summary>
        /// <param name="text">the text for the message</param>
        /// <returns>the ScoreMessage</returns>
        public abstract ScoreMessage CreateDesignerMessage(string text);

        #endregion Implementation of IHasVisualDesigner interface

    }
}