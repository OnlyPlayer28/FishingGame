﻿using Core;
using Core.Components;
using Core.Debug;
using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.UI.Elements
{
    public class Button : ICLickable,IUIElement
    {
        public override bool isActive { get ; set ; }
        public  string name { get ; set; }
        public override Vector2 position 
        {
            get;
            set;
        }
        public override Vector2 size { get; set; }

        private Vector2 _size;

        public Text buttonText { get; set; }
        public override float layer 
        {
            get;
            set;
        }

        public Rect buttonSpriteSimple { get; set; }

        public Action onButtonClickAction { get; set; }

        private Sprite buttonSprite { get; set; }

        private Color originalColor { get; set; }
        private Color highlightColor { get; set; } = Color.LightGray;
        public Button(  Vector2 position, Vector2 size, float layer,string name = "defaultButton",bool isActive = true)
            :base(position,size,isActive)
        {
            this.isActive = isActive;
            this.name = name;
            this.position = position;
            this.size = size;
            this.layer = layer;
        }
        public Button SetOnButtonCLickAction(Action onButtonClickAction)
        {
            this.onButtonClickAction = onButtonClickAction;
            return this;
        }
        public Button SetSimpleSprite(Color outline,Color fill)
        {
            buttonSpriteSimple = new Rect(position, size, outline, true, layer).SetFillColor(fill);
            originalColor = buttonSpriteSimple.color;
            return this;
        }
        public Button SetButtonSprite(Sprite sprite,ContentManager content)
        {
            this.buttonSprite=sprite;
            buttonSprite.LoadContent(content);
            originalColor = buttonSprite.color;
            return this;
        }

        public Button SetHighlightColor(Color color)
        {
            highlightColor = color;
            return this;
        }
        public Button SetButtonText(string text,Color color,SpriteFont font)
        {
  
            float _layer = layer - .0001f;
            _layer = Math.Clamp(_layer, 0, 1);
            Console.WriteLine(_layer);
            buttonText = new Text(position+Vector2.One, text, color, font,layer:_layer);
            return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isActive) { return; }
            if(buttonSpriteSimple != null)
                buttonSpriteSimple.Draw(spriteBatch);
            if (buttonSprite != null) { 
                buttonSprite.Draw(spriteBatch);}
        }

        public void DrawText(SpriteBatch spriteBatch)
        {
            if (!isActive) { return; }
            if (buttonText != null)
            {
                buttonText.Draw(spriteBatch);
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!isActive) { return; }
            if (buttonSprite == null)
            {
                buttonSpriteSimple.color = originalColor;
            }
            else
            {
                buttonSprite.color = originalColor;
            };
        }
        public override void OnMouseClick()
        {
            onButtonClickAction?.Invoke();
        }

        public override void SetPositionAndBoundingBox()
        {
        }

        public override void OnMouseOver(object sender, EventArgs e)
        {
            if (buttonSprite == null) 
            { 
                buttonSpriteSimple.color = highlightColor; 
            }
            else 
            {
                buttonSprite.color = highlightColor;
            };
        }
    }
}
