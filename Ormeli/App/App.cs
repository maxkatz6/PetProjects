using Ormeli.Render;

namespace Ormeli.App
{
    public static class App
    {
        public static RenderClass Render { get; private set; }

        public static void Initialize(RenderClass render)
        {
            Render = render;
        }
    }
}
