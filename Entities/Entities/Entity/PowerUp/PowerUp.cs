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

namespace Gradius
{
    class PowerUp : Entity
    {
        
        public enum PowerUpColor { RED, BLUE }
        public PowerUpColor m_color;
        public Vector2 m_pos;
        public Texture2D m_sprite;
        public PowerUp(Game1 world, Vector2 powerupPos) : base(world)
        {
            if (world.powerUpCounter == 15)
            {
                m_color = PowerUpColor.BLUE;
                m_sprite = world.m_spritePowerUpBlue;
                world.powerUpCounter = 0;
            }
            else
            {
                m_color = PowerUpColor.RED;
                m_sprite = world.m_spritePowerUpRed;
            }
            m_pos = powerupPos;
        }

        public override void Update(GameTime gameTime)
        {
            m_pos.X -= Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 30);
            Player player;
            foreach (Entity e in m_world.m_entities)
            {
                if (e is Player)
                {
                    player = (Player)e;
                    if (this.TestCollision(player))
                    {
                        if (m_color == PowerUpColor.RED)
                        {
                            if (m_world.highlightedPowerUp < m_world.HUDPowerUp.Capacity - 1)
                                m_world.highlightedPowerUp += 1;
                            m_world.powerUpCounter++;
                        }
                        else
                        {
                            foreach (Entity ent in m_world.m_entities)
                                if (ent is Enemy)
                                    m_world.Remove(ent);
                        }
                        m_world.Remove(this);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(m_sprite, m_pos, Color.White);
        }

        public bool TestCollision(Player other)
        {

            Vector2 myHalf = new Vector2(m_sprite.Width * 0.5f, m_sprite.Height * 0.5f);
            Vector2 myMin = m_pos - myHalf;
            Vector2 myMax = m_pos + myHalf;

            Vector2 otherHalf = other.m_size * 0.5f;
            Vector2 otherMin = other.m_pos - otherHalf;
            Vector2 otherMax = other.m_pos + otherHalf;

            if ((myMax.X < otherMin.X) || (myMax.Y < otherMin.Y) ||
                (myMin.X > otherMax.X) || (myMin.Y > otherMax.Y))
            {

                return false;
            }

            return true;
        }
    
    }
}
