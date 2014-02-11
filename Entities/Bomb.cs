using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Frenemey.Entities
{
    public class Bomb : Entity
    {
        private const int BASE_RADIUS = 2; //# of tiles in 4 neighbor direction bomb blasts
        private const int FUZE_TIME = 80; //how long after dropping bomb explodes

        Image _bombImage;

        //has throwing player left the tile he originally dropped the bomb on
        bool ownerOff = false;

        public Bomb(Bomberman owner)
        {
            this.Radius = owner.BombRadius > 0 ? owner.BombRadius : BASE_RADIUS;

            _bombImage = new Image("Assets/Images/sprites.png", new Rectangle(0, 0, Global.GridSize, Global.GridSize));
            this.Graphic = _bombImage;

            this.Layer = 2;

            this.SetHitbox(Global.GridSize, Global.GridSize, (int)CollisionTag.Bomb);
            this.Collidable = false;

            this.Owner = owner;

            float x = Util.SnapToGrid(owner.X, Global.GridSize, 16);
            float y = Util.SnapToGrid(owner.Y, Global.GridSize);

            this.SetPosition(x, y);
        }

        public override void Update()
        {
            base.Update();

            if (this.Timer == FUZE_TIME)
            {
                this.Scene.Add(new Explosion(this, this.Radius));
                this.RemoveSelf();
                return;
            }

            if (this.Timer >= FUZE_TIME - 20)
            {
                this._bombImage.Shake = 6f;
            }

            //TODO figure out how to make player temporarily not collide when dropping
            var collidedPlayers = CollideEntities(this.X, this.Y, (int)CollisionTag.Player);

            if (!ownerOff)
            {
                ownerOff = true;
                if (collidedPlayers != null && collidedPlayers.Count > 0)
                {
                    foreach (Entity player in collidedPlayers)
                    {
                        if (player == this.Owner)
                        {
                            ownerOff = false;
                            break;
                        }
                    }
                }
            }

            this.Collidable = ownerOff;
        }

        public override void Render()
        {
            base.Render();

            //this.Hitbox.Render();
        }

        public override void Removed()
        {
            base.Removed();

            if (this.Owner != null)
                this.Owner.BombsAvailable += 1;
        }

        public Bomberman Owner { get; private set; }

        public int Radius
        {
            get;
            set;
        }
    }
}
