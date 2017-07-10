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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TermTwoGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PointingArrow : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D texture; // The basic attributes of the PointingArrow sprite.
        private Rectangle spriteRectangle;
        private Vector2 position;
        private const int BULLET_WIDTH = 63; 
        private const int BULLET_HEIGHT = 113;
        private Game game; // The game. 

        public PointingArrow(Game game, ref Texture2D theTexture,Vector2 initPos)
            : base(game)
        {
            // TODO: Construct any child components here
            texture = theTexture;
            position = new Vector2();
            spriteRectangle = new Rectangle(0, 0, BULLET_WIDTH, BULLET_HEIGHT);
            position = initPos;

            this.game = game;
        }     

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {            
         
            base.Update(gameTime);
        }

        public void updateArrrowPos(Vector2 updatedPos)
        {
            updatedPos = new Vector2(updatedPos.X -2, updatedPos.Y - 120);
            position = updatedPos;
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); // Make a new spritebatch
            
            sBatch.Draw(texture, position, spriteRectangle, Color.White); // Then draw the bullet with the texture, vector hitbox and back colour.

            base.Draw(gameTime);
        }

    }
}
