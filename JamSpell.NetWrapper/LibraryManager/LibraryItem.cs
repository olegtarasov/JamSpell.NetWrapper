namespace JamSpell.NetWrapper.LibraryManager
{
	public class LibraryItem
	{
		public LibraryItem(string fileName, byte[] resource, Os os, Bitness bitness)
		{
			FileName = fileName;
			Resource = resource;
			Os = os;
			Bitness = bitness;
		}

		public string FileName { get; set; }
		public byte[] Resource { get; set; }
		public Os Os { get; set; }
		public Bitness Bitness { get; set; }
	}
}