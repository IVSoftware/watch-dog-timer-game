*Watchdog Timer Demo*

![screenshot](https://github.com/IVSoftware/watch-dog-timer-game/blob/master/watch-dog-timer-game/Screenshot/screenshot.png)

    class WatchDogTimer
    {
        int _wdtCount = 0;
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);
        public void ThrowBone(Action action)
        {
            _wdtCount++;
            var capturedCount = _wdtCount;
            Task
                .Delay(Interval)
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    // If the 'captured' localCount has not changed after awaiting the Interval, 
                    // it indicates that no new 'bones' have been thrown during that interval.        
                    if (capturedCount.Equals(_wdtCount))
                    {
                        action();
                    }
                });
        }
    }