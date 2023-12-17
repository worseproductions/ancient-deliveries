using Godot;

namespace AncientDeliveries.scripts;

public partial class ControlScreen : Node {
	[Export] public PackedScene ScenePathToSwitch;

	public override void _Ready() {
		var button = GetNode<Button>("Button");
		button.GrabFocus();
		button.Pressed += () => GetTree().ChangeSceneToPacked(ScenePathToSwitch);
	}
}