using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Hook.Logger;
using System;
using System.Collections.Generic;

namespace Hook
{
    public class FontTestScreen : IScreen
    {
        private ScreenManager screenManager;
        private Sprite guy, dot, backdrop;
        private Font compressedFont, notCompressedFont, compressedFontUpscaled, notCompressedFontUpscaled, compressedFontDownscaled, notCompressedFontDownscaled;
        private Camera cam;

        public FontTestScreen(ScreenManager sm)
        {
            screenManager = sm;
            cam = new Camera(new Vector2(1280, 720), new Vector2(160, 120), sm.GraphicsDevice);

            var dotTex = new Texture2D(sm.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            dotTex.SetData(new Color[] { Color.White });
            dot = new Sprite(dotTex, new Vector2(320,240));
            dot.Color = Color.Red;
            dot.Origin = new Vector2(0.5f,0.5f);
            dot.Scale = new Vector2(10,10);

            backdrop = dot.GetCopy();
            backdrop.Color = Color.Wheat;
            backdrop.Origin = Vector2.Zero;
            backdrop.Position = new Vector2
            (
                cam.Position.X - cam.VirtualSize.X/2f,
                cam.Position.Y - cam.VirtualSize.Y/2f
            );
            backdrop.Scale = cam.VirtualSize;

            var tex = sm.Content.Load<Texture2D>("images/hero");
            guy = new Sprite(tex, new Vector2(100,100));
            guy.Scale = new Vector2(0.5f,0.5f);
            guy.Origin = new Vector2(0.2f,1);

            compressedFont    = new Font(sm.Content.Load<SpriteFont>("fonts/Calibri-36-Compressed"));
            notCompressedFont = new Font(sm.Content.Load<SpriteFont>("fonts/Calibri-36-Not-Compressed"));
            notCompressedFont = new Font(sm.Content.Load<SpriteFont>("fonts/Calibri-36-Not-Compressed"));

            compressedFontUpscaled = compressedFont.GetCopy();
            compressedFontUpscaled.SetScale(new Vector2(2,2));
            notCompressedFontUpscaled = notCompressedFont.GetCopy();
            notCompressedFontUpscaled.SetScale(new Vector2(2,2));
            compressedFontDownscaled = compressedFont.GetCopy();
            compressedFontDownscaled.SetScale(new Vector2(0.5f,0.5f));
            notCompressedFontDownscaled = notCompressedFont.GetCopy();
            notCompressedFontDownscaled.SetScale(new Vector2(0.5f,0.5f));
        }

        public void Update(float dt)
        {
            if (Keyboard.KeyDown(Keys.W))
            {
                cam.Zoom += 0.05f;
            }
            if (Keyboard.KeyDown(Keys.S))
            {
                cam.Zoom -= 0.05f;
            }

            var move = cam.Position;
            if (Keyboard.KeyDown(Keys.Left))   move.X -= 1;
            if (Keyboard.KeyDown(Keys.Right))  move.X += 1;
            if (Keyboard.KeyDown(Keys.Up))     move.Y -= 1;
            if (Keyboard.KeyDown(Keys.Down))   move.Y += 1;
            cam.Position = move;

            guy.Degrees += 360*dt/1000f; // One turn per second
        }

        public void Draw(float alpha)
        {
            cam.Update(screenManager.GraphicsDevice);

            screenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            screenManager.batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.Transform);
            backdrop.Draw(screenManager.batch);
            guy.Draw(screenManager.batch);
            dot.Draw(screenManager.batch);

            var phrase = "foobar";
            compressedFont.             Draw(screenManager.batch, phrase, new Vector2(200,0));
            compressedFontDownscaled.   Draw(screenManager.batch, phrase, new Vector2(200,50));
            compressedFontUpscaled.     Draw(screenManager.batch, phrase, new Vector2(200,100));
            notCompressedFont.          Draw(screenManager.batch, phrase, new Vector2(200,200));
            notCompressedFontDownscaled.Draw(screenManager.batch, phrase, new Vector2(200,250));
            notCompressedFontUpscaled.  Draw(screenManager.batch, phrase, new Vector2(200,300));

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
            guy.Dispose();
        }
    }
}
