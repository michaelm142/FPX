using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ComponentModel;

using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace FPSProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 Instance { get; set; }

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch { get; set; }

        Scene level;

        public Vector3 orthoValue;

        IntPtr targetWindowHandle = IntPtr.Zero;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            var gdm = Services.GetService(typeof(IGraphicsDeviceManager)) as IGraphicsDeviceManager;
            gdm.CreateDevice();
        }

        public Game1(IntPtr windowHandle)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            targetWindowHandle = windowHandle;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            var gdm = Services.GetService(typeof(IGraphicsDeviceManager)) as IGraphicsDeviceManager;
            gdm.CreateDevice();
        }

        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            var prams = e.GraphicsDeviceInformation.PresentationParameters;

            if (targetWindowHandle == IntPtr.Zero)
            {
                prams.BackBufferWidth = Settings.GetSetting<int>("ScreenWidth");
                prams.BackBufferHeight = Settings.GetSetting<int>("ScreenHeight");
                prams.DeviceWindowHandle = Window.Handle;
            }
            else
            {
                System.Windows.Forms.Control window = System.Windows.Forms.Control.FromHandle(targetWindowHandle);
                prams.BackBufferWidth = window.Width;
                prams.BackBufferHeight = window.Height;
                prams.DeviceWindowHandle = targetWindowHandle;
            }

            e.GraphicsDeviceInformation.PresentationParameters = prams;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (!GameCore.fonts.ContainsKey("SegoeUI"))
                GameCore.fonts.Add("SegoeUI", Content.Load<SpriteFont>("SegoeUI"));
            level = this.GetCurrentLevel();
            level.Load();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            GameCore.fonts.Clear();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                level.Save(Environment.CurrentDirectory + "\\test.xml");
                this.Exit();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
