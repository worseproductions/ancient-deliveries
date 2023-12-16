using System;
using Godot;
using Array = Godot.Collections.Array;
using Range = Godot.Range;

namespace AncientDeliveries.scripts;

/// <summary>
/// Main Game class
/// </summary>
public partial class Game : Node3D {
	[Export] private float _spawnMultiplier = 1.0f;
	[Export] private PackedScene[] _obstacleScenes;
	[Export] private float _crossRoadsSpawnInterval = 5.0f;
	[Export] private PackedScene _crossRoadsScene;
	[Export] private Marker3D _crossRoadsSpawnPoint;

	private float _obstacleSpawnInterval = 1.0f;
	private CanvasLayer _ui;
	private PauseMenu _pauseMenu;
	private Fader _gameOverMenu;
	private Control _gameUi;
	private Fader _decisionScreen;
	private CrossRoads _crossRoads;
	
	private float _obstacleSpawnTimer;
	private float _crossRoadsSpawnTimer;
	private bool _hasCrossRoadsSpawned;

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
		_gameOverMenu = _ui.GetNode<Fader>("GameOverMenu");
		_gameUi = _ui.GetNode<Control>("GameUI");
		_decisionScreen = _ui.GetNode<Fader>("DecisionScreen");
		OnPlayerHealthChanged(_player.Health);
		
		_pauseMenu.FadeOutComplete += () => GetTree().Paused = false;

		_player.Died += OnPlayerDie;
		_player.HealthChanged += OnPlayerHealthChanged;
	}

	public override void _Process(double delta) {
		if (_crossRoads == null) CrossRoadsTimer(delta);
		ObstacleTimer(delta);
	}
	
	private void CrossRoadsTimer(double delta) {
		_crossRoadsSpawnTimer += (float)delta;
		if (!(_crossRoadsSpawnTimer >= _crossRoadsSpawnInterval)) return;
		_crossRoadsSpawnTimer = 0.0f;
		SpawnCrossRoads();
	}

	private void ObstacleTimer(double delta) {
		_obstacleSpawnTimer += (float)delta;
		if (!(_obstacleSpawnTimer >= _obstacleSpawnInterval)) return;
		_obstacleSpawnTimer = 0.0f;
		_obstacleSpawnInterval = (float)new Random().NextDouble() * 0.5f + 0.5f * _spawnMultiplier;
		SpawnObstacle();
	}
	
	private void SpawnCrossRoads() {
		_crossRoads = _crossRoadsScene.Instantiate<CrossRoads>();
		_crossRoads.GlobalPosition = _crossRoadsSpawnPoint.GlobalPosition;
		_crossRoads.TargetReached += ShowDecisionScreen;
		AddChild(_crossRoads);
	}

	private void ShowDecisionScreen() {
		_decisionScreen.FadeIn();
	}

	private void DespawnCrossRoads() {
		_crossRoads.QueueFree();
		_crossRoads = null;
	}
	
	private void SpawnObstacle() {
		var index = new Random().Next(0, _obstacleScenes.Length);
		var obstacle = _obstacleScenes[index].Instantiate<Obstacle>();
		if (new Random().NextDouble() > 0.5) _leftObstaclePath.AddChild(obstacle);
		else _rightObstaclePath.AddChild(obstacle);
	}

	public override void _Input(InputEvent @event) {
		if (!@event.IsActionPressed("pause")) return;
		_pauseMenu.FadeIn();
		GetTree().Paused = true;
	}

	private void OnPlayerDie() {
		_gameOverMenu.FadeIn();
	}
	
	private void OnPlayerHealthChanged(int health) {
		var healthString = "";
		for (var i = 0; i < health; i++) {
			healthString += "4";
		}
		_gameUi.GetNode<Label>("HealthLabel").Text = healthString;
	}
}