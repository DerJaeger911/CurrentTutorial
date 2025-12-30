using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethellcourse.Scripts.Managers
{
    internal class DummyManager
    {
		private static DummyManager instance = new();
		public static DummyManager Instance => instance;

		// This is ready now so we take the Constructor
		private DummyManager()
		{
			GD.Print("DummyManager is Init");
		}
	}
}
