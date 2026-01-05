using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethellcourse.Scripts.Managers;

internal class GameManager
{
	private static GameManager instance = new();

	public static GameManager Instance => instance;
}
