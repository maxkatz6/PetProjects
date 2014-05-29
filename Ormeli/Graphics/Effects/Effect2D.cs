namespace Ormeli.Graphics.Effects
{
    public class Effect2D : ColTexEffect
    {
        public Effect2D(string file)
            : base(file)
        {
        }

        protected override void InitEffect()
        {
            Base.InitAttrib("Texture", BitmapVertex.Number);
        }
    }
}
