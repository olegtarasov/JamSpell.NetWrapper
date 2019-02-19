using System;
using System.Runtime.InteropServices;
using System.Text;
using JamSpell.NetWrapper.LibraryManager;
using JamSpell.NetWrapper.Logging;
using JamSpell.NetWrapper.Properties;

namespace JamSpell.NetWrapper
{
	public sealed partial class JamSpellWrapper : IDisposable
	{
		private static readonly NativeLibraryManager _libraryManager;
		private static readonly ILog _log = LogProvider.For<JamSpellWrapper>();
		private static readonly Encoding _utf16 = Encoding.Unicode;

		private IntPtr _jamSpell;

		static JamSpellWrapper()
		{
			_libraryManager = new NativeLibraryManager("JamSpell",
				new LibraryItem("jamspell.dll", Resources.win_JamSpell, Os.Windows, Bitness.x32),
				new LibraryItem("jamspell.dll", Resources.win_JamSpell_x64, Os.Windows, Bitness.x64));
		}

		/// <summary>
		/// Ctor.
		/// </summary>
		public JamSpellWrapper()
		{
			_libraryManager.LoadNativeLibrary();
			_jamSpell = CreateJamSpell();
		}

		/// <summary>
		/// Loads a model from binary file.
		/// </summary>
		/// <param name="modelFile">Model file path.</param>
		public void LoadModel(string modelFile)
		{
			LoadModel(_jamSpell, modelFile);
		}

		/// <summary>
		/// Trains a model from specified corpus, alphabet file and writes it to the model file.
		/// </summary>
		/// <param name="textFile">Text corpus to learn from.</param>
		/// <param name="alphabetFile">Alphabet file.</param>
		/// <param name="modelFile">A file to write the model to.</param>
		/// <returns>True if training was a success.</returns>
		public bool TrainLangModel(string textFile, string alphabetFile, string modelFile)
		{
			return TrainLangModel(_jamSpell, textFile, alphabetFile, modelFile);
		}

		/// <summary>
		/// Gets candidates for a specified word in a sentence.
		/// </summary>
		/// <param name="words">Words in a sentence.</param>
		/// <param name="position">Word position for which to retrieve candidates.</param>
		/// <returns>The array of replacement candidates, most likely first.</returns>
		public unsafe string[] GetCandidates(string[] words, int position)
		{
			var handles = new GCHandle[words.Length];
			var pointers = new IntPtr[words.Length];
			for (int i = 0; i < words.Length; i++)
			{
				handles[i] = GCHandle.Alloc(_utf16.GetBytes(words[i]), GCHandleType.Pinned);
				pointers[i] = handles[i].AddrOfPinnedObject();
			}

			IntPtr candidatesPtr;
			int count;
			fixed (IntPtr* ptr = pointers)
			{
				count = GetCandidates(_jamSpell, new IntPtr(ptr), handles.Length, position, new IntPtr(&candidatesPtr));
			}

			for (int i = 0; i < handles.Length; i++)
			{
				handles[i].Free();
			}

			var result = new string[count];
			for (int i = 0; i < count; i++)
			{
				var ptr = Marshal.ReadIntPtr(candidatesPtr, i * IntPtr.Size);
				result[i] = Marshal.PtrToStringUni(ptr);
			}

			DestroyStrings(candidatesPtr, count);

			return result;
		}

		private void ReleaseUnmanagedResources()
		{
			if (_jamSpell == IntPtr.Zero)
			{
				return;
			}

			DestroyJamSpell(_jamSpell);
			_jamSpell = IntPtr.Zero;
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~JamSpellWrapper()
		{
			ReleaseUnmanagedResources();
		}
	}
}