using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody2D
{
    [Export]
    private CaveGenerator caveGenerator { get; set; }

    [Export]
    private Node2D gun { get; set; }

    [Export]
    private Timer fireTimer { get; set; }

     [Export]
    private Node2D muzzle { get; set; }

    
	[Export]
    private PackedScene BulletScene { get; set; } // Reference to the bullet scene

    
    private Player player { get; set; }

    private Pathfinder pathfinder;
    private List<Vector2I> gridPath = new List<Vector2I>();
    private List<Vector2> worldPath = new List<Vector2>();
    [Export] private float maxChaseDistance = 100.0f; // Maximum distance to chase the player


    private int currentPathIndex = 0;
    private float speed = 150.0f; // Pixels per second
    private Vector2I gridPosition;

    private bool playerWithinRange;

    public override void _Ready()
    {
        Timer pathTimeout = GetNode<Timer>("PathTimer");
        Area2D bulletDetector = GetNode<Area2D>("BulletDetector");

        bulletDetector.BodyEntered += BulletDetectorBodyEntered;
        pathTimeout.Timeout += () => PathTimeout();
        fireTimer.Timeout += () => FireTimeout();

        // Initialise the pathfinder with the cave grid
        pathfinder = new Pathfinder(caveGenerator.Grid, CaveGenerator.WIDTH, CaveGenerator.HEIGHT, CaveGenerator.CELL_SIZE);
    }

    public override void _Draw()
    {
        if (worldPath == null || worldPath.Count < 2)
            return;

        int halfIndex = worldPath.Count / 2; // Halfway point

        for (int i = 0; i < gridPath.Count - 1; i++)
        {
            // Subtract position vector to draw from the origin
            DrawLine(worldPath[i] - Position, worldPath[i + 1] - Position, Colors.Black, 2.0f);
        }

        // Draw a square at each waypoint
        float squareSize = 8.0f; 
        Vector2 halfSize = new Vector2(squareSize / 2, squareSize / 2);

        foreach (var point in worldPath)
        {
            // Subtract position vector to draw from the origin
            Rect2 rect = new Rect2(point - Position - halfSize, new Vector2(squareSize, squareSize));
            DrawRect(rect, Colors.Red, filled: true);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (player != null)
        {
            if (Position.DistanceTo(player.Position) >= maxChaseDistance)
            {
                // MoveAlongPath(delta);
                // Player is not within range
                fireTimer.Stop();
            }
            else
            {
                // Player is within range
                if (fireTimer.IsStopped())
                    fireTimer.Start();
                playerWithinRange = true;
            }
        }

		gridPosition = (Vector2I)(Position / CaveGenerator.CELL_SIZE);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        AimGunAtPlayer();
    }

    private void MoveAlongPath(double delta)
    {
        if (gridPath == null || gridPath.Count == 0 )
            return;

        Vector2 targetPosition = worldPath[0];
        Vector2 direction = (targetPosition - Position).Normalized();
        Vector2 velocity = direction * speed;
        if (Position.DistanceTo(targetPosition) < 5.0f) 
        {
            Position = targetPosition;
        }

        Velocity = velocity;
        MoveAndSlide();

        QueueRedraw(); // Redraw to visualize the updated path
    }

    public void PathTimeout()
    {
        gridPath = pathfinder.FindPath(gridPosition, player.GetGridPosition());

        worldPath.Clear();
        foreach (var point in gridPath)
        {
            worldPath.Add(new Vector2((point.X * CaveGenerator.CELL_SIZE) + CaveGenerator.CELL_SIZE / 2, 
                (point.Y * CaveGenerator.CELL_SIZE ) + CaveGenerator.CELL_SIZE / 2));
        }

        QueueRedraw(); // Redraw to visualize the updated path
    }

    public void AimGunAtPlayer()
    {
		gun.LookAt(player.GlobalPosition);
		gun.RotationDegrees = Mathf.Wrap(gun.RotationDegrees, 0, 360);

		if (gun.RotationDegrees > 90 && gun.RotationDegrees < 270)
            gun.Scale = new Vector2(1, -1);
        else
            gun.Scale = new Vector2(1, 1);    
    }

    public void FireTimeout()
    {
        Bullet bulletInstance = BulletScene.Instantiate<Bullet>(); // Instantiate the bullet
        bulletInstance.GlobalPosition = muzzle.GlobalPosition; //    Set bullet spawn position
        bulletInstance.Rotation = gun.Rotation; // Set bullet rotation
        GetTree().Root.AddChild(bulletInstance); // Add bullet to the scene
    }

    public void BulletDetectorBodyEntered(Node2D body)
    {
        if (body is Bullet)
        {
            body.QueueFree();
            QueueFree();
        }
    }

    public void SetGridPosition(Vector2I pos)
    {
        gridPosition = pos;
        UpdatePosition();
    }

    public void SetCaveGenerator(CaveGenerator gen)
    {
        caveGenerator = gen;
    }

    public void UpdatePosition()
    {
        Position = new Vector2(gridPosition.X * CaveGenerator.CELL_SIZE, gridPosition.Y * CaveGenerator.CELL_SIZE);
    }

    public void SetPlayerRef(Player aplayer)
    {
        player = aplayer;
    }
}
