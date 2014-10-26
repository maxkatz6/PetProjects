namespace Ormeli.Graphics.Effects
{
    public class Effect2D : ColTexEffect
    {
        protected override void InitAttribs()
        {
            Base.InitAttrib("Texture", BitmapVertex.Number);
        }
    }
}
