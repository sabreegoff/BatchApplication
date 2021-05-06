using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BatchApplication
{
	public class Mixtape
	{
		public List<User> users { get; set; }
		public List<Playlist> playlists { get; set; }
		public List<Song> songs { get; set; }

		public static Mixtape Read(string url)
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
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static Mixtape ReadAndApplyChanges(Mixtape currentMixtape, string changes)
		{
			Mixtape updatedMixtape = currentMixtape;
			ChangeList changeList = ChangeList.ReadChanges(changes);

			foreach (Change c in changeList.changes)
			{
				try
				{
					if (c.user is not null)
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
								throw new NotSupportedException($"Unable to recognize {nameof(c.change_action)}: {c.change_action}\r\n"
									+ $"Could not make changes to user id: {c.user.id}");
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
								throw new NotSupportedException($"Unable to recognize {nameof(c.change_action)}: {c.change_action}\r\n"
									+ $"Could not make changes to playlist id: {c.playlist.id}");
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
								throw new NotSupportedException($"Unable to recognize {nameof(c.change_action)}: {c.change_action}\r\n"
								+$"Could not make changes to song id: {c.song.id}");

						}
					}
				}
				catch(NotSupportedException ex)
				{
					Console.WriteLine(ex.Message);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}

			return updatedMixtape;
		}

		public static Mixtape UpdateUser(Mixtape mixtape, User user)
		{
			//if the user is in valid, the user doesnt get updated. This is something I would log but not necessarily warn the user about? 
			User userToUpdate = mixtape.users.Where(u => u.id == user.id).FirstOrDefault();
			userToUpdate.name = user.name;

			return mixtape;
		}

		public static Mixtape AddUser(Mixtape mixtape, User user)
		{
			if (!mixtape.users.Any(u => u.name == user.name))
			{
				//I want the Ids to remain unique, similar to db ids.
				List<int> findMaxId = new List<int>();
				mixtape.users.ForEach(u => findMaxId.Add(Int32.Parse(u.id)));
				int maxId = findMaxId.Max();
				int newId = maxId++;
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
				List<int> findMaxId = new List<int>();
				mixtape.users.ForEach(u => findMaxId.Add(Int32.Parse(u.id)));
				int maxId = findMaxId.Max();
				int newId = maxId++;
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
				List<int> findMaxId = new List<int>();
				mixtape.users.ForEach(u => findMaxId.Add(Int32.Parse(u.id)));
				int maxId = findMaxId.Max();
				int newId = maxId++;
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
