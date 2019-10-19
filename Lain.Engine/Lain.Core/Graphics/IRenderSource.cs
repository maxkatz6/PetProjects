using System;
using Lain.Core;

namespace Lain.Graphics
{
    public interface IRenderSource
    {
        bool CountFPS { get; set; }
        Fps FPS { get; }
        object Handle { get; }
        void Run(Action draw, Action upd);
    }
}