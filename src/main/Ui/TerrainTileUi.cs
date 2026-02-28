using Godot;
using Godot.Collections;
using System;
using twentyfourtyeight.src.main;
using twentyfourtyeight.src.main.Mangager;

public partial class TerrainTileUi : Panel
{
	private TerrainEnum terrainEnum;

	private Hex hex;

	private TextureRect terrainImage;

	private Label terrainLabel, foodLabel, productionLabel;

	public override void _Ready()
	{
		this.terrainImage = this.GetNode<TextureRect>("OuterContainer/TerrainImage");
		this.terrainLabel = this.GetNode<Label>("OuterContainer/MarginContainer/InnerContainer/TerrainLabel");
		this.foodLabel = this.GetNode<Label>("OuterContainer/MarginContainer/InnerContainer/FoodLabel");
		this.productionLabel = this.GetNode<Label>("OuterContainer/MarginContainer/InnerContainer/ProductionLabel");
	}

	public void SetHex(Hex hex)
	{
		this.hex = hex;
		this.terrainImage.Texture = (Texture2D)AssetManager.Instance.GetTerrainTexture(hex.TerrainType);
		this.terrainLabel.Text = $"Terrain: {hex.TerrainType.ToString()}";
		this.foodLabel.Text = $"Food: {hex.Food}";
		this.productionLabel.Text = $"Production: {hex.Production}";
	}
}
