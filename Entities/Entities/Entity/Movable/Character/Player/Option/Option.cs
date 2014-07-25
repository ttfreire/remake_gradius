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

    public class Option : Player
    {
        static float SPEEDUP_INCREASE = 50.0f;
        KeyboardState previousKey = Keyboard.GetState();
        KeyboardState currentKey = Keyboard.GetState();
        float shootCooldown;
        float continuousShootCooldown;
        Player m_player;
        int m_option_trail_pos, m_initial_trail_pos;
        public AnimationController m_animator;

        public Option(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, 
                        Texture2D sprite, MovableType type, Texture2D ProjectileSprite, Player player, int initial_trail_pos ) :
            base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, ProjectileSprite, null)
        {
            shootCooldown = rateoffire;
            continuousShootCooldown = continuousrateoffire;
            m_depth -= 0.1f;
            m_player = player;
            m_initial_trail_pos = initial_trail_pos;
            int trail_pos = Math.Abs(m_player.m_trail_pos - m_initial_trail_pos) % TRAIL_SIZE;
            m_option_trail_pos = trail_pos;

            int[] optionAnimationFrames = { 85, 86 };
            Animation optionAnimation = new Animation(PlayType.Loop, optionAnimationFrames, 5.0f);
            int[] optionAnimationFramesExploded = { 87};
            Animation optionAnimationExploded = new Animation(PlayType.Loop, optionAnimationFramesExploded, 5.0f);
            Dictionary<string, Animation> optionAnimations = new Dictionary<string, Animation>() { { "forward", optionAnimation }, 
                                                                                                   { "up", optionAnimation }, 
                                                                                                   { "down", optionAnimation },
                                                                                                   { "exploded", optionAnimationExploded }};
            m_animator = new AnimationController(m_world.m_spriteEnemies, optionAnimations, 5, 18);
            currAnimation = "forward";
        }

        public override void Update(GameTime gameTime)
        {
            m_animator.Update(gameTime, currAnimation);
            
            //fill direction vector using the keyboard:
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            shootCooldown -= dt;
            continuousShootCooldown -= dt;
            m_dir = Vector2.Zero;
            currentKey = Keyboard.GetState();

            this.m_pos = m_player.m_trail[m_option_trail_pos];
            m_option_trail_pos = Math.Abs(m_player.m_trail_pos + m_initial_trail_pos) % TRAIL_SIZE;

            if (m_pos.X + m_size.X / 2 > 512)
                m_pos.X = 512 - m_size.X / 2;
            if (m_pos.Y + m_size.Y / 2 > 480)
                m_pos.Y = 480 - m_size.Y / 2;
            if (m_pos.X - m_size.X / 2 < 0)
                m_pos.X = m_size.X / 2;
            if (m_pos.Y - m_size.Y / 2 < 0)
                m_pos.Y = m_size.Y / 2;

            base.Update(gameTime);
        }



        public override void Shoot(Vector2 shotVel, Vector2 shotPos, Vector2 shotDir, ProjectileType type)
        {
            if (m_player.activePowerUps.Contains(PowerUpType.MISSILE))
            {
                shotVel = new Vector2(250, 250);
                shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
                shotDir = new Vector2(1, 1);
                Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, ProjectileType.MISSILE, this);
                this.m_world.Add(shot);
            }
            if (m_player.activePowerUps.Contains(PowerUpType.DOUBLE))
            {
                shotVel = new Vector2(250, -250);
                shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
                shotDir = new Vector2(1, -1);
                Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, ProjectileType.DOUBLE, this);
                this.m_world.Add(shot);
            }
            if (m_player.activePowerUps.Contains(PowerUpType.LASER))
            {
                shotVel = new Vector2(800, 0);
                shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
                shotDir = new Vector2(1, 0);
                Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize + new Vector2(25, 0), m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, ProjectileType.LASER, this);
                this.m_world.Add(shot);
            }
            else
            {
                base.Shoot(shotVel, shotPos, shotDir, ProjectileType.STANDARD);
            }
        }
        
        public override bool TestCollision(Movable other)
        {
            if (other.m_type != MovableType.Player)
                return base.TestCollision(other);
            else return false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (m_animator != null)
                spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, Color.White, 0.0f,
                new Vector2(m_animator.m_currentSpriteRect.Width, m_animator.m_currentSpriteRect.Height) / 2, 2, SpriteEffects.None, m_depth);
        }
    }

     
}
