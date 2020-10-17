using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core;
using Uso.Core.MIDI;
using Uso.Core.MIDI.Parser;

namespace Uso.Core.Song
{
    class MidiSong : Song
    {

        private MidiSong() { }

        public List<NoteEvent> Accomp { get; private set; }

        public long PPQ { get; private set; }

        public long InitialTempo { get; private set; }

        public List<Event> Events { get; private set; }

        public static Song FromMidi(MidiFile f)
        {
            var events = new List<Event>();

            foreach (var t in f.Tracks)
            {
                foreach (var e in t.MidiEvents)
                {
                    switch (e.MidiEventType)
                    {
                        case MidiEventType.MetaEvent:
                            if (e.MetaEventType == MetaEventType.Tempo)
                            {
                                events.Add(new TempoChangeEvent
                                {
                                    Time = e.Time,
                                    NewTempo = e.NewTempo,
                                });
                            }
                            break;
                        case MidiEventType.NoteOn:
                            events.Add(new NoteOnEvent
                            {
                                Time = e.Time,
                                Note = (byte)e.Note,
                                Velocity = (byte)e.Velocity,
                                Accomp = true,
                            });
                            break;
                        case MidiEventType.NoteOff:
                            events.Add(new NoteOffEvent
                            {
                                Time = e.Time,
                                Note = (byte)e.Note,
                                Velocity = (byte)e.Velocity,
                                Accomp = true,
                            });
                            break;
                    }
                }
            }

            return new MidiSong
            {
                Events = events,
                PPQ = f.TicksPerQuarterNote,
                InitialTempo = 60 * 1000 * 1000 / 120,
            };
        }
    }
}
