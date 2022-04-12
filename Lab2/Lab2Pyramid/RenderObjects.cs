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

        Texture texture;
        Shader shader;


        public RenderObjects(float[] _vertices, int[] _indices, Texture texture, Shader shader, int stride)
        {
            this.texture = texture;
            this.shader = shader;
            indicesLenght = _indices.Length;

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

        //public RenderSphere(float[] vertex,  Texture texture, Shader shader)
        //{
        //    this.texture = texture;
        //    this.shader = shader;
        //    indicesLenght = vertex.Length;
        //    _vertexBufferObject = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

        //    GL.BufferData(BufferTarget.ArrayBuffer, vertex.Length * sizeof(float), vertex,  BufferUsageHint.StaticDraw);
        //    _vertexArrayObject = GL.GenVertexArray();
        //    GL.BindVertexArray(_vertexArrayObject);

        //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        //    GL.EnableVertexAttribArray(0);
        //}
        //public void RenderWithoutIndices()
        //{
        //    GL.DrawArrays(PrimitiveType.Triangles, 0, indicesLenght);
        //}
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
            texture.Use(TextureUnit.Texture0);
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
