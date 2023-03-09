using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Hook.Logger;

namespace Hook
{
    // @TODO Make the timestep variable
    public class ScreenManager : Game
    {
        private GraphicsDeviceManager  graphics;
        private Stack<IScreen>         screens;

        public SpriteBatch  batch;

        public ScreenManager()
        {
            Content.RootDirectory    = Constants.RootAssetPath;
            Window.Title             = Constants.WindowTitle;
            Window.AllowUserResizing = Constants.AllowResize;
            Window.ClientSizeChanged += OnFinishedResizing;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth  = Constants.InitialWindowWidth;
            graphics.PreferredBackBufferHeight = Constants.InitialWindowHeight;
            graphics.ApplyChanges();

            batch = new SpriteBatch(GraphicsDevice);

            IsMouseVisible = true;
            screens = new Stack<IScreen>();
            screens.Push(new EditorScreen(this));
        }

        protected override void Update(GameTime time)
        {
            Keyboard.Update();
            Mouse.Update();
            GamePads.Update();
            // @TODO Just pass the GameTime object instead
            screens.Peek().Update((float) time.ElapsedGameTime.TotalMilliseconds);
            base.Update(time);
        }

        protected override void Draw(GameTime time)
        {
            screens.Peek().Draw(1); // @TODO
            base.Draw(time);
        }

        public void Push(IScreen s)
        {
            screens.Push(s);
        }

        public void Pop()
        {
            screens.Pop().Dispose();
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            screens.Peek().Pause();
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            screens.Peek().Resume();
        }

        // @TODO detect the resize as it happens
        public void OnFinishedResizing(Object sender, EventArgs e)
        {
            graphics.PreferredBackBufferWidth  = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.ApplyChanges();
            screens.Peek().Resize();
        }

        protected override void UnloadContent()
        {
            while (screens.Count > 0)  screens.Pop().Dispose();
            batch.Dispose();
            base.UnloadContent();
        }
    }

    public interface IScreen
    {
        void Update  (float dt);
        void Draw    (float alpha);
        void Resize  ();
        void Pause   ();
        void Resume  ();
        void Dispose ();
    }
}
