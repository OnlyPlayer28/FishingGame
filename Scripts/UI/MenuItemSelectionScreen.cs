using Core.UI;
using Core.UI.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Debug;
using Core;
using Microsoft.Xna.Framework.Input;
using Core.Components;

namespace Fishing.Scripts.UI
{
    public class MenuItemSelectionEventArgs : EventArgs
    {
        public int ID;
    }
    public class SelectScreenActiveEventArgs :EventArgs
    {
        public bool active;
    }
    public class MenuItemSelectionScreen : IMenu
    {
        public Image backdrop;

        public EventHandler<MenuItemSelectionEventArgs> OnItemChooseEvent { get; set; }
        public EventHandler<SelectScreenActiveEventArgs> OnSetActiveEvent { get; set; }

        private List<Button> selectionButtons;
        private float waitAndCloseTimer;

        private bool isClosing = false;
        private Text titleText;

        private int currentPage = 0;
        private const float timeUntilClose = .25f;
        public MenuItemSelectionScreen(Vector2 position, Vector2 size, float layer, Canvas canvas, string name = "defaultMenu") 
            : base(position, size, layer, canvas, name)
        {
            waitAndCloseTimer = timeUntilClose;
            selectionButtons = new List<Button>();
            backdrop = new Image(new Rect(position, size+new Vector2(0,-11), Helper.HexToRgb("#542424"), true, layer).SetFillColor(Helper.HexToRgb("#6e3b34")), isActive: false);
            titleText = new Text(position + new Vector2(1, 1), "choose an item:", Color.White, Game1.Font_24, layer:layer);

            canvas.AddUIELement(backdrop);
            canvas.AddTextElement(titleText);
            int i = 0;
            foreach (var item in Game1.itemRegistry)
            {
                if (item.tags.HasFlag(Tags.AddableToMenu)) {
                    selectionButtons.Add(
                    new Button(position + new Vector2(1, 10)+new Vector2(0,i*10), new Vector2(size.X - 1, 10), layer - .0000001f, item.ID.ToString(),isActive: false, "")
                    .SetButtonText(item.name,Color.Wheat,Game1.Font_24)
                    .SetSimpleSprite(Helper.HexToRgb("#542424"), Helper.HexToRgb("#6e3b34"))

                    );

                    i++;
                }
            }
            foreach (var button in selectionButtons)
            {
                button.isActive = false;
                button.OnButtonClickEvent += OnSelectionMade;
                canvas.AddClickableElement(button);
            }
            foreach (var item in selectionButtons)
            {
                if (item.isActive)
                {
                    System.Diagnostics.Debug.WriteLine("selection buttons are  active");
                }
            }
        }
        public void OnSelectionMade(Object o, ButtonEventArgs e)
        {
            if (!isActive) { return; }
            OnItemChooseEvent?.Invoke(this, new MenuItemSelectionEventArgs { ID=Convert.ToInt16(e.buttonRef.name)});
            isClosing = true;
        }
        public override IActive SetActive(bool active)
        {
            OnSetActiveEvent?.Invoke(this, new SelectScreenActiveEventArgs { active=active});

            titleText.isActive = active;
            backdrop.isActive = active;
            selectionButtons.ForEach(p=>p.isActive = active);
            return base.SetActive(active);
        }

        public override void Update(GameTime gameTime)
        {
            if(!isActive) { return; }
            if (isClosing)
            {
                waitAndCloseTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(waitAndCloseTimer <= 0)
                {
                    isClosing = false;
                    waitAndCloseTimer = timeUntilClose;
                    SetActive(false);
                }
            }

            if (InputManager.AreKeysBeingPressedDown(keys: Keys.J)) { SetActive(false);}
            base.Update(gameTime);
        }

    }
}
