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

        private static string lastSongName;
        private static float timeSinceNewSongStartedPlaying;
        private static bool isStoringPreviousSong;
        private static float maxTimeToForgetLastSong = 15;
        private static TimeSpan lastSongEndTime;

        public static float musicVolume = 1f;


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
    

            if(isStoringPreviousSong)
            {
                
                timeSinceNewSongStartedPlaying += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSinceNewSongStartedPlaying > maxTimeToForgetLastSong)
                {
                    timeSinceNewSongStartedPlaying = 0;
                    isStoringPreviousSong = false;
                }
            }

        }
        public static void Dispose()
        {
            waveBank.Dispose();
            soundBank.Dispose();
            audioEngine.Dispose();
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
        /// <summary>
        /// Volume parameter is a modifier that modifies the master volume-> volume=.5f means it's gonna be half the master volume
        /// </summary>
        /// <param name="songName"></param>
        /// <param name="looping"></param>
        /// <param name="volume"></param>
        public static void PlaySong(string songName,bool looping = false,float volume = 1f)
        {
            /* if(lastSongName != songName) { lastSongName = songName;isStoringPreviousSong = true;timeSinceNewSongStartedPlaying = 0; lastSongEndTime = MediaPlayer.PlayPosition; }
             if(lastSongName == songName&&isStoringPreviousSong) { MediaPlayer.PlayPosition = lastSongEndTime; }*/
            MediaPlayer.Volume = musicVolume * volume;
          
            MediaPlayer.IsRepeating = looping;
           MediaPlayer.Play( songCollection.Where(p => p.Name == songName).First());
           // soundBank.PlayCue("ocean_waves");
        }
    }
}
