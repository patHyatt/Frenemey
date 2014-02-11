using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frenemey.Entities;
using Otter;

namespace Frenemey.Scenes
{
    public class ResultScene : Scene
    {
        Text _titleMessage;
        Text _instructionMessage;

        public ResultScene(Bomberman winner)
        {
            string message = "Everyone is a loser.";
            if (winner != null)
                message = string.Format("{0} is the greatest!", winner.Session.Name);

            _titleMessage = new Text(message, 32)
            {
                Color = Color.Red,
                X = this.HalfWidth,
                Y = this.HalfHeight,
            };

            _instructionMessage = new Text("Press any key to replay", size: 20)
            {
                Color = Color.White,
                OutlineThickness = .5f,
                X = this.HalfWidth,
                Y = this.HalfHeight + 200
            };

            this.AddGraphic(_titleMessage);
            this.AddGraphic(_instructionMessage);

            this.OnUpdate = () =>
            {
                if (Game.Input.ButtonPressed(AxisButton.Any) || Game.Input.KeyPressed(Key.Any))
                {
                    Game.SwitchScene(new GameScene(1, Game.Input.JoysticksConnected));
                }
            };
        }

        public override void Update()
        {
            base.Update();

            if (this.Timer % 50 == 0)
                this._titleMessage.Shake = 5f;

            if (Timer % 35 == 0)
                this._instructionMessage.OutlineColor = Color.Yellow;
            else if (Timer % 65 == 0)
                this._instructionMessage.OutlineColor = Color.White;
        }
    }
}
