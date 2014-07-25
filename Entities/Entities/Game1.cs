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
    public enum GameStates {PLAYING, PAUSED}
    public List<Keys> KONAMI_CODE = new List<Keys>() { Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A };
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
    public Texture2D m_spriteHatch;

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
    public HUDController m_hudController;

    GameStates m_currentGameState;
    KeyboardState m_currentKeyboardState, m_previousKeyboardState;

    List<Keys> m_cheat;
    float cheatResetTimer = 10000;
    bool bool_noriko = false;

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
      m_currentGameState = GameStates.PLAYING;

      highlightedPowerUp = 0;
      powerUpCounter = 0;
      cheatResetTimer = 10000;
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
      m_spriteHatch = Content.Load<Texture2D>("docs-bigblast");
      m_spriteBoss = Content.Load<Texture2D>("boss");
      m_spriteHUDpowerups = Content.Load<Texture2D>("powerups");
      m_spriteHUDlife = Content.Load<Texture2D>("life");
      //m_spriteFont = Content.Load<SpriteFont>("Arial");
      m_spriteFont = Content.Load<SpriteFont>("SpriteFont1");
      
      //add map
      //m_entities.Add(m_worldMap);
        

      //add HUD
      HUDpowerup speedHUD = new HUDpowerup(4, m_spriteHUDpowerups, PowerUpType.SPEEDUP, 0, 0);
      HUDpowerup missileHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.MISSILE, 0, 1);
      HUDpowerup doubleHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.DOUBLE, 0, 2);
      HUDpowerup laserHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.LASER, 0, 3);
      HUDpowerup optionHUD = new HUDpowerup(2, m_spriteHUDpowerups, PowerUpType.OPTION, 0, 4);
      HUDpowerup shieldHUD = new HUDpowerup(1, m_spriteHUDpowerups, PowerUpType.SHIELD, 0, 5);
      List<HUDpowerup> listHUDPowerups = new List<HUDpowerup> { speedHUD, missileHUD, doubleHUD, laserHUD, optionHUD, shieldHUD };

      m_hudController = new HUDController(listHUDPowerups, m_spriteHUDlife, new Vector2 (250, 440), m_spriteFont);


      //add player...
      m_entities.Add(new Player(this, new Vector2(40, 240), //pos
                                new Vector2(m_spriteViper.Width, m_spriteViper.Height), 
                                100, //vel
                                800, // maxvel
                                10, // friction
                                200.0f, // rate of fire
                                700.0f, // continuous rate of fire
                                m_spriteViper, MovableType.Player, m_spriteProjectile, null));
      m_player = (Player) m_entities[0];
      m_cheat = new List<Keys>();
    }

    protected override void UnloadContent() {
        m_entities.Clear();
    }

    protected override void Update(GameTime gameTime) {
        if (m_hudController.m_lives == 0)
        {
            UnloadContent();
            
            LoadContent();
            Initialize();
        }

        m_currentKeyboardState = Keyboard.GetState();
        cheatResetTimer -= gameTime.ElapsedGameTime.Milliseconds;
        if (cheatResetTimer <= 0)
        {
            cheatResetTimer = 10000;
            m_cheat.Clear();
            bool_noriko = false;
        }
        if (m_currentGameState == GameStates.PLAYING)
        {
            if (m_currentKeyboardState.IsKeyDown(Keys.Enter) && !m_previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                m_currentGameState = GameStates.PAUSED;
                m_previousKeyboardState = m_currentKeyboardState;
            }
            if (m_currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();
            if (m_currentKeyboardState.IsKeyUp(Keys.Enter))
            {
                m_previousKeyboardState = m_currentKeyboardState;
            }
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

        if (m_currentGameState == GameStates.PAUSED)
        {
            

            if (m_currentKeyboardState.IsKeyDown(Keys.Enter) && !m_previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                m_currentGameState = GameStates.PLAYING;
            }
            if (m_currentKeyboardState.IsKeyDown(Keys.Up) && m_previousKeyboardState.IsKeyUp(Keys.Up))
            {
                m_cheat.Add(Keys.Up);
            }
            if (m_currentKeyboardState.IsKeyDown(Keys.Down) && m_previousKeyboardState.IsKeyUp(Keys.Down))
            {
                m_cheat.Add(Keys.Down);
            }
            if (m_currentKeyboardState.IsKeyDown(Keys.Left) && m_previousKeyboardState.IsKeyUp(Keys.Left))
            {
                m_cheat.Add(Keys.Left);
            }
            if (m_currentKeyboardState.IsKeyDown(Keys.Right) && m_previousKeyboardState.IsKeyUp(Keys.Right))
            {
                m_cheat.Add(Keys.Right);
            }
            if (m_currentKeyboardState.IsKeyDown(Keys.A) && m_previousKeyboardState.IsKeyUp(Keys.A))
            {
                m_cheat.Add(Keys.A);
            }
            if (m_currentKeyboardState.IsKeyDown(Keys.B) && m_previousKeyboardState.IsKeyUp(Keys.B))
            {
                m_cheat.Add(Keys.B);
            }
            if (m_cheat.Count == 10)
                if (m_cheat[0] == KONAMI_CODE[0] &&
                    m_cheat[1] == KONAMI_CODE[1] &&
                    m_cheat[2] == KONAMI_CODE[2] &&
                    m_cheat[3] == KONAMI_CODE[3] &&
                    m_cheat[4] == KONAMI_CODE[4] &&
                    m_cheat[5] == KONAMI_CODE[5] &&
                    m_cheat[6] == KONAMI_CODE[6] &&
                    m_cheat[7] == KONAMI_CODE[7] &&
                    m_cheat[8] == KONAMI_CODE[8] &&
                    m_cheat[9] == KONAMI_CODE[9])
                {
                    bool_noriko = true;
                    Player player = (Player)m_entities.Find(s => s is Player);
                    player.activePowerUps = new List<PowerUpType>() { PowerUpType.SPEEDUP, PowerUpType.LASER, PowerUpType.MISSILE, PowerUpType.OPTION, PowerUpType.SHIELD };
                    int option_trail = 25;
                    Option option = new Option(this, player.m_pos - new Vector2(500, 0), player.m_size / 2, player.m_maxVel, player.m_accel, player.m_friction, player.m_rateOfFire, player.m_continuousRateOfFire, m_spriteEnemies, MovableType.Option, player.m_ProjectileSprite, player, option_trail);
                    Add(option);
                    player.m_maxVel += player.SPEEDUP_INCREASE;
                    m_cheat.Clear();
                }
        }
        m_previousKeyboardState = m_currentKeyboardState;
    }

    protected override void Draw(GameTime gameTime) {

      GraphicsDevice.Clear(Color.CornflowerBlue);

      m_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

      

      m_worldMap.Draw(gameTime, m_spriteBatch);
      //draw all entities...
      foreach(Entity e in m_entities)
        e.Draw(gameTime, m_spriteBatch);

      if (bool_noriko)
          m_spriteBatch.DrawString(m_spriteFont, "I LOVE NORIKO", Vector2.Zero, Color.White, 0.0f,
                Vector2.Zero, 1, SpriteEffects.None, 0);
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
