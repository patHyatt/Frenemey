using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Frenemey
{
    public class TitleScene : Scene
    {
        public TitleScene() : base()
        {
            this.AddGraphic(new Text("Frenemey", 20)
            {
                X = 60,
                Y = 60,
                Color = Color.Red
            });

            this.AddGraphic(new Text("How many people are playing?", 20)
            {
                X = 60,
                Y = 120,
                Color = Color.Green
            });


            UIListMenu menu = new UIListMenu();
            menu.AddListItem(1, "Solo");
            menu.AddListItem(2, "Two");
            menu.AddListItem(3, "Three");
            menu.AddListItem(4, "Four");

            this.OnUpdate = () =>
            {
                if (Game.Input.ButtonPressed(AxisButton.Any) || Game.Input.KeyPressed(Key.Return))
                {
                    Game.SwitchScene(new GameScene(1, Game.Input.JoysticksConnected));
                }
            };

        }
    }
}
