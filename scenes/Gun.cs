using Godot;
using System;

public partial class Gun : Node2D
{
	[Export]
    private PackedScene BulletScene { get; set; } // Reference to the Enemy scene
	[Export]
    private Node2D Muzzle { get; set; }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());
		RotationDegrees = Mathf.Wrap(RotationDegrees, 0, 360);
		if (RotationDegrees > 90 && RotationDegrees < 270)
        {
            Scale = new Vector2(Scale.X, -1);
        }
        else
        {
            Scale = new Vector2(Scale.X, 1);
        }

		// Fire bullet when "fire" action is just pressed
        if (Input.IsActionJustPressed("fire") && BulletScene != null)
        {
            Node2D bulletInstance = BulletScene.Instantiate<Node2D>(); // Instantiate the bullet
            GetTree().Root.AddChild(bulletInstance); // Add bullet to the scene
            bulletInstance.GlobalPosition = Muzzle.GlobalPosition; // Set bullet spawn position
            bulletInstance.Rotation = Rotation; // Set bullet rotation
        }

	}
}
