using Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cameras
{
    public class Camera:ITaggable
    {
        public Vector2 position { get; set; }
        public string name { get; set; } = "camera";
        public float zoom { get; set; } = 1f;
        public Vector2 offset { get; set; } = Vector2.Zero;
        public Vector2 resolution { get; private set; }
        private IPosition objectToFollow { get; set; } 
        public bool isCentered { get; private set; }

        public Matrix transformMatrix { get; private set; }
        public Matrix textMatrix { get; private set; }
        public Matrix uiMatrix { get; private set; }

        private bool isShaking { get; set; } = false;
        private float shakeDuration { get; set; } = 0;
        private float shakeIntensity { get; set; } = 1;

        private Random random { get; set; }

        public Camera(Vector2 position, float zoom,  Vector2 resolution,string name = "camera")
        {
            this.position = position;
            this.zoom = zoom;
            this.resolution = resolution;
            this.name = name;
            transformMatrix = new Matrix();
            random = new Random();
        }
        public void SetCentered(bool isCentered)
        {
            this.isCentered = isCentered;
        }
        public void SetObjectToFollow(Object objectToFollow)
        {
            if (objectToFollow is IPosition obj)
                this.objectToFollow = obj;
            else
                this.objectToFollow = default;
        }
        public Camera SetShaking(bool shaking,float duration,float intensity = 1f)
        {
            isShaking = shaking;
            shakeDuration = duration;
            shakeIntensity = intensity;
            return this;
        }
        public void Update(GameTime gameTime)
        {
            transformMatrix = Matrix.CreateScale(zoom, zoom, 1);
            uiMatrix = Matrix.CreateScale(zoom, zoom, 1);
            if (isShaking)
            {
                shakeDuration-=(float)gameTime.ElapsedGameTime.TotalSeconds;
                offset = Vector2.Zero;
                offset = new Vector2((float)random.Next(-100, 100) / 100, (float)random.Next(-100, 100) / 100)*shakeIntensity;
            }
            if(shakeDuration <= 0){ isShaking=false;offset = Vector2.Zero; }

            if (objectToFollow != null)
            {
                transformMatrix *= Matrix.CreateTranslation(-objectToFollow.position.X, -objectToFollow.position.Y, 0);
            }

            transformMatrix *=Matrix.CreateTranslation(offset.X+position.X, offset.Y+position.Y, 0);
            transformMatrix = Matrix.CreateScale(1, 1, 1);

            uiMatrix *= Matrix.CreateTranslation(offset.X,offset.Y,0);

            textMatrix = Matrix.CreateTranslation(zoom+offset.X, zoom+offset.Y, 0);
            textMatrix *= transformMatrix;
            //textMatrix *= Matrix.CreateScale(2, 2, 1);

            transformMatrix = Matrix.CreateScale(zoom, zoom, 1);

        }


    }
}
