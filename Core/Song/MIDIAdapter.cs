
namespace Uso.Core.Song
{
    /// <summary>
    /// Converts SongEvents which can be played to MIDI to MIDIOutputEvent
    /// </summary>
    static class MIDIAdapter
    {
        public static MIDI.NoteEvent Convert(NoteEvent evt)
        {
            if (evt is NoteOnEvent)
                return new NoteOnWrapper { evt = evt };
            else
                return new NoteOffWrapper { evt = evt };
        }

        private class NoteEventWrapper : MIDI.NoteEvent
        {
            public NoteEvent evt;
            public long Time =>evt.Time;

            public byte Note => (byte)evt.Note;

            public byte Velocity => (byte)evt.Velocity;
        }

        private class NoteOnWrapper: NoteEventWrapper,MIDI.NoteOnEvent  { }
        private class NoteOffWrapper: NoteEventWrapper,MIDI.NoteOffEvent  { }
    }
}
