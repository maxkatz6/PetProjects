using System;

namespace Lain.Graphics
{
    public interface IRenderSource
    {
        bool CountFPS { get; set; } 
        int FPS { get; }
        object Handle { get; }
        void Run(Action draw, Action upd);
    }
}
