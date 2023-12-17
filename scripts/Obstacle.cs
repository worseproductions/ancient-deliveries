using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace AncientDeliveries.scripts;

public partial class Obstacle : PathFollow3D {
	[Export] private float _speed = 0.25f;
	[Export] private bool _loops = false;
	[Export] public bool EnableLights = false;
	
	private Area3D _area;
	private Node3D _lights;
	private AudioStreamPlayer3D _audioStreamPlayer3D;
	
	public override void _Ready() {
		_lights = GetNode<Node3D>("%Lights");
		_area = GetNode<Area3D>("Area3D");
		_audioStreamPlayer3D = GetNode<AudioStreamPlayer3D>("sfx");
		_audioStreamPlayer3D.PitchScale = (float)GD.RandRange(0.90f, 1.1f);
		_area.BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta) {
		_lights.Visible = EnableLights;

		var progressRatio = ProgressRatio;
		progressRatio += (float)delta * _speed;
		if (!_loops && progressRatio >= 1) QueueFree();
		ProgressRatio = progressRatio;
	}
	
	private void OnBodyEntered(Node body) {
		if (body is not Player player) return;
		player.TakeDamage();
		QueueFree();
	}
}