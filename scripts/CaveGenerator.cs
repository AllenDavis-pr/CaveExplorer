using Godot;
using System;
using System.Collections.Generic;

public partial class CaveGenerator : Node
{
	public const int WIDTH = 160;
	public const int HEIGHT = 120; 
	public const int CELL_SIZE = 35;

	 // 2D list to represent the cave grid, this is public so other scripts can access it
    public List<List<bool>> Grid = new List<List<bool>>();


	private Random random = new Random();
	[Export] public Color WallColor { get; set; } = new Color("#3B2F2F"); // Dark Brown
    [Export] public Color GroundColor { get; set; } = new Color("#A4A4A4"); // Light Gray

	private void InitialiseGrid()
	{
		// Create a 2D list filled with random walls and floors
        for (int x = 0; x < WIDTH; x++)
        {
            Grid.Add(new List<bool>());
			for (int y = 0; y < HEIGHT; y++)
			{
				Grid[x].Add(random.NextDouble() < 0.45);
			}
        }
	}

	private int CountNeighbouringWalls(int x, int y)
    {
        // Count the number of walls in the 8 neighboring cells
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue; // Skip the center cell

                int nx = x + i;
                int ny = y + j;

                // Check if the neighboring cell is out of bounds
                if (nx < 0 || nx >= WIDTH || ny < 0 || ny >= HEIGHT)
                {
                    count++; // Count out-of-bounds as walls
                }
                else if (Grid[nx][ny])
                {
                    count++; // Count walls
                }
            }
        }

        return count;
    }

	private void GenerateCave()
    {
        // Apply cellular automata rules to create cave-like structures
        for (int i = 0; i < 4; i++) // Number of iterations
        {
            List<List<bool>> newGrid = new List<List<bool>>();

            // Create a deep copy of the grid
            for (int x = 0; x < WIDTH; x++)
            {
                newGrid.Add(new List<bool>());
                for (int y = 0; y < HEIGHT; y++)
                {
                    int wallCount = CountNeighbouringWalls(x, y);
                    bool isWall = Grid[x][y];

                    if (isWall)
                    {
                        // Turn into empty space if not enough surrounding walls
                        newGrid[x].Add(wallCount > 3);
                    }
                    else
                    {
                        // Turn into a wall if too many surrounding walls
                       newGrid[x].Add(wallCount > 4);
                    }
                }
            }

            Grid = newGrid; // Update the grid for the next iteration
        }
    }

	private void DrawCave()
    {
        // Visualize the cave using ColorRect nodes
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                bool isWall = Grid[x][y];
                ColorRect cell = new ColorRect();
                cell.Size = new Vector2(CELL_SIZE, CELL_SIZE);
                cell.Position = new Vector2(x * CELL_SIZE, y * CELL_SIZE);
                cell.Color = isWall ? WallColor : GroundColor;

                AddChild(cell); // Add the cell to the scene tree

                if (isWall)
                {
                    StaticBody2D wallBody = new StaticBody2D();
                    CollisionShape2D collisionShape = new CollisionShape2D();
                    RectangleShape2D rectangleShape = new RectangleShape2D();

                    rectangleShape.Size = new Vector2(CELL_SIZE, CELL_SIZE);
                    collisionShape.Shape = rectangleShape;
                    collisionShape.Position = new Vector2(CELL_SIZE / 2, CELL_SIZE / 2);

                    wallBody.AddChild(collisionShape);
                    wallBody.Position = new Vector2(x * CELL_SIZE, y * CELL_SIZE);

                    AddChild(wallBody); // Add the collision body to the scene tree
                }

            }
        }
    }

    // Method to check if a specific cell is a wall
	public bool IsWall(int x, int y)
	{
        return Grid[x][y];
	}



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitialiseGrid();
        GenerateCave();
        DrawCave();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
