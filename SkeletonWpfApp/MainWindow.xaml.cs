using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Gestures;
using Microsoft.Gestures.Endpoint;
using Microsoft.Gestures.Stock.HandPoses;

namespace SkeletonWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GesturesServiceEndpoint _gesturesService;

        private FingerPosition IndexPosition;
        private FingerPosition ThumbPosition;
        private FingerPosition MiddlePosition;
        private FingerPosition RingPosition;
        private FingerPosition PinkyPosition;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += WindowLoaded;
            Closed += (s, args) => _gesturesService.Dispose();

        }
        private async void WindowLoaded(object sender, RoutedEventArgs windowLoadedArgs)
        {
            IndexPosition = new FingerPosition(this.Canvas, this.Dispatcher, this.IndexEllipse, Finger.Index);
            ThumbPosition = new FingerPosition(this.Canvas, this.Dispatcher, this.ThumbEllipse, Finger.Thumb);
            MiddlePosition = new FingerPosition(this.Canvas, this.Dispatcher, this.MiddleEllipse, Finger.Middle);
            RingPosition = new FingerPosition(this.Canvas, this.Dispatcher, this.RingEllipse, Finger.Ring);
            PinkyPosition = new FingerPosition(this.Canvas, this.Dispatcher, this.PinkyEllipse, Finger.Pinky);

            _gesturesService = GesturesServiceEndpointFactory.Create();
            await _gesturesService.ConnectAsync();

            await _gesturesService.RegisterToSkeleton(SkeletonHandler);
        }

        private void SkeletonHandler(object sender, HandSkeletonsReadyEventArgs e)
        {
            IndexPosition.UpdatePosition(e.DefaultHandSkeleton);
            ThumbPosition.UpdatePosition(e.DefaultHandSkeleton);
            MiddlePosition.UpdatePosition(e.DefaultHandSkeleton);
            RingPosition.UpdatePosition(e.DefaultHandSkeleton);
            PinkyPosition.UpdatePosition(e.DefaultHandSkeleton);
        }
    }
}
