using Godot;
using System;

public partial class PlayerInteract : Area2D
{
    private Node2D targetObject;
    private Player player;


    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        player = GetParent<Player>();
    }

    private void OnAreaEntered(Node2D body)
    {
		targetObject = body.GetParent<Node2D>();
        targetObject.QueueFree();
        player.TreasureFound();
    }
}
