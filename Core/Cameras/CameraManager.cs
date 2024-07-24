using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cameras;
using Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Cameras
{
    internal class CameraManager : IComponent
    {
        public List<Camera> cameras;
        public Camera currentCamera { get; private set; }

        public CameraManager()
        {
            cameras = new List<Camera>();
        }
        public void LoadContent(ContentManager contentManager)
        {
        }


        public CameraManager AddCamera(Camera camera)
        {
            cameras.Add(camera);
            return this;
        }

        public Camera GetCamera(string cameraName)
        {
            return cameras.Where(p => p.name == cameraName).FirstOrDefault();
        }
        
        public CameraManager SetCurrentCamera(string cameraName)
        {
            currentCamera = GetCamera(cameraName);
            return this;
        }
        public Matrix GetCurrentMatrix()
        {
            return currentCamera.transformMatrix;
        }
        public Camera GetCurrentCamera() { return  currentCamera; }

        public void Update(GameTime gameTime)
        {
            currentCamera.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

    }
}
