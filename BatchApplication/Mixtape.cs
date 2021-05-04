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
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string change_action { get; set; }
		public string id { get; set; }
		public string name { get; set; }
	}

	public class Playlist
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string change_action { get; set; }
		public string id { get; set; }
		public string user_id { get; set; }
		
		public string[] song_ids { get; set; }
	}

	public class Song
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string change_action { get; set; }
		public string id { get; set; }
		public string artist { get; set; }
		public string title { get; set; }
	}
}
