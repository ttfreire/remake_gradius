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

  public class Game1 : Microsoft.Xna.Framework.Game {

    GraphicsDeviceManager m_graphics;
    SpriteBatch m_spriteBatch;

    Texture2D m_spritePacman;
    Texture2D m_spriteGhost;

    public List<Entity> m_entities = new List<Entity>();
    List<Entity> to_add = new List<Entity>();
    List<Entity> to_remove = new List<Entity>();


    public Game1() {

      m_graphics = new GraphicsDeviceManager(this);

      m_graphics.PreferredBackBufferWidth  = 1280;
      m_graphics.PreferredBackBufferHeight = 800;
      m_graphics.ApplyChanges();

      Content.RootDirectory = "Content";
    }

    protected override void Initialize() {

      base.Initialize();
    }

    protected override void LoadContent() {

      m_spriteBatch = new SpriteBatch(GraphicsDevice);

      m_spritePacman = Content.Load<Texture2D>("pacman");
      m_spriteGhost  = Content.Load<Texture2D>("ghost");

      //add player...
      m_entities.Add(new Player(this, new Vector2(40, 240), new Vector2(32, 32), 200, 800, 10, m_spritePacman, MovableType.Player));
      m_entities.Add(new Player(this, new Vector2(100, 240), new Vector2(32, 32), 200, 800, 10, m_spritePacman, MovableType.Player));

      //add enemies...
      m_entities.Add(new Enemy(this, new Vector2(160, 240), new Vector2(32, 32), 100, 400, 10, m_spriteGhost, MovableType.Enemy));
      m_entities.Add(new Enemy(this, new Vector2(480, 240), new Vector2(32, 32), 100, 400, 10, m_spriteGhost, MovableType.Enemy));

      //Random r = new Random();
      //for(int c = 0; c < 100; c++)
      //  m_entities.Add(new Enemy(this,
      //    new Vector2((float)r.NextDouble() * 640.0f, (float)r.NextDouble() * 480.0f),
      //      new Vector2(32, 32), 100, 400, 10, m_spriteGhost));
    }

    protected override void UnloadContent() {}

    protected override void Update(GameTime gameTime) {

      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      
      // add new entities...
      if (to_add.Count > 0)
      {
          foreach (Entity e in to_add)
              m_entities.Add(e);
          to_add.Clear();
      }

      // remove entities...
      if (to_remove.Count > 0)
      {
          foreach (Entity e in to_remove)
              m_entities.Remove(e);
          to_remove.Clear();
      }

      //update all entities...
        foreach (Entity e in m_entities)
          e.Update(gameTime);

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {

      GraphicsDevice.Clear(Color.CornflowerBlue);

      m_spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

      //draw all entities...
      foreach(Entity e in m_entities)
        e.Draw(gameTime, m_spriteBatch);

      m_spriteBatch.End();

      base.Draw(gameTime);
    }

    public void Add(Entity item)
    {
        to_add.Add(item);
    }

    public void Remove(Entity item)
    {
        to_remove.Add(item);
    }
  }
}
