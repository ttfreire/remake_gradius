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

  public enum MovableType { None, Player, Enemy, Projectile, Option };

  public class Movable : Entity {

    public Vector2 m_pos;
    public Vector2 m_size;
    public MovableType m_type;
    public bool m_isAnimatedByState;
    public Movable(Game1 world):base(world) { }
    public Movable(Game1 world, Vector2 pos, Vector2 size, MovableType type, bool isAnimatedByState)
        : base(world)
    {
      
      m_pos  = pos;
      m_size = size;
      m_type = type;
      m_isAnimatedByState = isAnimatedByState;
    }

    public override bool TestCollision(Movable other) {

      Vector2 myHalf = m_size * 0.5f;
      Vector2 myMin  = m_pos - myHalf;
      Vector2 myMax  = m_pos + myHalf;

      Vector2 otherHalf = other.m_size * 0.5f;
      Vector2 otherMin  = other.m_pos - otherHalf;
      Vector2 otherMax  = other.m_pos + otherHalf;

      if (this.m_type == other.m_type)
          return false;
      if ((myMax.X < otherMin.X) || (myMax.Y < otherMin.Y) ||
          (myMin.X > otherMax.X) || (myMin.Y > otherMax.Y)) {

        return false;
      }
      
      return true;
    }

    public MovableType getType(){
        return m_type;
    }

      public virtual void Die()
      {
          m_world.Remove(this);
      }
  }
}
