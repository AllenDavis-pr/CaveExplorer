using Godot;
using System;

public partial class EnemyGenerator : Node
{
    [Export]
    private PackedScene enemyScene { get; set; } // Reference to the Enemy scene

    [Export]
    private CaveGenerator caveGenerator { get; set; } // Reference to the cave generator

    [Export]
    private Player player { get; set; } // Reference to the player

    [Export]
    private int enemyCount = 4; // Number of enemies to spawn

    public override void _Ready()
    {
        Random random = new Random();

        for (int i = 0; i < enemyCount; i++)
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
            Enemy newEnemy = (Enemy)enemyScene.Instantiate();
			newEnemy.SetCaveGenerator(caveGenerator);
			newEnemy.SetPlayerRef(player);
            AddChild(newEnemy); // Add to the scene tree

            // Set enemy position
            Vector2I enemyPosition = new Vector2I(x2, y2);
            newEnemy.SetGridPosition(enemyPosition);
        }
    }
}
