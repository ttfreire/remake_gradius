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
        public Vector2 m_screenMiddle;
        public int screenWidth;
        public int screenHeigth;
        public WorldMap(Game1 world, Map map, Rectangle bounds):base(world)
        {
            m_map = map;
            m_view = bounds;
            m_screenMiddle = new Vector2(m_view.Width, m_view.Height) / 2;
            screenWidth = m_view.Width;
            screenHeigth = m_view.Height;
        }

        public override void Update(GameTime gameTime)
        {
            //MAP COLLIDER
            for (int ol = 0; ol < m_map.ObjectLayers.Count; ol++)
            {
                for (int o = 0; o < m_map.ObjectLayers[ol].MapObjects.Length; o++)
                {
                    if (m_map.ObjectLayers[ol].MapObjects[o].Bounds != null)
                    {
                        Movable mov;
                        foreach (Entity e in m_world.m_entities)
                        {
                            if (e is Player || e is Projectile)
                            {
                                mov = (Movable)e;
                                if (m_map.ObjectLayers[ol].MapObjects[o].Bounds.Intersects(new Rectangle((int)(mov.m_pos.X - mov.m_size.X / 2) + m_view.X,
                                                                                                  (int)(mov.m_pos.Y - mov.m_size.Y / 2),
                                                                                                     (int)mov.m_size.X,
                                                                                                  (int)mov.m_size.Y)))
                                {
                                    mov.Die();
                                }
                            }
                        }
                    }

                }
            }
                m_view.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 30);
            }
        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            m_map.Draw(spriteBatch, m_view);
        }


    }
}
