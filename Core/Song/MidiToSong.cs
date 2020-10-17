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
        private class MidiEvent : NoteEvent
        {
            public long Tick { get; set; }

            public bool On { get; set; }

            public byte Note { get; set; }

            public byte Velocity { get; set; }

        }
        private MidiSong() { }

        public List<NoteEvent> Accomp { get; private set; }

        public long PPQ { get; private set; }

        public long InitialTempo { get; private set; }

        public static Song fromMidi(MidiFile f)
        {
            var notes = new List<NoteEvent>();

            foreach (var t in f.Tracks)
            {
                foreach (var e in t.MidiEvents)
                {
                    switch (e.MidiEventType)
                    {
                        case MidiEventType.NoteOn:
                            notes.Add(new MidiEvent
                            {
                                Tick = e.Time,
                                On = true,
                                Note = (byte)e.Note,
                                Velocity = (byte)e.Velocity
                            });
                            break;
                        case MidiEventType.NoteOff:
                            notes.Add(new MidiEvent
                            {
                                Tick = e.Time,
                                On = false,
                                Note = (byte)e.Note,
                                Velocity = (byte)e.Velocity
                            });
                            break;
                    }
                }
            }

            return new MidiSong
            {
                Accomp = notes,
                PPQ = f.TicksPerQuarterNote,
                InitialTempo = 60*1000*1000/120,
            };
        }
    }
}
