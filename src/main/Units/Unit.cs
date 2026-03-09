using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using twentyfourtyeight.src.main;

public partial class Unit : Node2D
{
    public Vector2I Coords { get; set; }

    public virtual string UnitName => "Default";

    public virtual int ProductionCost => 0;

    public Civilisation civ { get; set; }

    public virtual int MaxHp { get; set; }

    public virtual int Hp { get; set; }

    public virtual int MaxMovePoints { get; set; }

    public virtual int MovePoints { get; set; }

    public bool Selected { get; set; }

    public Area2D Collider { get; set; }

	public Sprite2D Sprite { get; set; }

    public static Dictionary<Type, PackedScene> UnitSceneResources {  get; private set; }
	public static Dictionary<Type, Texture2D> UiImages {  get; private set; }

    public override void _Ready()
    {
		this.Collider = this.GetNode<Area2D>("Sprite2D/Area2D");
        this.Sprite = this.GetNode<Sprite2D>("Sprite2D");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouse && mouse.ButtonMask == MouseButtonMask.Left)
		{
			PhysicsDirectSpaceState2D spaceState = this.GetWorld2D().DirectSpaceState;
            PhysicsPointQueryParameters2D point = new();
			point.CollideWithAreas = true;
			point.Position = this.GetGlobalMousePosition();
            Godot.Collections.Array<Godot.Collections.Dictionary> result = spaceState.IntersectPoint(point);
			if(result.Count > 0 && (Area2D)result[0]["collider"] == this.Collider)
			{
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

	public void SetCiv(Civilisation civ) 
	{ 
		this.civ = civ;
		this.Sprite.Modulate = civ.TerritoryColor;
		this.civ.Units.Add(this);
	}

	public void SetSelected()
	{
		this.Selected = true;
		Color color = new Color(this.Sprite.Modulate);
		color.V = color.V - 0.25f;
        this.Sprite.Modulate = color;
    }

	public void SetDeselected()
	{
		this.Selected = false;
        this.Sprite.Modulate = this.civ.TerritoryColor;
    }
}