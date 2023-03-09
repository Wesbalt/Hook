using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Hook.Logger;
using System;
using System.Collections.Generic;

namespace Hook
{
    public class EditorScreen : IScreen
    {
        private ScreenManager screenManager;
        private List<Sprite> guiSprites, worldSprites;
        private Sprite mouseScreenDot, mouseGuiDot, mouseWorldDot;
        private Camera guiCam, worldCam;

        public EditorScreen(ScreenManager sm)
        {
            screenManager = sm;
            var virtualSize = new Vector2(1280, 720);
            guiCam = new Camera(virtualSize, Vector2.Divide(virtualSize, 2), sm.GraphicsDevice);
            worldCam = new Camera(virtualSize*0.75f, Vector2.Zero, sm.GraphicsDevice);

            var dotTex = new Texture2D(sm.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            dotTex.SetData(new Color[] { Color.White });

            guiSprites   = new List<Sprite>();
            worldSprites = new List<Sprite>();

            var dot = new Sprite(dotTex, Vector2.Zero);
            dot.Color = new Color(255, 0, 0, 128);
            dot.Origin = new Vector2(-0.1f, -0.1f);
            dot.Scale = new Vector2(50, 50);
            guiSprites.Add(dot);

            dot = new Sprite(dotTex, guiCam.VirtualSize);
            dot.Color = new Color(255, 0, 0, 128);
            dot.Origin = new Vector2(1.1f, 1.1f);
            dot.Scale = new Vector2(50, 50);
            guiSprites.Add(dot);

            dot = new Sprite(dotTex, guiCam.VirtualSize/2f);
            dot.Color = new Color(0, 255, 0, 128);
            dot.Origin = new Vector2(1,1);
            dot.Scale = new Vector2(200,100);
            guiSprites.Add(dot);

            worldSprites.Add(new Sprite
            (
                sm.Content.Load<Texture2D>("images/background"),
                Vector2.Zero
            ));

            worldSprites.Add(new Sprite
            (
                sm.Content.Load<Texture2D>("images/hero"),
                new Vector2(100,100)
            ));

            worldSprites.Add(new Sprite
            (
                sm.Content.Load<Texture2D>("images/apple"),
                new Vector2(150,100)
            ));

            mouseScreenDot = new Sprite(dotTex, Vector2.Zero);
            mouseScreenDot.Scale = new Vector2(10,10);
            mouseScreenDot.Color = new Color(255,0,0,128);

            mouseGuiDot = mouseScreenDot.GetCopy();
            mouseGuiDot.Color = new Color(0,255,0,128);

            mouseWorldDot = mouseScreenDot.GetCopy();
            mouseWorldDot.Color = new Color(0,0,255,128);
        }

        public void Update(float dt)
        {
            worldCam.Zoom += Mouse.ScrollDelta() / 1000f;
            if (Mouse.LeftDown())
            {
                worldCam.Position -= Mouse.MoveDelta().ToVector2() / worldCam.Scale;
            }

            var mouseScreenPos = Mouse.Position().ToVector2();
            var mouseGuiPos = Vector2.Transform
            (
                mouseScreenPos,
                Matrix.Invert(guiCam.Transform)
            );
            var mouseWorldPos = Vector2.Transform
            (
                mouseScreenPos,
                Matrix.Invert(worldCam.Transform)
            );
            mouseScreenDot.Position = mouseScreenPos;
            mouseGuiDot.Position    = mouseGuiPos;
            mouseWorldDot.Position  = mouseWorldPos;
        }

        public void Draw(float alpha)
        {
            screenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            worldCam.Update(screenManager.GraphicsDevice);
            screenManager.batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, worldCam.Transform);
            foreach (var it in worldSprites)  it.Draw(screenManager.batch);
            mouseWorldDot.Draw(screenManager.batch);
            screenManager.batch.End();

            guiCam.Update(screenManager.GraphicsDevice);
            screenManager.batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, guiCam.Transform);
            foreach (var it in guiSprites)  it.Draw(screenManager.batch);
            mouseGuiDot.Draw(screenManager.batch);
            screenManager.batch.End();

            screenManager.batch.Begin();
            mouseScreenDot.Draw(screenManager.batch);
            screenManager.batch.End();
        }

        public void Resize()
        {
            //
        }

        public void Pause()
        {
            //
        }

        public void Resume()
        {
            //
        }

        public void Dispose()
        {
            foreach (var it in guiSprites)    it.Dispose();
            foreach (var it in worldSprites)  it.Dispose();
        }
    }
}
