using System;
using System.Collections.Generic;
using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace samples
{
    public class Samples
    {
        private static Sdl2Window _window;
        private static GraphicsDevice _gd;
        private static CommandList _cl;
        private static ImGuiRenderer _ir;
        
        private static float tickTime = 1.0f / 60.0f;
        private static List<IDemo> _demos = new();

        private static IDemo _currentDemo;
        
        public static void Main(string[] args)
        {
            VeldridStartup.CreateWindowAndGraphicsDevice(
                new WindowCreateInfo(50, 50, 1280, 720, WindowState.Normal, "Mirphys Testbed"),
                new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
                out _window,
                out _gd
            );

            _cl = _gd.ResourceFactory.CreateCommandList();
            _ir = new ImGuiRenderer(
                _gd,
                _gd.MainSwapchain.Framebuffer.OutputDescription,
                _window.Width,
                _window.Height);
            
            _window.Resized += () =>
            {
                _gd.MainSwapchain.Resize((uint) _window.Width, (uint) _window.Height);
                _ir.WindowResized(_window.Width, _window.Height);
            };
            
            GetDemos();
            // just load the first one
            LoadDemo(_demos[0]);

            // draw loop
            while (_window.Exists)
            {
                var snapshot = _window.PumpEvents();
                _ir.Update(tickTime, snapshot);

                DrawMenuBar();
                DrawInfoWindow();
                _currentDemo.DrawImgui();

                _cl.Begin();
                _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
                _cl.ClearColorTarget(0, new RgbaFloat(0, 0, 0, 1f));
                _ir.Render(_gd, _cl);
                _cl.End();
                _gd.SubmitCommands(_cl);
                _gd.SwapBuffers(_gd.MainSwapchain);
            }
        }

        private static void LoadDemo(IDemo demo)
        {
            // todo: reset world, other stuff probably
            Console.WriteLine("Loading world for {0}", demo.Name);
            demo.SetupWorld();
            _currentDemo = demo;
        }

        private static void GetDemos()
        {
            var demos = Helpers.GetImplementationsOf<IDemo>();
            foreach (var type in demos)
            {
                Console.WriteLine("type: {0}", type);
                var impl = (IDemo) Activator.CreateInstance(type);
                _demos.Add(impl);
            }
        }

        private static void DrawMenuBar()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Demos"))
                {
                    foreach (IDemo demo in _demos)
                    {
                        if (ImGui.MenuItem(demo.Name))
                        {
                            LoadDemo(demo);
                        }
                    }
                    ImGui.EndMenu();
                }
                if (ImGui.MenuItem("Quit"))
                {
                    _window.Close();
                }
                ImGui.EndMainMenuBar();
            }
        }

        private static void DrawInfoWindow()
        {
            ImGui.PushItemWidth(12.0f);
            if (ImGui.Begin("Info"))
            {
                ImGui.Text(_currentDemo.Name);
                ImGui.Spacing();
                ImGui.TextWrapped(_currentDemo.Desc);
            }
        }
    }
}