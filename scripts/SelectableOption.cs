using Godot;

namespace AncientDeliveries.scripts;

public partial class SelectableOption : Control {
	
	[Signal]
	public delegate void SelectedEventHandler();
	
	private Button _button;
	public bool IsCorrectOption;
	
	public override void _Ready() {
		_button = GetNode<Button>("Button");
		_button.Pressed += () => EmitSignal(SignalName.Selected);
	}

	public override void _Process(double delta) {
	}
	
	public void SetText(string text) {
		_button.Text = text;
	}

	public void _GrabFocus() {
		_button.GrabFocus();
	}
}