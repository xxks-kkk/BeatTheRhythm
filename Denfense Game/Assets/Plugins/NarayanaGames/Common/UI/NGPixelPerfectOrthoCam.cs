/****************************************************
 *  (c) 2013 narayana games UG (haftungsbeschr�nkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;
using NarayanaGames.Common;

/// <summary>
///     Attach this to a camera that you want to use for pixel perfect
///     GUI rendering. The camera will automatically be switched to
///     "ortho" and the size will automatically be half the screen size.
/// </summary>
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode()]
public class NGPixelPerfectOrthoCam : MonoBehaviour {

    void Update() {
        if (!this.GetComponent<Camera>().orthographic) {
            this.GetComponent<Camera>().orthographic = true;
        }

        if (this.GetComponent<Camera>().orthographicSize != (int)(Screen.height * 0.5F)) {
            this.GetComponent<Camera>().orthographicSize = (int)(Screen.height * 0.5F);
        }
    }

}
