using Godot;
using System;

public partial class Bullet : Node2D
{
    private const int Speed = 1000; // Bullet speed

    public override void _Process(double delta)
    {
        Position += Transform.X * (float)(Speed * delta); // Move the bullet forward
    }

    private void OnScreenExited()
    {
        QueueFree(); // Destroy the bullet when it exits the screen
    }

    public override void _Ready()
    {
        // Manually connect the signal from VisibleOnScreenNotifier2D
        VisibleOnScreenNotifier2D notifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        if (notifier != null)
        {
            notifier.ScreenExited += OnScreenExited;
        }
    }
}
