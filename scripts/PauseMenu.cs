using Godot;

namespace AncientDeliveries.scripts;

public partial class PauseMenu : Fader {

	public override void _Input(InputEvent @event) {
		if (!@event.IsActionPressed("pause")) return;
		FadeOut();
	}
}