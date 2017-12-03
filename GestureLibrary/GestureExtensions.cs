using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Gestures;

namespace GestureLibrary
{
    public static class GestureExtensions
    {
        public static void AddTriggerEventToSegments(this Gesture gesture,
            EventHandler<GestureSegmentTriggeredEventArgs> trigger)
        {
            if (gesture == null)
                throw new NullReferenceException(nameof(gesture));

            foreach (var segment in gesture.Segments)
            {
                segment.Triggered += trigger;
            }
        }

        public static void RemovTriggerEventFromSegments(this Gesture gesture,
            EventHandler<GestureSegmentTriggeredEventArgs> trigger)
        {
            if (gesture == null)
                throw new NullReferenceException(nameof(gesture));

            foreach (var segment in gesture.Segments)
            {
                segment.Triggered -= trigger;
            }
        }
    }
}
