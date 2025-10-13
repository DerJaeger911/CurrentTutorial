using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godot3dPlatformerCourse.Scripts.Game.Player
{
	internal interface IPlayer
	{
		void TakeDamage(int amount);
		void UpdateScore(int amount);
	}
}
