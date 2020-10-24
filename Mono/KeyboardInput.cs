using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core.MIDI;

namespace Uso.Mono
{
    class KeyboardInput
    {
        private Core.MIDI.Listener listener;

        public KeyboardInput(Listener listener)
        {
            this.listener = listener;
        }
        Dictionary<Keys, int> notes = new Dictionary<Keys, int> {
            { Keys.Q,60},
            { Keys.W,62},
            { Keys.E,64},
            { Keys.R,65},

        };

        private KeyboardState ps;
        public void Update(KeyboardState st)
        {
            foreach (var note in notes)
            {
                if (ps.IsKeyDown(note.Key) != st.IsKeyDown(note.Key))
                {
                    if (st.IsKeyDown(note.Key))
                    {
                        listener.SendMessage(new Core.MIDI.NoteOnEvent
                        {
                            Note = (byte)note.Value,
                            Velocity = 127,
                        });
                    }
                    else
                    {
                        listener.SendMessage(new Core.MIDI.NoteOffEvent
                        {
                            Note = (byte)note.Value,
                            Velocity = 127,
                        });
                    }
                }
            }
            ps = st;
        }
    }
}
