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
        public string currAnimation;
        AnimationController m_animator;

        public PowerUp(Game1 world, Vector2 powerupPos) : base(world)
        {
            int[] powerUpAnimationFrames;
            Animation powerUpAnimation;

            if (world.powerUpCounter == 15)
            {
                m_color = PowerUpColor.BLUE;
                m_sprite = world.m_spriteEnemies;
                world.powerUpCounter = 0;
                powerUpAnimationFrames = new int[]{ 70, 71, 72 };
                powerUpAnimation = new Animation(PlayType.Once, powerUpAnimationFrames, 3.0f);
            }
            else
            {
                m_color = PowerUpColor.RED;
                m_sprite = world.m_spriteEnemies;
                powerUpAnimationFrames = new int[] { 75, 76, 77 };
                powerUpAnimation = new Animation(PlayType.Once, powerUpAnimationFrames, 3.0f);
            }
            Dictionary<string, Animation> powerUpAnimations = new Dictionary<string,Animation>() { { "default", powerUpAnimation } };
            m_animator = new AnimationController(m_world.m_spriteEnemies, powerUpAnimations, 5, 18);
            currAnimation = "default";
            m_pos = powerupPos;
        }

        public override void Update(GameTime gameTime)
        {
            m_animator.Update(gameTime, currAnimation);
            m_pos.X -= Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 33);
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
                            if (m_world.m_hudController.m_selectedPowerupIndex < m_world.m_hudController.m_powerupHUD.Count)
                            {
                                m_world.highlightedPowerUp += 1;
                                m_world.m_hudController.nextPowerupHUD();
                            }
                            
                        }
                        else
                        {
                            foreach (Entity ent in m_world.m_entities)
                                if (ent is Enemy)
                                    m_world.Remove(ent);
                        }
                        m_world.Remove(this);
                        m_world.m_hudController.updateScore(500);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (m_animator != null)
                spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, Color.White, 0.0f,
                new Vector2(m_animator.m_currentSpriteRect.Width, m_animator.m_currentSpriteRect.Height) / 2, 2, SpriteEffects.None, 0);
        }

        public bool TestCollision(Player other)
        {
            if(other.m_type == MovableType.Player)
            {
                Vector2 myHalf = new Vector2(m_animator.m_currentSpriteRect.Width * 0.5f, m_animator.m_currentSpriteRect.Height * 0.5f);
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
            return false;
        }
    
    }
}
