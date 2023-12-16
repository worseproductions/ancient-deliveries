using Godot;
using System;

public partial class MenuHandler : SceneSwitchLogic
{
	[Export] public String startScenePath;
	[Export] public String otherScenePath;
	[Export] public Button startButton;
	[Export] public Button otherButton;
	public override void _Ready()
	{
		startButton.Pressed += () =>
		{
			SceneSwitchEventHandler(GetTree(), startScenePath);
		};
		otherButton.Pressed += () =>
		{
			SceneSwitchEventHandler(GetTree(), otherScenePath);
		};
	}


	

}
