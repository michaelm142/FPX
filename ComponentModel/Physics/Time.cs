using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FPX
{
    public class Time : IGameComponent, IUpdateable
    {
        public bool Enabled { get; private set; } = true;

        public int UpdateOrder => int.MaxValue;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        private GameTime _gameTime;
        public static GameTime GameTime
        {
            get { return Instance._gameTime; }
        }

        private static Time Instance;

        public static float deltaTime
        {
            get {
                return ((float)Instance._gameTime.ElapsedGameTime.TotalSeconds *
                  Settings.GetSetting<float>("TimeScale"));
            }
        }

        public void Initialize()
        {
            if (Instance != null)
                throw new InvalidOperationException("There is more than one instance of time!");

            Instance = this;
            _gameTime = new GameTime();
        }

        public void Update(GameTime gameTime)
        {
            this._gameTime = gameTime;
        }
    }
}
