namespace samples
{
    public class DemoSingleBox : IDemo
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