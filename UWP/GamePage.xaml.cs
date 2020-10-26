using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uso.Mono.Screens;
using Uso.UWP;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uso.UWP
{
    public sealed partial class GamePage : Page
    {
        readonly MainGame _game;

        public GamePage()
        {
            this.InitializeComponent();

            // Create the game.
            var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<MainGame>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
        }
    }
}
