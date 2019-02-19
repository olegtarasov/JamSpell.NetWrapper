using System;
using System.Runtime.InteropServices;

namespace JamSpell.NetWrapper
{
	public sealed partial class JamSpellWrapper
	{
		private const string LibName = "jamspell";

		[DllImport(LibName, CallingConvention = CallingConvention.StdCall)]
		private static extern IntPtr CreateJamSpell();

		[DllImport(LibName, CallingConvention = CallingConvention.StdCall)]
		private static extern void DestroyJamSpell(IntPtr hPtr);

		[DllImport(LibName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		private static extern int GetCandidates(IntPtr hPtr, IntPtr words, int wordsCount, int position, IntPtr candidates);

		[DllImport(LibName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		private static extern void LoadModel(IntPtr hPtr, string path);

		[DllImport(LibName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		private static extern bool TrainLangModel(IntPtr hPtr, string textFile, string alphabetFile, string modelFile);

		[DllImport(LibName, CallingConvention = CallingConvention.StdCall)]
		private static extern void DestroyStrings(IntPtr strings, int cnt);
	}
}