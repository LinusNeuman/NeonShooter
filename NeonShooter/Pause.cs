using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BloomPostprocess;

// test for google play services
using Android.Gms;
using Android.Gms.Common;
using Android.Gms.Games;
using Android.Gms.Games.LeaderBoard;
using Android.Gms.Plus;
using Android.Gms.Plus.Model.People;
using Android.Gms.Common.Apis;
using Android.Views;
using BloomPostprocess;
// test for in app billing

namespace NeonShooter
{
    public class Pause
    {
        #region Textures

        public static Texture2D ResumeTxt { get; private set; }
        public static Texture2D AchievementTxt { get; private set; }
        public static Texture2D MenuTxt { get; private set; }
        public static Texture2D BgTxt { get; private set; }
        

        #endregion

        static Random rand = new Random();

        List<Button> buttonList = new List<Button>();

        public Pause()
        {
            buttonList.Add(new Button()
            {
                texture = ResumeTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - ResumeTxt.Width - 140, GameRoot.VirtualScreenSize.Y - ResumeTxt.Height - 680),
                bgameState = NeonShooter.Button.bGameState.ingame,
            });
            buttonList.Add(new Button()
            {
                texture = AchievementTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - AchievementTxt.Width - 140, GameRoot.VirtualScreenSize.Y - AchievementTxt.Height - 430),
                bgameState = NeonShooter.Button.bGameState.none,
            });
            buttonList.Add(new Button()
            {
                texture = MenuTxt,
                Position = new Vector2(GameRoot.VirtualScreenSize.X - MenuTxt.Width - 140, GameRoot.VirtualScreenSize.Y - MenuTxt.Height - 200),
                bgameState = NeonShooter.Button.bGameState.menu,
            });
        }


        public static void Load(ContentManager content)
        {
            ResumeTxt = content.Load<Texture2D>("Paused/Paused_Resume");
            AchievementTxt = content.Load<Texture2D>("Paused/Paused_Ach");
            MenuTxt = content.Load<Texture2D>("Paused/Paused_Menu");
            BgTxt = content.Load<Texture2D>("Paused/Paused");
        }

        public void Update(ContentManager Content)
        {
            HandleTouchInput(Content);
        }

        public void HandleTouchInput(ContentManager Content)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();


                for (int i = 0; i < buttonList.Count; i++)
                {
                    if ((gesture.Position.X * GameRoot.tempScale.X > buttonList[i].Position.X && gesture.Position.X * GameRoot.tempScale.X < buttonList[i].Position.X + buttonList[i].texture.Width &&
                        gesture.Position.Y * GameRoot.tempScale.Y > buttonList[i].Position.Y && gesture.Position.Y * GameRoot.tempScale.Y < buttonList[i].Position.Y + buttonList[i].texture.Height))
                    {
                        if (buttonList[i].bgameState == Button.bGameState.menu)
                        {
                            PlayerShip.Instance.Kill();
                            PlayerShip.Instance.ResetGame();

                            EntityManager.ResetGame();

                            Menu.Load(Content);
                            Sound.LoadTheme(Content);

                            Sound.Music.Dispose();
                            Sound.Explosion.Dispose();
                            Sound.Shot.Dispose();
                            Sound.Spawn.Dispose();
                            Bullet.BulletDamage.Dispose();
                            Bullet.BulletSpeed.Dispose();
                            Bullet.BulletStandard.Dispose();
                            Bullet.BulletTank.Dispose();
   

                            PlayerShip.Pixel.Dispose();
                            PlayerShip.PlayerDmgShip.Dispose();
                            PlayerShip.PlayerSpdShip.Dispose();
                            PlayerShip.PlayerStndShip.Dispose();
                            PlayerShip.PlayerTnkShip.Dispose();

                            EntityManager.ResetGame();

                            EnemySpawner.Follower.Dispose();
                            EnemySpawner.Wanderer_Part1.Dispose();
                            EnemySpawner.Wanderer_Part2.Dispose();

                            Menu.gameState = Menu.GameState.menu;


                        }

                        if (buttonList[i].bgameState == Button.bGameState.ingame)
                        {
                            
                            MediaPlayer.Resume();

                            MediaPlayer.IsRepeating = true;

                            int f = Array.FindIndex(BloomSettings.PresetSettings, row => row.Name == "Default");
                            GameRoot.Instance.bloom.Settings = BloomSettings.PresetSettings[f];



                            Menu.gameState = Menu.GameState.ingame;
                        }

                        if (buttonList[i].texture == AchievementTxt)
                        {
                            NeonShooter.Activity1 activity = GameRoot.Activity as NeonShooter.Activity1;
                            if (activity.pGooglePlayClient.IsConnected)

                                activity.StartActivityForResult(GamesClass.Achievements.GetAchievementsIntent(activity.pGooglePlayClient), Activity1.REQUEST_ACHIEVEMENTS);
                        }
                    }
                    else
                    {
                        if (gesture.GestureType == GestureType.Tap)
                        {
                            Color yellow = new Color(47, 206, 251);
                            //Color color2 = ColorUtil.HSVToColor(5, 0.5f, 0.8f);

                            for (int o = 0; o < 4; o++)
                            {
                                float speed = 6f * (1f - 1 / rand.NextFloat(1f, 6f));

                                Color color = Color.Lerp(Color.White, yellow, rand.NextFloat(0, 1));

                                var state = new ParticleState()
                                {
                                    Velocity = rand.NextVector2(speed, speed),
                                    Type = ParticleType.None,
                                    LengthMultiplier = 0.5f
                                };

                                GameRoot.ParticleManager2.CreateParticle(Art.LineParticle, gesture.Position * GameRoot.tempScale, color, 190, new Vector2(1f, 1f), state);
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BgTxt, Vector2.Zero, Color.White);
            for (int i = 0; i < buttonList.Count; i++)
            {
                spriteBatch.Draw(buttonList[i].texture, buttonList[i].Position, Color.White);
            }
        }
    }
}