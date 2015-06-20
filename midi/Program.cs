using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NAudio.Midi;
using Newtonsoft.Json;

namespace midi
{
	internal static class Program
	{
		private static readonly MidiOut MidiOut = new MidiOut(0);

		public static void Main()
		{
			JsonConvert.DefaultSettings = () =>
			{
				var settings = new JsonSerializerSettings();
				settings.Converters.Add(new Tab.PartConverter());
				return settings;
			};
			while (true)
			{
				var files = new DirectoryInfo(Environment.CurrentDirectory + @"\tabs").GetFiles("*.json");
				for (var index = 0; index < files.Length; ++index)
					Console.WriteLine(index + " " + files[index].Name);
				int n;
				if (!int.TryParse(Console.ReadLine(), out n) || n > files.Length) break;
				
				using (var streamReader = new StreamReader(@"tabs\" + files[n].Name))
				{
					var tablature = JsonConvert.DeserializeObject<Tab>(streamReader.ReadToEnd());
					Console.Title = tablature.Name + " | Type: " + tablature.Type + " Sleep: " + tablature.Sleep;

					MidiOut.Send(MidiMessage.ChangePatch((int) tablature.Type, 1).RawData);
					MidiOut.Play(tablature, tones =>
					{
						Console.Clear();
						Console.WriteLine(" " + new string('─', tones.Count*4 + 1));
						Console.ForegroundColor = ConsoleColor.White;
						for (var o = 8; o >= 0; o--)
						{
							var s = tones.Aggregate("", (c, t) => c + (t >= o ? "|███" : "|   "));
							Console.WriteLine(o + s + "|\r\n " + s + "|");
						}
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.WriteLine(" " + new string('─', tones.Count*4 + 1));
						Console.WriteLine("   c   C   d   D   e   f   F   g   G   a   A   b ");
					});
				}
			}
		}

		public static void Play(this MidiOut midiOut, Tab tab, Action<List<int>> act = null)
		{
			var visNote = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

			foreach (var parts in tab.Parts)
			{
				for (var step = 0; step < parts[0].Length; ++step)
				{
					for (var o = 0; o < parts.Length; ++o)
						if (parts[o].Notes[step].Tone != Tones.none)
						{
							foreach (Tones tones in Enum.GetValues(typeof (Tones)))
								if (tones != Tones.none)
									visNote[(int) tones] = parts[o].Notes[step].Tone == tones ? parts[o].Octave : visNote[(int) tones];
							midiOut.Send(MidiMessage.StartNote(parts[o].Notes[step].Id, sbyte.MaxValue, 1).RawData);
						}
					if (act != null)
						if (visNote.Exists(i1 => i1 != -1))
						{
							act(visNote);
							visNote = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
						}
					Thread.Sleep(tab.Sleep);
				}
			}
		}
	}
}