﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FPX
{
    public static class GameCore
    {
        public static GameForm Window { get; private set; }
        public static Game gameInstance { get; private set; }

        public static SpriteBatch spriteBatch { get; set; }

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

        public static Game CreateGameInstance(IntPtr? windowHandle = null)
        {
            var gametype = Assembly.GetEntryAssembly().GetTypes().ToList().Find(t => t.BaseType.FullName == "Microsoft.Xna.Framework.Game");
            ConstructorInfo gameConstructor = null;
            if (windowHandle != null)
                gameConstructor = gametype.GetConstructors().ToList().Find(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(IntPtr));
            if (gameConstructor == null)
            {
                var instance = Activator.CreateInstance(gametype);
                gameInstance = (Game)instance;
            }
            else
                gameInstance = Activator.CreateInstance(gametype, windowHandle) as Game;

            PropertyInfo instanceProperty = gametype.GetProperties().ToList().Find(p => p.Name == "Instance");
            if (instanceProperty == null)
                throw new InvalidOperationException("Game instance must have a static 'Instance' property.");
            instanceProperty.GetSetMethod().Invoke(gameInstance, new object[] { gameInstance });

            gameInstance.Exiting += GameInstance_Exiting;

            gameInstance.Components.Add(new Time());
            gameInstance.Components.Add(new Physics());
            gameInstance.Components.Add(new Graphics());
            gameInstance.Components.Add(new Input());

            gameInstance.Activated += Outval_Activated;

            return gameInstance;
        }

        private static void Outval_Activated(object sender, EventArgs e)
        {
            PropertyInfo spriteBatchProperty = gameInstance.GetType().GetProperties().ToList().Find(p => p.PropertyType == typeof(SpriteBatch));
            if (spriteBatchProperty == null)
                Debug.LogWarning("Game instance does not expose spriteBatch property");
            spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatchProperty.GetSetMethod().Invoke(gameInstance, new object[] { spriteBatch });
        }

        private static void GameInstance_Exiting(object sender, EventArgs e)
        {
            IsRunning = false;
        }

        [STAThread]
        public static void Run()
        {
            Debug.BackgroundColor = ConsoleColor.Red;
            Debug.Log("ENGINE LAUNCH");
            Debug.ResetColors();

            Window = new GameForm();
            Window.Width = Settings.GetSetting<int>("ScreenWidth");
            Window.Height = Settings.GetSetting<int>("ScreenHeight");
            Window.AutoValidate = AutoValidate.Disable;
            var windowHandle = Window.Handle;


            // Thread t = new Thread(new ParameterizedThreadStart(GameLoop));
            // t.Start(windowHandle);
            WaitCallback callback = new WaitCallback(GameLoop);
            ThreadPool.QueueUserWorkItem(callback, windowHandle);
            Window.Show();
        }


        public static void LoadScene(string sceneName)
        {
            if (!IsRunning || gameInstance == null)
            {
                Debug.LogError("Engine is not currently running");
                return;
            }

            Scene scene = new Scene();
            scene.sceneName = sceneName;
            scene.Load();

            gameInstance.Components.Add(scene);
        }

        public static void Editor()
        {
            while (!IsRunning || gameInstance == null)
                Thread.Sleep(1);

            Editor.Editor editor = new Editor.Editor();

            gameInstance.Components.Add(editor);
        }

        private static void GameLoop(object param)
        {
            IntPtr windowHandle = (param == null && param.GetType() != typeof(object[])) ? IntPtr.Zero : (IntPtr)param;
            List<IGameComponent> Components = null;
            if (windowHandle.GetType() == typeof(object[]))
            {
                var prams = param as object[];
                windowHandle = (IntPtr)prams[0];
                Components = (prams[1] as IGameComponent[]).ToList();
            }


            IsRunning = true;
            using (Game gameInstance = CreateGameInstance(windowHandle))
            {
                if (Components != null)
                {
                    Components.ForEach(c => gameInstance.Components.Add(c));
                    Components = null;
                }

                while (IsRunning)
                {
                    gameInstance.RunOneFrame();
                    // Window.Invoke(new UpdateGameForm(delegate() { Window.Validate(); Thread.Sleep(1); }));
                }
            }

            Camera.Active = null;
            Settings.ShutDown();
            Debug.DumpLog();

            Debug.BackgroundColor = ConsoleColor.DarkGray;
            Debug.ForegroundColor = ConsoleColor.Black;
            Debug.Log("Engine Shutdown");
            Debug.ResetColors();
        }
        delegate void UpdateGameForm();
    }

}
