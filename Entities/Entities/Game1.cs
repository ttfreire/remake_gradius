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
using FuncWorks.XNA.XTiled;
namespace Gradius {

  public class Game1 : Microsoft.Xna.Framework.Game {

    public GraphicsDeviceManager m_graphics;
    SpriteBatch m_spriteBatch;

    Texture2D m_spriteViper;
    Texture2D m_spriteFan;
    public Texture2D m_spriteBasicProjectile;
    Map m_background;
    Rectangle mapView;

    public List<Entity> m_entities = new List<Entity>();
    List<Entity> to_add = new List<Entity>();
    List<Entity> to_remove = new List<Entity>();

    WorldMap m_worldMap;

    public Game1() {

      m_graphics = new GraphicsDeviceManager(this);

      m_graphics.PreferredBackBufferWidth  = 512;
      m_graphics.PreferredBackBufferHeight = 480;
      m_graphics.ApplyChanges();

      Content.RootDirectory = "Content";
    }

    protected override void Initialize() {

      base.Initialize();
      mapView = m_graphics.GraphicsDevice.Viewport.Bounds;
    }

    protected override void LoadContent() {

      m_spriteBatch = new SpriteBatch(GraphicsDevice);

      m_spriteViper = Content.Load<Texture2D>("ship");
      m_spriteBasicProjectile = Content.Load<Texture2D>("basic_projectile");
      m_background = Content.Load<Map>("map2");
      m_worldMap = new WorldMap(this, m_background, m_graphics.GraphicsDevice.Viewport.Bounds);
      m_spriteFan = Content.Load<Texture2D>("fan");
      
      //add map
      //m_entities.Add(m_worldMap);

      //add player...
      m_entities.Add(new Player(this, new Vector2(40, 240), new Vector2(m_spriteViper.Width, m_spriteViper.Height), 100, 800, 10, 0.5f, 1.0f, m_spriteViper, MovableType.Player, m_spriteBasicProjectile));

      //add enemy
      m_entities.Add(new Enemy(this, new Vector2(450, 100), new Vector2(m_spriteFan.Width, m_spriteFan.Height), 200, 800, 800, 0, 0, m_spriteFan, MovableType.Player, m_spriteBasicProjectile, m_worldMap));
      m_entities.Add(new Enemy(this, new Vector2(500, 100), new Vector2(m_spriteFan.Width, m_spriteFan.Height), 200, 800, 800, 0, 0, m_spriteFan, MovableType.Player, m_spriteBasicProjectile, m_worldMap));
      m_entities.Add(new Enemy(this, new Vector2(550, 100), new Vector2(m_spriteFan.Width, m_spriteFan.Height), 200, 800, 800, 0, 0, m_spriteFan, MovableType.Player, m_spriteBasicProjectile, m_worldMap));
      m_entities.Add(new Enemy(this, new Vector2(600, 100), new Vector2(m_spriteFan.Width, m_spriteFan.Height), 200, 800, 800, 0, 0, m_spriteFan, MovableType.Player, m_spriteBasicProjectile, m_worldMap));
      m_entities.Add(new Enemy(this, new Vector2(650, 100), new Vector2(m_spriteFan.Width, m_spriteFan.Height), 200, 800, 800, 0, 0, m_spriteFan, MovableType.Player, m_spriteBasicProjectile, m_worldMap));    
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

      m_worldMap.Update(gameTime);

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

      m_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

      m_worldMap.Draw(gameTime, m_spriteBatch);
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
