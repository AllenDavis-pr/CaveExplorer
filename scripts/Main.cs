using Godot;
using System;

public partial class Main : Node2D
{
	[Export]
	private CaveGenerator caveGenerator { get; set; }
	[Export]
	private Player player { get; set; }

	[Export]
	private Enemy enemy { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		Random random = new Random();
		int x, y;

		// Keep trying to find a valid spawn point
		do
		{
			// Pick a random cell in the grid
			x = random.Next(0, CaveGenerator.WIDTH);
			y = random.Next(0, CaveGenerator.HEIGHT);
		}
		while (caveGenerator.IsWall(x, y)); // Repeat if the cell is a wall

		Vector2I playerPosition = new Vector2I(x, y);
		player.SetGridPosition(playerPosition);

		int x2, y2;

		// Keep trying to find a valid spawn point
		do
		{
			// Pick a random cell in the grid
			x2 = random.Next(0, CaveGenerator.WIDTH);
			y2 = random.Next(0, CaveGenerator.HEIGHT);
		}
		while (caveGenerator.IsWall(x, y)); // Repeat if the cell is a wall

		Vector2I enemyPosition = new Vector2I(x, y);
		enemy.SetGridPosition(enemyPosition);
		GD.Print("enemy grid position set");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
