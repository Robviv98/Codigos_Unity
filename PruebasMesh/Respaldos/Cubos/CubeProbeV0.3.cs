using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeProbe : MonoBehaviour
{
    public Vector3[] Vertices;
    public int[] Triangles;
    public Vector3[] Normales;

    public int cuadrax = 1, cuadray = 1, cuadraz = 1;

    int[,,] matriz;
    int[,,] matrizRespaldo;
    public int[] matrizlineal;

    int longitud=0;

    private void Start()
    {
        defmatriz();
        vertis();
        tria();
        norma();
        meshrend();

        //Debug.Log("Generacion");
    }

    void defmatriz()
    {
        matriz = new int[cuadrax, cuadray, cuadraz];
        matriz[0, 0, 0] = 1;
        matriz[cuadrax - 1, cuadray - 1, cuadraz - 1] = 1;
        matriz[cuadrax - 1, 0, 0] = 1;
        matriz[0, cuadray - 1, cuadraz - 1] = 1;


        //prueba poner 1 
        
        for (int x = 0; x < cuadrax; x++)
        {
            for (int y = 0; y < cuadray; y++)
            {
                for (int z = 0; z < cuadraz; z++)
                {
                    matriz[x, y, z] = 1;

                }
            }
        }

        matrizRespaldo=matriz;
        //prueba llenar 1 fin

        for (int x = 1; x < cuadrax-1; x++)
        {
            for (int y = 1; y < cuadray-1; y++)
            {
                for (int z = 1; z < cuadraz-1; z++)
                {

                    if(matriz[x, y+1, z] == 1)
                    {
                        matriz[x, y, z] = 0;
                    }
                    
                }
            }
        }



        //Prueba hacer matriz hueca




        //Prueba hacer matriz hueca fin


        for (int x = 0; x < cuadrax; x++)
        {
            for (int y = 0; y < cuadray; y++)
            {
                for (int z = 0; z < cuadraz; z++)
                {
                    if (matriz[x,y,z] == 1)
                    {
                        longitud++;
                    }
                }
            }
        }

        int a = 0, b = 0, c = 0;

        Debug.Log(cuadrax * cuadray * cuadraz);
        Debug.Log(longitud);

        matrizlineal = new int[cuadrax * cuadray * cuadraz];

        for(int i=0;i< cuadrax * cuadray * cuadraz; i++)
        {
            matrizlineal[i] = matriz[a, b, c];
            a++;
            if (a == cuadrax)
            {
                a = 0;
                b++;
            }
            if (b == cuadray)
            {
                b = 0;
                c++; 
            }
            if (c == cuadraz)
            {
                c = 0;
            }
        }



        Debug.Log("longitud: "+longitud);
    }


    void vertis()
    {
        Vertices = new Vector3[24 * cuadrax * cuadray * cuadraz];

        Vector3[] vertb ={

            //Abajo
            new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0),
            
            //Arriba
            new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0),

            //Derecha
            new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1),

            //Izquierda
            new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0),

            //Frente
            new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0),

            //Atras
            new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1)


        };

        int sumador=0;

        for (int x = 0; x < cuadrax; x++)
        {
            for (int y = 0; y < cuadray; y++)
            {
                for (int z = 0; z < cuadraz; z++)
                {
                    for (int rec = 0; rec < vertb.Length; rec++)
                    {
                        //Debug.Log("x: " + x + "y: " + y + "z: " + z);
                       // Debug.Log(sumador + rec);
                        Vertices[rec + sumador] = vertb[rec]+(new Vector3(x,y,z));

                        
                    }
                    sumador += vertb.Length;
                }
            }
            
        }
        //Debug.Log(Vertices);
        //Debug.Log(Vertices.Length);
        //Debug.LogError("pausador");
        //Vertices = vertb;
    }

    void tria()
    {
        Triangles = new int[36 * longitud];

        int[] triab =
        {
            //abajo
            0, 2, 1,
            0, 3, 2,

            //arriba
            4, 5, 6,
            4, 6, 7,

            //derecha
            8, 9, 10,
            8, 10, 11,

            //izquierda
            12, 13, 14,
            12, 14, 15,

            //frente
            16, 17, 18,
            16, 18, 19,

            //atras
            20, 21, 22,
            20, 22, 23


        };

        int suma = 0;
        int suma2 = 0;

        for(int i =0;i< cuadrax * cuadray * cuadraz; i++)
        {
            if (matriz[0, 0, 0] == 1)
            {
                for (int c = 0; c < 36; c++)
                {
                    Triangles[suma + c] = triab[c] + suma2 + i;
                }
            }
            if (matrizlineal[i] == 1)
            {
                suma += 36;
                
            }
            suma2 += 23;

        }

        //Triangles = triab;

    }

    void norma()
    {
        //definicion 1 cubo

        //5) Define each vertex's Normal
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 forward = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;


        Vector3[] normals = new Vector3[]
        {
            down, down, down, down,             // Bottom
	        left, left, left, left,             // Left
	        forward, forward, forward, forward,	// Front
	        back, back, back, back,             // Back
	        right, right, right, right,         // Right
	        up, up, up, up	                    // Top
        };

        Normales = normals;

    }

    void meshrend()
    {
        AssetDatabase.DeleteAsset("Assets/mesh/cubemesh.asset");
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
     //   Debug.Log("Vertices: "+ Vertices.Length);
     //   Debug.Log("Triangles: " + Triangles.Length);
     //   Debug.Log("Normales: " + Normales.Length);
        //mesh.normals = Normales;
        mesh.Optimize();
        mesh.RecalculateNormals();
        //AssetDatabase.DeleteAsset("Assets/mesh/cubemesh.asset");
        AssetDatabase.CreateAsset(mesh, "Assets/mesh/cubemesh.asset");
    }


}
