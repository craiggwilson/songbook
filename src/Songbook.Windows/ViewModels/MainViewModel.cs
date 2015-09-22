using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace Songbook.Windows.ViewModels
{
    public sealed class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            HideSettingsCommand = ReactiveCommand.Create();
            ShowSettingsCommand = ReactiveCommand.Create();
            Settings = new SettingsViewModel();
            SongsTab = new SongsTabViewModel();
        }

        public ReactiveCommand<object> HideSettingsCommand { get; }
        public ReactiveCommand<object> ShowSettingsCommand { get; }

        public SettingsViewModel Settings { get; }

        public SongsTabViewModel SongsTab { get; }
    }
}
