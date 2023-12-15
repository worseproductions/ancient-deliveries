using System;
using Godot;

namespace AncientDeliveries.scripts;

public partial class Road : Polygon2D {
	
	[Export] private float _speed = 100f;

	public override void _Ready() {
	}

	public override void _Process(double delta) {
		var offset = TextureOffset;
		offset.Y -= (float) delta * _speed;
		TextureOffset = offset;
	}
}