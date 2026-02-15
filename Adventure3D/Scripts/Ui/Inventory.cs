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

	private Rogue playerPreview;
	private Player player;
	private GridContainer weaponsGrid;
	private GridContainer shieldsGrid;
	private GridContainer stylesGrid;
	private TabContainer tabContainer;
	private Label itemContainerLabel;
	private Panel borderPanel;

	private static readonly Color[] BorderColors =
{
	Colors.LimeGreen,
	Colors.Firebrick,
	Colors.Goldenrod
};

	public override void _Ready()
	{
		this.Hide();
		this.playerPreview = (Rogue)this.GetTree().GetFirstNodeInGroup("PlayerPreview");
		this.player = (Player)this.GetTree().GetFirstNodeInGroup("Player");
		this.tabContainer = this.GetNode<TabContainer>("HBoxContainer/ItemPanelContainer/MarginContainer2/TabContainer");
		this.weaponsGrid = this.tabContainer.GetNode<GridContainer>("Weapons");
		this.shieldsGrid = this.tabContainer.GetNode<GridContainer>("Shields");
		this.stylesGrid = this.tabContainer.GetNode<GridContainer>("Styles");
		this.itemContainerLabel = this.GetNode<Label>("HBoxContainer/ItemPanelContainer/Control/ItemContainerLabel");
		this.borderPanel = this.GetNode<Panel>("HBoxContainer/ItemPanelContainer/MarginContainer/Panel");

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
			this.itemContainerLabel.Text = this.tabContainer.GetChild(this.tabContainer.CurrentTab).Name;
			Color color = BorderColors[this.tabContainer.CurrentTab];
			var stylebox = (StyleBoxFlat)this.borderPanel.GetThemeStylebox("panel");
			stylebox.BorderColor = color;
			this.itemContainerLabel.AddThemeColorOverride("font_color", color);
		}

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			this.GetTree().Quit();
		}

		if (Input.IsActionJustPressed("run"))
		{
			this.playerPreview.Unequip();
			this.player.Unequip();
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
		foreach(GridContainer grid in this.tabContainer.GetChildren())
		{
			foreach(Node node in grid.GetChildren())
			{
				node.QueueFree();
			}
		}
		this.CreateInventoryItems(this.weaponsGrid, this.player.CollectedWeapons, this.player.CurrentWeaponIndex);
		this.CreateInventoryItems(this.shieldsGrid, this.player.CollectedShields, this.player.CurrentShieldIndex);
		this.CreateInventoryItems(this.stylesGrid, this.player.CollectedStyles, this.player.CurrentStyleIndex);

		Control firstItem = this.weaponsGrid.GetChild<Control>(0);
		this.Focus();
	}

	private async void Focus()
	{
		await this.ToSignal(this.GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
		if (this.tabContainer.GetChild<Control>(this.tabContainer.CurrentTab).GetChildCount() >0)
		{
			this.tabContainer.GetChild<Control>(this.tabContainer.CurrentTab).GetChild<Control>(0).GrabFocus();
		}
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
			if(i == index)
			{
				this.playerPreview.AddEquipment(equipmentItem);
			}
		}
	}
}
