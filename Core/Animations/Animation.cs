using Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Animations
{
    public enum AnimationState
    {
        Playing,
        Stopped,
        Paused
    }
    public class Animation:INameable
    {
        public int FPS { get; private set; }

        private float frameTime;
        private string path = "default";
        private float timeUntilNextFrame = 0;

        public AnimationState state { get; private set; }
        public string name { get ; set ; }

        private int currentFrame = 0;
        private Rectangle[] frames;
        public bool isLooping { get; private set; } = true;
        public Animation(string name,int FPS, params Rectangle[] frames)
        {
            this.FPS = FPS;
            this.name  = name;
            frameTime = 1f/(float)FPS;
            this.frames = frames;
        }
        public void SetLooping(bool looping)
        {
            isLooping = looping;
        }

        public Animation SetPath( string path)
        {
            this.path = path;
            return this;
        }
        public Animation Play() 
        {
            state = AnimationState.Playing;
            return this;
        }
        public Animation Pause()
        {
            state = AnimationState.Paused;
            return this;
        }
        public Animation Resume()
        {
            state = AnimationState.Playing;
            return this;
        }
        public Animation Stop()
        {
            state = AnimationState.Stopped;
            return this;
        }
        public void Update(GameTime gameTime)
        {
            if(state == AnimationState.Playing)
            {
                timeUntilNextFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timeUntilNextFrame >= frameTime)
                {
                    if(!isLooping&&currentFrame == frames.Length-1)
                    {
                        Stop();
                    }
                    currentFrame = currentFrame == frames.Length-1 ? 0 : currentFrame + 1;
                    timeUntilNextFrame = 0;
                }
            }
        }
        public Rectangle GetCurrentFrameRect()
        {
            return frames[currentFrame];
        }
    }
}
