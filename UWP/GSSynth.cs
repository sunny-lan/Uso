
using System;
using System.Threading.Tasks;
using Uso.Core.MIDI;
using Windows.Devices.Midi;

namespace Uso.UWP
{
    class GsSynthManager :MidiManager
    {
        public GsSynthManager() { }
        public async Task<MidiOutput> CreateOutput()
        {
            var r = await MidiSynthesizer.CreateAsync();
            return new GSSynth { intSynth = r };
        }

        private class GSSynth : MidiOutput
        {
            public MidiSynthesizer intSynth;

            public void SendMessage(MIDIOutputEvent evt)
            {
                IMidiMessage msg;
                if (evt.On)
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
