using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lab2Pyramid
{
    class RenderObjects
    {
        public readonly int _vertexArrayObject = GL.GenVertexArray();
        public readonly int _elementBufferObject = GL.GenBuffer();
        public readonly int _vertexBufferObject = GL.GenBuffer();

        public readonly int indicesLenght;

        Texture Diffuse, Specular;
        Shader shader;


        public RenderObjects(float[] _vertices, int[] _indices, Texture diffuse, Texture specular, Shader shader, int stride)
        {
            this.Diffuse = diffuse;
            this.shader = shader;
            this.Specular = specular;
            indicesLenght = _indices.Length;

            //shader.SetInt("texture0", 0);

            //shader.SetInt("texture1", 0);
            GL.BindVertexArray(_vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.NamedBufferStorage(
               _vertexBufferObject,
               _vertices.Length * sizeof(float),        // the size needed by this buffer
               _vertices,                           // data to initialize with
               BufferStorageFlags.MapWriteBit);    // at this point we will only write to the buffer

            GL.VertexArrayAttribBinding(_vertexArrayObject, 0, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayObject, 0);

            var positionLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);

            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 3 * sizeof(float));

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), 6 * sizeof(float));

            // link the vertex array and buffer and provide the stride as size of Vertex
            GL.VertexArrayVertexBuffer(_vertexArrayObject, 0, _vertexBufferObject, IntPtr.Zero, stride * sizeof(float));



            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.DynamicDraw);//staticDraw


        }

        public void RenderTorus()
        {
            GL.DrawElements(PrimitiveType.TriangleStrip, indicesLenght, DrawElementsType.UnsignedInt, 0);//PrimitiveType.Triangles
        }

        public void Render()
        {
            GL.DrawElements(PrimitiveType.Triangles, indicesLenght, DrawElementsType.UnsignedInt, 0);//PrimitiveType.Triangles
        }
        public void ApplyTexture()
        {
            Diffuse.Use(TextureUnit.Texture0);
            Specular.Use(TextureUnit.Texture1);
            shader.Use();
        }
        public void Bind()
        {
            GL.BindVertexArray(_vertexArrayObject);
        }
        
        public void Motion(float[] _vertices, int[] _indices, int stride)
        {


            shader.SetInt("texture0", 0);
            GL.BindVertexArray(_vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.NamedBufferStorage(
               _vertexBufferObject,
               _vertices.Length * sizeof(float),        // the size needed by this buffer
               _vertices,                           // data to initialize with
               BufferStorageFlags.MapWriteBit);    // at this point we will only write to the buffer

            GL.VertexArrayAttribBinding(_vertexArrayObject, 0, 0);
            GL.EnableVertexArrayAttrib(_vertexArrayObject, 0);

            var positionLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);

            var normalLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), 6 * sizeof(float));

            // link the vertex array and buffer and provide the stride as size of Vertex
            GL.VertexArrayVertexBuffer(_vertexArrayObject, 0, _vertexBufferObject, IntPtr.Zero, stride * sizeof(float));



            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        }
    }
}
