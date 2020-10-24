using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uso.Core.MIDI
{
    interface Device
    {
        string Name { get; }
    }

    interface Manager
    {
        Task<IEnumerable<Device>> ListInputDevices();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination">If null, select a sensible default output device</param>
        /// <returns>A Listener when SendMessage is called will output a midi event to the destination</returns>
        Task<Listener> CreateOutput(Device destination = null);

        /// <summary>
        /// Calls acceptor.SendMessage whenever there is a midi event on source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="acceptor"></param>
        /// <returns></returns>
        Task CreateInput(Device source, Listener acceptor);
    }
}