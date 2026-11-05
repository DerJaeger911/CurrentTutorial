using Godot;
using System;
using System.Diagnostics.CodeAnalysis;

public partial class InteractionController : RayCast3D
{
	[Export]
	private Label interactPromptLabel;
	
	public override void _Process(double delta)
	{
		GodotObject collider = this.GetCollider();
		this.interactPromptLabel.Text = "";

		if(collider != null && collider is InteractableObject interactable)
		{
			if (!interactable.CanInteract)
			{
				return;
			}

			this.interactPromptLabel.Text = "[E]" + interactable.InteractPrompt;

			if (Input.IsActionJustPressed("interact"))
			{
				interactable.Interact();
			}
		}

		

	}
}
