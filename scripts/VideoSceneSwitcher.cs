using Godot;
using System;

public partial class VideoSceneSwitcher : SceneSwitchLogic
{
	[Export] public VideoStreamPlayer vsp;
	// Called when the node enters the scene tree for the first time.
	[Export] public String scenePath;
	public override void _Ready()
	{
		vsp.Finished += () =>
		{
			SceneSwitchEventHandler(GetTree(), scenePath);
		};
	}
}
