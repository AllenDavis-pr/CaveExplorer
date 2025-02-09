using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	[Export]
	private CaveGenerator caveGenerator { get; set; }

	[Export]
	private int speed = 40;

	private Vector2I gridPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		Velocity = direction * speed;
		MoveAndSlide();
		gridPosition = (Vector2I)(Position / CaveGenerator.CELL_SIZE);
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
