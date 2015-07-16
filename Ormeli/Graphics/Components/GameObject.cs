using System;
using Ormeli.Graphics.Drawable;

namespace Ormeli.Graphics
{
	public struct GameObject
	{
		public Model Model { get; set; }
		public Transform Transform { get; set; }
		public Action<GameObject> Script { get; set; }

		public GameObject()
		{
			Model = null;
			Transform = new Transform();
			Script = null;
		}
	}
}