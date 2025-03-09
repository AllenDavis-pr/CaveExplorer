using Godot;
using System;

public partial class MainMenu : Control
{
	[Export]
    private Button playButton { get; set; } // Reference to the play button 
	
	[Export]
    private Button exitButton { get; set; } // Reference to the exit butotn
	[Export]
    private Button settingsButton { get; set; } // Reference to the Enemy scene


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playButton.ButtonUp += PlayButtonUp;
		exitButton.ButtonUp += ExitButtonUp;
		settingsButton.ButtonUp += SettingsButtonUp;

	}

	private void PlayButtonUp()
	{	
		GetTree().ChangeSceneToFile("scenes/Main.tscn");
	}

	private void ExitButtonUp()
	{
		GetTree().Quit();
	}
	
	private void SettingsButtonUp()
	{
		GetTree().ChangeSceneToFile("scenes/Settings.tscn");
	}

}
