using Dcozysandbox.Scripts.Constants.Paths;
using Godot;

namespace Dcozysandbox.Scripts.Textures;

internal static class TextureCache
{
	private static Texture2D apple;
	public static Texture2D Apple =>
		apple ??= GD.Load<Texture2D>(TexturePaths.Apple);
}
