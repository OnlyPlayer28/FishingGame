using Core.InventoryManagement;
using Fishing.Scripts.Minigames;
using Fishing.Scripts.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing.Scripts
{
    public class Player
    {
        public int fishingLevel { get; set; } = 0;

        public float fishHookReactionTime { get; private set; } = 1.5f;
        Random random = new Random();

        public int money { get; set; } = 0;
        public Inventory inventory { get; set; }
        public Player()
        {
            inventory = new Inventory();
        }

        public float CalculateCurrentFishCatchTime(Vector2 minAndMax)
        {
            return (float)random.Next((int)minAndMax.X,(int)minAndMax.Y);
        }

        public void OnFishCatch(Object sender,FishingMinigameEventArgs e)
        {
            ((FishingScene)Game1.stateManager.GetActiveGameState()).boat.fishingState = FishingState.FishingResults;
            ((FishingScene)Game1.stateManager.GetActiveGameState()).fishingResultsScreen.SetFish(e.fishID).SetActive(true);
        }

        public void OnFishHook(Object sender,FishHookEventArgs e)
        {
            e.fishingMinigame.OnFishCatchEvent += OnFishCatch;
        }
    }
}
