using Godot;
using System;

public partial class TreasureGenerator : Node
{
    [Export]
    private PackedScene treasureScene { get; set; } // Reference to the Enemy scene

    [Export]
    private CaveGenerator caveGenerator { get; set; } // Reference to the cave generator

    [Export]
    private int treasureCount = 4; // Number of treasures to spawn

    private int treasuresFound = 0;

    [Export]
    private Label treasureCounterLabel;

    public override void _Ready()
    {

        UpdateTreasureLabelText();

        Random random = new Random();

        for (int i = 0; i < treasureCount; i++)
        {
            int x2, y2;

            // Keep trying to find a valid spawn point
            do
            {
                x2 = random.Next(0, CaveGenerator.WIDTH);
                y2 = random.Next(0, CaveGenerator.HEIGHT);
            }
            while (caveGenerator.IsWall(x2, y2)); // Ensure it’s not a wall

            // Instantiate the enemy
            Node2D newTreasure = (Node2D)treasureScene.Instantiate();
            AddChild(newTreasure); // Add to the scene tree

            // Set enemy position
            Vector2I treasurePosition = new Vector2I(x2 * CaveGenerator.CELL_SIZE, y2 * CaveGenerator.CELL_SIZE);
        	newTreasure.Position = treasurePosition;

        }
    }

    public void AddToCounter()
    {
        treasuresFound++;
        UpdateTreasureLabelText();
    }   

    private void UpdateTreasureLabelText()
    {
        if (treasureCounterLabel != null)
        {
            treasureCounterLabel.Text = $"Treasures: [{treasuresFound}/{treasureCount}]";
        }
    }
}
