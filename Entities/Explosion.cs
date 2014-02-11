using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Frenemey.Entities
{
    public class Explosion : Entity
    {
        Image _horizontalImage;
        Image _veritcalImage;

        Color _color = new Color("FF8C00");

        public Explosion(Entity from, int radius)
        {
            this.SetPosition(from.X, from.Y);
            this.X = from.X + Global.GridSize / 2;
            this.Y = from.Y + Global.GridSize / 2;
            this.Radius = radius;

            this.LifeSpan = 60;


            int numberOfTiles = this.Radius * 2 - 1;
            int size = numberOfTiles * Global.GridSize;
            //vertical explosion
            var vertical = new BoxCollider(Global.GridSize, size, (int)CollisionTag.Explosion);
            vertical.CenterOrigin();
            this.AddCollider(vertical);
            //horizontal explosion
            var horizontal = new BoxCollider(size, Global.GridSize, (int)CollisionTag.Explosion);
            horizontal.CenterOrigin();
            this.AddCollider(horizontal);

            this._veritcalImage = Image.CreateRectangle(size, Global.GridSize, _color);
            this._veritcalImage.CenterOrigin();
            this._horizontalImage = Image.CreateRectangle(Global.GridSize, size, _color);
            this._horizontalImage.CenterOrigin();

            this.Graphics.AddRange(new[] { this._veritcalImage, this._horizontalImage });

            this.Layer = 0;
        }

        public override void Update()
        {
            base.Update();

            var alpha = .5f;
            if (this.Timer % 6 == 0)
                alpha = 1f;

            foreach (Image image in this.Graphics)
            {
                image.Alpha = alpha;
            }
        }

        public override void Render()
        {
            base.Render();

            //foreach (Collider coll in this.Colliders)
            //    coll.Render();
        }

        public int Radius { get; set; }
    }
}
