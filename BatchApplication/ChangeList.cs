using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BatchApplication
{
	public class ChangeList
	{
		public Change[] changes { get; set; }

		public static ChangeList ReadChanges(string url)
		{
			try
			{
				using (WebClient wc = new WebClient())
				{
					string jsonString = wc.DownloadString(url);

					ChangeList changeList = JsonSerializer.Deserialize<ChangeList>(jsonString);

					return changeList;
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
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
