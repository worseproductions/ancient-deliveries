using System;
using Godot;

namespace AncientDeliveries.scripts;

public partial class WaitForKeyPress : SceneSwitchLogic
{
    [Export] public String scenePathToSwitch;
    public override void _Input(InputEvent @event) {
        if (!@event.IsActionPressed("confirm")) return;
        SceneSwitchEventHandler(GetTree(), scenePathToSwitch);
    }
}