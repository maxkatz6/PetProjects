using System;

namespace Ormeli.App
{
    public static class App
    {
        public static RenderClass Render { get; private set; }

        public static void Initialize(RenderClass render, IntPtr handle)
        {
            Render = render;
            Render.Initialize(handle);
        }
    }
}
