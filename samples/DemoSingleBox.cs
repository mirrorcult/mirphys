using ImGuiNET;
using mirphys;

namespace samples
{
    public class DemoTwoBoxes : IDemo
    {
        public string Name => "Two Boxes";
        public string Desc => "Just two boxes, sitting there, menacingly.";
        
        public void SetupWorld(World w)
        {
            RectBody box1 = new RectBody(new(-5f, 5f), new(2.0f, 2.0f), 10);
            RectBody box2 = new RectBody(new(5f, 5f), new(4.0f, 4.0f), 40);

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