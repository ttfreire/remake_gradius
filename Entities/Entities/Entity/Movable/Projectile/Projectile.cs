﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gradius
{

    public class Projectile : Movable
    {

        public Vector2 m_dir = new Vector2(1,0);
        public Vector2 m_vel;

        public Texture2D m_sprite;
        public Vector2 m_spriteSize;

        public float m_depth = 0.5f;

        public Projectile(Game1 world, Vector2 pos, Vector2 size, Texture2D sprite, Vector2 velocity, MovableType type)
            : base(world, pos, size, type)
        {
            m_sprite = sprite;
            m_spriteSize = new Vector2(m_sprite.Width, m_sprite.Height);
            m_vel = velocity;
        }

        public override void Update(GameTime gameTime)
        {

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //normalize direction:
            float d = m_dir.Length();
            if (d > 0.0f)
                m_dir = m_dir / d;

            //update position:
            {
                for (int axis = 0; axis <= 1; axis++)
                {
                    Vector2 prevPos = m_pos;

                    if (axis == 0)
                        m_pos.X = m_pos.X + m_vel.X * dt;
                    else
                        m_pos.Y = m_pos.Y + m_vel.Y * dt;

                    Entity enemy = new Entity(m_world);
                    bool isColliding = false;
                    foreach (Entity e in m_world.m_entities)
                    {
                        if (e != this)
                        {
                            if (e.TestCollision(this))
                            {
                                isColliding = true;
                                enemy = e;
                                break;
                            }
                        }
                    }

                    if (isColliding)
                    {
                        m_world.Remove(enemy);
                        m_world.Remove(this);
                    }
                }

            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(m_sprite, m_pos, null, Color.White, 0.0f,
                m_spriteSize * 0.5f, m_size / m_spriteSize, SpriteEffects.None, m_depth);
        }
    }
}