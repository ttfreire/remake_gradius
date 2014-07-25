using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gradius
{
    public class SpawnController
    {
        WorldMap m_worldMap;
        List<int> activatedSpawners = new List<int>();
        public SpawnController(Game1 m_world)
        {
            m_worldMap = m_world.m_worldMap;
        }

        public void Update(GameTime gameTime)
        {
            for (int o = 0; o < m_worldMap.m_map.ObjectLayers["enemy_spawns"].MapObjects.Length; o++)
            {
                if (m_worldMap.m_map.ObjectLayers["enemy_spawns"].MapObjects[o].Bounds != null)
                {
                    Vector2 spawn_pos = new Vector2(m_worldMap.m_map.ObjectLayers["enemy_spawns"].MapObjects[o].Bounds.X - m_worldMap.m_view.X,
                                                    m_worldMap.m_map.ObjectLayers["enemy_spawns"].MapObjects[o].Bounds.Y - m_worldMap.m_view.Y);

                    if (spawn_pos.X <= m_worldMap.screenWidth && !activatedSpawners.Contains(o))
                    {
                        Dictionary<string, FuncWorks.XNA.XTiled.Property> dict = m_worldMap.m_map.ObjectLayers["enemy_spawns"].MapObjects[o].Properties;
                        spawnEnemy(dict, spawn_pos);
                        activatedSpawners.Add(o);
                    }
                }
            }
        }

        public void spawnEnemy(Dictionary<string, FuncWorks.XNA.XTiled.Property> enemyProperties, Vector2 enemyPosition)
        {
            MovableType enemyType;
            Vector2 enemySize;
            float enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire, enemyContinuousrateoffire;
            Texture2D enemySprite, enemyProjectileSprite;
            List<Enemy> enemySquad;
            bool enemyDropsPowerUp, enemyHasSquad;
            AnimationController enemyAnimator;
            int enemyQuantity = (int)enemyProperties["quantity"].AsInt32;
            enemyHasSquad = (bool)enemyProperties["hasSquad"].AsBoolean;
            enemyDropsPowerUp = (bool)enemyProperties["dropsPowerUp"].AsBoolean;
            enemyType = MovableType.Enemy;

            if(enemyHasSquad)
                enemySquad = new List<Enemy>();
            else
                enemySquad = null;

            switch (enemyProperties["type"].Value)
            {
                case "Fan":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteEnemies, null, 5, 18, null);
                        enemyMaxVel = 200;
                        enemyAccel = 800;
                        enemyFriction = 800;
                        enemyRateoffire = 0;
                        enemyContinuousrateoffire = 0;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            Fan newFan = new Fan(m_worldMap.m_world, enemyPosition + new Vector2(50 * i, 0), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newFan);
                            if (enemyHasSquad)
                                newFan.addToSquad();
                        }
                    }
                    break;

                case "Garun":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteEnemies, null, 5, 18, null);
                        enemyMaxVel = 80;
                        enemyAccel = 800;
                        enemyFriction = 800;
                        enemyRateoffire = 0;
                        enemyContinuousrateoffire = 0;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            int y = 0;
                            if (i % 2 == 0)
                                y = 50;
                            Garun newGarun = new Garun(m_worldMap.m_world, enemyPosition + new Vector2(50 * i, y), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newGarun);
                            if (enemyHasSquad)
                                newGarun.addToSquad();
                        }
                    }
                    break;

                case "Rugal":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteEnemies, null, 5, 18, null);
                        enemyMaxVel = 80;
                        enemyAccel = 800;
                        enemyFriction = 800;
                        enemyRateoffire = 0;
                        enemyContinuousrateoffire = 0;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            int y = 0;
                            if (i % 2 == 0)
                                y = 50;
                            Rugal newRugal = new Rugal(m_worldMap.m_world, enemyPosition + new Vector2(50 * i, y), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newRugal);
                            if (enemyHasSquad)
                                newRugal.addToSquad();
                        }
                    }
                    break;

                case "Dee-01":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteEnemies, null, 5, 18, null);
                        enemyMaxVel = 50;
                        enemyAccel = 500;
                        enemyFriction = 500;
                        enemyRateoffire = 1.0f;
                        enemyContinuousrateoffire = 1.0f;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            Dee newdee = new Dee(m_worldMap.m_world, enemyPosition + new Vector2(50 * i, 0), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newdee);
                            if (enemyHasSquad)
                                newdee.addToSquad();
                        }
                    }
                    break;

                case "Ducker":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteEnemies, null, 5, 18, null);
                        enemyMaxVel = 50;
                        enemyAccel = 500;
                        enemyFriction = 500;
                        enemyRateoffire = 1.0f;
                        enemyContinuousrateoffire = 1.0f;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            Ducker newDucker = new Ducker(m_worldMap.m_world, enemyPosition - new Vector2(m_worldMap.m_view.Width * i, 0), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newDucker);
                            if (enemyHasSquad)
                                newDucker.addToSquad();
                        }
                    }
                    break;

                case "Dagumu":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteEnemies, null, 5, 18, null);
                        enemyMaxVel = 50;
                        enemyAccel = 500;
                        enemyFriction = 500;
                        enemyRateoffire = 1.0f;
                        enemyContinuousrateoffire = 1.0f;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            Hatch newHatch = new Hatch(m_worldMap.m_world, enemyPosition + new Vector2(50 * i, 0), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteHatch, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newHatch);
                            if (enemyHasSquad)
                                newHatch.addToSquad();
                        }
                    }
                    break;

                case "Rush":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteEnemies, null, 5, 18, null);
                        enemyMaxVel = 200;
                        enemyAccel = 200;
                        enemyFriction = 500;
                        enemyRateoffire = 1.0f;
                        enemyContinuousrateoffire = 1.0f;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            Rush newRush = new Rush(m_worldMap.m_world, enemyPosition + new Vector2(0, i * 50), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newRush);
                            if (enemyHasSquad)
                                newRush.addToSquad();
                        }
                    }
                    break;

                case "Jumper":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteEnemies, null, 5, 18, null);
                        enemyMaxVel = 150;
                        enemyAccel = 150;
                        enemyFriction = 500;
                        enemyRateoffire = 1.0f;
                        enemyContinuousrateoffire = 1.0f;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            Jumper newJumper = new Jumper(m_worldMap.m_world, enemyPosition + new Vector2(i * 50, 0), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newJumper);
                            if (enemyHasSquad)
                                newJumper.addToSquad();
                        }
                    }
                    break;

                case "boss":
                    {
                        enemyAnimator = new AnimationController(m_worldMap.m_world.m_spriteBoss, null, 2, 1, null);
                        enemyMaxVel = 100;
                        enemyAccel = 500;
                        enemyFriction = 500;
                        enemyRateoffire = 1.0f;
                        enemyContinuousrateoffire = 5.0f;

                        for (int i = 1; i <= enemyQuantity; i++)
                        {
                            Boss newBoss = new Boss(m_worldMap.m_world, new Vector2(m_worldMap.screenWidth - enemyAnimator.m_spriteSheet.Width / 2, m_worldMap.m_screenMiddle.Y), new Vector2(enemyAnimator.m_currentSpriteRect.Width,
                                                    enemyAnimator.m_currentSpriteRect.Height), enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newBoss);
                            if (enemyHasSquad)
                                newBoss.addToSquad();
                        }
                    }
                    break;

                case "volcano":
                    {
                        enemyAnimator = null;
                        enemyMaxVel = 0;
                        enemyAccel = 0;
                        enemyFriction = 0;
                        enemyRateoffire = 0.8f;
                        enemyContinuousrateoffire = 0.8f;

                        for (int i = 0; i < enemyQuantity; i++)
                        {
                            Volcano newVolcano = new Volcano(m_worldMap.m_world, enemyPosition + new Vector2(300 * i, 0), Vector2.Zero, enemyMaxVel, enemyAccel, enemyFriction, enemyRateoffire,
                                                    enemyContinuousrateoffire, m_worldMap.m_world.m_spriteEnemies, enemyType,
                                                    m_worldMap.m_world.m_spriteProjectile, enemySquad, m_worldMap, enemyDropsPowerUp,
                                                    enemyAnimator);
                            m_worldMap.m_world.m_entities.Add(newVolcano);
                            if (enemyHasSquad)
                                newVolcano.addToSquad();
                        }
                    }
                    break;
            }
        }
        
    }
}
