using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Library.Components
{
    /// <summary>
    /// Displays an exception.
    /// </summary>
    /// <remarks>Adapted from http://nickgravelyn.com/2008/10/catching-exceptions-on-xbox-360/. </remarks>
    public class ExceptionDebugGame : Game
    {
        /// <summary>
        /// The name of the font to display the exception with.
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Creates a new execption debug game.
        /// </summary>
        /// <param name="exception">The exception to display.</param>
        public ExceptionDebugGame(Exception exception)
        {
            Exception = exception;
            FontName = "Fonts/TextSmall";

            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _font = Content.Load<SpriteFont>(FontName);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(
                 _font,
                 "**** CRASH LOG ****",
                 new Vector2(100f, 100f),
                 Color.White);
            _spriteBatch.DrawString(
                 _font,
                 "Press Back to Exit",
                 new Vector2(100f, 120f),
                 Color.White);
            _spriteBatch.DrawString(
                 _font,
                 string.Format("Exception: {0}", Exception.Message),
                 new Vector2(100f, 140f),
                 Color.White);
            _spriteBatch.DrawString(
                 _font,
                 string.Format("Stack Trace:\n{0}", Exception.StackTrace),
                 new Vector2(100f, 160f),
                 Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

       private SpriteBatch _spriteBatch;
       private SpriteFont _font;

       private readonly Exception Exception;
    }
}
