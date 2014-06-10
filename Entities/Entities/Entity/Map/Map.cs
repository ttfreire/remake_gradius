using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;

namespace Gradius
{
     public class Map : Entity
    {
             GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle mapView;
        Map m_background;

        protected override void Initialize()
        {
            mapView = graphics.GraphicsDevice.Viewport.Bounds;
        }

        
         
         protected override void Update(GameTime gameTime) {
             Rectangle delta = mapView;
         
                  delta.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 4);

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            m_background.Draw(gameTime, spriteBatch) ;
            spriteBatch.End();

            base.Draw(gameTime,spriteBatch);
        }
    }
}
