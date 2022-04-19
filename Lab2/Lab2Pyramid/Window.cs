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
        private Shader _shader;

        private Texture _texture_tree;
        private Texture _texture_red;
        private Texture _texture_green;
        private Texture _texture_yellow;
        private Texture _texture_blue;

        // update with lighting
        private Texture _texture_specular_tree;
        private Texture _texture_specular_red;
        private Texture _texture_specular_green;
        private Texture _texture_specular_yellow;
        private Texture _texture_specular_blue;
        // update with lighting

        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;


        private List<RenderObjects> _renderObjects = new List<RenderObjects>();

        private double _timeLimit = 100;
        private double _time = 0;
        bool isDisassemble = false; //Разобрана ли пирамидка

        int NumberMoveObject = 1;// от 1 до 8 //номер двигаемой фигуры
        const int NumberOfFigure = 9; //количество фигур
        int NumberOfMoveObjects = 1; //число подвинутых фигур
        
        // update with lighting
        private readonly Vector3 _lightPos = new Vector3(0.0f, 3.0f, 0.0f); //точка освещения
        // update with lighting

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        //Подгрузить и настроить первоначальные данные
        protected override void OnLoad()
        {
            base.OnLoad();
            _camera = new Camera(Vector3.UnitZ * 1, Size.X / (float)Size.Y);

            Cylinder Cyl1 = new Cylinder(0f, -10.0f, 7.3f, 6f, 1f);//0, -19, 7.3 //0, -10, 7.3
            Sphere Butt = new Sphere(1.3f, 0.0f, -10.0f, -6.5f);
            Cylinder Cyl = new Cylinder(0f, -10.0f, 0f, 0.45f, 13.8f);
            Torus t1 = new Torus(75, 75, 3.5f, 1.15f, 0.0f, -10.0f, 5.5f);
            Torus t2 = new Torus(75, 75, 3.0f, 1.05f, 0.0f, -10.0f, 3.3f);
            Torus t3 = new Torus(75, 75, 2.6f, 0.95f, 0.0f, -10.0f, 1.3f);
            Torus t4 = new Torus(75, 75, 2.2f, 0.85f, 0.0f, -10.0f, -0.5f);
            Torus t5 = new Torus(75, 75, 1.8f, 0.75f, 0.0f, -10.0f, -2.1f);
            Torus t6 = new Torus(75, 75, 1.4f, 0.65f, 0.0f, -10.0f, -3.5f);
            Torus t7 = new Torus(75, 75, 1.0f, 0.55f, 0.0f, -10.0f, -4.75f);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _texture_tree = Texture.LoadFromFile("../../../Resources/tree.jpg");
            _texture_red = Texture.LoadFromFile("../../../Resources/new_red.jpg");
            _texture_blue = Texture.LoadFromFile("../../../Resources/blue.jpg");
            _texture_green = Texture.LoadFromFile("../../../Resources/new_green.jpg");
            _texture_yellow = Texture.LoadFromFile("../../../Resources/new_yellow.jpeg");

            _texture_specular_tree = Texture.LoadFromFile("../../../Resources/tree_specular.jpg");
            _texture_specular_red = Texture.LoadFromFile("../../../Resources/new_red_specular.jpg");
            _texture_specular_blue = Texture.LoadFromFile("../../../Resources/blue_specular.jpg");
            _texture_specular_green = Texture.LoadFromFile("../../../Resources/new_green_specular.jpg");
            _texture_specular_yellow = Texture.LoadFromFile("../../../Resources/new_yellow_specular.jpg");

            _shader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            DefineShader(_shader);


            _renderObjects.Add(new RenderObjects(Cyl.GetAllTogether(), Cyl.GetIndices(), _texture_tree, _texture_specular_tree, _shader, 8));
            _renderObjects.Add(new RenderObjects(Butt.GetAllTogether(), Butt.GetIndices(), _texture_blue, _texture_specular_blue, _shader, 8));
            _renderObjects.Add(new RenderObjects(t7.GetAllTogether(), t7.GetIndices(), _texture_green, _texture_specular_green, _shader, 8));
            _renderObjects.Add(new RenderObjects(t6.GetAllTogether(), t6.GetIndices(), _texture_red, _texture_specular_red, _shader, 8));
            _renderObjects.Add(new RenderObjects(t5.GetAllTogether(), t5.GetIndices(), _texture_yellow, _texture_specular_yellow, _shader, 8));
            _renderObjects.Add(new RenderObjects(t4.GetAllTogether(), t4.GetIndices(), _texture_blue, _texture_specular_blue, _shader, 8));
            _renderObjects.Add(new RenderObjects(t3.GetAllTogether(), t3.GetIndices(), _texture_green, _texture_specular_green, _shader, 8));
            _renderObjects.Add(new RenderObjects(t2.GetAllTogether(), t2.GetIndices(), _texture_red, _texture_specular_red, _shader, 8));
            _renderObjects.Add(new RenderObjects(t1.GetAllTogether(), t1.GetIndices(), _texture_yellow, _texture_specular_yellow, _shader, 8));
            _renderObjects.Add(new RenderObjects(Cyl1.GetAllTogether(), Cyl1.GetIndices(), _texture_tree, _texture_specular_tree, _shader, 8));

            _shader.Use();
        }

        //настраиваем шейдер
        private void DefineShader(Shader _lightingShader)
        {
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            _lightingShader.SetVector3("viewPos", _lightPos);

            _lightingShader.SetInt("material.diffuse", 0);//диффузное изображение
            _lightingShader.SetInt("material.specular", 1);//отражённое изображение
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 1000.0f);

            _lightingShader.SetVector3("light.position", _lightPos);
            _lightingShader.SetVector3("light.direction", _lightPos);
            _lightingShader.SetFloat("light.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader.SetFloat("light.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(40.5f)));
            _lightingShader.SetFloat("light.constant", 1.0f);
            _lightingShader.SetFloat("light.linear", 0.09f);
            _lightingShader.SetFloat("light.quadratic", 0.032f);
            _lightingShader.SetVector3("light.ambient", new Vector3(2.2f));//0.2f
            _lightingShader.SetVector3("light.diffuse", new Vector3(1.5f));//0.5f
            _lightingShader.SetVector3("light.specular", new Vector3(1.0f));
        }

        //Логика отрисовки (тоесть сама отрисовка)
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            DefineShader(_shader);

            if (!isDisassemble)
                DisassemblePyramid();
            else
                AssemblePyramid();

            SwapBuffers();
            _time += 100.0 * e.Time;
        }

        //Функция движения сферы
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

        //Функция движения тора
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
            if (NumberOfMoveObjects == 8)
            {
                NumberMoveObject--;
                NumberOfMoveObjects = 8;
            }
            

        }

        //Функция движения сферы вниз
        public void MotionSphereDown()
        {
            var Object = _renderObjects[1];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)((_timeLimit / 4) - (float)(_time / 4)), 0);
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
                NumberMoveObject--;
            }
        }

        //Функция движения тора вниз
        public void MotionTorusDown(int i)
        {
            var Object = _renderObjects[i];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, (float)((_timeLimit / 4) - (float)(_time / 4 - 0.3f)), 0);
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
                NumberMoveObject--;
                NumberOfMoveObjects--;
            }
        }

        //Функция отрисовки цилиндра (палки)
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

        //Функция отрисовки цилиндра (площадки)
        public void DrawCylinder1()
        {
            var Object = _renderObjects[9];
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

        //Функция отрисовки сферы в начальной позиции
        public void DrawSphereInStartPosition()
        {
            var Object = _renderObjects[1];
            var RotationMatrixX1 = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(90));
            var RotationMatrixX2 = Matrix4.CreateTranslation(0, 0, 0);
            var model = Matrix4.Identity;
            Object.Bind();
            model *= RotationMatrixX1 * RotationMatrixX2;
            Object.ApplyTexture();
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            Object.Render();
            if (_time > _timeLimit * 2)
            {
                _time = 0;
                NumberMoveObject++;
                isDisassemble = false;
            }
        }

        //Функция отрисовки сферы в верхней позиции
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

        //Функция отрисовки тора в начальной позиции
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

        //Функция отрисвоки тора в верхней похиции
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
            if (_time > _timeLimit * 2 && i == 8)
            {
                _time = 0;
                isDisassemble = true;
            }
        }

        //Разобрать пирамидку
        public void DisassemblePyramid()
        {
            DrawCylinder1();
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
                DrawTorusInUp(i);
        }       
        
        //Собрать пирамидку
        public void AssemblePyramid()
        {
                DrawCylinder1();
                DrawCylinder();
                for (int i = NumberOfMoveObjects; i >= 2; i--)
                {
                    if (i == NumberMoveObject)
                    {
                        MotionTorusDown(i);
                        continue;
                    }
                    DrawTorusInUp(i);
                }
                for (int i = NumberOfFigure - 1; i > NumberOfMoveObjects; i--)
                    DrawTorus(i);
                if (NumberMoveObject == 1)
                    MotionSphereDown();
                else if (NumberMoveObject == 0)
                    DrawSphereInStartPosition();
                else
                    DrawSphere();
        }

        //Логика работы приложение (обработка нажатий)
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

                //Если раскомментить этот фрагмент кода, то мы сможем перемещать нашу камеру в пространстве, путём изменения её позиции.
            /*const float cameraSpeed = 3.5f;
           
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
            */

            
            const float sensitivity = 0.2f;
            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // Изначально попадаем сюда, чтобы записать данные и дальше их использовать
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Вычислить смещение положения мыши
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Применяем тангаж камеры и рыскание (мы фиксируем тангаж в классе камеры)
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
            
        }

        // В функции колеса мыши мы управляем всем масштабированием камеры.
        // Это просто делается путем изменения угла обзора камеры.
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            // Нам нужно обновить соотношение сторон после изменения размера окна.
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }

}
