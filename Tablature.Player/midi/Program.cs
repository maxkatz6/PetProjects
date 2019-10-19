namespace midi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using NAudio.Midi;
    using Newtonsoft.Json;

    internal static class Program
    {
        private static readonly int tonesCount = Enum.GetValues(typeof(Tone)).Length - 1;
        private const int octaveCount = 8;

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
                {
                    Console.WriteLine(index + " " + files[index].Name);
                }

                if (!int.TryParse(Console.ReadLine(), out var n) || n > files.Length)
                {
                    break;
                }

                using (var streamReader = new StreamReader(@"tabs\" + files[n].Name))
                {
                    var tablature = JsonConvert.DeserializeObject<Tab>(streamReader.ReadToEnd());
                    Console.Title = tablature.Name + " | Type: " + tablature.Type + " Sleep: " + tablature.Sleep;

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" " + new string('─', (tonesCount * 4) + 1));
                    Console.ResetColor();
                    for (var o = 8; o >= 0; o--)
                    {
                        var s = string.Concat(Enumerable.Range(0, tonesCount).Select(_ => "|   "));
                        Console.WriteLine(o + s + "|\r\n " + s + "|");
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" " + new string('─', (tonesCount * 4) + 1));
                    Console.WriteLine("   c   C   d   D   e   f   F   g   G   a   A   b ");
                    Console.ResetColor();

                    MidiOut.Send(MidiMessage.ChangePatch((int)tablature.Type, 1).RawData);
                    MidiOut.Play(tablature, tones =>
                    {
                        for (var octaveIndex = 0; octaveIndex <= octaveCount; octaveIndex++)
                        {
                            var octave = octaveCount - octaveIndex;
                            for (var toneIndex = 0; toneIndex < tonesCount; toneIndex++)
                            {
                                var tone = tones[toneIndex];
                                var toneColor = tone >= octave ? "███" : "   ";
                                var x = 1 + toneIndex + 1 + (toneIndex * 3);
                                var y = 1 + (octaveIndex * 2);
                                Console.SetCursorPosition(x, y);
                                Console.Write(toneColor);
                                Console.SetCursorPosition(x, y + 1);
                                Console.Write(toneColor);
                            }
                        }
                    });
                    Console.Clear();
                }
            }
        }

        public static void Play(this MidiOut midiOut, Tab tab, Action<IList<int>>? act = null)
        {
            var visNote = new List<int>(tonesCount) { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            var sleepTicks = (tab.Sleep * TimeSpan.TicksPerMillisecond);
            var lastTicks = DateTime.Now.Ticks - sleepTicks;

            foreach (var parts in tab.Parts)
            {
                for (var step = 0; step < parts[0].Length; ++step)
                {
                    for (var o = 0; o < parts.Length; ++o)
                    {
                        if (parts[o].Notes[step].Tone != Tone.none)
                        {
                            foreach (var tones in Enum.GetValues(typeof(Tone)))
                            {
                                if (tones is Tone tone
                                    && tone != Tone.none)
                                {
                                    visNote[(int)tones] = parts[o].Notes[step].Tone == tone ? parts[o].Octave : visNote[(int)tones];
                                }
                            }

                            midiOut.Send(MidiMessage.StartNote(parts[o].Notes[step].Id, sbyte.MaxValue, 1).RawData);
                        }
                    }

                    if (act != null && visNote.Exists(i1 => i1 != -1))
                    {
                        act(visNote);
                        visNote = new List<int>(tonesCount) { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                    }

                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        return;
                    }

                    var nowTicks = DateTime.Now.Ticks;
                    var diffTicks = nowTicks - lastTicks;

                    Thread.Sleep(TimeSpan.FromTicks(Math.Max(sleepTicks - diffTicks, 0)));

                    lastTicks = DateTime.Now.Ticks;
                }
            }
        }
    }
}