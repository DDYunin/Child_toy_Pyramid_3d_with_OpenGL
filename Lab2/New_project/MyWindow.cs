using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace New_project
{
    public class MyWindow : GameWindow
    {

        private readonly float[] _vertices =
       {
            -0.5f, -0.5f, 0.0f, // Bottom-left vertex
             0.5f, -0.5f, 0.0f, // Bottom-right vertex
             0.0f,  0.5f, 0.0f  // Top vertex
        };  // Массив задаёт вершины треугольника, тоесть координаты, где его вершины будут находиться

        private int _vertexBufferObject;
                                                    //какие-то два рандомных объекта
        private int _vertexArrayObject; 


        private float FrameTime = 0.0f;
        private int fps = 0;
        public MyWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));
            Console.WriteLine(GL.GetString(StringName.Vendor));

            VSync = VSyncMode.On; //Вертикальная синхронизация, уменьшает число фпс до допустимого значения для нашего глаза
        }

       //Метод загружает данные
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.Bisque); //устанавливает цвет, которым будет очищаться наш задний буфер (фон)
            GL.ClearColor(255 / 255.0f, 228 / 255.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.CullFace);
 

            
        }

       //Тут производится отрисовка кадров
        protected override void OnRenderFrame(FrameEventArgs e)
        {
           
            GL.Clear(ClearBufferMask.ColorBufferBit); //тут происходит очистка нашей формы каждый кадр

            

            SwapBuffers(); //осуществляет двойную буфферизацию

            base.OnRenderFrame(e);
        }
        // Вся логика вычислений производится тут
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            FrameTime += (float)e.Time;
            fps++;
            if (FrameTime >= 1.0f)
            {
                Title = $"FPS = {fps}";
                FrameTime = 0.0f;
                fps = 0;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            { 
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

           
        }

       
        protected override void OnUnload()
        {
            

            base.OnUnload();
        }
    }
}
