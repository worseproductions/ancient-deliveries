using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace AncientDeliveries.scripts;

public partial class DecisionScreen : Fader {
	[Signal]
	public delegate void ActionCorrectEventHandler();

	[Signal]
	public delegate void ActionWrongEventHandler();

	[Export] private float _timerDuration = 5.0f;
	private float _timer = 0.0f;
	private TextureProgressBar _timerBar;
	private bool _stopTimer = true;


	public List<string> Jobs;
	public string CorrectJob;

	private Button _leftOption;
	private Button _straightOption;
	private Button _rightOption;

	private int _correctOptionIndex;

	public override void _Ready() {
		base._Ready();

		_leftOption = GetNode<Button>("%LeftOption");
		_straightOption = GetNode<Button>("%StraightOption");
		_rightOption = GetNode<Button>("%RightOption");
		_timerBar = GetNode<TextureProgressBar>("%TimerBar");
		_timerBar.MaxValue = _timerDuration;
		_timer = 0.0f;
		_timerBar.Value = _timer;

		_leftOption.Pressed += () => {
			GD.Print($"Option selected: 0 / {_correctOptionIndex}");
			EmitSignal(_correctOptionIndex == 0 ? SignalName.ActionCorrect : SignalName.ActionWrong);
			_stopTimer = true;
		};

		_straightOption.Pressed += () => {
			GD.Print($"Option selected: 1 / {_correctOptionIndex}");
			EmitSignal(_correctOptionIndex == 1 ? SignalName.ActionCorrect : SignalName.ActionWrong);
			_stopTimer = true;
		};

		_rightOption.Pressed += () => {
			GD.Print($"Option selected: 2 / {_correctOptionIndex}");
			EmitSignal(_correctOptionIndex == 2 ? SignalName.ActionCorrect : SignalName.ActionWrong);
			_stopTimer = true;
		};
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (_stopTimer) return;
		_timer += (float)delta;
		_timerBar.Value = _timer;

		if (!(_timer >= _timerDuration)) return;
		_timer = 0.0f;
		_stopTimer = true;
		EmitSignal(SignalName.ActionWrong);
	}

	public void SetJobs(List<string> jobs) {
		Jobs = jobs;
		_correctOptionIndex = new Random().Next(0, 3);
		var correctJob = Jobs.First();
		Jobs.Remove(correctJob);
		GD.Print($"Correct job: {correctJob}");
		var otherJobs = Jobs.ToArray();
		GD.Print($"Other jobs: {otherJobs.Stringify()}");

		_leftOption.Text = _correctOptionIndex == 0 ? correctJob : otherJobs[new Random().Next(0, otherJobs.Length)];
		_straightOption.Text = _correctOptionIndex == 1 ? correctJob : otherJobs[new Random().Next(0, otherJobs.Length)];
		_rightOption.Text = _correctOptionIndex == 2 ? correctJob : otherJobs[new Random().Next(0, otherJobs.Length)];
		_leftOption.GrabFocus();
		GD.Print(_leftOption.HasFocus());
		
		_timer = 0.0f;
		_stopTimer = false;
		_timerBar.Value = _timer;
	}
}