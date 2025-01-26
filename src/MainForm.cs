using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using MyMusicApp.Models;
using MyMusicApp.Services;

namespace MyMusicApp
{
    public partial class MainForm : Form
    {
        private readonly MusicService _musicService;
        private SoundPlayer _soundPlayer;

        public MainForm(MusicService musicService)
        {
            _musicService = musicService;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshMusicList();
        }

        private void RefreshMusicList()
        {
            listBoxMusic.Items.Clear();
            var musicList = _musicService.FetchAllMusic();
            foreach (var music in musicList)
            {
                listBoxMusic.Items.Add($"Id: {music.Id}, Title: {music.Title}, Artist: {music.Artist}, Genre: {music.Genre}");
            }
        }

        private void btnAddMusic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV files (*.wav)|*.wav";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    var fileName = Path.GetFileNameWithoutExtension(filePath);

                    var music = new Music
                    {
                        Id = _musicService.FetchAllMusic().Count() + 1,
                        Title = fileName,
                        Artist = "Unknown",
                        Genre = "Unknown",
                        FilePath = filePath // Save the file path
                    };
                    _musicService.SaveMusic(music);
                    RefreshMusicList();
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (listBoxMusic.SelectedItem != null)
            {
                var selectedMusic = listBoxMusic.SelectedItem.ToString();
                var musicTitle = selectedMusic.Split(',')[1].Split(':')[1].Trim();
                var music = _musicService.FetchAllMusic().FirstOrDefault(m => m.Title == musicTitle);
                if (music != null)
                {
                    var filePath = music.FilePath; // Use the saved file path
                    if (File.Exists(filePath))
                    {
                        _soundPlayer = new SoundPlayer(filePath);
                        _soundPlayer.Play();
                    }
                    else
                    {
                        MessageBox.Show("Music file not found.");
                    }
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _soundPlayer?.Stop();
        }
    }
}
