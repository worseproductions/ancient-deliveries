using Godot;
using System;

public partial class LinkCaller : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += () =>
		{
			OS.ShellOpen("https://worseproductions.itch.io/");
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
