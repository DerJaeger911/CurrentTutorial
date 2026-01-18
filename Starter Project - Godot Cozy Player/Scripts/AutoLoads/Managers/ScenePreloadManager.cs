using Dcozysandbox.Scripts.Extensions;
using Godot;
using System.Collections.Generic;

namespace Dcozysandbox.Scripts.AutoLoads.Managers;

internal partial class ScenePreloadManager : GodotObject
{
	public static readonly ScenePreloadManager Instance = new();

	private readonly string[] scenesToLoad =
	{
		"res://Scenes/characters/blob.tscn"
	};

	private readonly Dictionary<PreloadEnum, PackedScene> cache = new();

	private ScenePreloadManager()
	{
		GD.Print("ScenePreloadManager is loaded");
		this.PreloadScenes();
	}

	private void PreloadScenes()
	{
		foreach (string path in this.scenesToLoad)
		{
			string keyString = path.GetBaseName().GetFile().CapitalizeFirst();

			if (System.Enum.TryParse(keyString, out PreloadEnum keyEnum))
			{
				var scene = GD.Load<PackedScene>(path);
				if (scene != null)
				{
					this.cache[keyEnum] = scene;
					GD.Print($"[Preload] Cached: {keyEnum}");
				}
			}
			else
			{
				GD.PrintErr($"[Preload] Fehler: Konnte Szene unter {path} nicht laden!");
			}
		}
	}

	public T Instantiate<T>(PreloadEnum key) where T : Node
	{
		if (!this.cache.TryGetValue(key, out var scene))
		{
			GD.PrintErr($"[Preload] Key '{key}' nicht im Cache gefunden! Hast du die Szene in 'scenesToLoad' registriert?");
			return null;
		}

		return scene.Instantiate<T>();
	}
}
