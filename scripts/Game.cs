using Godot;

namespace AncientDeliveries.scripts;

/// <summary>
/// Main Game class
/// </summary>
public partial class Game : Node3D {
	
	private Road _road;
	private Player _player;
	public override void _Ready() {
		_road = GetNode<Road>("%Road");
		_player = GetNode<Player>("%Player");
	}

	public override void _Process(double delta) {
	}
}