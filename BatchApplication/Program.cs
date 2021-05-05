using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace BatchApplication
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			string providedMixtape = "https://gist.githubusercontent.com/jmodjeska/0679cf6cd670f76f07f1874ce00daaeb/raw/a4ac53fa86452ac26d706df2e851fb7d02697b4b/mixtape-data.json";
			string changeList = "C:\\Users\\SGoff\\source\\repos\\BatchApplication\\BatchApplication\\ChangeList.json";

			Mixtape mixtape = ReadMixtape(providedMixtape);
			Mixtape mixtapeWithChages = ReadAndApplyChanges(mixtape, changeList);
			string jsonString = SerializeJson(mixtapeWithChages);

			Console.WriteLine(jsonString);
		}

		public static Mixtape ReadMixtape(string url)
		{
			try
			{
				using (WebClient wc = new WebClient())
				{
					string jsonString = wc.DownloadString(url);

					Mixtape mixtape = JsonSerializer.Deserialize<Mixtape>(jsonString);

					return mixtape;
				};
			}
			catch(Exception ex)
			{
				throw new Exception($"Unable to parse Json from {url}", ex);
			}
		}

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
				throw new Exception($"Unable to parse Json from {url}", ex);
			}
		}


		public static Mixtape ReadAndApplyChanges(Mixtape currentMixtape, string changes)
		{
			Mixtape updatedMixtape = currentMixtape;
			ChangeList changeList = ReadChanges(changes);

			foreach (Change c in changeList.changes)
			{
				if(c.user is not null)
				{
					switch (c.change_action)
					{
						case ("update"):
							updatedMixtape = UpdateUser(updatedMixtape, c.user);
							break;
						case ("add"):
							updatedMixtape = AddUser(updatedMixtape, c.user);
							break;
						case ("delete"):
							updatedMixtape = RemoveUser(updatedMixtape, c.user);
							break;
						default:
							throw new Exception($"Unable to recognize {nameof(c.change_action)}: {c.change_action}");
					}
				}

				if (c.playlist is not null)
				{
					switch (c.change_action)
					{
						case ("update"):
							updatedMixtape = UpdatePlaylist(updatedMixtape, c.playlist);
							break;
						case ("add"):
							updatedMixtape = AddPlaylist(updatedMixtape, c.playlist);
							break;
						case ("delete"):
							updatedMixtape = RemovePlaylist(updatedMixtape, c.playlist);
							break;
						default:
							throw new Exception($"Unable to recognize {nameof(c.change_action)}: {c.change_action}");
					}
				}

				if (c.song is not null)
				{
					switch (c.change_action)
					{
						case ("update"):
							updatedMixtape = UpdateSong(updatedMixtape, c.song);
							break;
						case ("add"):
							updatedMixtape = AddSong(updatedMixtape, c.song);
							break;
						case ("delete"):
							updatedMixtape = RemoveSong(updatedMixtape, c.song);
							break;
						default:
							throw new Exception($"Unable to recognize {nameof(c.change_action)}: {c.change_action}");
					}
				}
			}

			return updatedMixtape;
		}

		public static Mixtape UpdateUser(Mixtape mixtape, User user)
		{
			User userToUpdate = mixtape.users.Where(u => u.id == user.id).FirstOrDefault();
			userToUpdate.name = user.name;

			return mixtape;
		}

		public static Mixtape AddUser(Mixtape mixtape, User user)
		{
			if (!mixtape.users.Any(u => u.name == user.name))
			{
				int newId = mixtape.users.Count();
				newId++;
				user.id = newId.ToString();
				mixtape.users.Add(user);
			}

			return mixtape;
		}

		public static Mixtape RemoveUser(Mixtape mixtape, User user)
		{
			if (mixtape.users.Any(u => u.id == user.id))
			{
				mixtape.users.Remove(mixtape.users.Where(u => u.id == user.id).First());
			}

			return mixtape;
		}

		public static Mixtape UpdatePlaylist(Mixtape mixtape, Playlist playlist)
		{
			Playlist playlistToUpdate = mixtape.playlists.Where(p => p.id == playlist.id).FirstOrDefault();
			playlistToUpdate.user_id = playlist.user_id;
			playlistToUpdate.song_ids = playlist.song_ids;

			return mixtape;
		}

		public static Mixtape AddPlaylist(Mixtape mixtape, Playlist playlist)
		{
			if (!mixtape.playlists.Any(p => p.id == playlist.id))
			{
				int newId = mixtape.playlists.Count();
				newId++;
				playlist.id = newId.ToString();
				mixtape.playlists.Add(playlist);
			}

			return mixtape;
		}

		public static Mixtape RemovePlaylist(Mixtape mixtape, Playlist playlist)
		{
			if (mixtape.playlists.Any(p => p.id == playlist.id))
			{
				mixtape.playlists.Remove(mixtape.playlists.Where(p => p.id == playlist.id).First());
			}

			return mixtape;
		}

		public static Mixtape UpdateSong(Mixtape mixtape, Song song)
		{
			Song songToUpdate = mixtape.songs.Where(s => s.id == song.id).FirstOrDefault();
			songToUpdate.artist = song.artist;
			songToUpdate.title = song.title;

			return mixtape;
		}

		public static Mixtape AddSong(Mixtape mixtape, Song song)
		{
			if (!mixtape.songs.Any(s => s.id == song.id))
			{
				int newId = mixtape.songs.Count();
				newId++;
				song.id = newId.ToString();
				mixtape.songs.Add(song);
			}

			return mixtape;
		}

		public static Mixtape RemoveSong(Mixtape mixtape, Song song)
		{
			if (mixtape.songs.Any(s => s.id == song.id))
			{
				mixtape.songs.Remove(mixtape.songs.Where(s => s.id == song.id).First());
			}

			return mixtape;
		}

		public static string SerializeJson(Mixtape objectToJson)
		{
			string jsonString = JsonSerializer.Serialize(objectToJson);

			return jsonString;

		}
	}
}

//public static Mixtape ApplyUserChanges(Mixtape mixtape, ChangeList changeList)
//{
//	Mixtape updatedMixtape = mixtape;

//	switch (c.change_action)
//	{
//		case ("update"):
//			Console.WriteLine("asdf");
//			break;
//		case ("add"):
//			Console.WriteLine("asdf");
//			break;
//		case ("delete"):
//			Console.WriteLine("asdf");
//			break;
//		default:
//			Console.WriteLine("asdf");
//			break;

//	}

//	foreach (Change c in changeList.changes)
//	{
//		switch(c.change_action)
//		{
//			//case ("update"):
//			//	Console.WriteLine("asdf");
//			//	break;
//			//case
//		}

//		//if(c.change_action == "update")
//		//{
//		//	//use linq so we arent doing this manually
//		//	User userToUpdate = updatedMixtape.users.Where(user => user.id == u.id).FirstOrDefault();
//		//	userToUpdate.name = u.name;
//		//}

//		//if (c.change_action == "add")
//		//{
//		//	if (!updatedMixtape.users.Any(user => user.name == u.name))
//		//	{
//		//		int newId = updatedMixtape.users.Count();
//		//		newId++;
//		//		u.id = newId.ToString();
//		//		u.change_action = null;
//		//		updatedMixtape.users.Add(u);
//		//	}
//		//}

//		//if (c.change_action == "delete")
//		//{
//		//	if (updatedMixtape.users.Any(user => user.id == u.id))
//		//	{
//		//		updatedMixtape.users.Remove(updatedMixtape.users.Where(us => us.id == u.id).First());

//		//		var orderedUpdatedMixtape = updatedMixtape.users.OrderBy(user => user.id);

//		//		foreach (User us in orderedUpdatedMixtape.Where(user => Int32.Parse(user.id) > Int32.Parse(u.id)))
//		//		{
//		//			int idAsInt = Int32.Parse(us.id);
//		//			idAsInt--;
//		//			us.id = idAsInt.ToString();
//		//		}
//		//	}
//		//}
//	}

//	return updatedMixtape;
//}