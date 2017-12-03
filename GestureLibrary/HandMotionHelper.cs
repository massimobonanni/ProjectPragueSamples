using System.Runtime.CompilerServices;
using Microsoft.Gestures;

namespace GestureLibrary
{
    public static class HandMotionHelper
    {
        private static string GetMotionName([CallerMemberName] string propertyName = null)
        {
            return propertyName;
        }


        public static HandMotion Retract => new HandMotion(GetMotionName(),
                                                            new PalmMotion(Hand.RightHand, DepthMotionSegment.Backward));
    }
}
