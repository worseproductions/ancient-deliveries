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
	private ProgressBar _timerBar;


	public List<string> Jobs;
	public string CorrectJob;

	private SelectableOption _leftOption;
	private SelectableOption _straightOption;
	private SelectableOption _rightOption;

	private int _correctOptionIndex;

	public override void _Ready() {
		base._Ready();

		_leftOption = GetNode<SelectableOption>("HFlowContainer/LeftOption");
		_straightOption = GetNode<SelectableOption>("HFlowContainer/StraightOption");
		_rightOption = GetNode<SelectableOption>("HFlowContainer/RightOption");
		_leftOption.GrabFocus();
		_timerBar = GetNode<ProgressBar>("%TimerBar");
		_timerBar.MaxValue = _timerDuration;
		
		_leftOption.Selected += () => {
			GD.Print($"Option selected: 0 / {_correctOptionIndex}");
			EmitSignal(_correctOptionIndex == 0 ? SignalName.ActionCorrect : SignalName.ActionWrong);
		};
		
		_straightOption.Selected += () => {
			GD.Print($"Option selected: 1 / {_correctOptionIndex}");
			EmitSignal(_correctOptionIndex == 1 ? SignalName.ActionCorrect : SignalName.ActionWrong);
		};
		
		_rightOption.Selected += () => {
			GD.Print($"Option selected: 2 / {_correctOptionIndex}");
			EmitSignal(_correctOptionIndex == 2 ? SignalName.ActionCorrect : SignalName.ActionWrong);
		};
	}

	public override void _Process(double delta) {
		base._Process(delta);
		_timer += (float)delta;
		_timerBar.Value = _timer;
		
		if (!(_timer >= _timerDuration)) return;
		_timer = 0.0f;
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

		_leftOption.SetText(_correctOptionIndex == 0 ? correctJob : otherJobs[new Random().Next(0, otherJobs.Length)]);
		_straightOption.SetText(_correctOptionIndex == 1 ? correctJob : otherJobs[new Random().Next(0, otherJobs.Length)]);
		_rightOption.SetText(_correctOptionIndex == 2 ? correctJob : otherJobs[new Random().Next(0, otherJobs.Length)]);
	}
}