using Godot;
using System;
using System.Collections.Generic;

public class Pathfinder
{

    private List<List<bool>> grid; // Reference to your cave grid
    private int width;
    private int height;
    private int cellSize;

    private readonly Vector2I[] directions = new Vector2I[]
    {
        new Vector2I(0, -1), // Up
        new Vector2I(0, 1),  // Down
        new Vector2I(-1, 0), // Left
        new Vector2I(1, 0)   // Right
    };

    public Pathfinder(List<List<bool>> grid, int width, int height, int cellSize)
    {
        this.grid = grid;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
    }

    // Djikstra implementation
    public List<Vector2I> FindPath(Vector2I startPosition, Vector2I endPosition)
    {
        var priorityQueue = new PriorityQueue<Vector2I, int>(); 
        var cameFrom = new Dictionary<Vector2I, Vector2I>();
        var costSoFar = new Dictionary<Vector2I, int>();

        priorityQueue.Enqueue(startPosition, 0);
        cameFrom[startPosition] = startPosition;
        costSoFar[startPosition] = 0;   

        while (priorityQueue.Count > 0)
        {
            Vector2I current = priorityQueue.Dequeue();

            // If we reached the target, reconstruct the path
            if (current == endPosition)
                return ReconstructPath(cameFrom, startPosition, endPosition);

            foreach (var direction in directions)
            {
                Vector2I neighbor = current + direction;
                // out of bounds
                if (neighbor.X < 0 || neighbor.X >= width || neighbor.Y < 0 || neighbor.Y >= height)
                    continue;


                if (grid[neighbor.X][neighbor.Y]) 
                    continue; // Skip if  wall
                

                int newCost = costSoFar[current] + 1; // Uniform cost (each step = 1)
                
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    int priority = newCost; // Dijkstra doesn't use heuristics
                    priorityQueue.Enqueue(neighbor, priority);
                    cameFrom[neighbor] = current;
                }
            }
        }


        return new List<Vector2I>();
    }

    private List<Vector2I> ReconstructPath(Dictionary<Vector2I, Vector2I> cameFrom, 
        Vector2I start, Vector2I end)
    {
        var path = new List<Vector2I>();
        Vector2I current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        
        path.Reverse();
        return path;
    }
}