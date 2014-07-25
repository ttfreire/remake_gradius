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
    class Garun : Enemy
    {
      float center;
      float offset; //The offset to add to your Y
      float radius; //Whatever you want your radius to be
      public enum EnemyState {NONE,  FORWARD, DIAGONAL, RETREAT, EXPLODED }
      public EnemyState m_currentState = EnemyState.FORWARD;
      public AnimationController m_animator;
      float m_timeToDie;

      public Garun(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;

          int[] garunAnimationFramesMoving = { 25, 26, 27, 28, 27, 26, 25 };
          Animation garunAnimationMoving = new Animation(PlayType.Loop, garunAnimationFramesMoving, 10.0f);
          int[] garunAnimationFramesExploded = { 80, 81, 82, 83 };
          Animation garunAnimationExploded = new Animation(PlayType.Once, garunAnimationFramesExploded, 5.0f);
          Dictionary<string, Animation> garunAnimations = new Dictionary<string, Animation>() { { "moving", garunAnimationMoving },
                                                                                        { "exploded", garunAnimationExploded} };
          m_animator = new AnimationController(m_world.m_spriteEnemies, garunAnimations, 5, 18);
          currAnimation = "moving";

          center = m_pos.Y;
          offset = 0; //The offset to add to your Y
          radius = 50; //Whatever you want your radius to be
      }

    public override void Update(GameTime gameTime) 
    {
        m_animator.Update(gameTime, currAnimation);
        switch (m_currentState)
        {
            case EnemyState.FORWARD:
                {
                    m_dir = new Vector2(-1, 0);
                    double msElapsed = gameTime.TotalGameTime.TotalMilliseconds / 150;
                    offset = (float)Math.Sin(msElapsed) * radius;
                    m_pos.Y = center + offset;
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

          if (m_animator != null)
              spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, Color.White, 0.0f,
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
