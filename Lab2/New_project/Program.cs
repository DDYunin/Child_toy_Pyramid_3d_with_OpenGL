using OpenTK.Mathematics;
using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace New_project
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "LearnOpenTK - Coordinates Systems",

                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Compatability, // compatability даёт возможность использовать новое и старое ядро openGL
            };

            using (MyWindow window = new MyWindow(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}
