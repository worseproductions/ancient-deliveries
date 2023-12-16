using Godot;

namespace AncientDeliveries.scripts;

public partial class Car : Area2D {
	[Export] private float _speed = 200f;
	[Export] private Vector2 _minScale = new(0.5f, 0.5f);
	[Export] private Vector2 _maxScale = new(1.5f, 1.5f);
	public override void _Ready() {
		BodyEntered += OnBodyEntered;
		Scale = _minScale;
	}

	public override void _Process(double delta) {
		var pos = GlobalPosition;
		pos.Y += (float) delta * _speed;
		GlobalPosition = pos;
		var scale = Scale;
		scale += new Vector2((float) delta * 0.1f, (float) delta * 0.1f);
		Scale = scale;
	}
	
	private static void OnBodyEntered(Node body) {
		if (body is Player player) {
			player.TakeDamage();
		}
	}
}