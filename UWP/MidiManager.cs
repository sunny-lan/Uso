
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Uso.Core.MIDI;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;

namespace Uso.UWP
{
    class UWPMidiManager : Manager
    {
        public UWPMidiManager() { }

        public async Task CreateInput(Device source, Listener acceptor)
        {
            if (source is MidiDevice d)
            {
                var inPort = await MidiInPort.FromIdAsync(d.info.Id);
                inPort.MessageReceived += (sender, args) =>
                {
                    switch (args.Message)
                    {
                        case MidiNoteOnMessage on:
                            acceptor.SendMessage(new NoteOnEvent
                            {
                                Note = on.Note,
                                Velocity = on.Velocity
                            });
                            break;
                        case MidiNoteOffMessage off:
                            acceptor.SendMessage(new NoteOffEvent
                            {
                                Note = off.Note,
                                Velocity = off.Velocity
                            });
                            break;
                    }
                };
            }
            else throw new ArgumentException("Was given unlisted device");
        }



        public async Task<Listener> CreateOutput(Device destination = null)
        {
            if (destination != null)
            {
                throw new ArgumentException("Only default device is available");
            }
            var r = await MidiSynthesizer.CreateAsync();
            return new GSSynth { intSynth = r };
        }

        private class MidiDevice : Core.MIDI.Device
        {
            public string Name => info.Name;
            public DeviceInformation info;
        }

        public async Task<IEnumerable<Device>> ListInputDevices()
        {
            // Find all input MIDI devices
            string midiInputQueryString = MidiInPort.GetDeviceSelector();
            DeviceInformationCollection midiInputDevices = await DeviceInformation.FindAllAsync(midiInputQueryString);
            return midiInputDevices.Select(x =>
            {
                return new MidiDevice { info = x };
            });

        }

        private class GSSynth : Listener
        {
            public MidiSynthesizer intSynth;

            public void SendMessage(Event evt)
            {
                IMidiMessage msg;
                switch (evt)
                {
                    case NoteOnEvent on:
                        msg = new MidiNoteOnMessage(evt.Channel, on.Note, on.Velocity);
                        break;
                    case NoteOffEvent off:
                        msg = new MidiNoteOffMessage(evt.Channel, off.Note, off.Velocity);
                        break;
                    case ControlEvent ctrl:
                        msg = new MidiControlChangeMessage(evt.Channel, ctrl.Controller, ctrl.ControlValue);
                        break;
                    default:
                        throw new ArgumentException("unsupported message type");
                }
                intSynth.SendMessage(msg);
            }
        }
    }

}
