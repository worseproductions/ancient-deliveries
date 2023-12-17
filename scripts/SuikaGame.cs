using Godot;

namespace AncientDeliveries.scripts;

public partial class SuikaGame : Node2D {
	// Called when the node enters the scene tree for the first time.

	[Signal]
	public delegate void BoxFullEventHandler();

	private Area2D _fullArea;
	private CollisionPolygon2D _boxCollider;
	private Sprite2D _floorRight;
	private Sprite2D _floorLeft;
	private Label _containerLabel;
	private float _checkTimer = 0.0f;
	private float _checkDuration = 1f;
	private bool _isChecking = false;
	private int _containerCount = 0;
	
	private float _emptyTimer = 0.0f;
	private float _emptyDuration = 1f;
	private bool _isEmptying = false;
	
	public override void _Ready() {
		_fullArea = GetNode<Area2D>("FullArea");
		_boxCollider = GetNode<CollisionPolygon2D>("%BoxCollider");

		_floorLeft = GetNode<Sprite2D>("%FloorLeft");
		_floorRight = GetNode<Sprite2D>("%FloorRight");


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
		_floorRight.RotationDegrees = 0f;
		_floorLeft.RotationDegrees = 0f;
		GD.Print("Enabled collider");
		_isEmptying = false;
		_isChecking = true;
		_emptyTimer = 0f;
	}

	public void Empty() {
		_containerCount++;
		_floorRight.RotationDegrees = -90f;
		_floorLeft.RotationDegrees = 90f;
		_boxCollider.Disabled = true;
		_isEmptying = true;
		GD.Print("Disabled collider");
	}
	
	private void StartTimer(Node body) {
		GD.Print("start timer");
		_checkTimer = 0.0f;
		_isChecking = true;
	}
	
	private void ResetTimer(Node body) {
		if (body is not Package) return;
		GD.Print("stop timer");
		_checkTimer = 0.0f;
		_isChecking = false;
	}
}