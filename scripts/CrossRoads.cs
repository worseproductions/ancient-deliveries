using Godot;

namespace AncientDeliveries.scripts;

public partial class CrossRoads : Node3D {

	[Export] private float _speed = 100f;
	
	[Signal] public delegate void TargetReachedEventHandler();
	
	private Marker3D _targetPoint;
	
	public override void _Ready() {
		_targetPoint = GetParent().GetNode<Marker3D>("%CrossRoadsTargetPoint");
	}

	public override void _Process(double delta) {
		// move from position to _targetPoint
		var pos = GlobalPosition;
		var targetPos = _targetPoint.GlobalPosition;
		var distance = pos.DistanceTo(targetPos);
		if (distance < 0.5f) {
			GD.Print("Target reached!");
			EmitSignal(SignalName.TargetReached);
		};
		var direction = (targetPos - pos).Normalized();
		pos += direction * _speed * (float)delta;
		GlobalPosition = pos;
	}
}