using Godot;

namespace AncientDeliveries.scripts;

public partial class MenuHandler : SceneSwitchLogic {
	[Export] public PackedScene StartScene;
	[Export] public PackedScene OptionsScene;
	private Button _startButton;
	private Button _optionsButton;

	public override void _Ready() {
		_startButton = GetNode<Button>("%StartButton");
		_optionsButton = GetNode<Button>("%OptionsButton");
		_startButton.GrabFocus();
		_startButton.Pressed += () => GetTree().ChangeSceneToPacked(StartScene);
		_optionsButton.Pressed += () => GetTree().ChangeSceneToPacked(OptionsScene);
	}
}