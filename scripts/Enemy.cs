using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D
{
    [Export]
    private CaveGenerator caveGenerator { get; set; }

    
    [Export]
    private Player player { get; set; }

    private Pathfinder pathfinder;
    private List<Vector2I> gridPath = new List<Vector2I>();
    private List<Vector2> worldPath = new List<Vector2>();


    private int currentPathIndex = 0;
    private float speed = 150.0f; // Pixels per second
    private Vector2I gridPosition;

    public override void _Ready()
    {
        Timer myTimer = GetNode<Timer>("Timer");
        myTimer.Timeout += () => Timeout();

        // Initialize the pathfinder with the cave grid
        pathfinder = new Pathfinder(caveGenerator.Grid, CaveGenerator.WIDTH, CaveGenerator.HEIGHT, CaveGenerator.CELL_SIZE);
    }

    public override void _Draw()
    {
        if (worldPath == null || worldPath.Count < 2)
            return;

        int halfIndex = worldPath.Count / 2; // Halfway point

        for (int i = 0; i < gridPath.Count - 1; i++)
        {
            DrawLine(worldPath[i] - Position, worldPath[i + 1] - Position, Colors.Black, 2.0f);
        }

         // Draw a square at each waypoint
        float squareSize = 8.0f; // Adjust size as needed
        Vector2 halfSize = new Vector2(squareSize / 2, squareSize / 2);

        foreach (var point in worldPath)
        {
            Rect2 rect = new Rect2(point - Position - halfSize, new Vector2(squareSize, squareSize));
            DrawRect(rect, Colors.Red, filled: true);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        // MoveAlongPath(delta);
		gridPosition = (Vector2I)(Position / CaveGenerator.CELL_SIZE);
    }

    private void MoveAlongPath(double delta)
    {
        if (gridPath == null || gridPath.Count == 0 || currentPathIndex >= gridPath.Count)
            return;

        Vector2 targetPosition = worldPath[currentPathIndex];
        Vector2 direction = (targetPosition - Position).Normalized();
        Vector2 velocity = direction * speed;
        if (Position.DistanceTo(targetPosition) < 5.0f) 
        {
            Position = targetPosition;

            if (currentPathIndex >= worldPath.Count)
            {
                velocity = Vector2.Zero; // Stop when the path ends
                return;
            }
        }

        Velocity = velocity;
        MoveAndSlide();

        QueueRedraw(); // Redraw to visualize the updated path

    }

    public void Timeout()
    {
        gridPath = pathfinder.FindPath(gridPosition, player.GetGridPosition());

        worldPath.Clear();
        foreach (var point in gridPath)
        {
            worldPath.Add(new Vector2((point.X * CaveGenerator.CELL_SIZE) + CaveGenerator.CELL_SIZE / 2, (point.Y * CaveGenerator.CELL_SIZE ) + CaveGenerator.CELL_SIZE / 2));
        }

        QueueRedraw(); // Redraw to visualize the updated path
    }

    public void SetGridPosition(Vector2I pos)
    {
        gridPosition = pos;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Position = new Vector2(gridPosition.X * CaveGenerator.CELL_SIZE, gridPosition.Y * CaveGenerator.CELL_SIZE);
    }
}
