using UnityEngine;
using System.Collections;

namespace NarayanaGames.ScoreFlashComponent {
    /// <summary>
    ///     Interface that custom renderers can implement if they are based on Unity GUI
    ///     and have an OnGUI method. This will make them render properly in the designer.
    /// </summary>
    public interface IHasOnGUI {

        /// <summary>
        ///     The <a href="http://docs.unity3d.com/Documentation/ScriptReference/MonoBehaviour.OnGUI.html">OnGUI()</a> method.
        /// </summary>
        void OnGUI();
    }
}