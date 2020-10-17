namespace Uso.Core.MIDI
{

    
    public interface MIDIOutputEvent
    {
        long Tick { get; }
        bool On { get; }
        byte Note { get; }
        byte Velocity { get; }
    }
}
