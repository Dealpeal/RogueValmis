using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboMapReader
{
	/// <summary>
	/// Represents a tileset exported in JSON format
	/// </summary>
	internal class TiledTileSetJSON
	{
		public int columns;
		public string image;
		public int imageheight;
		public int imagewidth;
		public int margin;
		public string name;
		public int spacing;
		public int tilecount;
		public string tiledversion;
		public int tileheight;
		public int tilewidth;
		public string type;
		public string version;
	}
}
