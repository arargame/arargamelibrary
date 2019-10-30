using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public class Sound
    {
        private SoundEffect Effect { get; set; }
        private SoundEffectInstance Instance { get; set; }

        public string Artist { get; set; }
        public string Title { get; set; }

        public float Volume
        {
            get
            {
                return Instance.Volume;
            }
            set
            {
                Instance.Volume = value;
            }
        }

        public float Pitch
        {
            get
            {
                return Instance.Pitch;
            }
            set
            {
                Instance.Pitch = value;
            }
        }

        public float Pan
        {
            get
            {
                return Instance.Pan;
            }
            set
            {
                Instance.Pan = value;
            }
        }

        public SoundState State
        {
            get
            {
                return Instance.State;
            }
        }

        public bool IsLooped
        {
            get
            {
                return Instance.IsLooped;
            }
            set
            {
                Instance.IsLooped = value;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return Effect.Duration;
            }
        }


        public Sound(SoundEffect effect, string artist = "unknown", string title = "unknown", float volume = 1f, float pitch = 0f, float pan = 0f, bool isLooped = false)
        {
            Effect = effect;

            Instance = Effect.CreateInstance();

            Instance.Volume = volume;

            Instance.Pitch = pitch;

            Instance.Pan = pan;

            Instance.IsLooped = isLooped;

            Artist = artist;

            Title = title;
        }

        public void ChangeVolume(bool isGoingUp = true, float amount = 0.2f)
        {
            if (isGoingUp)
            {
                Instance.Volume = MathHelper.Clamp(Instance.Volume + amount, 0f, 1f);
            }
            else
            {
                Instance.Volume = MathHelper.Clamp(Instance.Volume - amount, 0f, 1f);
            }
        }

        public void ChangePitch(bool isGoingUp = true, float amount = 0.2f)
        {
            if (isGoingUp)
            {
                Instance.Pitch = MathHelper.Clamp(Instance.Pitch + amount, -1, 1);
            }
            else
            {
                Instance.Pitch = MathHelper.Clamp(Instance.Pitch - amount, -1, 1);
            }
        }

        public void ChangePan(bool isGoingUp = true, float amount = 0.2f)
        {
            if (isGoingUp)
            {
                Instance.Pan = MathHelper.Clamp(Instance.Pan + amount, -1, 1);
            }
            else
            {
                Instance.Pan = MathHelper.Clamp(Instance.Pan - amount, -1, 1);
            }
        }



        //public void Initialize()
        //{

        //}

        //public void Load()
        //{

        //}

        //public Sound SetEffect(SoundEffect effect)
        //{
        //    Effect = effect;

        //    return this;
        //}

        //public Sound SetEffect(string file)
        //{
        //    if (File.Exists(file))
        //    {
        //        using(var stream = File.OpenRead(file))
        //        {
        //            Effect = SoundEffect.FromStream(stream);
        //        }
        //    }
        //    else
        //    {
        //        //Log
        //    }

        //    return this;
        //}



        public void Play()
        {
            Task.Run(() =>
            {
                Instance.Play();
            });
        }

        public void Pause()
        {
            Task.Run(() =>
            {
                Instance.Pause();
            });
        }
    }
}
