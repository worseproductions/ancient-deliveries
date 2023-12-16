using Godot;

namespace AncientDeliveries.scripts;

public partial class Player : CharacterBody3D {
	[Export] private float _speed = 300.0f;
	[Export] private int _health = 3;
	
	[Signal] public delegate void DiedEventHandler();
	
	public override void _PhysicsProcess(double delta) {
		var velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		var direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero) {
			velocity.X = direction.X * _speed;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void TakeDamage() {
		_health--;
		GD.Print($"Health: {_health}");
		if (_health > 0) return;
		GD.Print("Game Over");
		GetTree().Paused = true;
		EmitSignal(SignalName.Died);
	}
}