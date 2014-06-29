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

namespace Gradius {

  public class Player : Character {
    KeyboardState previousKey = Keyboard.GetState();
    KeyboardState currentKey = Keyboard.GetState();
    float shootCooldown;
    float continuousShootCooldown;
    
    List<PowerUpState> activePowerUps;

    public Player(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite, MovableType type, Texture2D ProjectileSprite) :
        base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, ProjectileSprite)
    {
        shootCooldown = rateoffire;
        continuousShootCooldown = continuousrateoffire;
        m_depth -= 0.1f;
        activePowerUps = new List<PowerUpState>();
    }

    public override void Update(GameTime gameTime) {

      //fill direction vector using the keyboard:
      float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
      shootCooldown -= dt; 
      m_dir = Vector2.Zero;
      currentKey = Keyboard.GetState();

      if (currentKey.IsKeyUp(Keys.Z))
          previousKey = currentKey;

      if (currentKey.IsKeyDown(Keys.Right))
        m_dir.X += 1.0f;

      if (currentKey.IsKeyDown(Keys.Left))
      {
          m_dir.X += -1.0f;
          //previousKey = currentKey;
      }

      if (currentKey.IsKeyDown(Keys.Up))
      {
          m_dir.Y += -1.0f;
          //previousKey = currentKey;
      }

      if (currentKey.IsKeyDown(Keys.Down))
      {
          m_dir.Y += 1.0f;
          //previousKey = currentKey;
      }

      if (currentKey.IsKeyDown(Keys.Z) && !previousKey.IsKeyDown(Keys.Z) && shootCooldown <= 0)
      {
          shootCooldown = m_rateOfFire;
          Shoot();
      }

      if (currentKey.IsKeyDown(Keys.Z) && previousKey.IsKeyDown(Keys.Z))
      {
          continuousShootCooldown -= dt;
          if (continuousShootCooldown <= 0)
          {
              continuousShootCooldown = 1.0f;
              Shoot();
          }
      }

      if (currentKey.IsKeyDown(Keys.X))
      {
          this.usePowerUp();
      }

      if (m_pos.X + m_size.X / 2 > 512)
          m_pos.X = 512 - m_size.X / 2;
      if (m_pos.Y + m_size.Y / 2 > 480)
          m_pos.Y = 480 - m_size.Y / 2;
      if (m_pos.X - m_size.X / 2 < 0)
          m_pos.X = m_size.X / 2;
      if (m_pos.Y - m_size.Y / 2 < 0)
          m_pos.Y = m_size.Y / 2;

      previousKey = currentKey;

      base.Update(gameTime);
    }

    public void usePowerUp()
    {
        switch (m_world.highlightedPowerUp)
        {
            case 1:
            {
                if(!activePowerUps.Contains(PowerUpState.SPEEDUP))
                    activePowerUps.Add(PowerUpState.SPEEDUP);
                m_world.highlightedPowerUp = 0;
                m_maxVel += 100;
            }
                break;
            case 2:
            {
                if (!activePowerUps.Contains(PowerUpState.MISSILE))
                {
                    activePowerUps.Add(PowerUpState.MISSILE);
                    m_world.highlightedPowerUp = 0;
                }
            }
                break;
            case 3:
            {
                if (!activePowerUps.Contains(PowerUpState.DOUBLE))
                {
                    activePowerUps.Add(PowerUpState.DOUBLE);
                    m_world.highlightedPowerUp = 0;
                }
                    
                if (activePowerUps.Contains(PowerUpState.LASER))
                    activePowerUps.Remove(PowerUpState.LASER);
            }
                break;
            case 4:
            {
                if (!activePowerUps.Contains(PowerUpState.LASER))
                {
                    activePowerUps.Add(PowerUpState.LASER);
                    m_world.highlightedPowerUp = 0;
                }

                if (activePowerUps.Contains(PowerUpState.DOUBLE))
                    activePowerUps.Remove(PowerUpState.DOUBLE);
            }
                break;
            case 5:
            {
                m_maxVel += 100;
            }
                break;
            case 6:
            {
                m_maxVel += 100;
            }
                break;
        }
      }
    
    public override void Shoot()
    {
        if (activePowerUps.Contains(PowerUpState.MISSILE))
        {
            Vector2 shotVel = new Vector2(250, 250);
            Vector2 shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
            Vector2 shotDir = new Vector2(1, 1);
            Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
            this.m_world.Add(shot);
        }
        if (activePowerUps.Contains(PowerUpState.DOUBLE))
        {
            Vector2 shotVel = new Vector2(250, -250);
            Vector2 shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
            Vector2 shotDir = new Vector2(1, -1);
            Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
            this.m_world.Add(shot);
        }
        if (activePowerUps.Contains(PowerUpState.LASER))
        {
            Vector2 shotVel = new Vector2(5000, 0);
            Vector2 shotPos = new Vector2(this.m_pos.X + this.m_size.X / 2, this.m_pos.Y);
            Vector2 shotDir = new Vector2(1, 0);
            Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
            this.m_world.Add(shot);
        }
        else
        {
            base.Shoot();
        }
    }
    }
}
