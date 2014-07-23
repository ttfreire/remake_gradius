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
    public GameTime m_gametime;
    public GraphicsDeviceManager m_graphics;
    SpriteBatch m_spriteBatch;

    public Texture2D m_spriteViper;
    public Texture2D m_spriteFan;
    public Texture2D m_spriteEnemies;

    public Texture2D m_spritePowerUpRed;
    public Texture2D m_spritePowerUpBlue;
    public Texture2D m_spriteProjectile;
    public Texture2D m_spriteBoss;

    public Texture2D m_spriteHUDpowerups;
    public Texture2D m_spriteHUDlife;
    public SpriteFont m_spriteFont;
    
    Map m_background;
    Rectangle mapView;

    public List<Entity> m_entities = new List<Entity>();
    List<Entity> to_add = new List<Entity>();
    List<Entity> to_remove = new List<Entity>();
    Player m_player;
    public WorldMap m_worldMap;

    public List<PowerUpType> HUDPowerUp;
    public int highlightedPowerUp = 0;
    public int powerUpCounter = 0;

    SpawnController enemySpawnController;
    HUDController m_hudController;
    
    

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
      HUDPowerUp = new List<PowerUpType>();
      HUDPowerUp.Add(PowerUpType.NONE);
      HUDPowerUp.Add(PowerUpType.SPEEDUP);
      HUDPowerUp.Add(PowerUpType.MISSILE);
      HUDPowerUp.Add(PowerUpType.DOUBLE);
      HUDPowerUp.Add(PowerUpType.LASER);
      HUDPowerUp.Add(PowerUpType.OPTION);
      HUDPowerUp.Add(PowerUpType.SHIELD);

      enemySpawnController = new SpawnController(this);
    }

    protected override void LoadContent() {

      m_spriteBatch = new SpriteBatch(GraphicsDevice);

      m_spriteViper = Content.Load<Texture2D>("ship-score");
      m_spriteProjectile = Content.Load<Texture2D>("projectile-shield");
      m_background = Content.Load<Map>("newmap");
      m_worldMap = new WorldMap(this, m_background, m_graphics.GraphicsDevice.Viewport.Bounds);
      m_spriteFan = Content.Load<Texture2D>("fan");
      m_spritePowerUpRed = Content.Load<Texture2D>("powerupred");
      m_spritePowerUpBlue = Content.Load<Texture2D>("powerupblue");
      m_spriteEnemies = Content.Load<Texture2D>("enemy-powerups");
      m_spriteBoss = Content.Load<Texture2D>("boss");
      m_spriteHUDpowerups = Content.Load<Texture2D>("powerups");
      m_spriteHUDlife = Content.Load<Texture2D>("life");
      m_spriteFont = Content.Load<SpriteFont>("Arial");
      
      //add map
      //m_entities.Add(m_worldMap);
        

      //add HUD
      HUDpowerup speedHUD = new HUDpowerup(5, m_spriteHUDpowerups, PowerUpType.SPEEDUP, 0, 0);
      HUDpowerup missileHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.MISSILE, 0, 1);
      HUDpowerup doubleHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.DOUBLE, 0, 2);
      HUDpowerup laserHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.LASER, 0, 3);
      HUDpowerup optionHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.OPTION, 0, 4);
      HUDpowerup shieldHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.SHIELD, 0, 5);
      List<HUDpowerup> listHUDPowerups = new List<HUDpowerup> { speedHUD, missileHUD, doubleHUD, laserHUD, optionHUD, shieldHUD };

      m_hudController = new HUDController(listHUDPowerups, m_spriteHUDlife, new Vector2 (290, 440), m_spriteFont);


      //add player...
      m_entities.Add(new Player(this, new Vector2(40, 240), //pos
                                new Vector2(m_spriteViper.Width, m_spriteViper.Height), 
                                100, //vel
                                800, // maxvel
                                10, // friction
                                50.0f, // rate of fire
                                500.0f, // continuous rate of fire
                                m_spriteViper, MovableType.Player, m_spriteProjectile, null));
      m_player = (Player) m_entities[0];
      
    }

    protected override void UnloadContent() {}

    protected override void Update(GameTime gameTime) {

      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();
      m_gametime = gameTime;
      float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

      m_hudController.Update(gameTime);
        
        
        enemySpawnController.Update(gameTime);
      

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

      m_hudController.Draw(gameTime, m_spriteBatch);
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
