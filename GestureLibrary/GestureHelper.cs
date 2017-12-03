using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Gestures;

namespace GestureLibrary
{
    public static class GestureHelper
    {

        public static IEnumerable<Gesture> Gestures()
        {
            yield return SlingShot();
            yield return OpenCloseHand();
            yield return MakePeace();
            yield return RotateRight();
        }

        private static string GetGestureName([CallerMemberName] string propertyName = null)
        {
            return propertyName;
        }

        public static Gesture SlingShot()
        {
            var notPinch1 = HandPoseHelper.NotPinch;
            var pinch = HandPoseHelper.Pinch;
            var notPinch2 = HandPoseHelper.NotPinch;
            var retract = HandMotionHelper.Retract;

            Gesture gesture = new Gesture(GetGestureName(),
                notPinch1, pinch, retract, notPinch2);

            return gesture;
        }

        public static Gesture OpenCloseHand()
        {
            var closedFist = HandPoseHelper.ClosedFist;
            var openHand = HandPoseHelper.OpenHand;

            Gesture gesture = new Gesture(GetGestureName(),
                closedFist, openHand);

            return gesture;
        }

        public static Gesture RotateRight()
        {
            var rotateSet = HandPoseHelper.RotateSet;
            var rotateGo = HandPoseHelper.RotateGo;

            return new Gesture(GetGestureName(), rotateSet, rotateGo);
        }

        public static Gesture MakePeace()
        {
            var closedFist = HandPoseHelper.ClosedFist;
            var victory = HandPoseHelper.Victory;

            return new Gesture(GetGestureName(), closedFist, victory);
        }
    }
}
