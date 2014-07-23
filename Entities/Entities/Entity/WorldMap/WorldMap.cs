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
        public Map m_map;
        public Rectangle m_view;
        public Vector2 m_screenMiddle;
        public int screenWidth;
        public int screenHeigth;
        bool isMoving = true;
        bool eventIsFinished = false;
        float eventTimerMax, eventTimer;
        bool bossIsFinished = false;
        float bossTimerMax, bossTimer;
        public WorldMap(Game1 world, Map map, Rectangle bounds):base(world)
        {
            m_map = map;
            m_view = bounds;
            m_screenMiddle = new Vector2(m_view.Width, m_view.Height) / 2;
            screenWidth = m_view.Width;
            screenHeigth = m_view.Height;
            eventTimerMax = (float)m_map.ObjectLayers["event"].MapObjects[0].Properties["timer"].AsInt32;
            eventTimer = eventTimerMax;
            bossTimerMax = (float)m_map.ObjectLayers["event"].MapObjects[1].Properties["timer"].AsInt32;
            bossTimer = bossTimerMax;
        }

        public override void Update(GameTime gameTime)
        {

            //MAP COLLIDER
            for (int o = 0; o < m_map.ObjectLayers["colliders"].MapObjects.Length; o++)
            {
                if (m_map.ObjectLayers["colliders"].MapObjects[o].Bounds != null)
                {
                    Movable mov;
                    foreach (Entity e in m_world.m_entities)
                    {
                        if (e is Player || e is Projectile)
                        {
                            mov = (Movable)e;
                            if (m_map.ObjectLayers["colliders"].MapObjects[o].Bounds.Intersects(new Rectangle((int)(mov.m_pos.X - mov.m_size.X / 2) + m_view.X,
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
                Vector2 event_pos = new Vector2(m_map.ObjectLayers["event"].MapObjects[0].Bounds.X - m_view.X,
                                                m_map.ObjectLayers["event"].MapObjects[0].Bounds.Y - m_view.Y);
                Vector2 boss_pos = new Vector2(m_map.ObjectLayers["event"].MapObjects[1].Bounds.X - m_view.X,
                                                m_map.ObjectLayers["event"].MapObjects[1].Bounds.Y - m_view.Y);

                if ((event_pos.X <= screenWidth && !eventIsFinished) ||
                    (boss_pos.X <= screenWidth && !bossIsFinished))
                {
                    isMoving = false;
                }
            if(isMoving)
                m_view.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 33);
            else
            {
                if (!eventIsFinished)
                {
                    eventTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (eventTimer <= 0)
                    {
                        isMoving = true;
                        eventIsFinished = true;
                        m_world.m_entities.RemoveAll(s => s is Volcano);
                    }
                }
                if (eventIsFinished && !bossIsFinished)
                {
                    bossTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (bossTimer <= 0)
                    {
                        //isMoving = true;
                        bossIsFinished = true;
                        m_world.m_entities.RemoveAll(s => s is Boss);
                    }
                }
            }
            }
        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            m_map.Draw(spriteBatch, m_view);
        }


    }
}
