using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using Lab2Pyramid;
using OpenTK.Mathematics;
using System;
using LearnOpenTK.Common; //в этом пространстве хранится Texture.cs
using LearnOpenTK; //в этом пространстве времён хранятся Camera.cs и Shader.cs
using System.Collections.Generic;

namespace Lab2Pyramid
{
    public class Window : GameWindow
    {
        float[] _vertices, _vertices1;
        int[] _indices, _indices1;

        private Shader _shader;

        private Texture _texture;


        private Texture _texture2;

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private List<RenderSphere> _renderObjects = new List<RenderSphere>();

        private double _time;
        private int coef = 1;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _camera = new Camera(Vector3.UnitZ * 6, Size.X / (float)Size.Y);

            Sphere Butt = new Sphere(0.6f, 2f, 2f, 2f);
            Cylinder Cyl = new Cylinder(0f, 0f, 0f, 0.2f, 4f, 30, 90);
            Cyl.buildVerticesSmooth(); //  запихнуть эту функций куд-нибдуь внутрь класса

            Torus t = new Torus(100, 100, 1f, 0.5f);
            t.GetVertices();
            t.GetNormals();
            t.GetTexCoords();


            //_vertices = Head.GetVertecies();
            //_indices = Head.GetIndices();

            _vertices1 = Butt.GetVertecies();
            _indices1 = Butt.GetIndices();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _texture = Texture.LoadFromFile("../../../Resources/awesomeface.png");
            _texture2 = Texture.LoadFromFile("../../../Resources/awesomeface.png");
            _shader = new Shader("../../../Shaders/shader.vert", "../../../Shaders/shader.frag");



            _renderObjects.Add(new RenderSphere(Cyl.GetAllTogether(), Cyl.GetIndices(), _texture, _shader, 8));
            _renderObjects.Add(new RenderSphere(t.GetAllTogether(),t.GetIndices(), _texture, _shader, 8));
            _renderObjects.Add(new RenderSphere(Butt.GetAllTogether(), Butt.GetIndices(), _texture2, _shader, 8));

            _shader.Use();



            _texture.Use(TextureUnit.Texture0);

            _texture2.Use(TextureUnit.Texture1);




            // We initialize the camera so that it is 3 units back from where the rectangle is.
            // We also give it the proper aspect ratio.

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorGrabbed = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _time += 500.0 * e.Time * coef * Math.Cos(_time / 40);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //if (_time > 60) coef = -1;
            //if (_time < -60) coef = 1;

            int i = 0;

            //var Object = _renderObjects[0];
            foreach (var Object in _renderObjects)
            {
                Object.Bind();

                //var RotationMatrixX = Matrix4.CreateTranslation(-(float)(_time / 40), 0, 0);

                //var RotationMatrixY = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));

                var model = Matrix4.Identity /* RotationMatrixY*/;// * RotationMatrixZ * RotationMatrixX;

                Object.ApplyTexture();
                //var Object1 = _renderObjects[1];
                // Object1.Bind();

                //var RotationMatrixX = Matrix4.CreateTranslation(-(float)(_time / 40), 0, 0);

                //var RotationMatrixY = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));

                //var model = Matrix4.Identity /* RotationMatrixY*/;// * RotationMatrixZ * RotationMatrixX;

            //Object1.ApplyTexture();



                _shader.SetMatrix4("model", model);
                _shader.SetMatrix4("view", _camera.GetViewMatrix());
                _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

                Object.Render();
                //Object1.Render();

            }
        

       
            
            // GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
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

            const float cameraSpeed = 1.5f;
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
