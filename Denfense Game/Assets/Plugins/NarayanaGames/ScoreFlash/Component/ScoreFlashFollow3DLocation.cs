/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using UnityEngine;
using System.Collections;

using NarayanaGames.Common;

namespace NarayanaGames.ScoreFlashComponent {
    public class ScoreFlashFollow3DLocation {
        private ScoreFlashFollow3D target;
        /// <summary>
        ///     Can be used by 3D renders to get ScoreFlashFollow3D instance,
        ///     in particular the reference camera.
        /// </summary>
        public ScoreFlashFollow3D Target {
            get { return target; }
        }
        private bool lastVisible = true;
        private Vector3 lastPosition;
        private Vector3 lastVelocity;
        private Vector3 currentVelocity;
        private Vector3 currentPosition;

        private float leaveBehind = 0;
        private float loseMomentum = 0;

        private Vector2 screenPositionOffset;
        private Vector3 worldPositionOffset;
        private Camera referenceCamera;

        private float lastTime = 0F;
        private float lastTimeCheckedVisibility = 0F;

        private float lastFixedTime = 0F;

        private bool wasEnabled = true;

        internal void Update() {
            // make sure this is only executed once per frame
            if (Time.time == lastTime) {
                return;
            }
            lastTime = Time.time;

            if (target != null && target.gameObject.activeInHierarchy && !wasEnabled) {
                Init();
            }
            wasEnabled = target == null ? false : target.gameObject.activeInHierarchy;

            if (wasEnabled) {


                if (Time.time - lastTimeCheckedVisibility > 0.5F) {
                    lastTimeCheckedVisibility = Time.time + Random.Range(0F, 0.2F); // "distribute load"

                    // Problem with this approach is scene view cameras and stuff like minimaps using cameras
                    lastVisible = target.TargetRenderer == null ? true : target.TargetRenderer.isVisible;

                    // if visible in any camera - check if we are really visible in the relevant camera (this is very expensive!)
                    if (lastVisible) {
                        // problem with this approach is that it is pretty hard on performance => only do it twice per second
                        lastVisible = target.TargetRenderer == null
                            ? true
                            : NGUtil.IsRendererVisibleFrom(GetCamera(false), target.TargetRenderer);
                    }
                }

                // make sure changes in editor are immediately used
                leaveBehind = 0.95F * NGEasing.EaseOut(0, 1, target.leaveBehind, 3F); // convert from linear to useful
                loseMomentum = 15F * NGEasing.EaseIn(0, 1, target.loseMomentum, 3F);
                UpdateScreenPositionOffset();

                if (target.leaveBehind == 1) {
                    return;
                }

                // make currentVelocity "almost" last velocity
                currentVelocity = Vector3.Lerp(currentVelocity, lastVelocity, (1F - leaveBehind) * 0.9F);
            } else {
                lastVelocity = Vector3.zero;
            }
            if (target != null && target.IsFrozen) {
                return;
            }
            currentVelocity = Vector3.Lerp(currentVelocity, lastVelocity, loseMomentum * Time.deltaTime);
            currentPosition += currentVelocity * Time.deltaTime;
            // when we use "leaveBehind" and change the values, there's an offset generated
            // the following code makes sure this offset eventually disappears
            if (wasEnabled) {
                if (leaveBehind > 0 && loseMomentum < 1) {
                    currentPosition = Vector3.Lerp(currentPosition, lastPosition, (1F - leaveBehind) * Time.deltaTime);
                } else { // otherwise: fix it right above the item
                    currentPosition = lastPosition;
                }
            }
        }

        internal void UpdateScreenPositionOffset() {
            screenPositionOffset = target.screenPositionOffset;
            screenPositionOffset.y *= -1F; // invert "UnityGUI style" coordinates so we get "screen coordinates"
            worldPositionOffset = target.worldPositionOffset;
        }

        internal void FixedUpdate() {
            if (target != null) {
                if (Time.fixedTime == lastFixedTime) {
                    return;
                }
                lastFixedTime = Time.fixedTime;

                lastVelocity = (target.transform.position - lastPosition) / Time.fixedDeltaTime;
                lastPosition = target.transform.position;
            }
        }

        internal bool IsVisible {
            get { return lastVisible; }
        }

        internal Vector3 CurrentOriginalPosition {
            get {
                return currentPosition;
            }
        }

        /// <summary>
        ///     Can be used by 3D renders to get the current real position.
        /// </summary>
        public Vector3 CurrentTranslatedPosition {
            get {
                return currentPosition + worldPositionOffset;
            }
        }

        internal Vector2 CurrentScreenOffset {
            get {
                return screenPositionOffset;
            }
        }

        private bool hasSentReferenceCameraError = false;

        internal Camera GetCamera(bool isSceneView) {
            if (isSceneView && Camera.current != null) {
                return Camera.current;
            }
            if (target != null && target.referenceCamera != null) {
                referenceCamera = target.referenceCamera;
            }
            if (referenceCamera == null) {
                referenceCamera = Camera.main;
                if (referenceCamera == null) {
                    if (!hasSentReferenceCameraError) {
                        hasSentReferenceCameraError = true;
                        Debug.LogWarning("No reference camera assigned, and no camera with tag MainCamera found in scene - please assign tag MainCamera to your main camera!");
                    }
                    referenceCamera = (Camera)Object.FindObjectOfType(typeof(Camera));
                }
            }
            return referenceCamera;
        }

        internal ScoreFlashFollow3DLocation(ScoreFlashFollow3D target) {
            this.target = target;
            Init();
        }

        internal void Init() {
            referenceCamera = target.referenceCamera;
            lastPosition = target.transform.position;
            currentPosition = lastPosition;
            if (target.CachedRigidbody != null) {
                lastVelocity = target.CachedRigidbody.velocity;
            } else {
                lastVelocity = Vector3.zero;
            }
            currentVelocity = lastVelocity;
            Update();
        }
    }
}