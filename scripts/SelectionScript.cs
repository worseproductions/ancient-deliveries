using Godot;
using System;

public partial class SelectionScript : Node
{
	[Export] public PackedScene sceneToInstantiate;
	[Export] public Node node;
	private Timer myTimer;
	[Export] public int elementCount = 3;
	private String[] button_functions = {
		nameof(Action1),
		nameof(Action2),
		nameof(Action3),
	};
	[Export] public float timerDuration = 0.5f; // Set the duration in seconds

	// [Export] public PackedScene sceneToInstantiateTo;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load the scene file
		sceneToInstantiate = ResourceLoader.Load<PackedScene>("res://scenes/blueprints/selectable-option.tscn");
		// Get reference to the Timer node
		myTimer = GetNode<Timer>("Timer");

		// Connect the "timeout" signal of the Timer to a method in this script
		myTimer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));

		// Set the timer's wait time (in seconds)
		myTimer.WaitTime = timerDuration;
		myTimer.OneShot = true;

		// Start the timer
		myTimer.Start();
	}
	
	private void OnTimerTimeout()
	{
		// Check if the scene is loaded properly
		if (sceneToInstantiate == null)
		{
			GD.Print("Failed to load the scene.");
			return;
		}
		
		// Instantiate the scene onto the control
		var instantiatedScene = sceneToInstantiate.Instantiate();
		if (instantiatedScene is Control newNode)
		{
			Button myButton = newNode.GetNode<Button>("Button");
			node.AddChild(newNode);
			myButton.Connect("pressed", new Callable(this, button_functions[elementCount-1]));
		}
		else
		{
			GD.Print("Instantiated scene is not of type Control.");
		}
		// GD.Print("Timer expired!");
		// Perform actions you want when the timer expires
		// Stop the timer, reset values, etc.
		elementCount -= 1;
		if(elementCount > 0) {
			// Restart the timer to make it run continuously
			myTimer.Start();
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
