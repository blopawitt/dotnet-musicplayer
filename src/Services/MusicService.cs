using System.Collections.Generic;
using MyMusicApp.Models;

namespace MyMusicApp.Services
{
    public class MusicService
    {
        private readonly List<Music> _musicList;

        public MusicService()
        {
            _musicList = new List<Music>();
        }

        public IEnumerable<Music> FetchAllMusic()
        {
            return _musicList;
        }

        public void SaveMusic(Music music)
        {
            _musicList.Add(music);
        }
    }
}