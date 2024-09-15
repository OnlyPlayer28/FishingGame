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
    internal abstract class IScene : IComponent
    {
        public string name { get; }
        public bool isActive { get; protected set; }
        public bool isDrawing { get; protected set; }

        public List<IComponent> components { get; set; } = new List<IComponent>();

        public IScene(string name, bool isActive = false, bool isDrawing = false)
        {
            this.name = name;
            this.isActive = isActive;
            this.isDrawing = isDrawing;
        }

        public  virtual IScene SetActive(bool active)
        {
            isActive = active;
            InputManager.inputState = GameInputState.Gameplay;
            return this;
        }

        public virtual IScene SetDrawing(bool drawing)
        {
            isDrawing = drawing;
            return this;
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (var component in components)
            {
                component.LoadContent(contentManager);
            }
        }
        public virtual void Update(GameTime gameTime) 
        {
            foreach (var item in components)
            {
                item.Update(gameTime);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach(var item in components)
            {
                item.Draw(spriteBatch);
                
            }
        }
        public abstract void DrawUI(SpriteBatch spriteBatch);
        public abstract void DrawText(SpriteBatch spriteBatch);
    }
}
