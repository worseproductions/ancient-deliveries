using Godot;

namespace AncientDeliveries.scripts;

public partial class MenuHandler : SceneSwitchLogic {
	[Export] public PackedScene StartScene;
	[Export] public AudioStream ButtonSfx;
	[Export] public PackedScene OptionsScene;
	private Button _startButton;
	private Button _optionsButton;
	private AudioStreamPlayer _audioStreamPlayer;
	public override void _Ready() {
		_startButton = GetNode<Button>("%StartButton");
		_optionsButton = GetNode<Button>("%OptionsButton");
		
		_startButton.Pressed += () => GetTree().ChangeSceneToPacked(StartScene);
		_optionsButton.Pressed += () => GetTree().ChangeSceneToPacked(OptionsScene);
		
		_audioStreamPlayer = new AudioStreamPlayer();
		_audioStreamPlayer.Stream = ButtonSfx;
		_audioStreamPlayer.Bus = new StringName("SFX");
		_startButton.AddChild(_audioStreamPlayer);
		
		_startButton.FocusEntered += () => _audioStreamPlayer.Play();
		_optionsButton.FocusEntered += () => _audioStreamPlayer.Play();
		
		_startButton.GrabFocus();

	}
}