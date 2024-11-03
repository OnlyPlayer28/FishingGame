using Core;
using Core.Components;
using Core.Debug;
using Core.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Core.UI.Elements
{
    internal class Bar : IUIElement
    {
        public Vector2 position { get ; set; }
        public Vector2 size { get; set ; }
        public bool isActive { get; set; }
        public string name { get; set; }
        public float layer { get; set; }
        private bool displayProgressOnMouseOver { get; set; } = false;
        private float value { get; set; } = 0;

        public  float minValue { get;private set; }
        public float maxValue { get; private set; }
        private Rect fillRectangle { get; set; }
        private Color fillColor {  get; set; } = Color.Red;
        public Bar(Vector2 position, Vector2 size, bool isActive, string name, float layer, bool displayProgressOnMouseOver)
        {
            this.position = position;
            this.size = size;
            this.isActive = isActive;
            this.name = name;
            this.layer = layer;
            this.displayProgressOnMouseOver = displayProgressOnMouseOver;
        }
        public Bar SetMaxAndMinValues(float min, float max)
        {
            this.minValue = min;
            this.maxValue = max;
            return this;
        }
        public float GetValue()
        {
            return value;
        }
        public Bar SetValue(float value)
        {
            this.value = value;
            return this;
        }
        public Bar DecreaseValue(float value)
        {
            this.value -=value;
            return this;
        }
        public Bar SetFillColor(Color color)
        {
            this.fillColor = color;
            return this;
        }

        public Bar SetProgressDisplayOnMouseOver(bool displayProgress)
        {
            displayProgressOnMouseOver = displayProgress;
            return this;    
        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void LoadContent(ContentManager contentManager)
        {

        }

        public void OnMouseOver(object sender, EventArgs e)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void OnMouseOver(Object o,MouseInputEventArgs e)
        {
            if (displayProgressOnMouseOver)
            {

            }
        }
    }
}
