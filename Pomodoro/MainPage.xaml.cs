﻿using System;
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

        public MainPage()
        {
            this.InitializeComponent();
            RunWallclock();
        }

        private void ResetDefaults()
        {
            PomodoroDuration = Duration.Text = "20";
            RequestedIntervalInMinutes = ReminderInterval.Text = "5";
            ElementSoundPlayer.State = ElementSoundPlayerState.On;
            dispatcherTimer = null;
            wallClock = null;
        }

        private void DurationSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            HandleDurationSliderChange(sender);
        }

        private void IntervalSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            HandleIntervalSliderChange(sender);
        }

        private void HandleIntervalSliderChange(object sender)
        {
            if (!pageLoaded) return;
            if (sender is Slider slider)
            {
                // the requested interval must not be greater than the requested session duration...
                var interval = slider.Value > Int32.Parse(Duration.Text) ? Int32.Parse(Duration.Text) : slider.Value;
                ReminderInterval.Text = interval.ToString();
                RequestedIntervalInMinutes = ReminderInterval.Text;
            }
        }

        private void HandleDurationSliderChange(object sender)
        {
            if (sender is Slider slider)
            {
                Duration.Text = slider.Value.ToString();
                PomodoroDuration = Duration.Text;
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer = null;
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
            _requestedDurationInMinutes = Int32.Parse(PomodoroDuration); // minutes
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, Int32.Parse(RequestedIntervalInMinutes), 0);



            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            var startTimerSpeech = string.Format($"You have {_requestedDurationInMinutes} minutes remaining");
            TimeRemaining.Text = $"{_requestedDurationInMinutes}";
            elapsedMinutes = Int32.Parse(RequestedIntervalInMinutes);
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

        void DispatcherTimer_Tick(object sender, object e)
        {
            var time = DateTimeOffset.Now;
            var span = time - lastTime;
            lastTime = time;
            var ticksRemaining = _requestedDurationInMinutes - elapsedMinutes;
            string remainingSpokenFormat = string.Empty;
            var tempMinutesLeft = ticksRemaining <= 0 ? 0 : ticksRemaining;
            switch (tempMinutesLeft)
            {
                case 1:
                    remainingSpokenFormat = "You have 1 minute remaining";
                    break;
                case 0:
                    remainingSpokenFormat = "Your session has finished";
                    break;
                default:
                    remainingSpokenFormat = $"You have {ticksRemaining} minutes remaining";
                    break;

            }
            ReadText(remainingSpokenFormat);
            TimeRemaining.Text = tempMinutesLeft.ToString();
            elapsedMinutes = elapsedMinutes + Int32.Parse(RequestedIntervalInMinutes);
            if (elapsedMinutes > _requestedDurationInMinutes)
            {
                stopTime = time;
                dispatcherTimer.Stop();
                dispatcherTimer = null;
                span = stopTime - startTime;
                elapsedMinutes = 0;
            }
        }

        private void Duration_TextChanged(object sender, TextChangedEventArgs e)
        {
            PomodoroDuration = Duration.Text;
            DurationSlider.Value = Int32.Parse(PomodoroDuration);
        }
        private void ReminderInterval_TextChanged(object sender, TextChangedEventArgs e)
        {
            RequestedIntervalInMinutes = ReminderInterval.Text;
            IntervalSlider.Value = int.Parse(RequestedIntervalInMinutes);
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
            ResetDefaults();
        }

    }
}