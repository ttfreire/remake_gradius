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

  
  public class Enemy : Character {
      public WorldMap worldmap;
      public List<Enemy> mySquad;
      public Enemy(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite, 
          MovableType type, Texture2D projectileSprite, List<Enemy> squad) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite)
      {
          mySquad = squad;
      }

    public override void Update(GameTime gameTime) 
    {
      base.Update(gameTime);
    }

    public void addToSquad()
    {
        if(mySquad != null)
            mySquad.Add(this);
    }

    public override void Die()
    {
        Console.WriteLine("Enemy ID = " + m_id.ToString());
        if (mySquad != null && mySquad.Count == 1)
            dropPowerUp();
        mySquad.Remove(this);
        m_world.Remove(this);
    }

      public void dropPowerUp()
      {
          PowerUp powerup = new PowerUp(m_world, this.m_pos);
          m_world.Add(powerup);
          m_world.powerUpCounter++;
      }
  }


}
