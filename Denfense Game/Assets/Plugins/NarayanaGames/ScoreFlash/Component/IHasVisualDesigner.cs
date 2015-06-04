/****************************************************
 *  (c) 2013 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;
using NarayanaGames.Common.UI;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Interface for components that have a score flash style visual designer.
    ///     The interface speaks to you, and says "I has visual designer".
    /// </summary>
    public interface IHasVisualDesigner {

#if UNITY_EDITOR
        /// <summary>
        ///     Is the game object currently selected?
        /// </summary>
        bool IsSelected { get; }

        /// <summary>
        ///     Are multiple instances of that object currently selected?
        /// </summary>
        bool IsMultiSelect { get; }
#endif

        /// <summary>
        ///     Is the designer actually switched on?
        /// </summary>
        bool IsDesignMode { get; }

        /// <summary>
        ///     The score flash instance that is used to render messages, or the score
        ///     flash instance itself.
        /// </summary>
        ScoreFlash DefaultScoreFlash { get; }

        /// <summary>
        ///     The relative, normalized position.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        ///     An optional world position / offset. If you're not using this,
        ///     just return null (not Vector3.zero but the real null ;-) ).
        /// </summary>
        Vector3? PositionWorld { get; set; }

        /// <summary>
        ///     Return <c>true</c>, if you need the visual designer of your component
        ///     to support setting the screen alignment.
        /// </summary>
        bool SupportsScreenAlign { get; }

        /// <summary>
        ///     The screen alignment of this component. Will only be changed by designer
        ///     if <see cref="SupportsScreenAlign"/> returns true.
        /// </summary>
        NGAlignment.ScreenAlign ScreenAlign { get; set; }

        /// <summary>
        ///     Should the screen alignment be locked? Will only be changed by designer
        ///     if <see cref="SupportsScreenAlign"/> returns true.
        /// </summary>
        bool LockScreenAlign { get; set; }



        /// <summary>
        ///     Return <c>true</c>, if you need the visual designer of your component
        ///     to support setting the inner anchor.
        /// </summary>
        bool SupportsInnerAnchor { get; }

        /// <summary>
        ///     The inner anchor of this component. Will only be changed by designer
        ///     if <see cref="SupportsInnerAnchor"/> returns true.
        /// </summary>
        NGAlignment.ScreenAlign InnerAnchor { get; set; }

        /// <summary>
        ///     Should the inner anchor be locked? Will only be changed by designer
        ///     if <see cref="SupportsInnerAnchor"/> returns true.
        /// </summary>
        bool LockInnerAnchor { get; set; }

        /// <summary>
        ///     The maximum width of the items.
        /// </summary>
        float MaxWidth { get; set; }

        /// <summary>
        ///     The minimum padding of the items.
        /// </summary>
        float MinPaddingX { get; set; }

        /// <summary>
        ///     Creates a ScoreMessage to be used in the designer.
        /// </summary>
        /// <param name="text">the text for the message</param>
        /// <returns>the ScoreMessage</returns>
        ScoreMessage CreateDesignerMessage(string text);
    }
}