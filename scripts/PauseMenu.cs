using Godot;

namespace AncientDeliveries.scripts;

public partial class PauseMenu : FadeIn {

	public override void _Input(InputEvent @event) {
		if (!@event.IsActionPressed("pause")) return;
		IsFadingIn = false;
		IsFadingOut = true;
	}
}