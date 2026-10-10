using Godot;

namespace Metroidvania.game.utility;

public static class ContainerFinderHelper
{
    	public static Node GetContainer<T>(Node2D caller, string targetContainer) where T : Node2D
	{
		if (caller.GetParent() is Node2D directParent && directParent.Name == targetContainer)
		{
			return directParent;
		}

		Node currentParent = GetParent<T>(caller);

		directParent = currentParent.GetNode<Node2D>(targetContainer);
		return directParent;
	}

	public static Node GetParent<T>(Node2D caller) where T : Node2D
	{
		Node currentParent = caller.GetParent();
		while (currentParent != null && currentParent is not T)
		{
			currentParent = currentParent.GetParent();
		}

		if (currentParent is not T)
		{
			GD.PushError(
				$"[ContainerFinderHelper] Konnte keinen '{typeof(T).Name}'-Node im übergeordneten Baum von '{caller.Name}' finden!"
			);
		}
		return currentParent;
	}
}