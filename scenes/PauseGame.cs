using Godot;
using System;

public partial class PauseGame : VBoxContainer
{

	[Export]
    private Button playButton { get; set; } 

	[Export]
    private Button exitButton { get; set; }
	 
	[Export]
    private Button settingsButton { get; set; }
	

	bool isPaused = true;
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			if (!isPaused)
			{
				Engine.TimeScale = 0;
				isPaused = true;
				Visible = true;	
			}
			else ResumeGame();
		}
	}

	void ResumeGame()
	{
		Engine.TimeScale = 1;
		isPaused = false;
		Visible = false;
	}
}
