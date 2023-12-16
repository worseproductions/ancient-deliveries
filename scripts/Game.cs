using System;
using Godot;
using Range = Godot.Range;

namespace AncientDeliveries.scripts;

/// <summary>
/// Main Game class
/// </summary>
public partial class Game : Node3D {
	[Export] private float _spawnMultiplier = 1.0f;
	[Export] private PackedScene[] _obstacleScenes;

	private float _obstacleSpawnInterval = 1.0f;
	private CanvasLayer _ui;
	private PauseMenu _pauseMenu;
	private FadeIn _gameOverMenu;
	
	private float _obstacleSpawnTimer = 0.0f;
	
	
	private Road _road;
	private Player _player;
	private Path3D _leftObstaclePath;
	private Path3D _rightObstaclePath;
	public override void _Ready() {
		_road = GetNode<Road>("%Road");
		_player = GetNode<Player>("%Player");
		_leftObstaclePath = GetNode<Path3D>("%LeftObstaclePath");
		_rightObstaclePath = GetNode<Path3D>("%RightObstaclePath");
		_ui = GetNode<CanvasLayer>("%UiLayer");
		_pauseMenu = _ui.GetNode<PauseMenu>("PauseMenu");
		_gameOverMenu = _ui.GetNode<FadeIn>("GameOverMenu");
		
		_pauseMenu.FadeOutComplete += () => GetTree().Paused = false;

		_player.Died += OnPlayerDie;
	}

	public override void _Process(double delta) {
		_obstacleSpawnTimer += (float)delta;
		if (!(_obstacleSpawnTimer >= _obstacleSpawnInterval)) return;
		_obstacleSpawnTimer = 0.0f;
		_obstacleSpawnInterval = (float)new Random().NextDouble() * 0.5f + 0.5f * _spawnMultiplier;
		SpawnObstacle();
	}

	public override void _Input(InputEvent @event) {
		if (!@event.IsActionPressed("pause")) return;
		_pauseMenu.IsFadingIn = true;
		GetTree().Paused = true;
	}

	private void SpawnObstacle() {
		var index = new Random().Next(0, _obstacleScenes.Length);
		var obstacle = _obstacleScenes[index].Instantiate<Obstacle>();
		if (new Random().NextDouble() > 0.5) _leftObstaclePath.AddChild(obstacle);
		else _rightObstaclePath.AddChild(obstacle);
	}

	private void OnPlayerDie() {
		_gameOverMenu.IsFadingIn = true;
	}
}