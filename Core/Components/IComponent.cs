using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    public interface  IComponent
    {
        public  void LoadContent(ContentManager contentManager);
        public  void Update(GameTime gameTime);
        public  void Draw(SpriteBatch spriteBatch);
    }
}
