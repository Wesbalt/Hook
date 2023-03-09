using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Hook.Logger;
using static System.Diagnostics.Debug;
using System;

namespace Hook
{
    public class Camera
    {
        public static readonly float MAX_ZOOM = 2;
        public static readonly float MIN_ZOOM = 0.2f;

        private Matrix transform;
        private Vector2 virtualSize;
        private Vector2 position;
        private Vector2 scale;
        private float zoom;

        public Camera(Vector2 virtualSize, Vector2 position, GraphicsDevice graphics)
        {
            this.virtualSize = virtualSize;
            this.position = position;
            zoom = 1;
            Update(graphics);
        }

        public Matrix Transform
        {
            get { return transform; }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                if (value > MAX_ZOOM)
                {
                    Debug("camera zoom is too large, setting it to {0} instead", MAX_ZOOM);
                    zoom = MAX_ZOOM;
                }
                else if (value < MIN_ZOOM)
                {
                    Debug("camera zoom is too small, setting it to {0} instead", MIN_ZOOM);
                    zoom = MIN_ZOOM;
                }
                else
                {
                    zoom = value;
                }
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
        }

        public Vector2 VirtualSize
        {
            get { return virtualSize; }
        }

        public void Update(GraphicsDevice graphics)
        {
            var view = graphics.Viewport;

            scale = new Vector2
            (
                view.Width  / virtualSize.X * zoom,
                view.Height / virtualSize.Y * zoom
            );

            transform = Matrix.CreateTranslation(-position.X, -position.Y, 0)
                * Matrix.CreateRotationZ(0)
                * Matrix.CreateScale(new Vector3(scale, 1))
                * Matrix.CreateTranslation(view.Width/2f, view.Height/2f, 0);
        }
    }

    // @TODO I'm garbage
    public class Font
    {
        private SpriteFont  font;
        private Color      color;
        private float      degrees;  // In degrees, going clockwise.

        // (0,0) is the upper left corner of the
        // text, (1,1) is the lower right corner.
        // Default is (0.5, 0.5), the center.
        private Vector2  origin;

        private Vector2  scale;

        public Font(SpriteFont font, Color color, float degrees, Vector2 origin, Vector2 scale)
        {
            this.font     = font;
            this.origin   = origin;
            this.scale    = scale;
            this.color    = color;
            SetDegrees(degrees);
        }

        public Font(SpriteFont font, Color color, float degrees, Vector2 origin)
            : this(font, color, degrees, origin, Vector2.One) {}

        public Font(SpriteFont font, Color color, float degrees)
            : this(font, color, degrees, new Vector2(0.5f,0.5f), Vector2.One) {}

        public Font(SpriteFont font, Color color)
            : this(font, color, 0, new Vector2(0.5f,0.5f), Vector2.One) {}

        public Font(SpriteFont font)
            : this(font, Color.White, 0, new Vector2(0.5f,0.5f), Vector2.One) {}

        public void Draw(SpriteBatch batch, string text, Vector2 position)
        {
            var pixelOrigin = origin; // @TODO fix me!
            batch.DrawString
            (
                font,
                text,
                position,
			    color,
			    degrees,
			    pixelOrigin,
                scale,
			    SpriteEffects.None,
                0  // layerDepth
            );
        }

        public void SetFont(SpriteFont font)
        {
            this.font = font;
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void SetDegrees(float degrees)
        {
            this.degrees = MathHelper.ToRadians(degrees);
        }

        public void SetOrigin(Vector2 origin)
        {
            this.origin = origin;
        }

        public void SetScale(Vector2 scale)
        {
            this.scale = scale;
        }

        public Font GetCopy()
        {
            return new Font(font, color, degrees, origin, scale);
        }
    }

    /*
     * Stores properties you can pass to SpriteBatch.Draw
     * such as texture, position and rotation.
     *
     * Rotation is clockwise. The origin (0,0) is the upper
     * left corner of the texture, (1,1) is the lower right
     * corner. Remember to dispose the texture!
     */
    public class Sprite
    {
        private Texture2D  texture;
        private Vector2    position;
        private Vector2    scale;
        private Vector2    origin;
        private float      degrees;
        private Color      color;

        // These are to avoid recalculations
        private float    radians;
        private bool     doUpdateRadians;
        private Vector2  pixelOrigin;
        private bool     doUpdatePixelOrigin;

        /*
         * Makes a Sprite with sensible defaults.
         */
        public Sprite(Texture2D texture, Vector2 position)
        : this(texture, position, Vector2.One, new Vector2(0.5f, 0.5f), 0, Color.White) {}

        public Sprite(Texture2D texture, Vector2 position, Vector2 scale,
                      Vector2   origin,  float   degrees,  Color   color)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            this.origin = origin;
            this.degrees = degrees;
            this.color = color;

            // Initial values don't matter since they will be recalculated immediately
            this.radians             = 0;
            this.doUpdateRadians     = true;
            this.pixelOrigin         = Vector2.Zero;
            this.doUpdatePixelOrigin = true;
        }

        public void Draw(SpriteBatch batch)
        {
            if (doUpdateRadians)
            {
                radians = MathHelper.ToRadians(degrees);
                doUpdateRadians = false;
            }

            if (doUpdatePixelOrigin)
            {
                pixelOrigin.X = origin.X * texture.Width;
                pixelOrigin.Y = origin.Y * texture.Height;
                doUpdatePixelOrigin = false;
            }

            batch.Draw
            (
                texture,
                position,
                null,  // source rectangle
			    color,
			    radians,
			    pixelOrigin,
                scale,
			    SpriteEffects.None,
                0  // layerDepth
            );
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; doUpdatePixelOrigin = true; }
        }

        // @Speed
        public Rectangle Bounds
        {
            get
            {
                var upperLeftCorner = new Point
                (
                    (int) (position.X - origin.X * scale.X + 0.5f),
                    (int) (position.Y - origin.Y * scale.Y + 0.5f)
                );
                return new Rectangle
                (
                    upperLeftCorner.X,
                    upperLeftCorner.Y,
                    (int) (texture.Width  * scale.X + 0.5f),
                    (int) (texture.Height * scale.Y + 0.5f)
                );
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; doUpdatePixelOrigin = true; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; doUpdatePixelOrigin = true; }
        }

        public float Degrees
        {
            get { return degrees; }
            set { degrees = value; doUpdateRadians = true; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Sprite GetCopy()
        {
            return new Sprite(texture, position, scale, origin, degrees, color);
        }

        public void Dispose()
        {
            texture.Dispose();
        }
    }
}
