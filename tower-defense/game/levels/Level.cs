using System.Collections.Generic;
using Godot;


public partial class Level : Node2D
{
    private PackedScene enemyScene = GD.Load<PackedScene>("res://game/enteties/enemies/Enemy.tscn");

    private Dictionary<TowerTypeEnums, string> towerScenes = new()
    {
        { TowerTypeEnums.Basic, "res://game/enteties/tower/TowerBasic.tscn" },
        { TowerTypeEnums.Blast, "res://game/enteties/tower/TowerBlaster.tscn" },
        { TowerTypeEnums.Mortar, "res://game/enteties/tower/TowerMortar.tscn" },
        

    };

    private List<Vector2I> usedCells = [];
    private PathFollow2D pathFollow;
    [Export]
    private Path2D path;
    [Export]
    private Sprite2D towerPreview;
    [Export]
    private Node2D towers;
    [Export]
    private TileMapLayer tilemapLayer;
    private Enemy enemy;
    private TowerTypeEnums selectedTower;

    private bool placeTower;
    public bool PlaceTower
    {
        get => this.placeTower;

        set
        {
            this.placeTower = value;
            this.towerPreview.Visible = value;
        }
    }




    public override void _Ready()
    {
        RenderingServer.SetDefaultClearColor(new Color("#dff6f5"));
        EventBus.Instance.PlaceTower += this.OnPlaceTower;
        EventBus.Instance.WaveStart += this.WaveStarted;
    }

    public override void _Input(InputEvent @event)
    {
        Vector2 rawPos = this.GetLocalMousePosition();
        Vector2I pos = new Vector2I((int)(rawPos.X / 16f), (int)(rawPos.Y / 16f));

        if (@event is InputEventMouseMotion && this.PlaceTower)
        {
            Vector2I towerPos = pos * 16;
            this.towerPreview.Position = towerPos;
        }

        if (@event is InputEventMouseButton mouseButton &&
        mouseButton.Pressed &&
        mouseButton.ButtonIndex == MouseButton.Left &&
        this.placeTower &&
        !this.usedCells.Contains(pos) &&
        this.tilemapLayer.GetCellTileData(pos) is TileData tileData &&
        (bool)tileData.GetCustomData("Usable"))
        {
            this.usedCells.Add(pos);
            Tower tower = GD.Load<PackedScene>(this.towerScenes[this.selectedTower]).Instantiate<Tower>();
            tower.Position = pos * 16;
            this.towers.AddChild(tower);
            this.placeTower = false;
            Data.Instance.Money -= (int)Data.TowerData[this.selectedTower]["cost"];
        }

        if (Input.IsActionJustPressed("exit"))
        {
            this.placeTower = false;
        }
    }

    private void OnPlaceTower(int idInt)
    {
        TowerTypeEnums id = (TowerTypeEnums)idInt;

        this.placeTower = true;
        this.selectedTower = id;
        string thumbnailPath = (string)Data.TowerData[id]["thumbnail"];
        this.towerPreview.Texture = GD.Load<Texture2D>(thumbnailPath);
        GD.Print(id);
    }

    public async void WaveStarted()
    {
        var data = Data.EnemyWaves[Data.Instance.CurrentWave];
        Data.Instance.CurrentWave++;

        foreach (var kvp in data)
        {
            EnemyTypeEnums enemyType = kvp.Key;
            int count = kvp.Value;

            for (int i = 0; i < count; i++)
            {
                this.pathFollow = new();
                this.enemy = this.enemyScene.Instantiate<Enemy>();
                this.enemy.Setup(this.pathFollow, enemyType);
                this.pathFollow.AddChild(this.enemy);
                this.path.AddChild(this.pathFollow);
                await this.ToSignal(this.GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
            }
        }
    }
}