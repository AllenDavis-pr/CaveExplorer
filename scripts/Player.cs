using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	[Export]
	private CaveGenerator caveGenerator { get; set; }

		
	[Export]
	private TreasureGenerator treasureGenerator { get; set; }

	[Export]
	private int speed = 40;
	[Export]
	private int sprintSpeed = 80; // Sprinting speed


	private Vector2I gridPosition;

	private Sprite2D sprite;
	public int treasuresFound;

    public override void _Ready()
    {
        base._Ready();

		sprite = GetNode<Sprite2D>("Body");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		bool isSprinting = Input.IsActionPressed("sprint");
        int currentSpeed = isSprinting ? sprintSpeed : speed;
        Velocity = direction * currentSpeed;

		MoveAndSlide();
		gridPosition = (Vector2I)(Position / CaveGenerator.CELL_SIZE);

    }

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (GetGlobalMousePosition().X < GlobalPosition.X)
            sprite.Scale = new Vector2(-1, 1);
		else
            sprite.Scale = new Vector2(1, 1);

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
		Vector2I tileGridPosition = (Vector2I)(position / CaveGenerator.CELL_SIZE);
		caveGenerator.DestroyTile(tileGridPosition - (Vector2I)normal);
    } 

	public void TreasureFound()
	{
		treasuresFound++;
		treasureGenerator.AddToCounter();
	}
}
