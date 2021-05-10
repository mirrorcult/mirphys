namespace samples
{
    public class Demo2 : IDemo
    {
        public string Name => "Simple Pendulum";
        public string Desc => "A box on a pendulum, with just one joint.";

        public void SetupWorld()
        {
        }

        public void DrawImgui()
        {
        }
    }
}