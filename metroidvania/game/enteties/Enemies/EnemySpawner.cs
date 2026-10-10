using Godot;
using System;
using Metroidvania.game.utility;

public partial class EnemySpawner : Marker2D
{
	[Export]
	public EnemyEnums EnemyType { get; set; }
	private string uniqueId;

	private bool isDefeated = false;
	public bool IsDefeated 
	{ 
		get => this.isDefeated;
		set
		{
			this.isDefeated = value;
			Data.Instance.EnemyData[this.uniqueId] = value;
		} 
	}

    public override void _Ready()
    {
        this.uniqueId = this.GetUniqueId();
		if (!Data.Instance.EnemyData.ContainsKey(this.uniqueId))
		{
			Data.Instance.EnemyData[this.uniqueId] = this.IsDefeated;
		}
		else
		{
			this.IsDefeated = Data.Instance.EnemyData[this.uniqueId];
		}
    }

	private string GetUniqueId()
	{
		string sceneName = this.GetOwner().SceneFilePath.GetBaseName();
		return sceneName + "-" + (string)this.Name;
	}
}
