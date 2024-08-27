using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Core.Audio
{
    public static class AudioManager
    {
        private static AudioEngine audioEngine;
        private static WaveBank waveBank;
        private static SoundBank soundBank;
        private static WaveBank musicBank;


        private static List<Song> songCollection;
        private static  Song testSong;
        static AudioManager()
        {
            songCollection = new List<Song>();
            audioEngine = new AudioEngine("Content/Audio/fishingSounds.xgs");
            waveBank = new WaveBank(audioEngine,"Content/Audio/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Audio/Sound Bank.xsb");
            musicBank = new WaveBank(audioEngine, "Content/Audio/Music Bank.xwb", 0, 50);
          
        }

        public static void LoadSongs(ContentManager contentManager,string path,params string[] songNames)
        {
            for (int i = 0; i < songNames.Length; i++)
            {
                songCollection.Add(contentManager.Load<Song>(path + songNames[i]));
            }

        }
        public static void Update(GameTime gameTime)
        {
            audioEngine.Update();
        }
        public static void PlayCue(string cueName)
        {
            try
            {
                soundBank.PlayCue(cueName);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static void PlaySong(string songName,bool looping = false)
        {
            MediaPlayer.IsRepeating = looping;
           MediaPlayer.Play( songCollection.Where(p => p.Name == songName).First());
           // soundBank.PlayCue("ocean_waves");
        }
    }
}
