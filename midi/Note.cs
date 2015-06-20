using System;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

//http://habrahabr.ru/post/148852/
namespace midi
{
	public struct Note
	{
		public byte Octave;
		public Tones Tone;
		public int Id => 12 + Octave*12 + (int) Tone;

		public Note(byte oct, Tones t)
		{
			Octave = oct;
			Tone = t;
		}
		public Note(byte oct, string t) : this(oct, (Tones)Enum.Parse(typeof(Tones), t)){ }
		public Note(byte oct, char t) : this(oct, new string(t, 1)) { }
	}

	// ReSharper disable InconsistentNaming
	public enum Tones
	{
		none = -1, a = 9, A = 10, b = 11, c = 0, C = 1,d = 2, D = 3, e = 4, f = 5, F = 6, g = 7, G = 8
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
		BreathNoise,
		Seashore,
		BirdTweet,
		Telephone,
		Helicopter,
		Applause,
		Gunshot
	}
	public struct Tab
	{
		public struct Part
		{
			public byte Octave => Notes != null && Notes.Length > 0 ? Notes[0].Octave : (byte)0;
			public int Length => Notes != null && Notes.Length > 0 ? Notes.Length : 0;
			public Note[] Notes;
		}

		public class PartConverter : JsonConverter
		{
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				var part = (Part)value;
				writer.WriteStartObject();
				writer.WritePropertyName("Octave");
				serializer.Serialize(writer, part.Octave);
				writer.WritePropertyName("Notes");
				var s = new StringBuilder();
				foreach (var note in part.Notes)
					s.Append(note.Tone != Tones.none ? note.Tone.ToString() : "-");
				serializer.Serialize(writer, s);
				writer.WriteEndObject();
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				Part part;
				byte octave = 0;
				var s = string.Empty;
				while (reader.Read())
				{
					if (reader.TokenType != JsonToken.PropertyName)
						break;

					var propertyName = (string)reader.Value;
					if (!reader.Read())
						continue;

					switch (propertyName)
					{
						case "Octave": octave = serializer.Deserialize<byte>(reader); break;
						case "Notes": s = serializer.Deserialize<string>(reader); break;
					}
				}
				part.Notes = new Note[s.Length];
				for (int i = 0; i < s.Length; i++)
				{
					Tones t;
					if (s[i] == '-') t = Tones.none;
					else Enum.TryParse(s[i].ToString(), out t);
					part.Notes[i] = new Note(octave, t);
				}
				return part;
			}

			public override bool CanConvert(Type objectType)
			{
				return objectType == typeof(Part) || objectType == typeof(Part?);
			}
		}

		public Tab()
		{
			Name = String.Empty;
			Type = Instruments.AcousticPiano;
			Sleep = 100;
			Parts = new Part[0][];
		}

		public string Name;
		public Instruments Type;
		public int Sleep;
		public Part[][] Parts;
	}
}
