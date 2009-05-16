using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Library.Animation;

namespace Library.Audio
{
    /// <summary>
    /// Plays a sound effect.
    /// </summary>
    /// <remarks>Calling Update is not required to drive the animation.</remarks>
    public class AudioAnimation : IAnimation
    {
        /// <summary>
        /// Creates a new audio animation.
        /// </summary>
        /// <param name="effect">The effect to play.</param>
        public AudioAnimation(SoundEffect effect) : this(effect, 1f, 1f, 0f, false)
        {
        }

        /// <summary>
        /// Creates a new audio animation.
        /// </summary>
        /// <param name="effect">The effect to play</param>
        /// <param name="volume">Volume, ranging from 0.0f (silence) to 1.0f (full volume)</param>
        /// <param name="pitch">Pitch adjustment, ranging from -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Panning, ranging from -1.0f (full left) to 1.0f (full right).</param>
        /// <param name="loop">Whether to loop the sound indefinitely.</param>
        public AudioAnimation(SoundEffect effect, float volume, float pitch, float pan, bool loop)
        {
            _effect = effect;
            _volume = volume;
            _pitch = pitch;
            _pan = pan;
            _loop = loop;
        }

        /// <summary>
        /// Starts playing the effect.
        /// </summary>
        public void Start()
        {
            _instance = _effect.Play(_volume, _pitch, _pan, _loop);
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="time">The time elapsed, in seconds, since the last update.</param>
        /// <returns>True if the sound is still playing; otherwise, false.</returns>
        public bool Update(float time)
        {
            return (_instance.State == SoundState.Playing);
        }

        private SoundEffect _effect;
        private SoundEffectInstance _instance;

        private float _volume;
        private float _pitch;
        private float _pan;
        private bool _loop;
    }
}
