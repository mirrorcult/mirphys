using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading;
using ImGuiNET;
using mirphys;
using mirphys.Bodies;
using Veldrid;
using Veldrid.OpenGLBinding;
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
        private static BufferDescription _vbDescription;
        private static DeviceBuffer _vBuff;
        private static DeviceBuffer _iBuff;
        private static Shader[] _shaders;
        private static Pipeline _pl;

        private static readonly List<IDemo> _demos = new();
        private static IDemo _currentDemo;

        private static World World = new(new(0, mirphys.Constants.NormalGravity), 3);
        private static readonly float tickTime = 1.0f / 60.0f;
        private static readonly TimeSpan tickTimeTs = TimeSpan.FromSeconds(tickTime);

        // 16:9 because 1280x720 baybey
        private static int ScreenWidthWorld = 160;
        private static int ScreenHeightWorld = 90;

        // yeah yeah yeah eat my ass im lazy
        private const string VertexShaderPath = "../../../Shaders/basic.vert.glsl";
        private const string FragmentShaderPath = "../../../Shaders/basic.frag.glsl";

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
                var st = new Stopwatch();
                st.Start();
                var snap = _window.PumpEvents();
                foreach (var ev in snap.KeyEvents)
                {
                    if (ev.Key == Key.Escape)
                    { 
                        Environment.Exit(0);
                    }
                }
                if (_window.Exists)
                {
                    World.Step(tickTime);
                    _ir.Update(tickTime, snap);
                    DrawMenuBar();
                    DrawInfoWindow();
                    _currentDemo.DrawImgui();
                    
                    Render();
                }

                st.Stop();
                
                if (st.Elapsed > tickTimeTs)
                {
                    // tough shit I guess we can't hit 60 fps so we won't wait
                    continue;
                }
                Thread.Sleep(tickTimeTs - st.Elapsed);
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
                
            _vbDescription = new BufferDescription(
                4 * VertexPositionColor.SizeInBytes,
                BufferUsage.VertexBuffer);
            _vBuff = _gd.ResourceFactory.CreateBuffer(_vbDescription);

            var ibDescription = new BufferDescription(
                4 * sizeof(ushort),
                BufferUsage.IndexBuffer);
            _iBuff = _gd.ResourceFactory.CreateBuffer(ibDescription);
            
            var vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));

            var vertexShaderDesc = new ShaderDescription(
                ShaderStages.Vertex,
                File.ReadAllBytes(VertexShaderPath),
                "main");
            var fragmentShaderDesc = new ShaderDescription(
                ShaderStages.Fragment,
                File.ReadAllBytes(FragmentShaderPath),
                "main");

            _shaders = _gd.ResourceFactory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);
            
            GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription();
            pipelineDescription.BlendState = BlendStateDescription.SingleOverrideBlend;

            pipelineDescription.DepthStencilState = new DepthStencilStateDescription(
                depthTestEnabled: true,
                depthWriteEnabled: true,
                comparisonKind: ComparisonKind.LessEqual);

            pipelineDescription.RasterizerState = new RasterizerStateDescription(
                cullMode: FaceCullMode.Back,
                fillMode: PolygonFillMode.Solid,
                frontFace: FrontFace.Clockwise,
                depthClipEnabled: true,
                scissorTestEnabled: false);

            pipelineDescription.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            pipelineDescription.ResourceLayouts = System.Array.Empty<ResourceLayout>();

            pipelineDescription.ShaderSet = new ShaderSetDescription(
                vertexLayouts: new VertexLayoutDescription[] { vertexLayout },
                shaders: _shaders);

            pipelineDescription.Outputs = _gd.SwapchainFramebuffer.OutputDescription;
            _pl = _gd.ResourceFactory.CreateGraphicsPipeline(pipelineDescription);
        }

        private static void Render()
        {
            _cl.Begin();
            _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, RgbaFloat.Black);
            _cl.SetPipeline(_pl);

            foreach (Body b in World.Bodies)
            {
                if (b is RectBody)
                {
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
            // ???
            var buff = _gd.ResourceFactory.CreateBuffer(_vbDescription);
            _cl.SetVertexBuffer(0, buff);
            _cl.SetIndexBuffer(_iBuff, IndexFormat.UInt16, 0);
            
            ushort[] quadIndices = { 0, 1, 2, 3 };
            _gd.UpdateBuffer(_iBuff, 0, quadIndices);
            
            VertexPositionColor[] vertices =
            {
                // test
                new VertexPositionColor(WorldToScreen(b.TopLeft), RgbaFloat.White),
                new VertexPositionColor(WorldToScreen(b.TopRight), RgbaFloat.White),
                new VertexPositionColor(WorldToScreen(b.BottomLeft), RgbaFloat.White),
                new VertexPositionColor(WorldToScreen(b.BottomRight), RgbaFloat.White)
            };
            _gd.UpdateBuffer(buff, 0, vertices);

            _cl.DrawIndexed(
                indexCount: 4,
                instanceCount: 1,
                indexStart: 0,
                vertexOffset: 0,
                instanceStart: 0);
        }

        private static void LoadDemo(IDemo demo)
        {
            Console.WriteLine("Loading world for {0}", demo.Name);
            World.Clear();
            demo.SetupWorld(World);
            _currentDemo = demo;
        }

        // should i just be registering types or something instead of copious amounts of reflection? yes, but idc
        // cause this only runs once
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

        // dumb ass its literally 90 / 2 thats just 45. figure it out rider
        [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
        private static Vector2 WorldToScreen(Vec2 w)
        {
            // see funny ms paint drawing in assets
            var y = (w.Y - ScreenHeightWorld / 2) / (ScreenHeightWorld / 2);
            var x = w.X / ScreenWidthWorld;
            return new Vector2((float)x, (float)y);
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