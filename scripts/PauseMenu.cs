using Godot;

namespace AncientDeliveries.scripts;

public partial class PauseMenu : Fader {
	[Signal]
	public delegate void RestartEventHandler();


	private PackedScene _menuScene;
	private Button _pauseContinueButton;
	private Button _pauseRestartButton;
	private Button _pauseQuitButton;
	
	
	public override void _Ready() {
		base._Ready();
		_menuScene = ResourceLoader.Load<PackedScene>("res://scenes/mainmenu.tscn");
		_pauseContinueButton = GetNode<Button>("%ContinueButton");
		_pauseContinueButton.Pressed += FadeOut;
		_pauseContinueButton.GrabFocus();
		
		_pauseRestartButton = GetNode<Button>("%RestartButton2");
		_pauseRestartButton.Pressed += () => {
			EmitSignal(SignalName.Restart);
		};
		
		_pauseQuitButton = GetNode<Button>("%QuitButton");
		_pauseQuitButton.Pressed += () => {
			GetTree().Paused = false;
			GetTree().ChangeSceneToPacked(_menuScene);
		};
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (!@event.IsActionPressed("pause") || !Visible) return;
		FadeOut();
	}
}