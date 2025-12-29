using Godot;
using RoguelikeCourse.Scripts.Manager;

public partial class Menu : Control
{
    private Button playButton;

    private Button quitButton;

    public override void _Ready()
    {
        this.playButton = this.GetNode<Button>("PlayButton");
        this.quitButton = this.GetNode<Button>("QuitButton");
        this.playButton.Pressed += this.OnPlayPressed;
        this.quitButton.Pressed += this.OnQuitPressed;
        this.CallDeferred(nameof(LateInit));
	}

    private void LateInit()
    {
		GameStateManager.Instance.EnterMenu();
	}
    private void OnPlayPressed()
    {
        this.GetTree().ChangeSceneToFile("res://Scenes/Levels/level_1.tscn");
		GameStateManager.Instance.ExitMenu();
	}

    private void OnQuitPressed() 
    { 
        this.GetTree().Quit();
    }
}
