using Adventuregame.Scripts.Characters.Player;
using Adventuregame.Scripts.GlobalData;
using Adventuregame.Scripts.GlobalData.Enums;
using Adventuregame.Scripts.GlobalData.ObjectDataClasses;
using Adventuregame.Scripts.GlobalData.Preloads;
using Godot;

using System;
using System.Collections.Generic;

public partial class Inventory : Control
{
	[Export]
	private float panSpeed = 3;	

	private Node3D playerPreview;
	private Player player;
	private GridContainer weaponsGrid;
	private GridContainer shieldsGrid;
	private GridContainer stylesGrid;
	private TabContainer tabContainer;

	public override void _Ready()
	{
		this.playerPreview = (Node3D)this.GetTree().GetFirstNodeInGroup("PlayerPreview");
		this.player = (Player)this.GetTree().GetFirstNodeInGroup("Player");
		this.tabContainer = this.GetNode<TabContainer>("HBoxContainer/ItemPanelContainer/MarginContainer2/TabContainer");
		this.weaponsGrid = this.tabContainer.GetNode<GridContainer>("Weapons");
		this.shieldsGrid = this.tabContainer.GetNode<GridContainer>("Shields");
		this.stylesGrid = this.tabContainer.GetNode<GridContainer>("Styles");
		
	}
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("menu"))
		{
			PauseLogic.Instance.TogglePause(false);
		}
		int menuDir = (int)Input.GetAxis( "switch_weapon", "switch_shield");
		if (menuDir != 0)
		{
			this.tabContainer.CurrentTab = Mathf.PosMod(this.tabContainer.CurrentTab + menuDir, this.tabContainer.GetTabCount());
			this.Focus();
		}

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}
	}

	public override void _Process(Double delta)
	{
		var panDir = Input.GetAxis("pan_left","pan_right");
		this.playerPreview.RotateY((float)(panDir * delta * this.panSpeed));
	}

	public void Reveal()
	{
		this.Show();
		Input.MouseMode = Input.MouseModeEnum.Visible;
		this.CreateInventoryItems(this.weaponsGrid, this.player.CollectedWeapons, this.player.CurrentWeaponIndex);
		this.CreateInventoryItems(this.shieldsGrid, this.player.CollectedShields, this.player.CurrentShieldIndex);
		this.CreateInventoryItems(this.stylesGrid, this.player.CollectedStyles, this.player.CurrentStyleIndex);

		Control firstItem = this.weaponsGrid.GetChild<Control>(0);
		this.Focus();
	}

	private void Focus()
	{
		this.tabContainer.GetChild<Control>(this.tabContainer.CurrentTab).GetChild<Control>(0).GrabFocus();
	}

	private void CreateInventoryItems<T>(GridContainer container, List<T> equipnemtList, int index)
	where T : ItemDataBase
	{
		for (int i = 0; i < equipnemtList.Count; i++)
		{
			T equipmentItem = equipnemtList[i];
			Item itemInstance = (Item)PreloadScenes.ItemScene.Instantiate();
			itemInstance.Setup(equipmentItem);
			container.AddChild(itemInstance);
			itemInstance.Highlight(i.Equals(index));
		}
	}
}
