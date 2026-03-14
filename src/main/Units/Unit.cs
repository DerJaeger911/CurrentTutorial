using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using twentyfourtyeight.src.main;
using twentyfourtyeight.src.main.SignalHubs;

public partial class Unit : Node2D
{
    public Vector2I Coords { get; set; }

    public virtual string UnitName => "Default";

    public virtual int ProductionCost => 0;

    public Civilisation Civ { get; set; }

    public virtual int MaxHp { get; set; }

    public virtual int Hp { get; set; }

    public virtual int MaxMovePoints { get; set; }

    public virtual int MovePoints { get; set; }

    public virtual int AttackValue { get; set; } = 0;

    public bool Selected { get; set; }

    public Area2D Collider { get; set; }

    public Sprite2D Sprite { get; set; }

    public HexTileMap Map { get; set; }

    public static Dictionary<Type, PackedScene> UnitSceneResources { get; private set; }
    public static Dictionary<Type, Texture2D> UiImages { get; private set; }

    public static readonly HashSet<TerrainEnum> Impassable = new HashSet<TerrainEnum>
    {
        TerrainEnum.Water,
        TerrainEnum.ShallowWater,
        TerrainEnum.Ice,
        TerrainEnum.Mountain,
    };

    private List<Hex> validMovementHex = new();

    public static Dictionary<Hex, List<Unit>> UnitLocations = new Dictionary<Hex, List<Unit>>();

    public override void _Ready()
    {
        this.Collider = this.GetNode<Area2D>("Sprite2D/Area2D");
        this.Sprite = this.GetNode<Sprite2D>("Sprite2D");
        this.Map = this.GetNode<HexTileMap>("/root/Game/HexTileMap");
        this.validMovementHex = this.CalculateValidAdjacentMovementHex();

        this.AddUnitToLocationList(this.Map.GetHex(this.Coords));

        UISignals.OnEndTurn += this.ProcessTurn;
        UISignals.OnRightClickOnMapEventHandler += this.Move;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouse && mouse.ButtonMask == MouseButtonMask.Left)
        {
            PhysicsDirectSpaceState2D spaceState = this.GetWorld2D().DirectSpaceState;
            PhysicsPointQueryParameters2D point = new();
            point.CollideWithAreas = true;
            point.Position = this.GetGlobalMousePosition();
            Godot.Collections.Array<Godot.Collections.Dictionary> result = spaceState.IntersectPoint(point);
            if (result.Count > 0 && (Area2D)result[0]["collider"] == this.Collider)
            {
                UISignals.EmitUnitClicked(this);
                this.SetSelected();
                this.GetViewport().SetInputAsHandled();
            }
            else
            {
                this.SetDeselected();
            }
        }
    }

    public static void LoadUnitScenes()
    {
        UnitSceneResources = new Dictionary<Type, PackedScene>
        {
            {typeof(Settler), ResourceLoader.Load<PackedScene>("res://src/main/Units/Settler.tscn") },
            {typeof(Warrior), ResourceLoader.Load<PackedScene>("res://src/main/Units/Warrior.tscn") },
        };
    }

    public static void LoadTextures()
    {
        UiImages = new Dictionary<Type, Texture2D>
        {
            {typeof(Settler), ResourceLoader.Load<Texture2D>("res://Assets-4XHexMap/settler_image.png") },
            {typeof(Warrior), ResourceLoader.Load<Texture2D>("res://Assets-4XHexMap/warrior_image.jpg") },
        };
    }

    public void ProcessTurn()
    {
        this.MovePoints = this.MaxMovePoints;
    }

    public void CalculateCombat(Unit attacker, Unit defender)
    {
        defender.Hp -= attacker.AttackValue;
        attacker.Hp -= defender.AttackValue/2;

        if(defender.Hp <= 0)
        {
            defender.DestroyUnit();
        }

        if (attacker.Hp <= 0)
        {
            attacker.DestroyUnit();
        }
    }

    public void MoveToHex(Hex hex)
    {
        if (Impassable.Contains(hex.TerrainType))
        { 
            return; 
        }

        bool isTargetOccupied = UnitLocations.ContainsKey(hex) && UnitLocations[hex].Count > 0;

        if (!isTargetOccupied)
        {
            var oldHex = this.Map.GetHex(this.Coords);
            if (UnitLocations.ContainsKey(oldHex))
            {
                UnitLocations[oldHex].Remove(this);
            }

            this.Position = this.Map.MapToLocal(hex.Coordinates);
            this.Coords = hex.Coordinates;

            this.AddUnitToLocationList(hex);

            this.validMovementHex = this.CalculateValidAdjacentMovementHex();
            this.MovePoints -= 1;

            if (hex.IsCityCenter && hex.OwnerCity.Civ != this.Civ && this is Warrior)
            {
                hex.OwnerCity.ChangeOwnership(this.Civ);
                GD.Print("Uhhhh Fuck thats deep");
            }
        }
        else
        {
            this.Fight(hex);
        }
    }

    private void Fight(Hex hex)
    {
        Unit opp = UnitLocations[hex][0];

        if(opp.Civ != this.Civ)
        {
            this.CalculateCombat(this, opp);
        }
    }

    public void Move(Hex hex)
    {
        if (this.Selected && this.MovePoints > 0)
        {
            this.MoveToHex(hex);
            UISignals.EmitUnitClicked(this);
        }
    }

    public void DestroyUnit()
    {
        if (this.Selected)
        {
            UISignals.EmitSelectedUnitDestroyedEventHandler();
        }

        this.Civ.Units.Remove(this);
        Unit.UnitLocations[this.Map.GetHex(this.Coords)].Remove(this);
        this.QueueFree();
    }

    public List<Hex> CalculateValidAdjacentMovementHex()
    {
        List<Hex> hexes = [.. this.Map.GetSurroundingHexes(this.Coords)];
        hexes.Where(h => !Impassable.Contains(h.TerrainType)).ToList();


        return hexes;
    }

    public void SetCiv(Civilisation civ)
    {
        this.Civ = civ;
        GD.Print(civ);
        this.Sprite.Modulate = civ.TerritoryColor;
        this.Civ.Units.Add(this);
    }

    public void SetSelected()
    {
        if (!this.Selected)
        {
            this.Selected = true;
            Color color = new Color(this.Sprite.Modulate);
            color.V = color.V - 0.25f;
            this.Sprite.Modulate = color;
            this.validMovementHex = this.CalculateValidAdjacentMovementHex();
        }
    }

    public void SetDeselected()
    {
        this.Selected = false;
        this.validMovementHex.Clear();
        this.Sprite.Modulate = this.Civ.TerritoryColor;
    }

    public void AddUnitToLocationList(Hex hex)
    {
        if (!UnitLocations.ContainsKey(hex))
        {
            UnitLocations[hex] = new List<Unit>();
        }
        UnitLocations[hex].Add(this);
    }

    public void RandomMove()
    {
        Random rnd = new();
        this.validMovementHex = this.CalculateValidAdjacentMovementHex();
        Hex hex = this.validMovementHex.ElementAt(rnd.Next(validMovementHex.Count));

        this.MoveToHex(hex);
    }

    public override void _ExitTree()
    {
        UISignals.OnEndTurn -= this.ProcessTurn;
        UISignals.OnRightClickOnMapEventHandler -= this.Move;  
    }
}