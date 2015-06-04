/****************************************************
 *  (c) 2013 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

#define UNITY // are we in Unity? or outside?

using System.Collections;
#if UNITY
using UnityEngine;
#endif

namespace NarayanaGames.Common {
    /// <summary>
    ///     This class provides convenient access to several easing 
    ///     functions and is coded in an easy to understand manner.
    /// </summary>
    /// <remarks>
    ///     If you 
    ///     know how to use 
    ///     <a href="http://docs.unity3d.com/Documentation/ScriptReference/Mathf.Lerp.html">Mathf.Lerp()</a>, 
    ///     you know how to work with these (only that the "linear" is not at all linear here ;-) ).
    ///     Personally, I like to draw the precise curve in the Unity editor
    ///     using <a href="http://docs.unity3d.com/Documentation/ScriptReference/AnimationCurve.html">AnimationCurve</a>.
    ///     And I've added a convenient API to use those just like Mathf.Lerp(),
    ///     which is inspired by <a href="http://wiki.unity3d.com/index.php?title=Mathfx">Mathfx from the UnifyCommunity Wiki</a>.
    ///     However, there are a few cases where adding an AnimationCurve just 
    ///     doesn't cut it (like when you want to convert linear property inspector values
    ///     from 0 to 1 that would result in only values between 0.7 and 0.9 having any
    ///     noticeable effect to have a noticeable effect in the whole range).
    ///     So for that purpose, I've added my own implementation of some
    ///     of the "usual suspect" easing functions:
    ///  <p />
    ///     The basic EaseIn/EaseOut/EaseInOut methods are inspired by 
    ///     <a href="http://www.robertpenner.com/easing/">Robert
    ///     Penner's work</a> - however, they are written from scratch to be more 
    ///     readable, perfectly fit my needs and more versatile (but less 
    ///     performant). The bouncing formulas were created using the
    ///     <a href="http://timotheegroleau.com/Flash/experiments/easing_function_generator.htm">
    ///     Easing Function Generator</a>.
    ///     See also:
    ///     <a href="http://stackoverflow.com/questions/5436684/jquery-elastic-easing-equation">
    ///     Stackoverflow: jQuery elastic easing equation</a>
    /// </remarks>
    public static class NGEasing {

        #region Easing functions using AnimationCurve (only work in Unity games!)
#if UNITY
        /// <summary>
        ///     Lets you ease a float value based on any animation curve. Usage
        ///     is pretty much the same as <c>Mathf.Lerp()</c> only that you also pass
        ///     an Animation curve as parameter.
        /// </summary>
        /// <param name="curve">the curve to drive easing</param>
        /// <param name="from">the value to start from</param>
        /// <param name="to">the value to ease to</param>
        /// <param name="time">
        ///     the time in the animation (if you use animation curves with 
        ///     time from 0 to 1, this works exactly as Mathf.Lerp)
        /// </param>
        /// <returns>a value between <c>from</c> and <c>to</c> based on <c>time</c></returns>
        public static float EaseOnCurve(AnimationCurve curve, float from, float to, float time) {
            float distance = to - from;
            return from + curve.Evaluate(time) * distance;
        }

        /// <summary>
        ///     Lets you ease a Vector3 based on any animation curve. Usage
        ///     is pretty much the same as <c>Mathf.Lerp()</c> only that you also pass
        ///     an Animation curve as parameter.
        /// </summary>
        /// <param name="curve">the curve to drive easing</param>
        /// <param name="from">the value to start from</param>
        /// <param name="to">the value to ease to</param>
        /// <param name="time">
        ///     the time in the animation (if you use animation curves with 
        ///     time from 0 to 1, this works exactly as Mathf.Lerp)
        /// </param>
        /// <returns>a value between <c>from</c> and <c>to</c> based on <c>time</c></returns>
        public static Vector3 EaseOnCurve(AnimationCurve curve, Vector3 from, Vector3 to, float time) {
            Vector3 distance = to - from;
            return from + curve.Evaluate(time) * distance;
        }

        /// <summary>
        ///     Lets you ease a Color based on any animation curve. Usage
        ///     is pretty much the same as <c>Mathf.Lerp()</c> only that you also pass
        ///     an Animation curve as parameter.
        /// </summary>
        /// <param name="curve">the curve to drive easing</param>
        /// <param name="from">the value to start from</param>
        /// <param name="to">the value to ease to</param>
        /// <param name="time">
        ///     the time in the animation (if you use animation curves with 
        ///     time from 0 to 1, this works exactly as Mathf.Lerp)
        /// </param>
        /// <returns>a value between <c>from</c> and <c>to</c> based on <c>time</c></returns>
        public static Color EaseOnCurve(AnimationCurve curve, Color from, Color to, float time) {
            Color distance = to - from;
            return from + curve.Evaluate(time) * distance;
        }
#endif // UNITY

        #endregion Easing functions using AnimationCurve

        #region Easing functions that work without an AnimationCurve (could be used in any .NET project)

        /// <summary>
        ///     Available default ease types. You can use these with
        ///     <see cref="NGEasing.Generic(EaseType, float, float, float)"/>,
        ///     e.g. if you want to select which type of easing to use via
        ///     some setting.
        /// </summary>
        public enum EaseType {
            /// <summary>
            ///     Linear interpolation.
            /// </summary>
            Lerp,
            /// <summary>
            ///     Easing in, overshooting a little, going back ... and done.
            /// </summary>
            EaseInBackOut,
            /// <summary>
            ///     Ease in to the power of 1.5. Very soft easing, almost linear.
            /// </summary>
            EaseIn1_5,
            /// <summary>
            ///     Ease in to the power of 2.
            /// </summary>
            EaseIn2,
            /// <summary>
            ///     Ease in to the power of 3.
            /// </summary>
            EaseIn3,
            /// <summary>
            ///     Ease in to the power of 4.
            /// </summary>
            EaseIn4,
            /// <summary>
            ///     Ease in to the power of 5.
            /// </summary>
            EaseIn5,
            /// <summary>
            ///     Ease in to the power of 8. Pretty steep.
            /// </summary>
            EaseIn8,
            /// <summary>
            ///     Ease in to the power of 12. So ... this is very steep ;-)
            /// </summary>
            EaseIn12,
            /// <summary>
            ///     Ease out to the power of 1.5. Very soft easing, almost linear.
            /// </summary>
            EaseOut1_5,
            /// <summary>
            ///     Ease out to the power of 2.
            /// </summary>
            EaseOut2,
            /// <summary>
            ///     Ease out to the power of 2.
            /// </summary>
            EaseOut3,
            /// <summary>
            ///     Ease out to the power of 4.
            /// </summary>
            EaseOut4,
            /// <summary>
            ///     Ease out to the power of 5.
            /// </summary>
            EaseOut5,
            /// <summary>
            ///     Ease out to the power of 8. Pretty steep.
            /// </summary>
            EaseOut8,
            /// <summary>
            ///     Ease out to the power of 12. So ... this is very steep ;-)
            /// </summary>
            EaseOut12,
            /// <summary>
            ///     Ease in and out to the power of 1.5. Very soft easing, almost linear.
            /// </summary>
            EaseInOut1_5,
            /// <summary>
            ///     Ease in and out to the power of 2.
            /// </summary>
            EaseInOut2,
            /// <summary>
            ///     Ease in and out to the power of 3.
            /// </summary>
            EaseInOut3,
            /// <summary>
            ///     Ease in and out to the power of 4.
            /// </summary>
            EaseInOut4,
            /// <summary>
            ///     Ease in and out to the power of 5.
            /// </summary>
            EaseInOut5,
            /// <summary>
            ///     Ease in and out to the power of 8. Pretty steep.
            /// </summary>
            EaseInOut8,
            /// <summary>
            ///     Ease in and out to the power of 12. So ... this is very steep ;-)
            /// </summary>
            EaseInOut12,
        }

        /// <summary>
        ///     Generic easing for floats with any EaseType. Use this, if you want
        ///     to be able to configure the type of easing, e.g. as a public inspector
        ///     property.
        /// </summary>
        /// <param name="type">the easing method to use</param>
        /// <param name="from">Value to start from</param>
        /// <param name="to">Value to go to</param>
        /// <param name="frac">Value between 0 and 1</param>
        /// <returns>eased interpolation from <c>from</c> to <c>to</c></returns>
        public static float Generic(EaseType type, float from, float to, float frac) {
            #region No one wants to see this mapping from enum to methods ;-)
            // really? ok, you got my respect to dare looking down the rabbit hole
            switch (type) {
                case EaseType.EaseInBackOut:
                    return EaseInBackOut(from, to, frac);
                case EaseType.EaseIn1_5:
                    return EaseIn(from, to, frac, 1.5F);
                case EaseType.EaseIn2:
                    return EaseIn(from, to, frac, 2);
                case EaseType.EaseIn3:
                    return EaseIn(from, to, frac, 3);
                case EaseType.EaseIn4:
                    return EaseIn(from, to, frac, 4);
                case EaseType.EaseIn5:
                    return EaseIn(from, to, frac, 5);
                case EaseType.EaseIn8:
                    return EaseIn(from, to, frac, 8);
                case EaseType.EaseIn12:
                    return EaseIn(from, to, frac, 12);
                case EaseType.EaseOut1_5:
                    return EaseOut(from, to, frac, 1.5F);
                case EaseType.EaseOut2:
                    return EaseOut(from, to, frac, 2);
                case EaseType.EaseOut3:
                    return EaseOut(from, to, frac, 3);
                case EaseType.EaseOut4:
                    return EaseOut(from, to, frac, 4);
                case EaseType.EaseOut5:
                    return EaseOut(from, to, frac, 5);
                case EaseType.EaseOut8:
                    return EaseOut(from, to, frac, 8);
                case EaseType.EaseOut12:
                    return EaseOut(from, to, frac, 12);
                case EaseType.EaseInOut1_5:
                    return EaseInOut(from, to, frac, 1.5F);
                case EaseType.EaseInOut2:
                    return EaseInOut(from, to, frac, 2);
                case EaseType.EaseInOut3:
                    return EaseInOut(from, to, frac, 3);
                case EaseType.EaseInOut4:
                    return EaseInOut(from, to, frac, 4);
                case EaseType.EaseInOut5:
                    return EaseInOut(from, to, frac, 5);
                case EaseType.EaseInOut8:
                    return EaseInOut(from, to, frac, 8);
                case EaseType.EaseInOut12:
                    return EaseInOut(from, to, frac, 12);
                
                case EaseType.Lerp:
                default:
                    return Lerp(from, to, frac);
                // nothing to see here ;-)
            }
            #endregion No one wants to see this mapping from enum to methods ;-)
        }

        /// <summary>
        ///     Simple linear interpolation. This gives you the same result as
        ///     Mathf.Lerp(from, to, t) and also clamps time between 0 and 1,
        ///     so you don't have to worry about values less than 0 or larger 
        ///     than 1.
        /// </summary>
        /// <param name="from">Value to start from</param>
        /// <param name="to">Value to go to</param>
        /// <param name="frac">Value between 0 and 1</param>
        /// <returns>linear interpolation from <c>from</c> to <c>to</c></returns>
        public static float Lerp(float from, float to, float frac) {
            frac = Clamp01(frac);
            return (1F - frac) * from
                 + frac * to;
        }

        /// <summary>
        ///     Eases in.
        /// </summary>
        /// <param name="from">Value to start from</param>
        /// <param name="to">Value to go to</param>
        /// <param name="frac">Value between 0 and 1</param>
        /// <param name="pow">the higher this value, the more intense the easing gets</param>
        /// <returns>eased interpolation from <c>from</c> to <c>to</c></returns>
        public static float EaseIn(float from, float to, float frac, float pow) {
            frac = Clamp01(frac);
            frac = Pow(frac, pow);
            return (1F - frac) * from
                 + frac * to;
        }

        /// <summary>
        ///     Eases out.
        /// </summary>
        /// <param name="from">Value to start from</param>
        /// <param name="to">Value to go to</param>
        /// <param name="frac">Value between 0 and 1</param>
        /// <param name="pow">the higher this value, the more intense the easing gets</param>
        /// <returns>eased interpolation from <c>from</c> to <c>to</c></returns>
        public static float EaseOut(float from, float to, float frac, float pow) {
            frac = Clamp01(frac);

            // invert the power effect
            frac = Pow(1 - frac, pow);

            // invert the inversion
            return frac * from
                 + (1F - frac) * to;
        }

        /// <summary>
        ///     Eases in, and eases out.
        /// </summary>
        /// <param name="from">Value to start from</param>
        /// <param name="to">Value to go to</param>
        /// <param name="frac">Value between 0 and 1</param>
        /// <param name="pow">the higher this value, the more intense the easing gets</param>
        /// <returns>eased interpolation from <c>from</c> to <c>to</c></returns>
        public static float EaseInOut(float from, float to, float frac, float pow) {
            frac = Clamp01(frac);
            frac *= 2F; // double the speed

            float fracA = Pow(frac, pow); // first half: straight
            float fracB = Pow(frac - 2, pow); // second half: inverted

            float sign = Pow(-1, pow + 1); // fix sign "jumps"

            float diff = to - from;
            diff *= 0.5F; // half the distance
            return from + (frac < 1F // this is actually from 0.0 to 0.5 and 0.5 to 1.0
                           ? fracA * diff
                           : (2F + sign * fracB) * diff);
        }


        /// <summary>
        ///     Softly eases in, then overshoots a little, bounces back a 
        ///     little and comes to rest. 
        /// </summary>
        /// <remarks>
        ///     Generated using:
        ///     http://timotheegroleau.com/Flash/experiments/easing_function_generator.htm
        ///     Starting with preset "in-out cubic" and adjusting the values:
        ///     Using P0: 0, P1: 0.01, P2: 0.24, P3: 1.9495, P4: 0.77, P5: 1
        /// </remarks>
        /// <param name="from">Value to start from</param>
        /// <param name="to">Value to go to</param>
        /// <param name="frac">Value between 0 and 1</param>
        /// <returns>a nice curve from <c>from</c> to <c>to</c></returns>
        public static float EaseInBackOut(float from, float to, float frac) {
            // rename / change parameters to match output of easing function generator
            float t = frac;
            float b = from;
            float c = to - from;
            float d = 1F;

            // this is the output from the generator - just "F"s added ;-)
	        var ts = (t/=d) * t;
	        var tc = ts * t;
	        return b + c*(14.295F*tc*ts + -28.14F*ts*ts + 12.595F*tc + 2.2F*ts + 0.05F*t);
        }

        /// <summary>
        ///     Returns <c>f</c> raised to the power of <c>p</c>.
        ///     Convenience wrapper for System.Math.Pow that handles casting stuff.
        /// </summary>
        /// <param name="f">the number to be raised to the power of <c>p</c></param>
        /// <param name="p">the power to raise <c>f</c></param>
        /// <returns></returns>
        private static float Pow(float f, float p) {
            return (float)System.Math.Pow((double)f, p);
        }

        /// <summary>
        ///     This does the same as Unity's built in Mathf.Clamp01 but 
        ///     by having my own implementation I cut the dependency to
        ///     UnityEngine so I can also use (and test) this outside of
        ///     Unity.
        /// </summary>
        /// <param name="f">any float</param>
        /// <returns>a float between 0 and 1</returns>
        private static float Clamp01(float f) {
            if (f < 0) {
                return 0;
            } else if (f > 1) {
                return 1;
            } else {
                return f;
            }
        }
        #endregion Easing functions that work without an AnimationCurve

    }
}