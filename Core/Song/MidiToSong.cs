using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core;
using Uso.Core.MIDI;
using Uso.Core.MIDI.Parser;

namespace Uso.Core.Song
{
    class MidiToSong 
    {


        public static Song FromMidi(MidiFile f)
        {
            var events = new List<Event>();
            var displayEvents = new List<NoteEvent>();

            var lastOn = new Dictionary<int, NoteOnEvent>();

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
                            if (lastOn.ContainsKey(e.Note))
                                throw new ArgumentException("Bad midi file");

                            displayEvents.Add(lastOn[e.Note] = new NoteOnEvent
                            {
                                Time = e.Time,
                                Note = e.Note,
                                Velocity = e.Velocity,
                            });

                            events.Add(new OutputEvent { 
                                Time = e.Time ,
                                Output = new NoteOnOutput
                                {
                                    Note= (byte)e.Note,
                                    Velocity= (byte)e.Velocity,
                                }
                            });

                            break;
                        case MidiEventType.NoteOff:
                            if (!lastOn.ContainsKey(e.Note))
                                throw new ArgumentException("Bad midi file");

                            var x = lastOn[e.Note];

                            displayEvents.Add(x.Match = new NoteOffEvent
                            {
                                Time = e.Time,
                                Note = e.Note,
                                Velocity = e.Velocity,
                                Match = x,
                            });

                            lastOn.Remove(e.Note);

                            events.Add(new OutputEvent
                            {
                                Time = e.Time,
                                Output = new NoteOffOutput
                                {
                                    Note = (byte)e.Note,
                                    Velocity = (byte)e.Velocity,
                                }
                            });

                            break;
                    }
                }
            }

            return new Song
            (
                otherEvents : events,
                displayEvents:displayEvents,
                ppq : f.TicksPerQuarterNote,
                initialTempo : 60 * 1000 * 1000 / 120,
                judgedEvents: new List<NoteEvent>(),//TODO
                initialSignature:new TimeSignature
                {
                    //TODO
                }
                
            );
        }
    }
}
