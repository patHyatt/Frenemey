using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Otter;

namespace Frenemey.Entities
{
    /// <summary>
    /// Nonpassible brick, that can be destroyed
    /// </summary>
    public class Brick : Entity
    {
        public Brick()
        {
            this.Health = 1;
            this.SetHitbox(Global.GridSize, Global.GridSize, (int)CollisionTag.Brick);
        }

        public static void CreateFromXML(Scene scene, XmlAttributeCollection attributes)
        {
            Brick brick = new Brick();
            brick.SetPosition(attributes.Int("x", 0), attributes.Int("y", 0));
            brick.Graphic = new Image("Assets/Images/grasses.png", new Rectangle(256, 0, Global.GridSize, Global.GridSize));

            scene.Add(brick);
        }

        public override void Update()
        {
            base.Update();

            if (this.Overlap(this.X, this.Y, (int)CollisionTag.Explosion))
                this.Health -= 1;

            if (this.Health <= 0)
            {
                this.RemoveSelf();
            }
        }

        public int Health { get; set; }
    }
}
