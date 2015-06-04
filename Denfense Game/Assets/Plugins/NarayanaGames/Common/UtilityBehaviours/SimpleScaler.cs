using UnityEngine;
using System.Collections;

namespace NarayanaGames.Common.UtilityBehaviours {
    /// <summary>
    ///     This is a very simple scaler. Just set up the relevant animation
    ///     curves and frequency.
    /// </summary>
    public class SimpleScaler : MonoBehaviour {

        public Vector3 targetScale = Vector3.one;

        public AnimationCurve scaleX = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(0.5F, 1), new Keyframe(1, 0) });
        public AnimationCurve scaleY = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(0.5F, 1), new Keyframe(1, 0) });
        public AnimationCurve scaleZ = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(0.5F, 1), new Keyframe(1, 0) });

        public float frequency = 1;

        private Vector3 initialScale;
        private Vector3 currentScale;
        private float lastTimeStart;

        public void Awake() {
            initialScale = transform.localScale;
            lastTimeStart = Time.time;
        }

        /// <summary>
        ///     Scales this game object's transform with the given parameters.
        /// </summary>
        public void Update() {
            float timeDiff = (Time.time - lastTimeStart) * frequency;
            currentScale.x = NGEasing.EaseOnCurve(scaleX, initialScale.x, targetScale.x, timeDiff);
            currentScale.y = NGEasing.EaseOnCurve(scaleY, initialScale.y, targetScale.y, timeDiff);
            currentScale.z = NGEasing.EaseOnCurve(scaleZ, initialScale.z, targetScale.z, timeDiff);
            transform.localScale = currentScale;
            if (timeDiff >= 1) {
                lastTimeStart = Time.time;
            }
        }
    }
}