using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
    private const int Speed = 1000; // Bullet speed

    [Signal]
    public delegate void CollidedWithTileEventHandler(Vector2 position, Vector2 normal);

    public override void _Ready()
    {
        // Set the initial velocity based on the bullet's direction
        Velocity = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * Speed;

        // Manually connect the signal from VisibleOnScreenNotifier2D
        VisibleOnScreenNotifier2D notifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        if (notifier != null)
        {
            notifier.ScreenExited += OnScreenExited;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            if (collision.GetCollider() is TileMapLayer)
            {

                TileMapLayer tilelayer = collision.GetCollider() as TileMapLayer;
                EmitSignal(SignalName.CollidedWithTile, Position, collision.GetNormal());
                QueueFree();
                break;
            }
        }
    }

    private void OnScreenExited()
    {
        QueueFree(); // Destroy the bullet when it exits the screen
    }
}
