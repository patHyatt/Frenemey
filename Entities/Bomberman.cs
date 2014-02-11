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
    /// Player character which drops bombs. 
    /// </summary>
    public class Bomberman : Entity
    {
        Spritemap<string> _bomberImages;

        string lastDirection = "down";

        public Bomberman(Session session, Color color)
        {
            this.Session = session;
            this.Color = color;
            Initialize();
        }

        private void Initialize()
        {
            this.SetHitbox(30, 30, new[] { (int)CollisionTag.Player });

            _bomberImages = new Spritemap<string>("Assets/Images/player.png", Global.GridSize, Global.GridSize);
            _bomberImages.Add("standDown", new int[] { 2 }, new int[] { 10 });
            _bomberImages.Add("standUp", new int[] { 3 }, new int[] { 10 });
            _bomberImages.Add("standRight", new int[] { 6 }, new int[] { 10 });
            _bomberImages.Add("standLeft", new int[] { 6 }, new int[] { 10 });

            _bomberImages.Add("walkDown", new int[] { 1, 2 }, new int[] { 10, 10 });
            _bomberImages.Add("walkUp", new int[] { 4, 5 }, new int[] { 10, 10 });
            _bomberImages.Add("walkRight", new[] { 7, 8 }, new int[] { 10, 10 });
            _bomberImages.Add("walkLeft", new[] { 7, 8 }, new int[] { 10, 10 });
            _bomberImages.Play("standDown");

            this.Graphic = this._bomberImages;

            this.Speed = 250;
            this.BombsAvailable = 1;

            this.Session.Controller = Controller.Get360Controller(Session.Id);
            //Directionals 
            this.Session.Controller.AxisDPad.AddKey(Key.W, Direction.Up);
            this.Session.Controller.AxisDPad.AddKey(Key.A, Direction.Left);
            this.Session.Controller.AxisDPad.AddKey(Key.S, Direction.Down);
            this.Session.Controller.AxisDPad.AddKey(Key.D, Direction.Right);

            this.Session.Controller.AxisLeft.AddKey(Key.W, Direction.Up);
            this.Session.Controller.AxisLeft.AddKey(Key.A, Direction.Left);
            this.Session.Controller.AxisLeft.AddKey(Key.S, Direction.Down);
            this.Session.Controller.AxisLeft.AddKey(Key.D, Direction.Right);

            //Bombing
            this.Session.Controller.B.AddKey(Key.Space);


            this.Movement = new BasicMovement(this.Speed, this.Speed, 50);
            this.Movement.Collider = this.Hitbox;
            this.Movement.Axis = this.Session.Controller.AxisDPad;

            this.Movement.AddCollision((int)CollisionTag.Brick);
            this.Movement.AddCollision((int)CollisionTag.Bomb);
            this.Movement.AddCollision((int)CollisionTag.Wall);
            this.Movement.AddCollision((int)CollisionTag.Player);

            Image color = Image.CreateRectangle(Global.GridSize, this.Color);
            color.Blend = BlendMode.Multiply;
            this.Graphics.Add(color);


            this.AddComponent(this.Movement);
            this.AddCollider(this.Hitbox);
        }

        public static void CreateFromXML(Scene scene, XmlAttributeCollection attributes)
        {
            int playerNumber = attributes.Int("player", 0);

            Session playerSession;
            Color color = Color.None;

            if (playerNumber == 0)
            {
                playerSession = Global.PlayerOneSession;
                color = Color.White;
            }
            else if (playerNumber == 1)
            {
                playerSession = Global.PlayerTwoSession;
                color = Color.Yellow;
            }
            else if (playerNumber == 2)
            {
                playerSession = Global.PlayerThreeSession;
                color = Color.Blue;
            }
            else
            {
                playerSession = Global.PlayerFourSession;
                color = Color.Grey;
            }

            Bomberman player = new Bomberman(playerSession, color);

            player.SetPosition(attributes.Int("x", 0), attributes.Int("y", 0));

            scene.Add(player);
        }

        public override void Update()
        {
            base.Update();

            if (this.Overlap(this.X, this.Y, (int)CollisionTag.Explosion))
            {
                this.RemoveSelf();
                return;
            }

            if (this.Session.Controller.B.Pressed)
            {
                if (this.BombsAvailable > 0)
                {
                    var bomb = new Bomb(this);
                    this.Scene.Add(bomb);

                    this.BombsAvailable -= 1;
                }
            }

            _bomberImages.FlippedX = false;
            if (this.Session.Controller.Up.Down)
            {
                _bomberImages.Play("walkUp");
                lastDirection = "up";
            }
            else if (this.Session.Controller.Down.Down)
            {
                _bomberImages.Play("walkDown");
                lastDirection = "down";
            }
            else if (this.Session.Controller.Right.Down)
            {
                _bomberImages.Play("walkRight");
                lastDirection = "right";
            }
            else if (this.Session.Controller.Left.Down)
            {
                _bomberImages.FlippedX = true;
                _bomberImages.Play("walkLeft");
                lastDirection = "left";
            }
            else
            {
                switch (this.lastDirection)
                {
                    case "up":
                        _bomberImages.Play("standUp");
                        break;
                    case "left":
                        _bomberImages.FlippedX = true;
                        _bomberImages.Play("standLeft");
                        break;
                    case "down":
                        _bomberImages.Play("standDown");
                        break;
                    case "right":
                        _bomberImages.Play("standRight");
                        break;

                }
            }
        }

        public override void Render()
        {
            base.Render();
            //this.Hitbox.Render();
        }

        public int Speed { get; set; }

        public int BombsAvailable { get; set; }

        public int BombRadius { get; set; }

        public BasicMovement Movement { get; set; }

        public Session Session { get; private set; }

        public Color Color { get; set; }
    }
}
