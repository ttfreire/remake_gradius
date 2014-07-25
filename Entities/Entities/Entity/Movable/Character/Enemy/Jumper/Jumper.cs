using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gradius
{
    class Jumper : Enemy
    {
      float center;
      float default_maxVel;
      public enum EnemyState { NONE, FORWARD, BACK, LEAVE, EXPLODED }
      public EnemyState m_currentState = EnemyState.FORWARD;
      public AnimationController m_animator;
      float m_timeToDie;

      public Jumper(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
          center = m_pos.Y;
          default_maxVel = maxVel;
          int[] JumperAnimationFrames = { 35, 36, 37 };
          Animation JumperAnimation = new Animation(PlayType.Once, JumperAnimationFrames, 3.0f);
          int[] JumperAnimationFramesExploded = { 80, 81, 82, 83 };
          Animation JumperAnimationExploded = new Animation(PlayType.Once, JumperAnimationFramesExploded, 5.0f);

          Dictionary<string, Animation> JumperAnimations = new Dictionary<string, Animation>() { { "moving", JumperAnimation },
                                                                                                { "exploded", JumperAnimationExploded} };
          m_animator = new AnimationController(m_world.m_spriteEnemies, JumperAnimations, 5, 18);
          currAnimation = "moving";
      }

    public override void Update(GameTime gameTime) 
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        m_animator.Update(gameTime, currAnimation);
        switch (m_currentState)
        {
            case EnemyState.FORWARD:
                {
                    m_dir = -Vector2.UnitX;
                    if (m_pos.Y >= m_world.m_worldMap.m_view.Height - 80)
                        m_vel = new Vector2(-m_maxVel / 2, -m_maxVel);
                    if (m_pos.Y <= m_world.m_worldMap.m_view.Height - 120)
                        m_vel = new Vector2(-m_maxVel / 2, m_maxVel);
                    if (m_pos.X < m_world.m_worldMap.m_screenMiddle.X / 2)
                        m_currentState = EnemyState.BACK;
                }
                break;
            case EnemyState.BACK:
                {
                    m_dir = Vector2.UnitX;
                    if (m_pos.Y >= m_world.m_worldMap.m_view.Height - 80)
                        m_vel = new Vector2(m_maxVel / 2, -m_maxVel);
                    if (m_pos.Y <= m_world.m_worldMap.m_view.Height - 120)
                        m_vel = new Vector2(m_maxVel / 2, m_maxVel);
                    if (m_pos.X > m_world.m_worldMap.m_screenMiddle.X + m_world.m_worldMap.m_screenMiddle.X / 2)
                        m_currentState = EnemyState.LEAVE;
                }
                break;
            case EnemyState.LEAVE:
                {
                    m_dir = -Vector2.UnitX;
                    if (m_pos.Y >= m_world.m_worldMap.m_view.Height - 80)
                        m_vel = new Vector2(-m_maxVel / 2, -m_maxVel);
                    if (m_pos.Y <= m_world.m_worldMap.m_view.Height - 120)
                        m_vel = new Vector2(-m_maxVel / 2, m_maxVel);

                    if (m_pos.X < 0)
                        m_world.Remove(this);

                }
                break;
            case EnemyState.EXPLODED:
                {
                    m_dir = Vector2.Zero;
                    m_vel = Vector2.Zero;
                    currAnimation = "exploded";

                    m_timeToDie -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_timeToDie <= 0)
                        m_world.Remove(this);
                }
                break;
        }
        base.Update(gameTime);
    }

      public void dropPowerUp()
      {
          PowerUp powerup = new PowerUp(m_world, this.m_pos);
          m_world.Add(powerup);
          m_world.powerUpCounter++;
      }

      public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
      {
          Color color = Color.White;
          if (m_dropsPowerUp)
              color = Color.Red;
          if (m_animator != null)
              spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, color, 0.0f,
              new Vector2(m_animator.m_currentSpriteRect.Width, m_animator.m_currentSpriteRect.Height) / 2, 2, SpriteEffects.None, m_depth);
      }

      public override void Die()
      {
          base.Die();
          m_currentState = EnemyState.EXPLODED;
          isdead = true;
          m_timeToDie = 0.5f;
      }
    }
}
