/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Interface providing access to the layout properties of ScoreFlash.
    /// </summary>
    public interface IScoreFlashLayout {
        /// <summary>
        ///     Gets the maximum width of the messages.
        /// </summary>
        float MaxWidth { get; }
        /// <summary>
        ///     Gets the minimum x padding for the messages. The effect of 
        ///     this value depends on the horizontal aspect of innerAnchor.
        /// </summary>
        float MinPaddingX { get; }
    }
}