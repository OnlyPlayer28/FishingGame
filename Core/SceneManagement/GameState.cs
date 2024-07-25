using Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SceneManagement
{
    internal abstract class Scene : IComponent
    {
        public string name { get; }
        public bool isActive { get; protected set; }
        public bool isDrawing { get; protected set; }

        public List<IComponent> components { get; set; } = new List<IComponent>();

        public Scene(string name, bool isActive = false, bool isDrawing = false)
        {
            this.name = name;
            this.isActive = isActive;
            this.isDrawing = isDrawing;
        }

        public Scene SetActive(bool active)
        {
            isActive = active;
            return this;
        }

        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void DrawUI(SpriteBatch spriteBatch);
    }
}
