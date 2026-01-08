using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.Constants.Paths;

internal static class AnimationPaths
{
	public const string AnimationTree = "AnimationTree";
	public const string Parameters = "parameters";
	public const string BlendPosition = "/blend_position";

	public const string MoveStateMachine = Parameters + "/MoveStateMachine";
	public const string MsmPlayback = MoveStateMachine + "/playback";
	public const string MsmIdleBlend = MoveStateMachine + "/idle/blend_position";
	public const string MsmMoveBlend = MoveStateMachine + "/move/blend_position";

	public const string ToolStateMachine = Parameters + "/ToolStateMachine";
	public const string TsmPlayback = ToolStateMachine + "/playback";
	public const string TsmSwordBlend = ToolStateMachine + "/sword/blend_position";

	public const string OneShot = Parameters + "/OneShot";
	public const string OsRequest = OneShot + "/request";

}
