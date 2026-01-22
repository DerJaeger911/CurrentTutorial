using Dcozysandbox.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.AutoLoads.Managers;

internal interface IPlayerResourceManager
{
	void AddResource(ResourceEnum resource, int amount);
	
	bool CheckResource(ResourceEnum resource, int amount);

	void SubtractResource(ResourceEnum resource, int amount);

	int GetResourceCount(ResourceEnum resource);

	void PrintAll();
}
