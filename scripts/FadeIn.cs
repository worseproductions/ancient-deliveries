using Godot;

namespace AncientDeliveries.scripts;

public partial class FadeIn : Control {
	
	[Export] private float _fadeDuration = 1.0f;
	[Export] private bool _startVisible = false;
	
	[Signal] public delegate void FadeInCompleteEventHandler();
	[Signal] public delegate void FadeOutCompleteEventHandler();
	
	private float _fadeTimer = 0.0f;
	public bool IsFadingIn;
	public bool IsFadingOut;
	
	public override void _Ready() {
		Modulate = Modulate with { A = _startVisible ? 1f : 0f};
		Visible = _startVisible;
	}

	public override void _Process(double delta) {
		if (IsFadingIn) {
			Visible = true;
			_fadeTimer += (float)delta;
			Modulate = Modulate with { A = _fadeTimer / _fadeDuration };
			if (!(_fadeTimer >= _fadeDuration)) return;
			IsFadingIn = false;
			_fadeTimer = _fadeDuration;
			EmitSignal(SignalName.FadeInComplete);
		}

		if (IsFadingOut) {
			_fadeTimer -= (float)delta;
			Modulate = Modulate with { A = _fadeTimer / _fadeDuration };
			if (!(_fadeTimer <= 0)) return;
			IsFadingOut = false;
			_fadeTimer = 0.0f;
			Visible = false;
			EmitSignal(SignalName.FadeOutComplete);
		}
	}
}