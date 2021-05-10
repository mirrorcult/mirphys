using System;

namespace samples
{
    public interface IDemo
    {
        string Name { get; }
        string Desc { get; }
        void SetupWorld();
        void DrawImgui();
    }
}