using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Gestures;
using Microsoft.Gestures.Skeleton;

namespace SkeletonWpfApp
{
    public class FingerPosition
    {
        private Canvas Canvas;
        private Dispatcher Dispatcher;
        private Finger Finger;
        private FrameworkElement UIElement;

        private float MaxX = 200;
        private float MinX = -200;
        private float MaxY = 200;
        private float MinY = -100;

        public FingerPosition(Canvas canvas, Dispatcher dispatcher, FrameworkElement uiElement, Finger finger)
        {
            this.Canvas = canvas;
            this.Dispatcher = dispatcher;
            this.Finger = finger;
            this.UIElement = uiElement;
        }

    public void UpdatePosition(IHandSkeleton handSkeleton)
    {
        Dispatcher.Invoke(() =>
        {
            var fingerPosition = handSkeleton.FingerPositions[this.Finger];

            var deltaXSource = MaxX - MinX;
            var deltaXDest = (float)Canvas.ActualWidth;
            var xDest = (fingerPosition.X - MinX) * deltaXDest / deltaXSource;

            var deltaYSource = MaxY - MinY;
            var deltaYDest = (float)Canvas.ActualHeight;
            var yDest = (fingerPosition.Y - MinY) * deltaYDest / deltaYSource;

            Canvas.SetRight(UIElement, xDest);
            Canvas.SetTop(UIElement, yDest);
        });
    }
}
}
