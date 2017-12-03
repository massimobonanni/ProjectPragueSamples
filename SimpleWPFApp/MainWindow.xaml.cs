using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GestureLibrary;
using Microsoft.Gestures;
using Microsoft.Gestures.Endpoint;

namespace SimpleWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private GesturesServiceEndpoint GesturesService;

        private Gesture AcceptableGesture;
        private Gesture OptimalGesture;

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GesturesService = GesturesServiceEndpointFactory.Create();
            GesturesService.StatusChanged += GesturesService_StatusChanged;
            GesturesService.WarningLog += GesturesService_WarningLog;

            GesturesCombo.ItemsSource = GestureHelper.Gestures().OrderBy(g => g.Name);

            if (!await GesturesService.ConnectAsync())
            {
                MessageBox.Show("Errore durante la connessione al Gesture Service!");
            }
        }

        private Gesture CurrentGesture;


        private async Task RegisterGesture()
        {
            await UnregisterGesture();

            if (GesturesCombo.SelectedItem is Gesture gesture)
            {
                this.LogListBox.Items.Clear();
                gesture.AddTriggerEventToSegments(Pose_Triggered);
                gesture.Triggered += CurrentGesture_Triggered;
                await GesturesService.RegisterGesture(gesture);
            }
        }

        private async Task UnregisterGesture()
        {
            if (CurrentGesture != null)
            {
                CurrentGesture.RemovTriggerEventFromSegments(Pose_Triggered);
                CurrentGesture.Triggered += CurrentGesture_Triggered;
                await GesturesService.UnregisterGesture(CurrentGesture);
            }
        }

        private async void WriteLogMessage(string message, Color foregroundColor)
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                var logMessage = $"{DateTime.Now:HH:mm:ss.fff} - {message}";
                var item = new ListBoxItem();
                item.Foreground = new SolidColorBrush(foregroundColor);
                item.Content = logMessage;

                this.LogListBox.Items.Insert(0, item);
            });
        }

        private void CurrentGesture_Triggered(object sender, GestureSegmentTriggeredEventArgs e)
        {
            WriteLogMessage($"[GESTURE] - {e.GestureSegment} - {e.NestingPath}", Colors.Black);
        }

        private void GesturesService_WarningLog(object sender, WarningEventArgs exception)
        {
            WriteLogMessage($"{exception.Exception}", Colors.Red);
        }

        private void GesturesService_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            WriteLogMessage($"{e.Status }", Colors.Blue);
        }


        private void Pose_Triggered(object sender, GestureSegmentTriggeredEventArgs e)
        {
            WriteLogMessage($"[POSE] - {e.GestureSegment }", Colors.Green);
        }


        private Gesture IndiceSopraPollice_Rotazione90Gradi()
        {
            var hold = new HandPose("Hold", new FingerPose(new[] { Finger.Thumb, Finger.Index }, FingerFlexion.Open, PoseDirection.Forward),
                new FingertipDistanceRelation(Finger.Index, RelativeDistance.NotTouching, Finger.Thumb),
                new FingertipPlacementRelation(Finger.Index, RelativePlacement.Above, Finger.Thumb));
            // ... define the second pose, ...
            var rotate = new HandPose("Rotate", new FingerPose(new[] { Finger.Thumb, Finger.Index }, FingerFlexion.Open, PoseDirection.Forward),
                new FingertipDistanceRelation(Finger.Index, RelativeDistance.NotTouching, Finger.Thumb),
                new FingertipPlacementRelation(Finger.Index, RelativePlacement.Right, Finger.Thumb));
            return new Gesture("IndiceSopraPollice_Rotazione90Gradi", hold, rotate);
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            await RegisterGesture();
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            await UnregisterGesture();
        }

        private void ClearListButton_Click(object sender, RoutedEventArgs e)
        {
            this.LogListBox.Items.Clear();
        }
    }
}
