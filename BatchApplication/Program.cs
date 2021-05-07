using System;
using System.IO;
using System.Text.Json;

namespace BatchApplication
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			Console.Write("Enter URL or filename of mixtape: ");
			string providedMixtape = Console.ReadLine();

			Console.Write("Enter URL or filename of change list: ");
			string changeList = Console.ReadLine();

			if(string.IsNullOrWhiteSpace(providedMixtape))
			{
				providedMixtape = "https://gist.githubusercontent.com/jmodjeska/0679cf6cd670f76f07f1874ce00daaeb/raw/a4ac53fa86452ac26d706df2e851fb7d02697b4b/mixtape-data.json";
			}

			if (string.IsNullOrWhiteSpace(changeList))
			{
				providedMixtape = "ChangeList.json";
			}

			try
			{
				Mixtape mixtape = Mixtape.Read(providedMixtape);
				Mixtape mixtapeWithChages = Mixtape.ReadAndApplyChanges(mixtape, changeList);
				SerializeJsonToFile(mixtapeWithChages);

				Console.WriteLine(File.ReadAllText("Output.json"));

				Console.WriteLine("Press any key to close");
				Console.ReadKey(false);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static void SerializeJsonToFile(Mixtape objectToJson)
		{
			//It may not always be a good idea to pretty print here. Adds more bytes to deal with but I think it's a good idea for this project specifically
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
			};

			var jsonString = JsonSerializer.Serialize(objectToJson, options);

			File.WriteAllText("Output.json", jsonString);
		}
	}
}