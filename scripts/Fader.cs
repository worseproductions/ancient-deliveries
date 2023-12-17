using Godot;

namespace AncientDeliveries.scripts;

public partial class Fader : Control {
	
	[Export] private float _fadeInDuration = 1.0f;
	[Export] private float _waitDuration = 1.0f;
	[Export] private float _fadeOutDuration = 1.0f;
	[Export] private bool _startVisible = false;
	
	[Signal] public delegate void FadeInCompleteEventHandler();
	[Signal] public delegate void FadeOutCompleteEventHandler();
	
	private float _fadeInTimer = 0.0f;
	private float _waitingTimer = 0.0f;
	private float _fadeOutTimer = 0.0f;
	private bool _isFadingIn;
	private bool _willWait;
	private bool _isWaiting;
	private bool _isFadingOut;
	
	public override void _Ready() {
		Modulate = Modulate with { A = _startVisible ? 1f : 0f};
		Visible = _startVisible;
	}
	
	public void FadeWithWait() {
		FadeIn(true);
	}

	public void FadeIn(bool wait = false) {
		_isFadingIn = true;
		_isFadingOut = false;
		_willWait = wait;
	}

	public void FadeOut() {
		_isFadingIn = false;
		_isFadingOut = true;
	}

	public override void _Process(double delta) {
		if (_isFadingIn) {
			Visible = true;
			_fadeInTimer += (float)delta;
			Modulate = Modulate with { A = _fadeInTimer / _fadeInDuration };
			if (!(_fadeInTimer >= _fadeInDuration)) return;
			_isFadingIn = false;
			_fadeInTimer = 0;
			if (_willWait) _isWaiting = true;
			EmitSignal(SignalName.FadeInComplete);
			return;
		}
		
		if (_isWaiting) {
			_waitingTimer += (float)delta;
			if (!(_waitingTimer >= _waitDuration)) return;
			_isWaiting = false;
			_waitingTimer = 0.0f;
			FadeOut();
			return;
		}

		if (_isFadingOut) {
			_fadeOutTimer += (float)delta;
			Modulate = Modulate with { A = 1 - (_fadeOutTimer / _fadeOutDuration) };
			if (!(_fadeOutTimer >= _fadeOutDuration)) return;
			_isFadingOut = false;
			_fadeOutTimer = 0.0f;
			Visible = false;
			EmitSignal(SignalName.FadeOutComplete);
		}
	}
}