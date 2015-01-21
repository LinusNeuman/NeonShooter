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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace NeonShooter
{
    public class Joystick
    {
        private bool fingerIsDown;

        private int my_x;
        private int my_y;

        private JoystickKnob knob; // import and represent the joystickKnob.

        private Texture2D texture;
        private int x;
        private int y;

        private int offsetX; // To be used together with the usual X and Y variables, but they have the coordinates equal to the center of the joystick, this will be 0 +- the finger location. 
                             // Will make the knob follow the finger, and be used for calculations.
        private int offsetY;

        public Vector2 direction;

        public Joystick(int margin_left, int margin_bottom)
        {
            my_x = margin_left;
            my_y = margin_bottom;

            texture = Art.Joystick;

            TouchPanel.EnabledGestures = GestureType.FreeDrag | 
                GestureType.Tap | GestureType.Hold;
        }

        public void init()
        {
            this.x = my_x + this.texture.Width / 2;
            this.y = (int)GameRoot.ScreenSize.Y - my_y - this.texture.Height / 2;

            knob = new JoystickKnob();

            knob.x = this.x + offsetX;
            knob.y = this.y + offsetY;

            knob.originX = this.x + offsetX;
            knob.originY = this.y + offsetY;
        }

        public void Update()
        {
            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            if (fingerIsDown == false)
            {
                knob.x = knob.originX;
                knob.y = knob.originY;

                direction = Vector2.Zero;
            }

            while (TouchPanel.IsGestureAvailable)
            {
                

                GestureSample gesture = TouchPanel.ReadGesture();

                if (gesture.Position.X - this.x >= -160 && gesture.Position.X - (int)this.texture.Width <= 160
                    && gesture.Position.Y - this.y >= -160 && gesture.Position.Y - (int)this.texture.Height >= 160)
                {
                    fingerIsDown = true;

                    knob.x = (int)gesture.Position.X;
                    knob.y = (int)gesture.Position.Y;

                    direction.X = knob.x - this.x; // To calculate we need the position _relative_ to the centre of the joystick. 
                    direction.Y = knob.y - this.y;
                }

                //double final = Convert.ToDouble((gesture.Position.X - this.x) + (gesture.Position.Y - this.y));

                //double distance = Math.Sqrt(final);

                //if (distance <= 16)
                //{
                    
                    

                //}

                fingerIsDown = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(this.x, this.y), null,
                Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1f, 1f), SpriteEffects.None, 0);
            spriteBatch.Draw(knob.texture, new Vector2(knob.x + offsetX, knob.y + offsetY), null,
                Color.White, 0, new Vector2(knob.texture.Width / 2, knob.texture.Height / 2), new Vector2(1f, 1f), SpriteEffects.None, 0);

        }
    }
}