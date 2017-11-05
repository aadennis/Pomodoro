using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Media.SpeechSynthesis;

namespace Pomodoro
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer dispatcherTimer;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;
        DateTimeOffset stopTime;
        int timesTicked = 0;
        int _pomodoroDuration;
        static bool IsReading = false;
        public string PomodoroDuration { get; private set; }
        public string PomodoroReminderInterval { get; private set; }



        public MainPage()
        {
            this.InitializeComponent();
            // default is 20 minutes...
            PomodoroDuration = Duration.Text = "20";
            PomodoroReminderInterval = ReminderInterval.Text = "5";
        }

        private void MySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            HandleSliderChange(sender);
        }

        private void HandleSliderChange(object sender)
        {
            if (sender is Slider slider)
            {
                Duration.Text = slider.Value.ToString();
                PomodoroDuration = Duration.Text;
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            DispatcherTimerSetup();
            ReadText(PomodoroDuration);
        }
        public static async void ReadText(string myText)
        {
            if (IsReading) return;
            IsReading = true;

            var mediaPlayer = new MediaElement();
            using (var speech = new SpeechSynthesizer())
            {
                speech.Voice = SpeechSynthesizer.AllVoices.First();
                var stream = await speech.SynthesizeTextToStreamAsync(myText);
                mediaPlayer.SetSource(stream, stream.ContentType);
                mediaPlayer.Play();
            }
            IsReading = false;
        }

        public void DispatcherTimerSetup()
        {
            _pomodoroDuration = Int32.Parse(PomodoroDuration); // minutes
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, Int32.Parse(PomodoroReminderInterval), 0);
            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            var startTimerSpeech = string.Format($"You have {_pomodoroDuration} minutes remaining");
            TimeRemaining.Text = $"{_pomodoroDuration} minutes remaining";
            timesTicked = Int32.Parse(PomodoroReminderInterval);
            ReadText(startTimerSpeech);
            dispatcherTimer.Start();
        }

        void DispatcherTimer_Tick(object sender, object e)
        {
            var time = DateTimeOffset.Now;
            var span = time - lastTime;
            lastTime = time;
            var ticksRemaining = _pomodoroDuration - timesTicked;
            string remainingFormat = string.Empty;
            switch (ticksRemaining)
            {
                case 1:
                    remainingFormat = "You have 1 minute remaining";
                    break;
                case 0:
                    remainingFormat = "Your session has finished";
                    break;
                default:
                    remainingFormat = $"You have {ticksRemaining} minutes remaining";
                    break;

            }
            ReadText(remainingFormat);
            TimeRemaining.Text = remainingFormat;
            timesTicked = timesTicked + Int32.Parse(PomodoroReminderInterval);
            if (timesTicked > _pomodoroDuration)
            {
                stopTime = time;
                dispatcherTimer.Stop();
                span = stopTime - startTime;
            }
        }

        private void Duration_TextChanged(object sender, TextChangedEventArgs e)
        {
            PomodoroDuration = Duration.Text;
        }
        private void ReminderInterval_TextChanged(object sender, TextChangedEventArgs e)
        {
            PomodoroReminderInterval = ReminderInterval.Text;
        }
    }
}