using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
//using OpenTK.Mathematics;

namespace Third
{
    class View 

    {
        private int uniform_pos;
        private int uniform_aspect;
        private Vector3d campos = new Vector3d(0,1,3);
        private int aspect =1;
        private int _uColor;

        private int attribute_vpos;
        public Vector3[] vertdata = new Vector3[] {
            new Vector3(-1f, -1f, 0f),
            new Vector3( 1f, -1f, 0f),
            new Vector3( 1f, 1f, 0f),
            new Vector3(-1f, 1f, 0f) };

    // private int _uMvpMatrixLocation;
    //private int _uNormalMatrixLocation;

    //private Matrix4 _mvpMatrix;
    //private int _amountOfVertices = 0;

    //private bool _isUpdating = false;


    //public View() : base(600, 600, new GraphicsMode(32, 24, 0, 8)) { }

        int BasicProgramID;
        public void loadShader(String filename, ShaderType type, uint program, out uint address)
         {
             address = (uint)GL.CreateShader(type);
             using (System.IO.StreamReader sr = new StreamReader(filename))
             {
                 GL.ShaderSource((int)address, sr.ReadToEnd());
             }
             GL.CompileShader(address);
             GL.AttachShader(program, address);
             Console.WriteLine(GL.GetShaderInfoLog((int)address));
         }
        public void InitShaders()
        {
            uint BasicProgramID = (uint)GL.CreateProgram(); // создание объекта программы
            loadShader("C:\\Users\\fareh\\source\repos\\Third\\raytracing.vert", ShaderType.VertexShader, BasicProgramID,

            out uint BasicVertexShader);

            loadShader("C:\\Users\\fareh\\source\repos\\Third\\raytracing.frag", ShaderType.FragmentShader, BasicProgramID,

            out uint BasicFragmentShader);
            GL.LinkProgram(BasicProgramID);
            // Проверяем успех компоновки
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog((int)BasicProgramID));
        }
            /*int vbo_position;
            

            GL.GenBuffers(1, out vbo_position);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length *
            Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.Uniform3(uniform_pos, campos.X,campos.Y, campos.Z);
            GL.Uniform1(uniform_aspect, aspect);*/




            //GL.UseProgram(BasicProgramID);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        
        private void InitArrayBuffer(string attributeName)
        {
            int vbo;
            GL.CreateBuffers(1, out vbo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    
                GL.BufferData(BufferTarget.ArrayBuffer,
                    (IntPtr)(vertdata.Length * Vector3d.SizeInBytes),vertdata, BufferUsageHint.StaticDraw);
                int attributeLocation = GL.GetAttribLocation(BasicProgramID, attributeName);
                GL.VertexAttribPointer(attributeLocation, 3, VertexAttribPointerType.Double, false, 0, 0);
                GL.EnableVertexAttribArray(attributeLocation);
            }


       

        public void InitBuffers()
        {
            

            InitArrayBuffer("vPosition");
            InitArrayBufferCamera();
            // InitArrayBuffer("direction");

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int indexBuffer;
            GL.CreateBuffers(1, out indexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            int[] index = { 0, 1, 2, 3 };
                GL.BufferData(BufferTarget.ElementArrayBuffer,
                sizeof(int) * vertdata.Length, index, BufferUsageHint.StaticDraw);
            GL.DrawElements(PrimitiveType.Quads, vertdata.Length, DrawElementsType.UnsignedInt, 0);
        


    }

        private void InitArrayBufferCamera()
        {
            int uniform_cameraLocation = GL.GetUniformLocation(BasicProgramID, "cameraPosition");
            GL.Uniform3(uniform_cameraLocation, campos.X, campos.Y, campos.Z);

            int uniform_aspectLocation = GL.GetUniformLocation(BasicProgramID, "aspect");
            GL.Uniform1(uniform_aspectLocation, aspect);
        }



    
    

        public void Shaders () { 
        
          
            var vShaderSource =
                
                   @" 
                #version 130
                uniform float aspect;
                uniform vec3 cameraPosition;
                in vec3 vPosition; //Входные переменные vPosition - позиция вершины
                out vec3 origin, direction;
                void main()
                {
                gl_Position = vec4(vPosition, 1.0);
                direction = normalize(vec3(vPosition.x*aspect, vPosition.y, -1.0));
                origin = cameraPosition;
                }

                ";
            var fShaderSource =
                @"
                    #version 130
                    in vec3 origin, direction;
                    out vec4 outputColor;
                    ";
            var vShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vShader, vShaderSource);
            GL.CompileShader(vShader);
            Console.WriteLine(GL.GetShaderInfoLog(vShader));

            var fShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fShader, fShaderSource);
            GL.CompileShader(fShader);
            Console.WriteLine(GL.GetShaderInfoLog(fShader));

            BasicProgramID = GL.CreateProgram();
            GL.AttachShader(BasicProgramID, vShader);
            GL.AttachShader(BasicProgramID, fShader);
            GL.LinkProgram(BasicProgramID);
            GL.UseProgram(BasicProgramID);
        }




        /*public void Draw()
        {
            uniform_pos = GL.GetUniformLocation(BasicProgramID, "uniform_pos");
            uniform_aspect = GL.GetUniformLocation(BasicProgramID, "uniform_aspect");
           _uColor = GL.GetUniformLocation(BasicProgramID, "uColor");
            GL.Uniform4(_uColor, 0f, 1f, 0f, 1f);
            

           // _amountOfVertices = InitShaders();

            

            
           // GL.UniformMatrix4(_uNormalMatrixLocation, false, ref _modelMatrix);
             //   GL.UniformMatrix4(_uMvpMatrixLocation, false, ref _mvpMatrix);
               // GL.DrawElements(PrimitiveType.Triangles, _amountOfVertices, DrawElementsType.UnsignedInt, 0);}
        
        /* #version 430
                    out vec4 FragColor;
                    in vec3 glPosition;
                    
                   void main ( void )
                  {
                  FragColor = vec4 ( abs(glPosition.xy), 0, 1.0);
                    }
                    ";*/



    }
}


