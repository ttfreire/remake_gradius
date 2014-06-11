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

    public class WorldMap : Entity
    {
        Map m_map;
        Rectangle m_view;

        public WorldMap(Game1 world, Map map, Rectangle bounds):base(world)
        {
            m_map = map;
            m_view = bounds;
        }

        public override void Update(GameTime gameTime) {
            m_view.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 4);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            m_map.Draw(spriteBatch, m_view);
        }


    }
}
