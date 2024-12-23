using Core.Components;
using Core.UI;
using Core.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI.Elements
{
    public class ArrowButtonsEventArgs : EventArgs
    {
        public float value;
        public string name;
    }
    public class ArrowButtons : ICLickable, IUIElement
    {
        public float value { get;private set; }
        public float valueIncrement { get; private set; } = 1f;
        private Button[] arrows = new Button[2];

        public EventHandler<ArrowButtonsEventArgs> OnValueChangedEvent;

        private Vector2? minAndMaxValues { get; set; } = null;

        public override Vector2 position { get ; set ; }
        public override Vector2 size { get; set ; }
        public override bool isActive { get; set; }
        public override float layer { get; set      ; }
        public bool ignoreMouseInput { get ; set ; }
        public string name { get; set; }
        private Color color { get; set; }
        public ArrowButtons(Vector2 position, int spacing, Sprite arrowSprite, bool isActive, Canvas canvas, string onCLickSound = "", string name = "default", Color color = default)
            : base(position, new Vector2((arrowSprite.size.X * 2) + spacing, arrowSprite.size.Y), isActive)
        {
            ignoreMouseInput = true;
            arrows = new Button[2] {
                new Button(new Sprite(arrowSprite).setPosition(position).SetColor(color),true,onCLickSound).SetButtonName("left"),
                new Button(new Sprite(arrowSprite).SetSpriteEffects(SpriteEffects.FlipHorizontally).SetColor(color).setPosition(new Vector2(position.X+arrowSprite.size.X+spacing,position.Y)),true,onCLickSound).SetButtonName("right")
            };

            arrows[0].OnButtonClickEvent += OnArrowsClick;
            arrows[1].OnButtonClickEvent += OnArrowsClick;
            canvas.AddClickableElement(arrows[0]);
            canvas.AddClickableElement(arrows[1]);

            this.name = name;
            this.color = color;
        }
        public ArrowButtons SetMinAndMaxValues (float min, float max)
        {
            minAndMaxValues = new Vector2 (min,max);
            return this;
        }
        public ArrowButtons SetValue(float value)
        {
            this.value = value;
            return this;
        }
         
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!isActive) return;

            arrows[0].Draw(spriteBatch);
            arrows[1].Draw(spriteBatch);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            arrows[0].LoadContent(contentManager);
            arrows[1].LoadContent(contentManager);
        }
        private void OnArrowsClick(object o,ButtonEventArgs e)
        {
            if (e.buttonRef.name == "left") { value = minAndMaxValues is null ? value-valueIncrement : value == minAndMaxValues.Value.X?minAndMaxValues.Value.Y:value-valueIncrement; }
            else { value = minAndMaxValues is null ? value + valueIncrement : value == minAndMaxValues.Value.Y ? minAndMaxValues.Value.X : value + valueIncrement; }
            OnValueChangedEvent?.Invoke(this,new ArrowButtonsEventArgs { value = value ,name=name});
        }
        public override void OnMouseClick()
        {
        }

        public override void OnMouseOver(object sender, EventArgs e)
        {
        }

        public override void SetPositionAndBoundingBox()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(!isActive && arrows[0].isActive) { arrows[0].isActive = false; arrows[1].isActive = false; }
            else  if(isActive&& !arrows[0].isActive) { arrows[0].isActive = true; arrows[1].isActive = true; }
            if(!isActive){return;}
            arrows[0].Update(gameTime);
            arrows[1].Update(gameTime);
        }
    }
}
