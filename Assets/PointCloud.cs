using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PointCloud : MonoBehaviour
{
    public Text text1;
    public Text text3;
    Renderer rend;
    private Mesh mesh;
    int numPoints = 60000;
    Texture2D tex;
    public float[][] z;
    // Use this for initialization
    void Start()
    {

        mesh = new Mesh();
        rend = GameObject.Find("Depth").GetComponent<Renderer>();    
        GetComponent<MeshFilter>().mesh = mesh;
        
    }
    void Update()

    {
        z = Globals.depth;
        CreateMesh();
    }
    void CreateMesh()
    {
        tex = rend.material.mainTexture as Texture2D;
        Vector3[] points = new Vector3[numPoints];
        int[] indecies = new int[numPoints];
        Color[] colors = new Color[numPoints];
        List<int> row = new List<int>();
        List<int> column = new List<int>();
        for (int j = 0; j< 450; ++j)
        {
            for (int k = 0; k< 448; ++k)
            {
                if (z[j][k] < 1)
                {
                    row.Add(j);
                    column.Add(k);
                }
            }
        }
        text1.text = row.Count.ToString();
        text3.text = "No !!!";
        if (row.Count <= numPoints)
        {   
            
            for (int i = 0; i < points.Length; ++i)
            {
               // points[i] = new Vector3((float)row[i]/ (float)1000 - 0.225f, (float)column[i]/ (float)1000 -0.224f, z[row[i]][column[i]]);
                points[i] = new Vector3(Random.Range(-0.07f, 0.07f), Random.Range(-0.07f, 0.07f), Random.Range(0.1f, 0.7f));
               // text3.text =  points[i].z.ToString();
                indecies[i] = i;
                colors[i] = new Color(1f, 0f, 0f, 1.0f);
            }
        }
       

        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indecies, MeshTopology.Points, 0);

    }
}