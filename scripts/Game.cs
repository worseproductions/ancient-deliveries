using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;

namespace AncientDeliveries.scripts;

/// <summary>
/// Main Game class
/// </summary>
public partial class Game : Node3D {
	[Export] private float _spawnMultiplier = 1.0f;
	[Export] private PackedScene[] _obstacleScenes;
	[Export] private PackedScene[] _bushScenes;
	[Export] private Path3D[] _bushPaths;
	[Export] private float _bushSpawnInterval = 1.0f;
	[Export] private float _crossRoadsSpawnInterval = 5.0f;
	[Export] private float _crossRoadsSpawnIntervalAddition = 2.0f;
	[Export] private PackedScene _crossRoadsScene;
	[Export] private Marker3D _crossRoadsSpawnPoint;
	[Export] private int _numberOfJobs = 6;
	[Export] private int _jobAddressLength = 2;
	[Export] private int _maxAddressLength = 5;

	private float _obstacleSpawnInterval = 1.0f;
	private CanvasLayer _ui;
	private PauseMenu _pauseMenu;
	private Fader _gameOverMenu;
	private Fader _gameUi;
	private DecisionScreen _decisionScreen;
	private Fader _correctOptionCutscene;
	private Fader _wrongOptionCutscene;
	private CrossRoads _crossRoads;
	private float _obstacleSpawnTimer;
	private float _bushSpawnTimer;
	private float _crossRoadsSpawnTimer;
	private bool _hasCrossRoadsSpawned;
	private Road _road;
	private Player _player;
	private Path3D _leftObstaclePath;
	private Path3D _rightObstaclePath;
	private List<string> _jobs;
	private RichTextLabel _packageList;
	private Fader _packageListContainer;
	private Node2D _packageContainer;
	private Marker2D _packageSpawnPoint;
	private PackedScene _packageScene;
	private SuikaGame _suikaGame;
	private Button _restartButton;

	public override void _Ready() {
		_road = GetNode<Road>("%Road");
		_player = GetNode<Player>("%Player");
		_leftObstaclePath = GetNode<Path3D>("%LeftObstaclePath");
		_rightObstaclePath = GetNode<Path3D>("%RightObstaclePath");
		_ui = GetNode<CanvasLayer>("%UiLayer");
		_pauseMenu = _ui.GetNode<PauseMenu>("PauseMenu");
		_pauseMenu.Restart += ReloadScene;
		
		_gameOverMenu = _ui.GetNode<Fader>("GameOverMenu");
		_gameUi = _ui.GetNode<Fader>("GameUI");
		_decisionScreen = _ui.GetNode<DecisionScreen>("DecisionScreen");
		_decisionScreen.ActionCorrect += OnDecisionActionCorrect;
		_decisionScreen.ActionWrong += OnDecisionActionWrong;
		_suikaGame = _ui.GetNode<SuikaGame>("%SuikaGame");
		_correctOptionCutscene = _ui.GetNode<Fader>("%CorrectOptionCutscene");
		_wrongOptionCutscene = _ui.GetNode<Fader>("%WrongOptionCutscene");
		
		_restartButton = _ui.GetNode<Button>("%RestartButton");
		_restartButton.Pressed += ReloadScene;
		
		_suikaGame.BoxFull += () => {
			_player.Heal();
			_suikaGame.Empty();
		};
		
		_packageContainer = _ui.GetNode<Node2D>("%SuikaGame");
		_packageSpawnPoint = _ui.GetNode<Marker2D>("%PackageSpawnPoint");
		_packageScene = ResourceLoader.Load<PackedScene>("res://scenes/blueprints/package.tscn");
		
		OnPlayerHealthChanged(_player.MaxHealth);
		
		_pauseMenu.FadeOutComplete += () => GetTree().Paused = false;
		_player.Died += OnPlayerDie;
		_player.HealthChanged += OnPlayerHealthChanged;

		_jobs = JobGenerator.GetJobs(_numberOfJobs, _jobAddressLength);
		_packageListContainer = _gameUi.GetNode<Fader>("%PackageListContainer");
		_packageList = _gameUi.GetNode<RichTextLabel>("%PackageList");
		// set the text to the jobs separated by a new line
		var jobsWithoutFirst = new List<string>(_jobs);
		jobsWithoutFirst.RemoveAt(0);
		StringBuilder sb = new();
		sb.Append("[color=red]").Append(_jobs.First()).AppendLine("[/color]");
		foreach (var job in jobsWithoutFirst) {
			sb.AppendLine(job);
		}
		_packageList.Text = sb.ToString();
		_packageListContainer.FadeWithWait();
		_gameUi.FadeIn();
	}

	public override void _Process(double delta) {
		if (_crossRoads == null) CrossRoadsTimer(delta);
		ObstacleTimer(delta);
		BushTimer(delta);
	}

	private void UpdateJobList() {
		var numberOfJobs = _jobs.Count;
		if (numberOfJobs <= 4) {
			_crossRoadsSpawnInterval += _crossRoadsSpawnIntervalAddition;
			_jobAddressLength = Math.Min(_maxAddressLength, _jobAddressLength + 1);
			var newJobs = JobGenerator.GetJobs(3, _jobAddressLength);
			// append new jobs to bottom of stack
			_jobs.AddRange(newJobs);
		}
		var jobsWithoutFirst = new List<string>(_jobs);
		jobsWithoutFirst.RemoveAt(0);
		StringBuilder sb = new();
		sb.Append("[color=red]").Append(_jobs.First()).AppendLine("[/color]");
		foreach (var job in jobsWithoutFirst) {
			sb.AppendLine(job);
		}
		_packageList.Text = sb.ToString();
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

	private void BushTimer(double delta) {
		_bushSpawnTimer += (float)delta;
		if (!(_bushSpawnTimer >= _bushSpawnInterval)) return;
		_bushSpawnTimer = 0.0f;
		_bushSpawnInterval = (float)new Random().NextDouble() * 0.5f + 0.5f * _spawnMultiplier;
		SpawnBush();
	}

	private void SpawnCrossRoads() {
		_crossRoads = _crossRoadsScene.Instantiate<CrossRoads>();
		AddChild(_crossRoads);
		_crossRoads.GlobalPosition = _crossRoadsSpawnPoint.GlobalPosition;
		_crossRoads.TargetReached += ShowDecisionScreen;
	}

	private void ShowDecisionScreen() {
		_gameUi.FadeOut();
		GD.Print("Showing decision screen");
		_decisionScreen.SetJobs(_jobs);
		_decisionScreen.FadeIn();
		GetTree().Paused = true;
	}

	private void OnDecisionActionCorrect() {
		GD.Print("Correct option");
		var package = _packageScene.Instantiate<Package>();
		_packageContainer.AddChild(package);
		package.GlobalPosition = _packageSpawnPoint.GlobalPosition + new Vector2(new Random().Next(-10, 10), 0);
		package.GlobalRotation = (float)new Random().NextDouble() * 360;
		CloseDecisionScreen(true);
	}
	
	private void OnDecisionActionWrong() {
		GD.Print("Wrong option");
		_player.TakeDamage();
		CloseDecisionScreen(false);
	}
	
	private void CloseDecisionScreen(bool correct) {
		UpdateJobList();
		DespawnCrossRoads();
		DespawnCars();
		_decisionScreen.FadeOut();
		var cutscene = correct ? _correctOptionCutscene : _wrongOptionCutscene;
		cutscene.FadeWithWait();
		cutscene.FadeOutComplete += () => {
			GD.Print("Restarting game");
			_gameUi.FadeIn();
			RestartLoop();
		};
	}

	private void RestartLoop() {
		GetTree().Paused = false;
		_packageListContainer.FadeWithWait();
	}

	private void ReloadScene() {
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	private void DespawnCrossRoads() {
		_crossRoads?.QueueFree();
		_crossRoads = null;
	}

	private void DespawnCars() {
		foreach (var child in _leftObstaclePath.GetChildren()) {
			child.QueueFree();
		}

		foreach (var child in _rightObstaclePath.GetChildren()) {
			child.QueueFree();
		}
	}

	private void SpawnObstacle() {
		var index = new Random().Next(0, _obstacleScenes.Length);
		var obstacle = _obstacleScenes[index].Instantiate<Obstacle>();
		if (new Random().NextDouble() > 0.5) {
			_leftObstaclePath.AddChild(obstacle);
		}
		else {
			_rightObstaclePath.AddChild(obstacle);
		}
	}
	
	private void SpawnBush() {
		
		var bushPathNumber = new Random().Next(0, _bushPaths.Length);
		for (var i = 0; i < bushPathNumber; i++) {
			var bushIndex = new Random().Next(0, _bushScenes.Length);
			var bush = _bushScenes[bushIndex].Instantiate<Obstacle>();
			_bushPaths[i].AddChild(bush);
		}
	}

	public override void _Input(InputEvent @event) {
		if (!@event.IsActionPressed("pause")) return;
		_pauseMenu.FadeIn();
		GetTree().Paused = true;
	}

	private void OnPlayerDie() {
		_gameUi.FadeOut();
		_gameUi.FadeOutComplete += () => {
			GetTree().Paused = true;
			_gameOverMenu.FadeIn();
			_restartButton.GrabFocus();
		};
	}

	private void OnPlayerHealthChanged(int health) {
		var healthString = "[color=red]";
		for (var i = 0; i < health; i++) {
			healthString += "4";
		}
		healthString += "[/color][color=lightgray]";
		for(var i = 0; i < _player.MaxHealth - health; i++) {
			healthString += "4";
		}
		_gameUi.GetNode<RichTextLabel>("HealthLabel").Text = healthString;
	}
}