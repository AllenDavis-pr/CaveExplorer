using Godot;
using System;

public partial class MainMenu : Control
{
	[Export]
    private Button playButton { get; set; } 

	[Export]
    private Button exitButton { get; set; }
	 
	[Export]
    private Button settingsButton { get; set; }


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
