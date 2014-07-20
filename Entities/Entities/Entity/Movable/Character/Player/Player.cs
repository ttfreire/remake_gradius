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
    public enum PlayerState { NONE, UP, FORWARD, DOWN }
    public PlayerState currentState = PlayerState.FORWARD;
    static float SPEEDUP_INCREASE = 50.0f;
    public static int TRAIL_SIZE = 100;
    KeyboardState previousKey = Keyboard.GetState();
    KeyboardState currentKey = Keyboard.GetState();
    float shootCooldown;
    float continuousShootCooldown;
    public List<Vector2> m_trail;
    public int m_trail_pos = 0;
    int m_option_count;
    AnimationController m_animator;

    public List<PowerUpState> activePowerUps;

    public Player(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire,
                    Texture2D sprite, MovableType type, Texture2D ProjectileSprite, AnimationController animator) :
        base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, ProjectileSprite)
    {
        shootCooldown = rateoffire;
        continuousShootCooldown = continuousrateoffire;
        m_depth -= 0.1f;
        activePowerUps = new List<PowerUpState>();
        m_trail = new List<Vector2>();
        for(int i = 0; i < TRAIL_SIZE; i++)
            m_trail.Add(this.m_pos);

        int[] playerAnimationFramesUp = { 0};
        Animation playerAnimationUp = new Animation(PlayType.Once, playerAnimationFramesUp, 3.0f);
        int[] playerAnimationFramesForward = { 1};
        Animation playerAnimationForward = new Animation(PlayType.Once, playerAnimationFramesForward, 3.0f);
        int[] playerAnimationFramesDown = { 2 };
        Animation playerAnimationDown = new Animation(PlayType.Once, playerAnimationFramesDown, 3.0f);
        int[] playerAnimationFramesExploded = { 4, 5, 6, 7 };
        Animation playerAnimationExploded = new Animation(PlayType.Once, playerAnimationFramesExploded, 3.0f);
        Dictionary<string, Animation> playerAnimations = new Dictionary<string,Animation>() { { "up", playerAnimationUp },
                                                                                        { "forward", playerAnimationForward },
                                                                                        { "down", playerAnimationDown },
                                                                                        { "exploded", playerAnimationExploded } };
        m_animator = new AnimationController(m_world.m_spriteViper, playerAnimations, 4, 3, this);
        currAnimation = "forward";
  }

    public override void Update(GameTime gameTime) {
      m_animator.Update(gameTime, currAnimation);
      float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
      shootCooldown -= dt;
      continuousShootCooldown -= dt;
      m_dir = Vector2.Zero;
      currentKey = Keyboard.GetState();

      if (currentKey.IsKeyUp(Keys.Z))
          previousKey = currentKey;

      if (currentKey.IsKeyDown(Keys.Right))
      {
          m_dir.X += 1.0f;
          m_trail[m_trail_pos] = this.m_pos;
          m_trail_pos = (m_trail_pos + 1) % TRAIL_SIZE;
      }

      if (currentKey.IsKeyDown(Keys.Left))
      {
          m_dir.X += -1.0f;
          m_trail[m_trail_pos] = this.m_pos;
          m_trail_pos = (m_trail_pos + 1) % TRAIL_SIZE;
      }

      if (currentKey.IsKeyDown(Keys.Up))
      {
          m_dir.Y += -1.0f;
          m_trail[m_trail_pos] = this.m_pos;
          m_trail_pos = (m_trail_pos + 1) % TRAIL_SIZE;
          currentState = PlayerState.UP;
          currAnimation = "up";
      }

      if (currentKey.IsKeyDown(Keys.Down))
      {
          m_dir.Y += 1.0f;
          m_trail[m_trail_pos] = this.m_pos;
          m_trail_pos = (m_trail_pos + 1) % TRAIL_SIZE;
          currentState = PlayerState.DOWN;
          currAnimation = "down";
      }

      if (currentKey.IsKeyUp(Keys.Down) && currentKey.IsKeyUp(Keys.Up))
      {
          currentState = PlayerState.FORWARD;
          currAnimation = "forward";
      }

      if (currentKey.IsKeyDown(Keys.Z) && !previousKey.IsKeyDown(Keys.Z) && shootCooldown <= 0)
      {
          shootCooldown = m_rateOfFire;
          Shoot(new Vector2(800, 0), new Vector2(this.m_pos.X, this.m_pos.Y), new Vector2(1, 0));
      }

      if (currentKey.IsKeyDown(Keys.Z) && previousKey.IsKeyDown(Keys.Z) && continuousShootCooldown < 0)
      {
          continuousShootCooldown = m_continuousRateOfFire;
          Shoot(new Vector2(800, 0), new Vector2(this.m_pos.X, this.m_pos.Y), new Vector2(1, 0));
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
                m_maxVel += SPEEDUP_INCREASE;
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
                List<Entity> optionlist = m_world.m_entities.FindAll(s => s is Option);
                if (optionlist.Count < 2)
                {
                    activePowerUps.Add(PowerUpState.OPTION);
                    m_world.highlightedPowerUp = 0;
                    int option_trail = 25 + 25 * optionlist.Count-1;
                    Option option = new Option(m_world, this.m_pos - new Vector2(500,0), this.m_size / 2, this.m_maxVel, this.m_accel, this.m_friction, this.m_rateOfFire, this.m_continuousRateOfFire, this.m_sprite, MovableType.Option, this.m_ProjectileSprite, this, option_trail, null);
                    m_world.Add(option);
                    m_option_count = m_option_count + 1;
                }
            }
                break;
            case 6:
            {
                m_world.highlightedPowerUp = 0;
            }
                break;
        }
      }

    public override void Shoot(Vector2 shotVel, Vector2 shotPos, Vector2 shotDir)
    {
        if (activePowerUps.Contains(PowerUpState.LASER))
        {
            shotVel = new Vector2(800, 0);
            shotDir = new Vector2(1, 0);
            Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize + new Vector2(25, 0), m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
            this.m_world.Add(shot);
        }
        else
        {
            base.Shoot(shotVel, shotPos, shotDir);
        }
        if (activePowerUps.Contains(PowerUpState.MISSILE))
        {
            shotVel = new Vector2(250, 250);
            shotDir = new Vector2(1, 1);
            Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
            this.m_world.Add(shot);
        }
        if (activePowerUps.Contains(PowerUpState.DOUBLE))
        {
            shotVel = new Vector2(250, -250);
            shotDir = new Vector2(1, -1);
            Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
            this.m_world.Add(shot);
        }

    }

    public override bool TestCollision(Movable other)
    {
        if (other.m_type != MovableType.Option)
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
