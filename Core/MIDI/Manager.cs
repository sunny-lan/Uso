using System.Threading.Tasks;

namespace Uso.Core.MIDI
{
    interface Manager
    {
        Task<Output> CreateOutput();
    }
}