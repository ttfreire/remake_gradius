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

  public class Entity {

    public Game1 m_world;
    private static int m_Counter = 0;
    public int m_id;

    public Entity(Game1 world) { 
      
      m_world = world;
      this.m_id = System.Threading.Interlocked.Increment(ref m_Counter);
    }

    public virtual void Update(GameTime gameTime) {}

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {}

    public virtual bool TestCollision(Movable other) { return false; }

    public virtual bool TestVision(Movable other, MovableType type) { return false; }

    
  }
}
