using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamSpell.NetWrapper;

namespace TestUtil
{
	class Program
	{
		static void Main(string[] args)
		{
			var spell = new JamSpellWrapper();

			//spell.LoadModel(@"C:\Models\model.bin");
			spell.TrainLangModel(@"C:\Projects\JamSpell\test_data\kapitanskaya_dochka.txt", @"C:\Projects\JamSpell\test_data\alphabet_ru.txt", @"c:\Models\test.bin");
			var candidates = spell.GetCandidates(new[] {"првет", "медвед"}, 0);
		}
	}
}
