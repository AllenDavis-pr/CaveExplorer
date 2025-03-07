using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	[Export]
	private CaveGenerator caveGenerator { get; set; }


	[Export]
	private int speed = 40;

	private Vector2I gridPosition;

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

    public void BulletCollidedWithTilemap(Vector2 position, Vector2 normal)
    {
		// bool xValid = Mathf.Abs(normal.X) == 1 || normal.X == 0;
		// bool yValid = Mathf.Abs(normal.Y) == 1 || normal.Y == 0;

		if (true)
		{
			Vector2I tileGridPosition = (Vector2I)(position / CaveGenerator.CELL_SIZE);
			caveGenerator.DestroyTile(tileGridPosition - (Vector2I)normal);
		}
    } 
}
