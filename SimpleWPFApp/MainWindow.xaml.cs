using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

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
            this.Unloaded += MainWindow_Unloaded;
        }

        private async void MainWindow_Unloaded(object a, RoutedEventArgs e)
        {
            await UnregisterGesture();
            GesturesService?.Disconnect();
            GesturesService?.Dispose();
        }

        private GesturesServiceEndpoint GesturesService;
        private Gesture CurrentGesture;

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

 


        private async Task RegisterGesture()
        {
            await UnregisterGesture();

            if (GesturesCombo.SelectedItem is Gesture gesture)
            {
                CurrentGesture = gesture;
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
                CurrentGesture = null;
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

        private async void SaveXamlButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentGesture != null)
            {
                var dlg = new SaveFileDialog();
                if (dlg.ShowDialog(this) == true)
                {
                    var gestureXaml = CurrentGesture.ToXaml();
                    
                    using (StreamWriter writer = File.CreateText(dlg.FileName))
                    {
                        await writer.WriteAsync(gestureXaml);
                    }
                    
                    MessageBox.Show("Salvataggio eseguito con successo!!");
                }
            }
        }
    }
}
