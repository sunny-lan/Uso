
using System;
using System.Threading.Tasks;
using Uso.Core.MIDI;
using Windows.Devices.Midi;

namespace Uso.UWP
{
    class GsSynthManager :Manager
    {
        public GsSynthManager() { }
        public async Task<Output> CreateOutput()
        {
            var r = await MidiSynthesizer.CreateAsync();
            return new GSSynth { intSynth = r };
        }

        private class GSSynth : Output
        {
            public MidiSynthesizer intSynth;

            public void SendMessage(NoteEvent evt)
            {
                IMidiMessage msg;
                if (evt is NoteOnEvent)
                {
                    msg = new MidiNoteOnMessage(0, evt.Note, evt.Velocity);
                }
                else
                {
                    msg = new MidiNoteOffMessage(0, evt.Note, evt.Velocity);
                }
                intSynth.SendMessage(msg);
            }
        }
    }

}
