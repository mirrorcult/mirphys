using System;
using mirphys;

namespace samples
{
    public interface IDemo
    {
        string Name { get; }
        string Desc { get; }
        void SetupWorld(World w);
        void DrawImgui();
    }
}