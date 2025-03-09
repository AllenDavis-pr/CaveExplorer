using Godot;
using System;

public partial class TimerLabel : Label
{
    [Export] public float StartTime = 599.0f; // Exported variable for timer duration (default 9:59)
    private Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.WaitTime = StartTime;
        timer.OneShot = true;
        timer.Start();
        UpdateTimerText();
        
        timer.Timeout += OnTimerTimeout;
    }

    public override void _Process(double delta)
    {
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = (int)(timer.TimeLeft / 60);
        int seconds = (int)(timer.TimeLeft % 60);
        Text = string.Format("{0}:{1:D2}", minutes, seconds); // Format as MM:SS
    }

    private void OnTimerTimeout()
    {
        Text = "0:00";
    }
}
