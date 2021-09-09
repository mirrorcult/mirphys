using ImGuiNET;
using mirphys;
using mirphys.Bodies;

namespace samples.Demos
{
    public class DemoTwoBoxes : IDemo
    {
        public string Name => "Two Boxes";
        public string Desc => "Just two boxes, sitting there, menacingly.";
        
        public void SetupWorld(World w)
        {
            RectBody box1 = new RectBody(new(-40f, 70f), new(4.0f, 4.0f), 40);
            RectBody box2 = new RectBody(new(40f, 70f), new(4.0f, 4.0f), 40);

            w.Add(box1);
            w.Add(box2);
        }

        public void DrawImgui()
        {
            if (ImGui.Begin("Ass"))
            {
                ImGui.Spacing();
                ImGui.Text("Ass");
                ImGui.Spacing();
            }
        }
    }
}