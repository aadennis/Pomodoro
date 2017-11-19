using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Media.SpeechSynthesis;
using Pomodoro.ViewModel;

namespace Pomodoro
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer dispatcherTimer;
        DispatcherTimer wallClock;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;
        DateTimeOffset stopTime;
        bool pageLoaded = false;
        int elapsedMinutes = 0;
        int _requestedDurationInMinutes;
        static bool IsReading = false;
        public string PomodoroDuration { get; private set; }
        public string RequestedIntervalInMinutes { get; private set; }

        public Utilities u = new Utilities();


        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
            RunWallclock();
        }

        private void ResetDefaults()
        {
            //PomodoroDuration = Duration.Text = DEFAULT_DURATION.ToString();
            //RequestedIntervalInMinutes = ReminderInterval.Text = DEFAULT_INTERVAL.ToString();
            ElementSoundPlayer.State = ElementSoundPlayerState.On;
            wallClock = null;
            ticksRemaining = 0;
            u.Interval = "0";
            elapsedMinutes = 0;
            dispatcherTimer.Start();
            u.ResetToRun();
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!u.CanStart()) return;
            DispatcherTimerSetup();
            ReadText(PomodoroDuration);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetDefaults();

        }
        public static async void ReadText(string myText)
        {
            if (IsReading) return;
            if (ElementSoundPlayer.State == ElementSoundPlayerState.Off) return;

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

        public void DispatcherTimerSetup() {
            _requestedDurationInMinutes = Int32.Parse(u.Duration); // minutes

            if (dispatcherTimer == null) {
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, Int32.Parse(u.Interval), 0);
            }

            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            var startTimerSpeech = string.Format($"You have {_requestedDurationInMinutes} minutes remaining");
            TimeRemaining.Text = $"{_requestedDurationInMinutes}";
            elapsedMinutes = Int32.Parse(u.Interval);
            ReadText(startTimerSpeech);
            dispatcherTimer.Start();
        }

        private void RunWallclock() {
            CurrentTime.Text = DateTime.Now.ToString("h:mm tt");
            wallClock = new DispatcherTimer();
            wallClock.Tick += WallClock_Tick;
            wallClock.Interval = new TimeSpan(0, 1, 0);
            wallClock.Start();
        }

        private void WallClock_Tick(object sender, object e) {
            CurrentTime.Text = DateTime.Now.ToString("h:mm tt");
        }

        int ticksRemaining;

        void DispatcherTimer_Tick(object sender, object e)
        {
            var time = DateTimeOffset.Now;
            var span = time - lastTime;
            lastTime = time;
            Int32.TryParse(u.Duration, out int x);
            ticksRemaining =  x - elapsedMinutes;
            string remainingSpokenFormat = string.Empty;
            var tempMinutesLeft = ticksRemaining <= 0 ? 0 : ticksRemaining;
            switch (tempMinutesLeft)
            {
                case 1:
                    remainingSpokenFormat = "You have 1 minute remaining";
                    break;
                case 0:
                    u.ResetToRun();
                    remainingSpokenFormat = "Your session has finished";
                    break;
                default:
                    remainingSpokenFormat = $"You have {ticksRemaining} minutes remaining";
                    break;

            }
            ReadText(remainingSpokenFormat);
            TimeRemaining.Text = tempMinutesLeft.ToString();
            elapsedMinutes = elapsedMinutes + Int32.Parse(u.Interval);
            if (elapsedMinutes > _requestedDurationInMinutes)
            {
                stopTime = time;
                dispatcherTimer.Stop();
                span = stopTime - startTime;
                elapsedMinutes = 0;
            }
        }


        private void ResetDuration(int temp) {
            u.Duration = u.DefaultDuration;
        }

        private void ResetInterval(int temp) {
            u.Interval = u.DefaultInterval;
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    ElementSoundPlayer.State = ElementSoundPlayerState.On;
                    ElementSoundPlayer.Volume = 1.0;
                }
                else
                {
                    ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                    ElementSoundPlayer.Volume = 0.0;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pageLoaded = true;
            u.Interval = u.DefaultInterval;
            u.Duration = u.DefaultDuration;
        }

       
    }
}