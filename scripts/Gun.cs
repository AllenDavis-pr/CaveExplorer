using Godot;
using System;

public partial class Gun : Node2D
{
	[Export]
    private PackedScene BulletScene { get; set; } // Reference to the bullet scene
	[Export]
    private Node2D Muzzle { get; set; }

 	[Export]
    private Player player { get; set; }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());
		RotationDegrees = Mathf.Wrap(RotationDegrees, 0, 360);

    
		if (RotationDegrees > 90 && RotationDegrees < 270)
            Scale = new Vector2(1, -1);
        else
            Scale = new Vector2(1, 1);

		// Fire bullet when "fire" action is just pressed
        if (Input.IsActionJustPressed("fire") && BulletScene != null)
        {
            Bullet bulletInstance = BulletScene.Instantiate<Bullet>(); // Instantiate the bullet
            bulletInstance.GlobalPosition = Muzzle.GlobalPosition; //    Set bullet spawn position
            bulletInstance.Rotation = Rotation; // Set bullet rotation
            GetTree().Root.AddChild(bulletInstance); // Add bullet to the scene

            bulletInstance.CollidedWithTile += player.BulletCollidedWithTilemap;
        }
        
	}
}
