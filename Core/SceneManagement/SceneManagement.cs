using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Components;
using Core.SceneManagement;

namespace Core.SceneManagement
{
    internal class SceneManagement : IComponent
    {
        private string? lastActiveScene = "";
        public List<IScene> gameStates { get; private set; }

        public SceneManagement(params IScene[] gameStates)
        {
            this.gameStates = gameStates.ToList();

        }
        public void LoadContent(ContentManager contentManager)
        {

            gameStates.ForEach(p => p.LoadContent(contentManager));
        }
        public SceneManagement SetActive(bool active, string sceneToSet)
        {
            gameStates.ForEach(p => p.SetActive(false));
            gameStates.First(p => p.name == sceneToSet).SetActive(active);
            return this;
        }
        /// <summary>
        /// Set's the desired scene to either be active or not, the other active scenes are going to get paused, but still be drawn.
        /// </summary>
        /// <param name="active"></param>
        /// <param name="sceneToSet"></param>
        /// <returns></returns>
        public SceneManagement SetActiveAndPause(bool active, string sceneToSet)
        {
            if (active)
            {

                lastActiveScene = gameStates.Where(p => p.isActive).First().name;
                gameStates.Where(p => p.isActive).First().SetDrawing(true);
                SetActive(true, sceneToSet);
            }
            else
            {
                gameStates.Where(p => p.name==lastActiveScene).First().SetDrawing(false);
                SetActive(true, lastActiveScene== null ?gameStates.First().name:lastActiveScene);
                lastActiveScene= null;
            }
            return this;
        }
        public IScene GetGameState(string name)
        {
            return gameStates.Where(p=>p.name ==  name).FirstOrDefault();
        }

        public IScene GetGameState<T>(T gameState)
        {
            return gameStates.Where(p=>p.GetType()== typeof(T)).FirstOrDefault();
        }

        public IScene GetActiveGameState()
        {
            return gameStates.Where(p=>p.isActive).FirstOrDefault();
        }
        public T GetActiveGameState<T>()
        {
            return gameStates.Where(p => p.isActive).FirstOrDefault() is T obj? obj:default;
        }
        public  void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in gameStates)
            {
                if(item.isActive||item.isDrawing) { item.Draw(spriteBatch); }
            }
        }

        public  void  DrawUI(SpriteBatch spriteBatch)
        {
            foreach (var item in gameStates)
            {
                if ((item.isActive || item.isDrawing)) { (item).DrawUI(spriteBatch); }
            }
        }

        public void DrawText(SpriteBatch spriteBatch)
        {
            foreach (var item in gameStates)
            {
                if ((item.isActive || item.isDrawing)) { (item).DrawText(spriteBatch); }
            }
        }

        public  void Update(GameTime gameTime)
        {
            foreach (var item in gameStates)
            {
                if (item.isActive) { item.Update(gameTime); }
            }
        }
    }
}
