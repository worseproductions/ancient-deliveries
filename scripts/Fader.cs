using Godot;

namespace AncientDeliveries.scripts;

public partial class Fader : Control {
	
	[Export] private float _fadeDuration = 1.0f;
	[Export] private bool _startVisible = false;
	
	[Signal] public delegate void FadeInCompleteEventHandler();
	[Signal] public delegate void FadeOutCompleteEventHandler();
	
	private float _fadeTimer = 0.0f;
	private bool _isFadingIn;
	private bool _isFadingOut;
	
	public override void _Ready() {
		Modulate = Modulate with { A = _startVisible ? 1f : 0f};
		Visible = _startVisible;
	}

	public void FadeIn() {
		_isFadingIn = true;
		_isFadingOut = false;
	}

	public void FadeOut() {
		_isFadingIn = false;
		_isFadingOut = true;
	}

	public override void _Process(double delta) {
		if (_isFadingIn) {
			Visible = true;
			_fadeTimer += (float)delta;
			Modulate = Modulate with { A = _fadeTimer / _fadeDuration };
			if (!(_fadeTimer >= _fadeDuration)) return;
			_isFadingIn = false;
			_fadeTimer = _fadeDuration;
			EmitSignal(SignalName.FadeInComplete);
		}

		if (_isFadingOut) {
			_fadeTimer -= (float)delta;
			Modulate = Modulate with { A = _fadeTimer / _fadeDuration };
			if (!(_fadeTimer <= 0)) return;
			_isFadingOut = false;
			_fadeTimer = 0.0f;
			Visible = false;
			EmitSignal(SignalName.FadeOutComplete);
		}
	}
}