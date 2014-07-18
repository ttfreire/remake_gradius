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
  public enum PowerUpState {NONE, SPEEDUP, MISSILE, DOUBLE, LASER, OPTION, SHIELD }
  public class Game1 : Microsoft.Xna.Framework.Game {

    public GraphicsDeviceManager m_graphics;
    SpriteBatch m_spriteBatch;

    public Texture2D m_spriteViper;
    public Texture2D m_spriteFan;
    public Texture2D m_spriteEnemies;

    public Texture2D m_spritePowerUpRed;
    public Texture2D m_spritePowerUpBlue;
    public Texture2D m_spriteBasicProjectile;
    
    Map m_background;
    Rectangle mapView;
    float upperEnemySpawnCooldown = 5.0f;
    float bottomEnemySpawnCooldown = 7.0f;

    public List<Entity> m_entities = new List<Entity>();
    List<Entity> to_add = new List<Entity>();
    List<Entity> to_remove = new List<Entity>();
    Player m_player;
    public WorldMap m_worldMap;
    List<Enemy> fanSquad1;

    public List<PowerUpState> HUDPowerUp;
    public int highlightedPowerUp = 0;
    public int powerUpCounter = 0;

    SpawnController enemySpawnController;
    static int[] playerAnimationFramesUp = { 0};
    static int[] playerAnimationFramesForward = { 1};
    static int[] playerAnimationFramesDown = { 2 };
    public Dictionary<int, int[]> playerAnimations = new Dictionary<int, int[]>() { { 0, playerAnimationFramesUp },
                                                                                    { 1, playerAnimationFramesForward },
                                                                                    { 2, playerAnimationFramesDown }};
    static int[] fanAnimationFrames = { 0, 1, 2, 3 };
    public Dictionary<int, int[]> fanAnimations = new Dictionary<int, int[]>() { { 0, fanAnimationFrames } };
    static int[] duckerAnimationFramesWalking = { 45, 46 };
    static int[] duckerAnimationFramesHolding = { 47 };
    public Dictionary<int, int[]> duckerAnimations = new Dictionary<int, int[]>() { { 0, duckerAnimationFramesWalking } ,
                                                                                    { 1, duckerAnimationFramesHolding },
                                                                                    { 2, duckerAnimationFramesHolding }};

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
      HUDPowerUp = new List<PowerUpState>();
      HUDPowerUp.Add(PowerUpState.NONE);
      HUDPowerUp.Add(PowerUpState.SPEEDUP);
      HUDPowerUp.Add(PowerUpState.MISSILE);
      HUDPowerUp.Add(PowerUpState.DOUBLE);
      HUDPowerUp.Add(PowerUpState.LASER);
      HUDPowerUp.Add(PowerUpState.OPTION);
      HUDPowerUp.Add(PowerUpState.SHIELD);

      enemySpawnController = new SpawnController(this);
    }

    protected override void LoadContent() {

      m_spriteBatch = new SpriteBatch(GraphicsDevice);

      m_spriteViper = Content.Load<Texture2D>("ship-score");
      m_spriteBasicProjectile = Content.Load<Texture2D>("basic_projectile");
      m_background = Content.Load<Map>("newmap");
      m_worldMap = new WorldMap(this, m_background, m_graphics.GraphicsDevice.Viewport.Bounds);
      m_spriteFan = Content.Load<Texture2D>("fan");
      m_spritePowerUpRed = Content.Load<Texture2D>("powerupred");
      m_spritePowerUpBlue = Content.Load<Texture2D>("powerupblue");
      m_spriteEnemies = Content.Load<Texture2D>("enemy-powerups");
      
      //add map
      //m_entities.Add(m_worldMap);

      //add player...
      
      AnimationController playerAnimator = new AnimationController(m_spriteViper, playerAnimations, 4, 3, null);
      m_entities.Add(new Player(this, new Vector2(40, 240), //pos
                                new Vector2(m_spriteViper.Width, m_spriteViper.Height), 
                                100, //vel
                                800, // maxvel
                                10, // friction
                                50.0f, // rate of fire
                                500.0f, // continuous rate of fire
                                m_spriteViper, MovableType.Player, m_spriteBasicProjectile,
                                playerAnimator, true));
      m_player = (Player) m_entities[0];
      
    }

    protected override void UnloadContent() {}

    protected override void Update(GameTime gameTime) {

      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
      enemySpawnController.Update(gameTime);
      upperEnemySpawnCooldown -= dt;
      bottomEnemySpawnCooldown -= dt;
      if (upperEnemySpawnCooldown <= 0)
      {
          upperEnemySpawnCooldown = 5.0f;
          //create squad
          fanSquad1 = new List<Enemy>();
          //add enemy
          /**
          for (int i = 1; i <= 5; i++)
          {
              AnimationController fanAnimator = new AnimationController(m_spriteEnemies, fanAnimations, 5, 18);
              Fan newEnemy = new Fan(this, new Vector2(m_graphics.PreferredBackBufferWidth + 50 * i, 100), new Vector2(m_spriteFan.Width, m_spriteFan.Height), 200, 800, 800, 0, 0, m_spriteFan, MovableType.Fan, m_spriteBasicProjectile, fanSquad1, m_worldMap, false, fanAnimator);
              m_entities.Add(newEnemy);
              newEnemy.addToSquad();
          }
          **/
      }
        if (bottomEnemySpawnCooldown <= 0)
      {
          bottomEnemySpawnCooldown = 5.0f;
          //create squad
          fanSquad1 = new List<Enemy>();
          //add enemy
          //for (int i = 0; i < 5; i++)
          //{
            /** 
            Ducker newEnemy = new Ducker(this, new Vector2(0, 50), // pos
                                        new Vector2(m_spriteFan.Width, m_spriteFan.Height), // size
                                        50, // maxvel
                                        500, // accel
                                        500, // friction
                                        1.0f, // rateoffire
                                        1.0f, // continuousrateoffire
                                        m_spriteFan, MovableType.Enemy, m_spriteBasicProjectile, null, m_worldMap, false, null);
              m_entities.Add(newEnemy);
              newEnemy.addToSquad();
             **/
          //}
      }
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
