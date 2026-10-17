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

    private Tower currentSelectedTower;
    public Tower CurrentSelectedTower
    {
        get => this.currentSelectedTower;

        set
        {
            this.currentSelectedTower = value;
            if (this.currentSelectedTower == null)
            {
                this.towerMenu = false;
            }
            else
            {
                this.towerMenu = true;
            }
        }
    }


    private bool towerMenu;

    private bool placeTower;
    public bool PlaceTower
    {
        get => this.placeTower;

        set
        {
            this.placeTower = value;
            this.towerPreview.Visible = value;
            GD.Print(this.towerPreview.Visible);
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
        Vector2I gridPos = pos * 16;

        if (@event is InputEventMouseMotion && this.PlaceTower)
        {

            this.towerPreview.Position = gridPos;
        }

        if (@event is InputEventMouseMotion && this.towerMenu)
        {
            if (this.CurrentSelectedTower is TowerMortar towerMortar)
            {
                towerMortar.CrosshairPosUpdate(gridPos);
            }
        }

        if (@event is InputEventMouseButton mouseButton)
        {
            //Tower selected
            if (mouseButton.Pressed &&
            mouseButton.ButtonIndex == MouseButton.Left &&
            this.CurrentSelectedTower != null)
            {
                if(this.CurrentSelectedTower is TowerMortar towerMortar)
                {
                    towerMortar.FinishPlacing();
                    this.CurrentSelectedTower = null;
                }
            }

            //Tower placing
            if (mouseButton.Pressed &&
            mouseButton.ButtonIndex == MouseButton.Left &&
            this.PlaceTower &&
            !this.usedCells.Contains(pos) &&
            this.tilemapLayer.GetCellTileData(pos) is TileData tileData &&
            (bool)tileData.GetCustomData("Usable"))
            {
                this.usedCells.Add(pos);
                Tower tower = GD.Load<PackedScene>(this.towerScenes[this.selectedTower]).Instantiate<Tower>();
                tower.LevelInstance = this;
                tower.Position = pos * 16;
                tower.Setup();
                this.towers.AddChild(tower);
                this.PlaceTower = false;
                Data.Instance.Money -= (int)Data.TowerData[this.selectedTower]["cost"];
            }
        }



        if (Input.IsActionJustPressed("exit"))
        {
            this.PlaceTower = false;
            this.CurrentSelectedTower = null;
        }
    }

    private void OnPlaceTower(int idInt)
    {
        TowerTypeEnums id = (TowerTypeEnums)idInt;

        this.PlaceTower = true;
        this.selectedTower = id;
        string thumbnailPath = (string)Data.TowerData[id]["thumbnail"];
        this.towerPreview.Texture = GD.Load<Texture2D>(thumbnailPath);
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