using Godot;

namespace AncientDeliveries.scripts;

public partial class Player : CharacterBody2D {
	[Export] public const float Speed = 300.0f;
	
	public override void _PhysicsProcess(double delta) {
		var velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		var direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero) {
			velocity.X = direction.X * Speed;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}