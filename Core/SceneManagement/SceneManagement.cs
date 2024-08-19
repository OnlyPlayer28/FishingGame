﻿using Microsoft.Xna.Framework;
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
        public List<IScene> gameStates { get; private set; }

        public SceneManagement(params IScene[] gameStates)
        {
            this.gameStates = gameStates.ToList();

        }
        public  void LoadContent(ContentManager contentManager)
        {

            gameStates.ForEach(p=>p.LoadContent(contentManager));
        }
        public SceneManagement SetActive(bool active,string sceneToSet)
        {
            gameStates.ForEach(p => p.SetActive(false));
            gameStates.First(p=>p.name == sceneToSet).SetActive(active);
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