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
    public  class CameraManager 
    {
        public static List<Camera> cameras = new List<Camera>();
        public static Camera currentCamera { get; private set; }


        public static void LoadContent(ContentManager contentManager)
        {
        }


        public static void AddCamera(Camera camera)
        {
            cameras.Add(camera);
        }

        public static Camera GetCamera(string cameraName)
        {
            return cameras.Where(p => p.name == cameraName).FirstOrDefault();
        }
        
        public static  void SetCurrentCamera(string cameraName)
        {
            currentCamera = GetCamera(cameraName);
        }
        public static Matrix GetCurrentMatrix()
        {
            return currentCamera.transformMatrix;
        }
        public static Camera GetCurrentCamera() { return  currentCamera; }

        public static void Update(GameTime gameTime)
        {
            currentCamera.Update(gameTime);
        }


    }
}
