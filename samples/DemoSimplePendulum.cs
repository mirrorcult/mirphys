namespace samples
{
    public class DemoSimplePendulum : IDemo
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