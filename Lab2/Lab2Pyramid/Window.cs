using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using Lab2Pyramid;
using System.Threading;
using OpenTK.Mathematics;
using System;
using LearnOpenTK.Common; //в этом пространстве хранится Texture.cs
using LearnOpenTK; //в этом пространстве времён хранятся Camera.cs и Shader.cs
using System.Collections.Generic;

namespace Lab2Pyramid
{
    public class Window : GameWindow
    {
        //float[] _vertices, _vertices1;
        //int[] _indices, _indices1;

        private Shader _shader;

        private Texture _texture_tree;
        private Texture _texture_red;
        private Texture _texture_green;
        private Texture _texture_yellow;
        private Texture _texture_blue;

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private List<RenderObjects> _renderObjects = new List<RenderObjects>();

        private double _timeLimit = 100;
        private double _time = 0;
        private int coef = 1;
        bool allDraw = false;

        int NumberMoveObject = 1;// от 1 до 8
        const int NumberOfFigure = 9;
        int NumberOfMoveObjects = 1;

        Sphere Butt;
        Cylinder Cyl;
        Torus t1;
        Torus t2;
        Torus t3;
        Torus t4;
        Torus t5;
        Torus t6;
        Torus t7;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _camera = new Camera(Vector3.UnitZ * 15, Size.X / (float)Size.Y);

            Butt = new Sphere(1.3f, 0.0f, 0.0f, -6.5f);
            Cyl = new Cylinder(0f, 0f, 0f, 0.45f, 13f);
            t1 = new Torus(75, 75, 3.5f, 1.15f, 0.0f, 0.0f, 5.5f);
            t2 = new Torus(75, 75, 3.0f, 1.05f, 0.0f, 0.0f, 3.3f);
            t3 = new Torus(75, 75, 2.6f, 0.95f, 0.0f, 0.0f, 1.3f);
            t4 = new Torus(75, 75, 2.2f, 0.85f, 0.0f, 0.0f, -0.5f);
            t5 = new Torus(75, 75, 1.8f, 0.75f, 0.0f, 0.0f, -2.1f);
            t6 = new Torus(75, 75, 1.4f, 0.65f, 0.0f, 0.0f, -3.5f);
            t7 = new Torus(75, 75, 1.0f, 0.55f, 0.0f, 0.0f, -4.75f);
            //Cyl.buildVerticesSmooth(); //  запихнуть эту функций куд-нибдуь внутрь класса


            //t.GetVertices();
            //t.GetNormals();
            //t.GetTexCoords();


            //_vertices = Head.GetVertecies();
            //_indices = Head.GetIndices();

            //_vertices1 = Butt.GetVertecies();
            //_indices1 = Butt.GetIndices();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _texture_tree = Texture.LoadFromFile("../../../Resources/tree.jpg");
            _texture_red = Texture.LoadFromFile("../../../Resources/new_red.jpg");
            _texture_blue = Texture.LoadFromFile("../../../Resources/blue.jpg");
            _texture_green = Texture.LoadFromFile("../../../Resources/new_green.jpg");
            _texture_yellow = Texture.LoadFromFile("../../../Resources/new_yellow.jpeg");
            _shader = new Shader("../../../Shaders/shader.vert", "../../../Shaders/shader.frag");



            _renderObjects.Add(new RenderObjects(Cyl.GetAllTogether(), Cyl.GetIndices(), _texture_tree, _shader, 8));
            _renderObjects.Add(new RenderObjects(Butt.GetAllTogether(), Butt.GetIndices(), _texture_blue, _shader, 8));
            _renderObjects.Add(new RenderObjects(t7.GetAllTogether(), t7.GetIndices(), _texture_green, _shader, 8));
            _renderObjects.Add(new RenderObjects(t6.GetAllTogether(), t6.GetIndices(), _texture_red, _shader, 8));
            _renderObjects.Add(new RenderObjects(t5.GetAllTogether(), t5.GetIndices(), _texture_yellow, _shader, 8));
            _renderObjects.Add(new RenderObjects(t4.GetAllTogether(), t4.GetIndices(), _texture_blue, _shader, 8));
            _renderObjects.Add(new RenderObjects(t3.GetAllTogether(), t3.GetIndices(), _texture_green, _shader, 8));
            _renderObjects.Add(new RenderObjects(t2.GetAllTogether(), t2.GetIndices(), _texture_red, _shader, 8));
            _renderObjects.Add(new RenderObjects(t1.GetAllTogether(), t1.GetIndices(), _texture_yellow, _shader, 8));
            
           
            
            

            _shader.Use();



           // _texture_tree.Use(TextureUnit.Texture0); - без понятия для чего это

            //_texture_yellow.Use(TextureUnit.Texture1); - без понятия для чего это




            // We initialize the camera so that it is 3 units back from where the rectangle is.
            // We also give it the proper aspect ratio.

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorGrabbed = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

           
            
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //if (_time > 60) coef = -1;
            //if (_time < -60) coef = 1;

            //Должно подниматься по одной фигуре
            //Thread.Sleep();
            DrawCylinder();
            if (NumberMoveObject == 1)
                MotionSphere();
            else
                DrawSphere();
            for (int i = NumberOfMoveObjects + 1; i < NumberOfFigure; i++)
            {
                if (i == NumberMoveObject)
                {
                    MotionTorus(i);
                    continue;
                }
                DrawTorus(i);
            }
            for (int i = 2; i <= NumberOfMoveObjects; i++)
            {
                DrawTorusInUp(i);
            }
            //Какой-то косяк с индексами, выяснить какой
            SwapBuffers();
            _time += 100.0 * e.Time * coef;// * Math.Cos(_time / 40);
        }

        public void MotionSphere()
        {
            var Object = _renderObjects[1];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)(_time / 4), 0);
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1 * RotationMatrixX2;
            Object.ApplyTexture();
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
            if (_time > _timeLimit)
            {
                _time = 0;
                NumberMoveObject++;
            }
        }

        public void MotionTorus(int i)
        {
            var Object = _renderObjects[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)(_time / 4 - 0.3f), 0);
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1 * RotationMatrixX2;
            Object.ApplyTexture();
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.RenderTorus();
            if (_time > _timeLimit)
            {
                _time = 0;
                NumberMoveObject++;
                NumberOfMoveObjects++;
            }
            

        }

        public void DrawCylinder()
        {
            var Object = _renderObjects[0];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1;
            Object.ApplyTexture();
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }

        public void DrawSphere()
        {
            var Object = _renderObjects[1];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)_timeLimit / 4, 0);
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1 * RotationMatrixX2;
            Object.ApplyTexture();
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
        }

        public void DrawTorus(int i)
        {
            var Object = _renderObjects[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1;
            Object.ApplyTexture();
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.RenderTorus();
        }

        public void DrawTorusInUp(int i)
        {
            var Object = _renderObjects[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)((_timeLimit / 4) - (i-1) * 0.45f), 0);
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1 * RotationMatrixX2;
            Object.ApplyTexture();
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.RenderTorus();
        }

        public void DisassemblePyramidEazyMode(int i)
        {
            var Object = _renderObjects[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)(_time / 4), 0);
            var model = Matrix4.Identity;
            switch (i)
            {
                case 0:
                    Object.Bind();
                    model *= RotationMatrixX1;
                    Object.ApplyTexture();
                    _shader.SetMatrix4("model", model);
                    _shader.SetMatrix4("view", _camera.GetViewMatrix());
                    _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
                    Object.Render();
                    break;
                case 1:
                    Object.Bind();
                    model *= RotationMatrixX1 * RotationMatrixX2;
                    Object.ApplyTexture();
                    _shader.SetMatrix4("model", model);
                    _shader.SetMatrix4("view", _camera.GetViewMatrix());
                    _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
                    Object.Render();
                    break;
                default:
                    Object.Bind();
                    model *= RotationMatrixX1 * RotationMatrixX2;
                    Object.ApplyTexture();
                    _shader.SetMatrix4("model", model);
                    _shader.SetMatrix4("view", _camera.GetViewMatrix());
                    _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
                    Object.RenderTorus();
                    break;
            }
        }

        public void AssemblePyramidEazyMode(int i)
        {
            var Object = _renderObjects[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, -(float)(_time / 4), 0);
            var model = Matrix4.Identity;
            switch (i)
            {
                case 0:
                    Object.Bind();
                    model *= RotationMatrixX1;
                    Object.ApplyTexture();
                    _shader.SetMatrix4("model", model);
                    _shader.SetMatrix4("view", _camera.GetViewMatrix());
                    _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
                    Object.Render();
                    break;
                case 1:
                    Object.Bind();
                    model *= RotationMatrixX1 * RotationMatrixX2;
                    Object.ApplyTexture();
                    _shader.SetMatrix4("model", model);
                    _shader.SetMatrix4("view", _camera.GetViewMatrix());
                    _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
                    Object.Render();
                    break;
                default:
                    Object.Bind();
                    model *= RotationMatrixX1 * RotationMatrixX2;
                    Object.ApplyTexture();
                    _shader.SetMatrix4("model", model);
                    _shader.SetMatrix4("view", _camera.GetViewMatrix());
                    _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
                    Object.RenderTorus();
                    break;
            }
        }

        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 3.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        // In the mouse wheel function, we manage all the zooming of the camera.
        // This is simply done by changing the FOV of the camera.
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            // We need to update the aspect ratio once the window has been resized.
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }

}
