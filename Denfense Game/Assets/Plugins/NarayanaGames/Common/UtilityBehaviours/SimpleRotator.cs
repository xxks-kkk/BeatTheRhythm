using UnityEngine;
using System.Collections;

namespace NarayanaGames.Common.UtilityBehaviours {
    /// <summary>
    ///     This is a very simple rotator. Just give it a rotation speed
    ///     and axis, and it will automatically let a Transform rotate
    ///     around that axis with the given speed.
    /// </summary>
    public class SimpleRotator : MonoBehaviour {

        /// <summary>
        ///     The rotation speed.
        /// </summary>
        public float rotationSpeed = 30F;

        /// <summary>
        ///     The rotation axis.
        /// </summary>
        public Vector3 rotationAxis = Vector3.up;

        /// <summary>
        ///     Rotates this game object's transform with the given speed around axis.
        /// </summary>
        public void Update() {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }
}