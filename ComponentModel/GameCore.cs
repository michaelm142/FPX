﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ComponentModel
{
    public class GameCore
    {
        public static GameCore Instance { get; private set; }

        public static Game gameInstance { get; private set; }

        public static SpriteBatch spriteBatch { get; private set; }

        public static bool IsRunning { get; private set; }

        public static Scene currentLevel
        {
            get { return gameInstance.Components.ToList().Find(c => c is Scene) as Scene; }
        }

        public static GraphicsDevice graphicsDevice
        {
            get { return gameInstance.GraphicsDevice; }
        }

        public static ContentManager content
        {
            get { return gameInstance.Content; }
        }

        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static Viewport viewport
        {
            get { return graphicsDevice.Viewport; }
        }

        public static Graphics Graphics
        {
            get { return gameInstance.Components.ToList().Find(c => c is Graphics) as Graphics; }
        }

        public static Game CreateGameInstance(string sceneName, IntPtr? windowHandle = null)
        {
            Game outval = null;
            var gametype = Assembly.GetEntryAssembly().GetTypes().ToList().Find(t => t.BaseType == typeof(Game));
            ConstructorInfo gameConstructor = null;
            if (windowHandle != null)
                gameConstructor = gametype.GetConstructors().ToList().Find(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(IntPtr));
            if (gameConstructor == null)
                outval = Activator.CreateInstance(gametype) as Game;
            else
                outval = Activator.CreateInstance(gametype, windowHandle) as Game;

            PropertyInfo instanceProperty = gametype.GetProperties().ToList().Find(p => p.Name == "Instance");
            if (instanceProperty == null)
                throw new InvalidOperationException("Game instance must have a static 'Instance' property.");
            instanceProperty.GetSetMethod().Invoke(gameInstance, new object[] { gameInstance });

            PropertyInfo spriteBatchProperty = gametype.GetProperties().ToList().Find(p => p.PropertyType == typeof(SpriteBatch));
            if (spriteBatchProperty == null)
                Debug.LogWarning("Game instance does not expose spriteBatch property");

            gameInstance = outval;
            gameInstance.Exiting += GameInstance_Exiting;

            Scene level = new Scene();
            level.sceneName = sceneName;

            outval.Components.Add(level);
            outval.Components.Add(new Physics());
            outval.Components.Add(new Graphics());

            typeof(Game).InvokeMember("isActive", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, gameInstance, new object[] { true });
            var methods = typeof(Game).GetMethods(BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);
            var initMethod = methods.ToList().Find(m => m.Name == "Initialize");
            var ensureHostMethod = methods.ToList().Find(m => m.Name == "EnsureHost");
            ensureHostMethod.Invoke(gameInstance, new object[] { });
            initMethod.Invoke(gameInstance, new object[] { });

            spriteBatch = gameInstance.GetType().GetProperties().ToList().Find(p => p.Name.ToLower() == "spritebatch").GetGetMethod().Invoke(gameInstance, null) as SpriteBatch;
            return outval;
        }

        private static void GameInstance_Exiting(object sender, EventArgs e)
        {
            IsRunning = false;
        }

        [STAThread]
        public static void Run(string sceneName, IntPtr? windowHandle = null)
        {
            Settings.Initialize();

            IsRunning = true;
            using (Game gameInstance = CreateGameInstance(sceneName, windowHandle))
            {
                Form gameForm = Form.FromHandle(gameInstance.Window.Handle) as Form;
                gameForm.Show();
                    gameInstance.Tick();
            }

            Camera.Active = null;
            Settings.ShutDown();
            Debug.DumpLog();
        }

        private static void GameForm_Paint(object sender, PaintEventArgs e)
        {
            gameInstance.GraphicsDevice.Present();
        }

        private static void BeginRun(Game game)
        {
            typeof(Game).GetMethods(BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance).ToList().Find(m => m.Name == "BeginRun").Invoke(game, null);
        }

        private static void EndRun(Game game)
        {
            typeof(Game).GetMethods(BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance).ToList().Find(m => m.Name == "EndRun").Invoke(game, null);
        }
    }
}
