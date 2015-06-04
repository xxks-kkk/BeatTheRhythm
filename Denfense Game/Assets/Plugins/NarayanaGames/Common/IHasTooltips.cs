using UnityEngine;
using System.Collections;

namespace NarayanaGames.Common {
    /// <summary>
    ///     Interface implemented by MonoBehaviours that have tooltips for 
    ///     their custom inspectors.
    /// </summary>
    public interface IHasTooltips {
#if UNITY_EDITOR
        /// <summary>
        ///     Such MonoBehaviours simply need to provide access to the
        ///     Tooltips text asset and everything will be very fine :-)
        /// </summary>
        TextAsset Tooltips { get; }
#endif
    }
}