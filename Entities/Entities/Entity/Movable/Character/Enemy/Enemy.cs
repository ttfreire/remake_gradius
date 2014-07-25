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
      public bool m_dropsPowerUp;
      public Enemy(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, bool dropsPowerUp) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite)
      {
          mySquad = squad;
          m_dropsPowerUp = dropsPowerUp;
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
        if (mySquad != null)
        {
            if (mySquad.Count == 1)
                dropPowerUp();
            mySquad.Remove(this);
        }
  
        if (m_dropsPowerUp)
            dropPowerUp();
    }

      public void dropPowerUp()
      {
          PowerUp powerup = new PowerUp(m_world, this.m_pos);
          m_world.Add(powerup);
          m_world.powerUpCounter++;
      }
  }


}
