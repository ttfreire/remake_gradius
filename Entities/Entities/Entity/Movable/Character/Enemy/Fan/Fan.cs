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
    class Fan : Enemy
    {
      
      public enum EnemyState { NONE, FORWARD, DIAGONAL, RETREAT, EXPLODED }
      public EnemyState m_currentState = EnemyState.FORWARD;
      public AnimationController m_animator;
      float m_timeToDie;

      public Fan(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
                    MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;

          int[] fanAnimationFramesMoving = { 0, 1, 2, 3 };
          Animation fanAnimationMoving = new Animation(PlayType.Loop, fanAnimationFramesMoving, 11.0f);
          int[] fanAnimationFramesExploded = { 80, 81, 82, 83 };
          Animation fanAnimationExploded = new Animation(PlayType.Once, fanAnimationFramesExploded, 5.0f);

          Dictionary<string, Animation> fanAnimations = new Dictionary<string, Animation>() { { "moving", fanAnimationMoving },
                                                                                        { "exploded", fanAnimationExploded} };
          m_animator = new AnimationController(m_world.m_spriteEnemies, fanAnimations, 5, 18);
          currAnimation = "moving";
      }

    public override void Update(GameTime gameTime) {
        m_animator.Update(gameTime, currAnimation);

        currentAnimationState = (int)m_currentState;
        switch (m_currentState)
        {
            case EnemyState.FORWARD:
                {
                    m_dir.X -= 1.0f;

                    if (m_pos.X < worldmap.m_screenMiddle.X)
                    {
                        m_currentState = EnemyState.DIAGONAL;
                        m_vel = Vector2.Zero;
                    }
                }
                break;
            case EnemyState.DIAGONAL:
                {
                    if (m_pos.Y <= worldmap.m_screenMiddle.Y)
                    {
                        m_dir.X += 1.0f;
                        m_dir.Y += 2.0f;
                    }
                    else
                    {
                        m_dir.X += 1.0f;
                        m_dir.Y -= 2.0f;
                    }
                    foreach (Entity e in m_world.m_entities)
                    {
                        if (e is Player)
                        {
                            Player player = (Player)e;
                            if (m_pos.Y >= player.m_pos.Y - player.m_spriteSize.Y / 4 &&
                                m_pos.Y <= player.m_pos.Y + player.m_spriteSize.Y / 4)
                            {
                                m_currentState = EnemyState.RETREAT;
                                m_vel = Vector2.Zero;
                            }
                            break;
                        }
                    }

                }
                break;
            case EnemyState.RETREAT:
                {
                    m_dir = Vector2.UnitX;
                    m_maxVel = 300;

                    if (m_pos.X > worldmap.screenWidth)
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
