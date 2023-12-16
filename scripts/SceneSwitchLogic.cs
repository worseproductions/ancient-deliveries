using Godot;
using System;

public partial class SceneSwitchLogic : Control
{
	static public void SceneSwitchEventHandler(SceneTree sceneTree, String scenePath)
	{
		// PackedScene sceneResource = (PackedScene)ResourceLoader.Load(scenePath);
		// if (node_to_remove != null)
		// {
		// 	RemoveChild(node_to_remove);
		// 	node_to_remove.QueueFree();
		// }

		sceneTree.ChangeSceneToFile(scenePath);
		// GetTree().Root.AddChild(sceneResource.Instantiate());
	}

}
