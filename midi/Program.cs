using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using NAudio.Midi;

namespace midi
{
	static class Program
	{
		static readonly MidiOut MidiOut = new MidiOut(0);
		static Instruments instrument = Instruments.MusicBox;
		public static void Main()
		{
			Console.WriteLine(instrument);
			MidiOut.Send(MidiMessage.ChangePatch((int)instrument, 1).RawData);
			using (var sr = new StreamReader("lilium_musicbox.txt"))
				foreach (var part in Regex.Split(sr.ReadToEnd(), @"(?:\r\n){2,}"))
					MidiOut.Play(part);
			Main();
        }

		public static void Play(this MidiOut midiOut, string n)
		{
            var na = n.Split('|');
			for (int i = 0; i < na[1].Length; i++)
			{
				for (int j = 0; j < na.Length / 2; j++)
					if (na[j*2 + 1][i] != '-')
					{
						var nt = new Note(byte.Parse(na[j*2]), na[j*2 + 1][i]);
                      //  Console.Write(nt.Octave.ToString() + nt.Tone);
						midiOut.Send(MidiMessage.StartNote(nt.Id, 100, 1).RawData);
					}
				//Console.Write(" ");
				Thread.Sleep(95);
			}
		}
		public enum Instruments
		{
			AcousticPiano,
			BriteAcouPiano,
			ElectricGrandPiano,
			HonkyTonkPiano,
			ElecPiano1,
			ElecPiano2,
			Harsichord,
			Clavichord,
			Celesta,
			Glockenspiel,
			MusicBox,
			Vibraphone,
			Marimba,
			Xylophone,
			TubularBells,
			Dulcimer,
			DrawbarOrgan,
			PercOrgan,
			RockOrgan,
			ChurchOrgan,
			ReedOrgan,
			Accordian,
			Harmonica,
			TangoAccordian,
			AcousticGuitar,
			SteelAcousGuitar,
			ElJazzGuitar,
			ElectricGuitar,
			ElMutedGuitar,
			OverdrivenGuitar,
			DistortionGuitar,
			GuitarHarmonic,
			AcousticBass,
			ElBassFinger,
			ElBassPick,
			FretlessBass,
			SlapBass1,
			SlapBass2,
			SynthBass1,
			SynthBass2,
			Violin,
			Viola,
			Cello,
			ContraBass,
			TremeloStrings,
			PizzStrings,
			OrchStrings,
			Timpani,
			StringEns1,
			StringEns2,
			SynthStrings1,
			SynthStrings2,
			ChoirAahs,
			VoiceOohs,
			SynthVoice,
			OrchestraHit,
			Trumpet,
			Trombone,
			Tuba,
			MutedTrumpet,
			FrenchHorn,
			BrassSection,
			SynthBrass1,
			SynthBrass2,
			SopranoSax,
			AltoSax,
			TenorSax,
			BaritoneSax,
			Oboe,
			EnglishHorn,
			Bassoon,
			Clarinet,
			Piccolo,
			Flute,
			Recorder,
			PanFlute,
			BlownBottle,
			Shakuhachi,
			Whistle,
			Ocarina,
			Lead1Square,
			Lead2Sawtooth,
			Lead3Calliope,
			Lead4Chiff,
			Lead5Charang,
			Lead6Voice,
			Lead7Fifths,
			Lead8BassLd,
			Pad1NewAge,
			Pad2Warm,
			Pad3Polysynth,
			Pad4Choir,
			Pad5Bowed,
			Pad6Metallic,
			Pad7Halo,
			Pad8Sweep,
			FX1Rain,
			FX2Soundtrack,
			FX3Crystal,
			FX4Atmosphere,
			FX5Brightness,
			FX6Goblins,
			FX7Echoes,
			FX8SciFi,
			Sitar,
			Banjo,
			Shamisen,
			Koto,
			Kalimba,
			Bagpipe,
			Fiddle,
			Shanai,
			TinkerBell,
			Agogo,
			SteelDrums,
			Woodblock,
			TaikoDrum,
			MelodicTom,
			SynthDrum,
			ReverseCymbal,
			GuitarFretNoise,
			Br