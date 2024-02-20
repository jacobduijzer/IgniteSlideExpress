using System.Timers;

namespace IgniteSlideExpress.Core
{
    public class Timer : ITimer
    {
        public event EventHandler<EventArgs>? SheetTimeElapsed;
    
        private readonly System.Timers.Timer _timer = new();

        public void Start(double interval)
        {
            _timer.Interval = interval;
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Elapsed -= TimerOnElapsed;
        }

        private void TimerOnElapsed(object? sender, ElapsedEventArgs e) =>
            SheetTimeElapsed?.Invoke(this, EventArgs.Empty);
    }
}
