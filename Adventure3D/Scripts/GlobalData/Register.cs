using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventuregame.Scripts.GlobalData;

internal partial class Register : Node
{
	override public void _Ready()
	{
		var equipment = new Equipment();
		this.AddChild(equipment);
		var pauseLogic = new PauseLogic();
		this.AddChild(pauseLogic);
	}
}
