﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FPX
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class World : Microsoft.Xna.Framework.Game
    {
        public static World Instance { get; set; }

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch { get; set; }

        public Scene Scene { get; private set; }

        IntPtr targetWindowHandle = IntPtr.Zero;

        public World()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
        }

        public World(IntPtr windowHandle)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
            AssetManager.Inilitize();
            foreach (var comp in Components.ToList().FindAll(c => c is IGameComponent).Cast<IGameComponent>())
                comp.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            if (!GameCore.fonts.ContainsKey("SegoeUI"))
                GameCore.fonts.Add("SegoeUI", GameCore.content.Load<SpriteFont>("SegoeUI"));

            Scene = Components.ToList().Find(c => c is Scene) as Scene;
            Scene.Load();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //Exit();

            foreach (var comp in Components.ToList().FindAll(c => c is IUpdateable).Cast<IUpdateable>())
                comp.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            foreach (IDrawable component in Components.ToList().FindAll(c => c is IDrawable).Cast<IDrawable>())
                component.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
