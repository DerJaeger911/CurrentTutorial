using Godot;
using System;
using System.Security.Cryptography.X509Certificates;
using twentyfourtyeight.src.main.SeedGeneration;

public partial class Global : Node
{
	public SeedSettings CurrentSettings { get; private set; }
	public override void _Ready()
	{
		_ = SeedSettings.Instance;
	}
}
