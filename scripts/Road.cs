using System;
using Godot;

namespace AncientDeliveries.scripts;

public partial class Road : MeshInstance3D {
	
	[Export] private float _speed = 1f;

	private BaseMaterial3D _material;

	public override void _Ready() {
		_material = Mesh.SurfaceGetMaterial(0) as BaseMaterial3D;
	}

	public override void _Process(double delta) {
		var offset = _material.Uv1Offset;
		offset.Y -= (float) delta * _speed;
		_material.Uv1Offset = offset;
	}
}