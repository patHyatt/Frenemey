using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Otter;

namespace Frenemey
{
    class MainLoop
    {
        static void Main(string[] args)
        {
            Game game = new Game("Frenemies", 800, 600, 60, false);
            game.SetWindow(800, 600);
            game.Title = "Frenemies";

            game.FirstScene = new TitleScene();


            //Debugging
            game.Debugger.ToggleKey = Key.Tilde;

            Global.PlayerOneSession = game.AddSession("P1");
            Global.PlayerTwoSession = game.AddSession("P2");
            Global.PlayerThreeSession = game.AddSession("P3");
            Global.PlayerFourSession = game.AddSession("P4");

            game.Start();
        }
    }
}
