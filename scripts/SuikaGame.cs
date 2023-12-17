using Godot;

namespace AncientDeliveries.scripts;

public partial class SuikaGame : Node2D {
	// Called when the node enters the scene tree for the first time.

	[Signal]
	public delegate void BoxFullEventHandler();

	private Area2D _fullArea;
	private CollisionPolygon2D _boxCollider;
	private float _checkTimer = 0.0f;
	private float _checkDuration = 1f;
	private bool _isChecking = false;
	
	private float _emptyTimer = 0.0f;
	private float _emptyDuration = 1f;
	private bool _isEmptying = false;
	
	public override void _Ready() {
		_fullArea = GetNode<Area2D>("FullArea");
		_boxCollider = GetNode<CollisionPolygon2D>("%BoxCollider");



		_fullArea.BodyEntered += StartTimer;
		_fullArea.BodyExited += ResetTimer;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (_isChecking) {
			_checkTimer += (float)delta;
			if (!(_checkTimer >= _checkDuration)) return;
			EmitSignal(SignalName.BoxFull);
		}

		if (!_isEmptying) return;
		_emptyTimer += (float)delta;
		if (!(_emptyTimer >= _emptyDuration)) return;
		_boxCollider.Disabled = false;
		GD.Print("Enabled collider");
		_isEmptying = false;
		_emptyTimer = 0f;
	}

	public void Empty() {
		_boxCollider.Disabled = true;
		_isEmptying = true;
		GD.Print("Disabled collider");
	}
	
	private void StartTimer(Node body) {
		_checkTimer = 0.0f;
		_isChecking = true;
	}
	
	private void ResetTimer(Node body) {
		if (body is not Package) return;
		_checkTimer = 0.0f;
		_isChecking = false;
	}
}