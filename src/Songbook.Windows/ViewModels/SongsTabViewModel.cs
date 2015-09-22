﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Songbook.Windows.ViewModels
{
    public class SongsTabViewModel : ReactiveObject
    {
        private SongViewModel _currentSong;
        private string _currentDirectory;
        private string _filter;
        private ReactiveList<SongListItemViewModel> _filteredSongs;
        private ObservableAsPropertyHelper<List<SongListItemViewModel>> _songs;

        public SongsTabViewModel()
        {
            OpenDirectoryCommand = ReactiveCommand.Create();

            _songs = this.WhenAnyValue(x => x.CurrentDirectory)
                .Select(LoadSongsFromDirectory)
                .ToProperty(this, x => x.Songs, new List<SongListItemViewModel>(), RxApp.MainThreadScheduler);

            _filteredSongs = new ReactiveList<SongListItemViewModel>();

            this.WhenAnyValue(x => x.Filter, x => x.Songs)
                .Subscribe(t => UpdateFilteredSongs(t.Item1, t.Item2));

            _currentSong = new SongViewModel(null);
        }

        public ReactiveCommand<object> OpenDirectoryCommand { get; }

        public string CurrentDirectory
        {
            get { return _currentDirectory; }
            set { this.RaiseAndSetIfChanged(ref _currentDirectory, value); }
        }

        public string Filter
        {
            get { return _filter; }
            set { this.RaiseAndSetIfChanged(ref _filter, value); }
        }

        public SongViewModel CurrentSong
        {
            get { return _currentSong; }
            set { this.RaiseAndSetIfChanged(ref _currentSong, value); }
        }

        public List<SongListItemViewModel> Songs
        {
            get { return _songs.Value; }
        }

        public IReadOnlyReactiveList<SongListItemViewModel> FilteredSongs
        {
            get { return _filteredSongs; }
        }

        public void LoadSong(SongListItemViewModel songItem)
        {
            CurrentSong = new SongViewModel(songItem.Path);
        }

        private List<SongListItemViewModel> LoadSongsFromDirectory(string directory)
        {
            if (directory == null)
            {
                return new List<SongListItemViewModel>();
            }

            return Directory.EnumerateFiles(directory)
                .Select(f => new SongListItemViewModel
                {
                    Path = f,
                    Name = Path.GetFileNameWithoutExtension(f)
                })
                .ToList();
        }

        private void UpdateFilteredSongs(string filter, List<SongListItemViewModel> songs)
        {
            using (_filteredSongs.SuppressChangeNotifications())
            {
                _filteredSongs.Clear();
                if (string.IsNullOrEmpty(filter))
                {
                    _filteredSongs.AddRange(songs);
                }
                else
                {
                    _filteredSongs.AddRange(songs.Where(x => x.Name.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase) >= 0));
                }
            }
        }
    }
}
