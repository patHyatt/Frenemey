using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frenemey.Assets;
using Frenemey.Entities;
using Frenemey.Scenes;
using Otter;

namespace Frenemey
{
    public class GameScene : Scene
    {
        public GameScene(int level, int numPlayers)
        {
            OgmoProject project = new OgmoProject(AssetManager.OgmoProject, AssetManager.OgmoImagePath);
            project.RegisterTag((int)CollisionTag.Wall, "Walls");
            project.RegisterTag((int)CollisionTag.Brick, "Bricks");
            project.RegisterTag((int)CollisionTag.Player, "Players");

            if (level != 1)
                throw new NotSupportedException();

            project.LoadLevel("Assets/Levels/Standard.oel", this);
        }

        public override void Update()
        {
            base.Update();

            var playersAlive = this.GetClass<Bomberman>();

            if (playersAlive.Count <= 1)
                Game.SwitchScene(new ResultScene(playersAlive.FirstOrDefault()));
        }
    }
}
