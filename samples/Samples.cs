using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using mirphys;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Veldrid.SPIRV;

namespace samples
{
    public class Samples
    {
        private static Sdl2Window _window;
        private static GraphicsDevice _gd;
        private static CommandList _cl;
        private static ImGuiRenderer _ir;
        private static DeviceBuffer _vBuff;
        private static DeviceBuffer _iBuff;
        private static Shader[] _shaders;
        private static Pipeline _pipeline;

        private static List<IDemo> _demos = new();
        private static IDemo _currentDemo;

        private static World World = new(new(0, mirphys.Constants.NormalGravity), 3);
        private static float tickTime = 1.0f / 60.0f;

        private static int ScreenWidthWorld = 50;
        private static int ScreenHeightWorld = 40;

        public static void Main(string[] args)
        {
            GetDemos();
            // just load the first one
            LoadDemo(_demos[0]);

            // vert buffers and stuff
            CreateResources();
            
            // loopy loop
            while (_window.Exists)
            {
                var snap = _window.PumpEvents();
                if (_window.Exists)
                {
                    World.Step(tickTime);
                    _ir.Update(tickTime, snap);
                    DrawMenuBar();
                    DrawInfoWindow();
                    _currentDemo.DrawImgui();
                    
                    Render();
                }
            }
        }

        private static void CreateResources()
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

            VertexPositionColor[] quadVertices =
            {
                new VertexPositionColor(new Vector2(-.75f, .75f), RgbaFloat.Red),
                new VertexPositionColor(new Vector2(.75f, .75f), RgbaFloat.Green),
                new VertexPositionColor(new Vector2(-.75f, -.75f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector2(.75f, -.75f), RgbaFloat.Yellow)
            };
            BufferDescription vbDescription = new BufferDescription(
                4 * VertexPositionColor.SizeInBytes,
                BufferUsage.VertexBuffer);
            _vBuff = _gd.ResourceFactory.CreateBuffer(vbDescription);

            BufferDescription ibDescription = new BufferDescription(
                4 * sizeof(ushort),
                BufferUsage.IndexBuffer);
            _iBuff = _gd.ResourceFactory.CreateBuffer(ibDescription);
        }

        private static void Render()
        {
            _cl.Begin();
            _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, RgbaFloat.Black);

            foreach (Body b in World.Bodies)
            {
                if (b is RectBody)
                {
                    ushort[] quadIndices = { 0, 1, 2, 3 };
                    _gd.UpdateBuffer(_iBuff, 0, quadIndices);
                    DrawRect((RectBody)b);
                }
            }
            
            _ir.Render(_gd, _cl);
            _cl.End();
            _gd.SubmitCommands(_cl);
            _gd.SwapBuffers(_gd.MainSwapchain);
        }

        private static void DrawRect(RectBody b)
        {
            VertexPositionColor[] quadVertices =
            {
                new VertexPositionColor(new Vector2(-.75f, .75f), RgbaFloat.Red),
                new VertexPositionColor(new Vector2(.75f, .75f), RgbaFloat.Green),
                new VertexPositionColor(new Vector2(-.75f, -.75f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector2(.75f, -.75f), RgbaFloat.Yellow)
            };
            _gd.UpdateBuffer(_vBuff, 0, quadVertices);

        }

        private static void LoadDemo(IDemo demo)
        {
            Console.WriteLine("Loading world for {0}", demo.Name);
            World.Clear();
            demo.SetupWorld(World);
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
    struct VertexPositionColor
    {
        public const uint SizeInBytes = 24;
        public Vector2 Position;
        public RgbaFloat Color;
        public VertexPositionColor(Vector2 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }
    }
}