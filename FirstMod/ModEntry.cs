using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Characters;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;
using System.Collections.Generic;
using StardewValley.Locations;

// tom is gay

namespace FirstMod
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {

        private AnimatedSprite texture;
        public Texture2D texture2;
        private CharacterCustomization characterCustomization;

        /*********
** Public methods
*********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
           

            texture2 = helper.Content.Load<Texture2D>(@"assets\dog.xnb");
            texture = new AnimatedSprite(helper.Content.Load<Texture2D>(@"assets\Dinosaur.xnb"));
            ControlEvents.KeyPressed += this.ControlEvents_KeyPress;
            LocationEvents.CurrentLocationChanged += this.LocationEvents_LocationChanged;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
        }

        /*********
        ** Private methods
        *********/
        /// <summary>The method invoked when the player presses a keyboard button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void ControlEvents_KeyPress(object sender, EventArgsKeyPressed e)
        {
            if (Context.IsWorldReady) // save is loaded
            {
                if(e.KeyPressed==Keys.C)
                {
                    editCharacter();
                }
                if (e.KeyPressed == Keys.Escape)
                {
                    
                }


            }
        }
        private void LocationEvents_LocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            this.Monitor.Log(e.NewLocation.name);
            foreach (Pet pet in Utility.getAllCharacters().OfType<Pet>().ToArray())
            {
                movePet(pet, e.NewLocation);
            }
            
        }


        private bool IsSimilar(Color original, Color test, int redDelta, int blueDelta, int greenDelta)
        {
            return Math.Abs(original.R - test.R) < redDelta && Math.Abs(original.G - test.G) < greenDelta && Math.Abs(original.B - test.B) < blueDelta;
        }

        /// <summary>The method invoked after the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        public void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            
            changeDogColour(Color.WhiteSmoke, Color.White, Color.Gray, Color.LightGray, Color.LightPink, Color.HotPink);


        }

        public void editCharacter()
        {
            List<int> shirtOptions = new List<int>();
            shirtOptions.Add(0);
            shirtOptions.Add(1);
            shirtOptions.Add(2);
            shirtOptions.Add(3);
            shirtOptions.Add(4);
            shirtOptions.Add(5);
            List<int> hairStyleOptions = new List<int>();
            hairStyleOptions.Add(0);
            hairStyleOptions.Add(1);
            hairStyleOptions.Add(2);
            hairStyleOptions.Add(3);
            hairStyleOptions.Add(4);
            hairStyleOptions.Add(5);
            List<int> accessoryOptions = new List<int>();
            accessoryOptions.Add(0);
            accessoryOptions.Add(1);
            accessoryOptions.Add(2);
            accessoryOptions.Add(3);
            accessoryOptions.Add(4);
            accessoryOptions.Add(5);
            this.characterCustomization = new CharacterCustomization(shirtOptions, hairStyleOptions, accessoryOptions, true);
            this.characterCustomization.randomButton.visible = false;
            
            Game1.activeClickableMenu = (IClickableMenu) characterCustomization;
        }

        public void changeDogColour(Color newbasecolor, Color newlightcolor, Color newdarkcolor, Color newbrightcolor, Color newlightcollar, Color newdarkcollar)
        {
            Color[] data = new Color[texture2.Width * texture2.Height];
            texture2.GetData<Color>(data);
            Color basecolor = new Color(213, 127, 0);
            Color lightcolor = new Color(242, 164, 55);
            Color darkcolor = new Color(145, 62, 0);
            Color brightcolor = new Color(255, 198, 0);
            Color lightcollar = new Color(113, 92, 132);
            Color darkcollar = new Color(97, 141, 248);
            for (int i = 0; i < data.Length; i++)
                if (data[i] == basecolor)
                    data[i] = newbasecolor;
            for (int i = 0; i < data.Length; i++)
                if (data[i] == lightcolor)
                    data[i] = newlightcolor;
            for (int i = 0; i < data.Length; i++)
                if (data[i] == darkcolor)
                    data[i] = newdarkcolor;
            for (int i = 0; i < data.Length; i++)
                if (data[i] == brightcolor)
                    data[i] = newbrightcolor;
            for (int i = 0; i < data.Length; i++)
                if (data[i] == lightcollar)
                    data[i] = newlightcollar;
            for (int i = 0; i < data.Length; i++)
                if (data[i] == darkcollar)
                    data[i] = newdarkcollar;
            texture2.SetData<Color>(data);
            AnimatedSprite dogsprite = new AnimatedSprite(texture2, 0, 32, 32);
            //Game1.player.FarmerRenderer.draw(spriteBatch, Game1.player.FarmerSprite.CurrentAnimationFrame, Game1.player.FarmerSprite.CurrentFrame, Game1.player.FarmerSprite.SourceRect, new Vector2(xPositionOnScreen - 2 + Game1.tileSize * 2 / 3 + Game1.tileSize * 2 - Game1.tileSize / 2, (yPositionOnScreen + 75) + IClickableMenu.borderWidth - Game1.tileSize / 4 + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2), Vector2.Zero, 0.8f, Color.White, 0f, 1f, Game1.player);
            foreach (Pet pet in Utility.getAllCharacters().OfType<Pet>().ToArray())
            {
                pet.sprite = dogsprite;
            }
            
            
        }

        public void movePet(Pet pet, GameLocation location)
        {
            FarmHouse homeOfFarmer = Utility.getHomeOfFarmer(Game1.player);
            Vector2 vector2 = Vector2.Zero;
            int num = 0;
            for (vector2 = new Vector2((float)Game1.player.getTileX(), (float)Game1.player.getTileY()); num < 50 && (!location.isTileLocationTotallyClearAndPlaceable(vector2) || !location.isTileLocationTotallyClearAndPlaceable(vector2 + new Vector2(1f, 0.0f)) || homeOfFarmer.isTileOnWall((int)vector2.X, (int)vector2.Y)); ++num)
                vector2 = new Vector2((float)Game1.player.getTileX(), (float)Game1.player.getTileY());
            if (num >= 50)
                return;
            Game1.warpCharacter((NPC)pet, location.name, vector2, false, false);
            
            //Game1.getFarm().characters.Remove((NPC)pet);
            pet.CurrentBehavior=0;
            pet.initiateCurrentBehavior();
            
        }
        

    }
    
}
