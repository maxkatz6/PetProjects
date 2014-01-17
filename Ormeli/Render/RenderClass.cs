﻿using System;
using Ormeli.Core.Patterns;
using Ormeli.Math;

namespace Ormeli.Render
{
    public abstract class RenderClass : Disposable
    {
        public abstract void Initialize(IntPtr handle);
        public abstract void BeginDraw();
        public abstract void BeginDraw(Color color);
        public abstract void EndDraw();
        public abstract void TurnZBufferOff();
        public abstract void TurnZBufferOn();
        public abstract void TurnOnAlphaBlending();
        public abstract void TurnOffAlphaBlending();
    }
}
