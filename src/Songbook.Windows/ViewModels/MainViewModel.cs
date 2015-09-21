using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Songbook.Windows.ViewModels
{
    public sealed class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            SongsTab = new SongsTabViewModel();
        }

        public SongsTabViewModel SongsTab { get; }
    }
}
