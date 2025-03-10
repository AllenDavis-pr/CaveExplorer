using Godot;
using System;

public partial class Settings : Control
{
	[Export]
    private Button mainMenuButton { get; set; } // Reference to the play button 
	
	
	[Export]
    private CheckButton checkButton { get; set; } // Reference to the play button 
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainMenuButton.ButtonUp += MainMenuButtonUp;
		checkButton.Toggled += CheckButtonToggled;

	}

	private void MainMenuButtonUp()
	{
		GetTree().ChangeSceneToFile("scenes/MainMenu.tscn");
	}
	
	private void CheckButtonToggled(bool isToggled)
	{
		if (isToggled)
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		else
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);

	}
	

}
