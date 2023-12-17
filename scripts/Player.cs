using System;
using Godot;

namespace AncientDeliveries.scripts;

public partial class Player : CharacterBody3D {
	[Export] private float _speed = 300.0f;
	[Export] public int MaxHealth = 5;

	[Export] public AudioStreamPlayer _audioStreamPlayer;
	
	public int Health;
	
	[Signal] public delegate void DiedEventHandler();
	[Signal] public delegate void HealthChangedEventHandler(int health);

	public override void _Ready() {
		Health = MaxHealth;
		EmitSignal(SignalName.HealthChanged, MaxHealth);
	}

	public override void _PhysicsProcess(double delta) {
		var velocity = Velocity;
		// load scene

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
		Health--;
		EmitSignal(SignalName.HealthChanged, Health);
		_audioStreamPlayer.Play();
		GD.Print($"Health: {Health}");
		if (Health > 0) return;
		GD.Print("Game Over");
		GetTree().Paused = true;
		EmitSignal(SignalName.Died);
	}

	public void Heal() {
		Health = Math.Min(MaxHealth, Health + 1);
		EmitSignal(SignalName.HealthChanged, Health);
	}
}