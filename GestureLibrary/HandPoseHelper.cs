using System.Runtime.CompilerServices;
using Microsoft.Gestures;

namespace GestureLibrary
{
    public static class HandPoseHelper
    {
        private static string GetPoseName([CallerMemberName] string propertyName = null)
        {
            return propertyName;
        }


        public static HandPose ClosedFist => new HandPose(GetPoseName(),
                                                          new FingerPose(new AllFingersContext(),FingerFlexion.Folded));

        public static HandPose OpenHand
        {
            get
            {
                var fingerContext = new AllFingersContext();
                return new HandPose(GetPoseName(),
                    new PalmPose(Hand.RightHand, PoseDirection.Undefined, PoseDirection.Undefined),
                    new FingerPose(fingerContext, FingerFlexion.Open),
                    new FingertipDistanceRelation(fingerContext, RelativeDistance.NotTouching));
            }
        }

        public static HandPose Victory => new HandPose(GetPoseName(),
                                                        new FingerPose(new[] { Finger.Index, Finger.Middle }, FingerFlexion.Open),
                                                        new FingertipDistanceRelation(Finger.Index, RelativeDistance.NotTouching, Finger.Middle),
                                                        new FingerPose(new[] { Finger.Thumb, Finger.Ring, Finger.Pinky }, FingerFlexion.Folded));

        public static HandPose BoyScoutGreeting => new HandPose(GetPoseName(),
                                                                new FingerPose(new[] { Finger.Index, Finger.Middle }, FingerFlexion.Open),
                                                                new FingertipDistanceRelation(Finger.Index, RelativeDistance.Touching, Finger.Middle),
                                                                new FingerPose(new[] { Finger.Thumb, Finger.Ring, Finger.Pinky }, FingerFlexion.Folded));

        public static HandPose Pinch
        {
            get
            {
                var fingerContext = new AllFingersContext();
                return new HandPose(GetPoseName(),
                    new PalmPose(new[] { Hand.RightHand, Hand.LeftHand }, PoseDirection.Forward, PoseDirection.Up),
                    new FingerPose(fingerContext.Fingers, FingerFlexion.Open),
                    new FingertipPlacementRelation(new[] { Finger.Middle, Finger.Ring, Finger.Pinky }, RelativePlacement.Above, new[] { Finger.Index, Finger.Thumb }),
                    new FingertipPlacementRelation(Finger.Index, RelativePlacement.Above, Finger.Thumb),
                    new FingertipDistanceRelation(new[] { Finger.Index, Finger.Thumb }, RelativeDistance.Touching));
            }
        }

        public static HandPose NotPinch => new HandPose(GetPoseName(),
                                                        new PalmPose(new[] { Hand.RightHand, Hand.LeftHand }, PoseDirection.Forward, PoseDirection.Up),
                                                        new FingerPose(new[] { Finger.Index, Finger.Thumb }, FingerFlexion.Open),
                                                        new FingertipDistanceRelation(new[] { Finger.Index, Finger.Thumb }, RelativeDistance.NotTouching));

        public static HandPose RotateSet => new HandPose(GetPoseName(),
                                                        new FingerPose(new[] { Finger.Thumb, Finger.Index }, FingerFlexion.Open, PoseDirection.Forward),
                                                        new FingertipPlacementRelation(Finger.Index, RelativePlacement.Above, Finger.Thumb),
                                                        new FingertipDistanceRelation(Finger.Index, RelativeDistance.NotTouching, Finger.Thumb));

        public static HandPose RotateGo => new HandPose(GetPoseName(),
                                                        new FingerPose(new[] { Finger.Thumb, Finger.Index }, FingerFlexion.Open, PoseDirection.Forward),
                                                        new FingertipPlacementRelation(Finger.Index, RelativePlacement.Right, Finger.Thumb),
                                                        new FingertipDistanceRelation(Finger.Index, RelativeDistance.NotTouching, Finger.Thumb));
    }
}
