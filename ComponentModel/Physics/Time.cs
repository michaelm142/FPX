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

        private GameTime gameTime;

        private static Time Instance;

        public static float deltaTime
        {
            get {
                return ((float)Instance.gameTime.ElapsedGameTime.TotalSeconds *
                  Settings.GetSetting<float>("TimeScale"));
            }
        }

        public void Initialize()
        {
            if (Instance != null)
                throw new InvalidOperationException("There is more than one instance of time!");

            Instance = this;
            gameTime = new GameTime();
        }

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }
    }
}
