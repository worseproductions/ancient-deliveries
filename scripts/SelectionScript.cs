using Godot;
using System;
using AncientDeliveries.scripts;

public partial class SelectionScript : Node
{
	[Export] public PackedScene sceneToInstantiate;
	[Export] public Node node;
	private String[] button_functions = {
		nameof(Action1),
		nameof(Action2),
		nameof(Action3),
	};

	// [Export] public PackedScene sceneToInstantiateTo;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Jobs: "+String.Join(", ",JobGenerator.GetJobs(6,5)));
		sceneToInstantiate = ResourceLoader.Load<PackedScene>("res://scenes/blueprints/selectable-option.tscn");
		if (sceneToInstantiate == null)
		{
			GD.Print("Failed to load the scene.");
			return;
		}
		
		foreach (var button_fn in button_functions)
		{
			var instantiatedScene = sceneToInstantiate.Instantiate();
			if (instantiatedScene is Control newNode)
			{
				Button myButton = newNode.GetNode<Button>("Button");
				node.AddChild(newNode);
				myButton.Connect("pressed", new Callable(this, button_fn));
			}
			else
			{
				GD.Print("Instantiated scene is not of type Control.");
			}
		}
	}
	
	private void Action1()
	{
		GD.Print("Action 1");
		// Your code logic when the button is pressed
	}
	private void Action2()
	{
		GD.Print("Action 2");
		// Your code logic when the button is pressed
	}
	private void Action3()
	{
		GD.Print("Action 3");
		// Your code logic when the button is pressed
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
