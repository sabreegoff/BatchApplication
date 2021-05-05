using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BatchApplication
{
	public class Mixtape
	{
		public List<User> users { get; set; }
		public List<Playlist> playlists { get; set; }
		public List<Song> songs { get; set; }
	}

	public class User
	{
		public string id { get; set; }
		public string name { get; set; }
	}

	public class Playlist
	{
		public string id { get; set; }
		public string user_id { get; set; }
		
		public string[] song_ids { get; set; }
	}

	public class Song
	{
		public string id { get; set; }
		public string artist { get; set; }
		public string title { get; set; }
	}
}
