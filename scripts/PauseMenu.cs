using Godot;

namespace AncientDeliveries.scripts;

public partial class PauseMenu : Fader {

	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (!@event.IsActionPressed("pause") || !Visible) return;
		FadeOut();
	}
}