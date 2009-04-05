using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Library.Input;

namespace Library.Screen
{
    /// <summary>
    /// A stack of screens. The stack is drawn from bottom to top. Only the top screen is active and updated.
    /// </summary>
    public class ScreenStack
    {
        /// <summary>
        /// The screen at the top of the stack.
        /// </summary>
        public Screen ActiveScreen
        {
            get
            {
                return (_stackScreens.Count > 0) ? _stackScreens[_stackScreens.Count - 1] : null;
            }
        }

        /// <summary>
        /// Updates the active screen.
        /// </summary>
        /// <param name="time">The elapsed time, in seconds, since the last update.</param>
        /// <param name="input">The current input state.</param>
        public void Update(float time)
        {
            _stackScreens.ForEach(screen => screen.Update(time));
            _poppedScreens.ForEach(screen => screen.Update(time));
            _poppedScreens.RemoveAll(screen => screen.State == ScreenState.Inactive);
        }

        /// <summary>
        /// Draws the visible screens in the stack.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw in.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_stackScreens.Count > 0)
            {
                // find the bottom opaque screen
                int bottom = _stackScreens.Count - 1;
                while (bottom > 0 && _stackScreens[bottom].ShowBeneath)
                {
                    bottom--;
                }

                // draw from the bottom up
                for (; bottom < _stackScreens.Count; bottom++)
                {
                    _stackScreens[bottom].Draw(spriteBatch);
                }
            }

            // draw the screens transitioning off
            _poppedScreens.ForEach(s => s.Draw(spriteBatch));
        }

        /// <summary>
        /// Adds a screen on the top of the stack.
        /// </summary>
        /// <param name="screen">The screen to push.</param>
        public void Push(Screen screen)
        {
            Screen active = ActiveScreen;
            if (active != null)
            {
                active.Hide(false);
            }
            _stackScreens.Add(screen);
            screen.Show(true);
        }

        /// <summary>
        /// Removes the screen at the top of the stack.
        /// </summary>
        /// <returns>The popped screen; null if the stack is empty.</returns>
        public Screen Pop()
        {
            // remove and hide the current active screen
            Screen active = ActiveScreen;
            if (active != null)
            {
                active.Hide(true);
                _stackScreens.RemoveAt(_stackScreens.Count - 1);
                _poppedScreens.Add(active);
            }

            // show the new active screen
            active = ActiveScreen;
            if (active != null)
            {
                active.Show(false);
            }
            return active;
        }

        /// <summary>
        /// Pops all the screens from the stack.
        /// </summary>
        public void PopAll()
        {
            _stackScreens.ForEach(screen => screen.Hide(true));
            _poppedScreens.AddRange(_stackScreens);
            _stackScreens.Clear();
        }

        private List<Screen> _stackScreens = new List<Screen>();
        private List<Screen> _poppedScreens = new List<Screen>();
    }
}
