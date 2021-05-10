using System;
using static samples.Helpers;

namespace samples
{
    public class Demo1 : IDemo
    {
        public string Name => "Single Box";
        public string Desc => "Just a box, sitting there, menacingly.";
        
        public void SetupWorld()
        {
        }

        public void DrawImgui()
        {
        }
    }
}