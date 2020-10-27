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
        private readonly ScreenManager mgr;
        private readonly Theme theme;
        private readonly SongSelect previous;
        private readonly Task<Song> midiLoad;
        private readonly Task<Listener> midiOutLoad;

        public SongLoader(
            ScreenManager mgr,
            Theme theme,
            SongSelect previous,
            SelectedSong selectedSong
            )
        {
            this.mgr = mgr;
            this.theme = theme;
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
            output.MainLayer.DrawString(theme.TestFont, "loading song", area.Location.ToVector2(), Color.White);

        }

        public void Update(GameTime gameTime)
        {

            var st = Keyboard.GetState();
            if (st.IsKeyDown(Keys.Escape))
            {
                //TODO cancel load task
                mgr.Switch(previous);
            }


            //upon both loading complete, switch
            if (midiLoad.IsCompleted && midiOutLoad.IsCompleted)
            {
                mgr.Switch(new PlayScreen(
                    mgr, theme, this,
                    midiLoad.Result,
                    new PlayScreen.SongSettings(midiOutLoad.Result)
                ));
            }
        }
    }
}
