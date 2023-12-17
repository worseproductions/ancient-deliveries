using Godot;

namespace AncientDeliveries.scripts;

public partial class SelectableOption : Button {
	
	[Signal]
	public delegate void SelectedEventHandler();
	public bool IsCorrectOption;
	
	public override void _Ready() {
		Pressed += () => EmitSignal(SignalName.Selected);
	}

	public override void _Process(double delta) {
	}
	
	public void SetText(string text) {
		Text = text;
	}
}