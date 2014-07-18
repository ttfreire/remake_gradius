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

namespace Gradius {

  public class Character : Movable {
    public AnimationController m_animator;
    public int currState;

    public Vector2 m_dir = Vector2.Zero;
    public Vector2 m_vel = Vector2.Zero;

    public float m_maxVel;
    public float m_accel;
    public float m_friction;
    public float m_rateOfFire;
    public float m_continuousRateOfFire;

    public Texture2D m_sprite;
    public Vector2 m_spriteSize;
    public Texture2D m_ProjectileSprite;
    public Vector2 m_ProjectileSpriteSize;

    public float m_depth = 0.5f;
    public List<Entity> visibleEntities;

    public Character(Game1 world, Vector2 pos, Vector2 size, float maxVel,
        float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite, MovableType type, Texture2D projectileSprite, AnimationController animator)
        : base(world, pos, size, type)
    {
      
      m_maxVel   = maxVel;
      m_accel    = accel;
      m_friction = friction;
      m_rateOfFire = rateoffire;
      m_continuousRateOfFire = continuousrateoffire;
      m_ProjectileSprite = projectileSprite;
      m_ProjectileSpriteSize = new Vector2(m_ProjectileSprite.Width, m_ProjectileSprite.Height);

      m_sprite = sprite;
      m_spriteSize = new Vector2(m_sprite.Width, m_sprite.Height);
      visibleEntities = new List<Entity>();
      m_animator = animator;
      currState = 0;
    }

    public override void Update(GameTime gameTime) {
        if (m_animator != null)
        {
            if(this.m_type == MovableType.Fan)
                m_animator.Update(gameTime, 0);
            else
                m_animator.Update(gameTime, currState);
            currState = m_animator.entityCurrentState;
        }
      float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

      //normalize direction:
      float d = m_dir.Length();
      if (d > 0.0f)
        m_dir = m_dir / d;

      //apply friction to velocity:
      if (d <= 0.0f) {

        float v = m_vel.Length();

        if (v > 0.0f) {

          float mul = (v - v * m_friction * dt) / v;

          m_vel = m_vel * mul;
        }
      }

      //update position:

      { //without collision:

         // m_pos = m_pos + m_vel * dt;
      }

      { //with collision:

        for (int axis = 0; axis <= 1; axis++)
        {
          Vector2 prevPos = m_pos;

          if (axis == 0)
            m_pos.X = m_pos.X + m_vel.X * dt;
          else
            m_pos.Y = m_pos.Y + m_vel.Y * dt;

          bool isColliding = false;
          foreach (Entity e in m_world.m_entities)
          {
            if (e != this && e is Character)
            {
              if (e.TestCollision(this))
              {
                isColliding = true;
                break;
              }
            }
          }

          if (isColliding)
          {
            if (axis == 0)
              m_vel.X = 0.0f;
            else
              m_vel.Y = 0.0f;

            m_pos = prevPos;
          }
        }

      }

      //update velocity with acceleration:

      m_vel = m_vel + m_dir * m_accel * dt;

      //limit velocity within max velocity:
      {
        float v = m_vel.Length();
        if (v > m_maxVel) {

          m_vel = m_vel / v;
          m_vel = m_vel * m_maxVel;
        }
      }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

        if (m_animator != null)
          spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, Color.White, 0.0f,
          new Vector2(m_animator.m_currentSpriteRect.Width, m_animator.m_currentSpriteRect.Height)/2, 2, SpriteEffects.None, m_depth);
      else
        spriteBatch.Draw(m_sprite, m_pos, null, Color.White, 0.0f,
          new Vector2(m_sprite.Width, m_sprite.Height)/2, 2, SpriteEffects.None, m_depth);
    }

    public virtual void Shoot(Vector2 shotVel, Vector2 shotPos, Vector2 shotDir)
    {
        Projectile shot = new Projectile(m_world, shotPos, m_ProjectileSpriteSize, m_ProjectileSprite, shotVel, shotDir, MovableType.Projectile, this);
        this.m_world.Add(shot);
    }

    public override bool TestVision(Movable other, MovableType type)
    {
        Vector2 myHalf = m_size * 0.5f;
        Vector2 myMin = m_pos - myHalf - new Vector2(256, 256);
        Vector2 myMax = m_pos + myHalf + new Vector2(256, 256);

        Vector2 otherHalf = other.m_size * 0.5f;
        Vector2 otherMin = other.m_pos - otherHalf;
        Vector2 otherMax = other.m_pos + otherHalf;

        if (other.getType() == type)
        {
            if ((myMax.X < otherMin.X) || (myMax.Y < otherMin.Y) ||
                (myMin.X > otherMax.X) || (myMin.Y > otherMax.Y))
            {
                return false;
            }
            return true;
        }
        else return false;
    }
  }
}
