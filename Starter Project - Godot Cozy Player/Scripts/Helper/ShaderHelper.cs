using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.Helper;

internal static class ShaderHelper
{
	public static void Flash(Node node, Material material)
	{
		Tween tween = node.GetTree().CreateTween();
		tween.TweenProperty(material, "shader_parameter/progress", 1.0, 0.2);
		tween.TweenProperty(material, "shader_parameter/progress", 0.0, 0.4);
	}
}
