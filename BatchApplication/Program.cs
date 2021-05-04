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
			string changeList = "C:\\Users\\SGoff\\source\\repos\\BatchApplication\\BatchApplication\\Changes.json";

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

		public static Mixtape ReadAndApplyChanges(Mixtape currentMixtape, string changes)
		{
			Mixtape updatedMixtape = currentMixtape;
			Mixtape changesMixtape = ReadMixtape(changes);

			foreach(User u in changesMixtape.users)
			{
				if(u.change_action == "update")
				{
					//use linq so we arent doing this manually
					foreach(User us in updatedMixtape.users)
					{
						if(u.id == us.id)
						{
							us.name = u.name;
						}
					}
				}

				if (u.change_action == "add")
				{
					if (!updatedMixtape.users.Any(user => user.id == u.id))
					{
						updatedMixtape.users.Add(u);
					}
				}

				if (u.change_action == "delete")
				{
					if (updatedMixtape.users.Any(user => user.id == u.id))
					{
						User removeUser = updatedMixtape.users.Where(us => us.id == u.id).First();
						updatedMixtape.users.Remove(removeUser);
					}
				}
			}

			return updatedMixtape;
		}

		public static string SerializeJson(Mixtape objectToJson)
		{
			string jsonString = JsonSerializer.Serialize(objectToJson);

			return jsonString;

		}
	}
}

