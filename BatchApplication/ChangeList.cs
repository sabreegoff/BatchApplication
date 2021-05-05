using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchApplication
{
	public class ChangeList
	{
		public Change[] changes { get; set; }
	}

	public class Change
	{
		public int user_id { get; set; }
		public string change_action { get; set; }
		public User user { get; set; }
		public Playlist playlist { get; set; }
		public Song song { get; set; }
	}
}
