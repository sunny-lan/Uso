using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Uso.Core.MIDI;
using Uso.Core.MIDI.Parser;
using Uso.Core.Song;
using Uso.Mono.Customization;

namespace Uso.Mono.Screens
{
    class SongLoader : Screen
    {
        private readonly MainGame.Globals globals;
        private readonly SongSelect previous;
        private readonly Task<Song> midiLoad;
        private readonly Task<Listener> midiOutLoad;

        public SongLoader(
            MainGame.Globals globals,
            SongSelect previous,
            SelectedSong selectedSong
            )
        {
            this.globals = globals;
            this.previous = previous;
            this.midiLoad = Task.Run(async () =>
            {
                var bytes = await File.ReadAllBytesAsync(selectedSong.SongFile);
                return MidiToSong.FromMidi(new MidiFile(bytes));
            });
            var midiMgr = new UWP.UWPMidiManager();
            midiOutLoad = midiMgr.CreateOutput();
        }


        public void Draw(GameLayers output, Rectangle area)
        {
            output.MainLayer.DrawString(globals.Theme.TestFont, "loading song", area.Location.ToVector2(), Color.White);

        }

        public void Update(GameTime gameTime)
        {

            var st = Keyboard.GetState();
            if (globals.InputManager.DidPress(Keys.Escape))
            {
                //TODO cancel load task
                globals.ScreenManager.Switch(previous);
            }


            //upon both loading complete, switch
            if (midiLoad.IsCompleted && midiOutLoad.IsCompleted)
            {
                globals.ScreenManager.Switch(new PlayScreen(
                    globals, this,
                    midiLoad.Result,
                    new PlayScreen.SongSettings(midiOutLoad.Result)
                )) ;
            }
        }
    }
}
