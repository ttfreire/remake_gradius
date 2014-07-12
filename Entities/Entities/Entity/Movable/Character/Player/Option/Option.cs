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

    public class Option : Player
    {
        static float SPEEDUP_INCREASE = 50.0f;
        KeyboardState previousKey = Keyboard.GetState();
        KeyboardState currentKey = Keyboard.GetState();
        float shootCooldown;
        float continuousShootCooldown;
        Player m_player;
        int m_trail_pos;

        public Option(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite, MovableType type, Texture2D ProjectileSprite, Player player) :
            base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, ProjectileSprite)
        {
            shootCooldown = rateoffire;
            continuousShootCooldown = continuousrateoffire;
            m_depth -= 0.1f;
            m_player = player;
            int trail_pos = Math.Abs(m_player.m_trail_pos - 50) % TRAIL_SIZE;
            m_trail_pos = trail_pos;
        }

        public override void Update(GameTime gameTime)
        {

            //fill direction vector using the keyboard:
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            shootCooldown -= dt;
            continuousShootCooldown -= dt;
            m_dir = Vector2.Zero;
            currentKey = Keyboard.GetState();

            this.m_pos = m_player.m_trail[m_trail_pos];
            m_trail_pos = Math.Abs(m_player.m_trail_pos + 50) % TRAIL_SIZE;

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



        public override void Shoot()
        {
            if (m_player.activePowerUps.Contains(PowerUpState.MISSILE))
            {
                Vector2 shotVel = new Vector2(250, 250);
                Vector2 shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
                Vector2 shotDir = new Vector2(1, 1);
                Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
                this.m_world.Add(shot);
            }
            if (m_player.activePowerUps.Contains(PowerUpState.DOUBLE))
            {
                Vector2 shotVel = new Vector2(250, -250);
                Vector2 shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
                Vector2 shotDir = new Vector2(1, -1);
                Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
                this.m_world.Add(shot);
            }
            if (m_player.activePowerUps.Contains(PowerUpState.LASER))
            {
                Vector2 shotVel = new Vector2(800, 0);
                Vector2 shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
                Vector2 shotDir = new Vector2(1, 0);
                Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize + new Vector2(25, 0), m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
                this.m_world.Add(shot);
            }
            else
            {
                base.Shoot();
            }
        }
        
        public override bool TestCollision(Movable other)
        {
            if (other.m_type != MovableType.Player)
                return base.TestCollision(other);
            else return false;
        }
    }

     
}