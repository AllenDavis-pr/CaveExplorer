using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	[Export]
	private CaveGenerator caveGenerator { get; set; }

		
	private AnimatedSprite2D sprite { get; set; }

	[Export]
	private int speed = 40;

	private Vector2I gridPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		Velocity = direction * speed;
		MoveAndSlide();
		gridPosition = (Vector2I)(Position / CaveGenerator.CELL_SIZE);

    }

	public override void _Process(double delta)
    {
		if (Velocity == Vector2.Zero)
		{
			sprite.Play("idle_bounce");
		}
		else
		{
			sprite.Play("walk_bounce");
		}


		if (GetGlobalMousePosition().X < GlobalPosition.X)
			sprite.FlipH = true;
		else
			sprite.FlipH = false;
    }

    public void UpdatePosition()
	{
		Position = new Vector2(gridPosition.X * CaveGenerator.CELL_SIZE, gridPosition.Y * CaveGenerator.CELL_SIZE);
	}

	public void SetGridPosition(Vector2I pos)
	{
		gridPosition = pos;
		UpdatePosition();
	}

	public Vector2I GetGridPosition()
	{
		return gridPosition;
	}
}
