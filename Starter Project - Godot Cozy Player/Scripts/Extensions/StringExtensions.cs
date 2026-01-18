using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcozysandbox.Scripts.Extensions;

internal static class StringExtensions
{
	public static string CapitalizeFirst(this string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		return char.ToUpper(input[0]) + input.Substring(1);
	}
}
