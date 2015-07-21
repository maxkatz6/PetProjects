using System;
using Lain.Graphics.Drawable;

namespace Lain.Graphics
{
	public struct GameObject
	{
		public Model Model { get; set; }
		public Transform Transform { get; set; }
		public Action<GameObject> Script { get; set; }
	}
}