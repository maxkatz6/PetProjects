using System;
using Ormeli.Graphics.Drawable;

namespace Ormeli.Graphics
{
	public class GameObject
	{
		public Model Model { get; set; }
		public Transform Transform { get; set; }
		public Action<GameObject> Script { get; set; }
	}
}