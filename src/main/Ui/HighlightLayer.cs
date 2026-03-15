using Godot;
using System;
using System.Collections.Generic;
using twentyfourtyeight.src.main;

public partial class HighlightLayer : TileMapLayer
{
	private int height;

	private int width;

	private List<Hex> currentlyHighlighted = new();

	private City current;

	public void Setup(int width, int height)
	{
		this.width = width;
		this.height = height;

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				this.SetCell(new Vector2I(x, y), 3, new Vector2I(0, 3));
			}
		}
		this.Visible = false;
        
	}

    public void SetHighlightLayerForCity(City city)
    {
        this.ResetHighlightLayer();
        this.current = city;
        this.HighlightCurrent(city);

        this.Visible = true;
    }

    public void ResetHighlightLayer()
    {
        foreach (Hex hex in this.currentlyHighlighted)
        {
            this.SetCell(hex.Coordinates, 0, new Vector2I(0, 3));
        }

        this.current = null;
        this.Visible = false;
    }

    public void RefreshLayer()
	{
		if(this.current != null)
		{
            this.HighlightCurrent(this.current);
			this.Visible = true;
        }
		else
		{
			this.Visible = false;
        }
	}

    public void HighlightCurrent(City city)
    {
        if (city.Territory == null || city.Territory.Count == 0)
        {
            return;
        }

        foreach (Hex hex in city.Territory)
        {
            this.currentlyHighlighted.Add(hex);
            this.SetCell(hex.Coordinates, -1);
            GD.Print("Cell set");
        }

        foreach (Hex hex in city.BorderTilePool)
        {
            this.currentlyHighlighted.Add(hex);
            this.SetCell(hex.Coordinates, -1);
            GD.Print("and again");
        }

        this.Visible = true;
    }
}
