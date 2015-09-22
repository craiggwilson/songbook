using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Songbook.Formats;
using Songbook.Formats.TwoLineTextFormat;
using Songbook.Structure;

namespace Songbook.Windows.ViewModels
{
    public class SongViewModel : ReactiveObject
    {
        private bool _isSaved;
        private string _path;
        private string _text;
        private string _title;
        private SongNode _originalSong;

        public SongViewModel(string path)
        {
            _path = path;
            if (_path != null)
            {
                var format = new SimpleTwoLineTextFormat(ParsingChordLookup.Instance);
                _originalSong = format.Read(File.OpenText(path));
                using (var writer = new StringWriter())
                {
                    format.Write(_originalSong, writer);
                    _text = writer.ToString();
                }
                _title = path;
            }
            else
            {
                _title = "Untitled...";
            }
        }

        public bool IsSaved
        {
            get { return _isSaved; }
            set { this.RaiseAndSetIfChanged(ref _isSaved, value); }
        }

        public string Path
        {
            get { return _path; }
            set { this.RaiseAndSetIfChanged(ref _path, value); }
        }

        public string Text
        {
            get { return _text; }
            set { this.RaiseAndSetIfChanged(ref _text, value); }
        }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }
    }
}
