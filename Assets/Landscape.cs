using UnityEngine;
using System.Collections;


public class Landscape : MonoBehaviour
{
    public Shader shader;
    public PointLight sun;

    public int gridSize = 65;
    public float maxHeight = 25;
    public float minHeight = -15;
    public float initialNoise = 15;
    private static Color brown = new Color(0.501f, 0.4f, 0.255f);
    private static Color darkGreen = new Color(0.173f, 0.690f, 0.216f);
    private static Color sand = new Color(0.761f, 0.698f, 0.502f);
    private static Color lightBrown = new Color(0.804f, 0.522f, 0.247f);
	float[,] grid;

    // Use this for initialization
    void Start()
    {
        // Add a MeshFilter component
        MeshFilter landscapeMesh = this.gameObject.AddComponent<MeshFilter>();
        landscapeMesh.mesh = this.CreateLandscapeMesh();

        // Add a MeshRenderer component
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;
    }

    void Update()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetColor("_PointLightColor", sun.color);
        renderer.material.SetVector("_PointLightPosition", sun.GetWorldPosition());
    }

    // Method to create a cube mesh with coloured vertices
    Mesh CreateLandscapeMesh()
    {
        Mesh m = new Mesh();
        m.name = "Landscape";

        // Define the vertices.
        float[,] grid = initializeGrid();
        diamondSquare(grid, gridSize, initialNoise);

        Vector3[,] points = new Vector3[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                points[i, j] = new Vector3(i, grid[i, j], j);
            }
        }

        Vector3[,] surfaceNormals = new Vector3[2 * (gridSize - 1), (gridSize - 1)];
        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int j = 0; j < gridSize - 1; j++)
            {
                surfaceNormals[i * 2, j] = Vector3.Normalize(Vector3.Cross((points[i, j + 1] - points[i, j]), (points[i + 1, j] - points[i, j])));
                surfaceNormals[i * 2 + 1, j] = Vector3.Normalize(Vector3.Cross((points[i + 1, j + 1] - points[i, j + 1]), (points[i + 1, j] - points[i, j + 1])));
            }
        }

        Vector3[,] vertexNormals = new Vector3[gridSize, gridSize];
        for (int i = 0; i < gridSize - 1; i++)
        {
            for (int j = 0; j < gridSize - 1; j++)
            {
                vertexNormals[i, j] += surfaceNormals[i * 2, j];
                vertexNormals[i, j + 1] += surfaceNormals[i * 2, j];
                vertexNormals[i + 1, j] += surfaceNormals[i * 2, j];
                vertexNormals[i + 1, j] += surfaceNormals[i * 2 + 1, j];
                vertexNormals[i + 1, j + 1] += surfaceNormals[i * 2 + 1, j];
                vertexNormals[i, j + 1] += surfaceNormals[i * 2 + 1, j];
            }
        }

        for(int i = 0; i < gridSize; i++)
        {
            for(int j = 0; j < gridSize; j++)
            {
                vertexNormals[i, j] = Vector3.Normalize(vertexNormals[i, j]);
            }
        }

        m.vertices = mapGridToArray(points, gridSize);

        // Define the vertex colours
        Color[] colors = new Color[m.vertices.Length];
        for (int i = 0; i < m.vertices.Length; i++)
        {
            if (m.vertices[i].y >= 20.0f)
            {
                colors[i] = Color.white;
            }else if (m.vertices[i].y >= 7.5f)
            {
                colors[i] = lightBrown;
            }else if (m.vertices[i].y >= 0.0f)
            {
                colors[i] = darkGreen;
            }else
            {
                colors[i] = sand;
            }
        }
        m.colors = colors;

        int[] triangles = new int[m.vertices.Length];
        for (int i = 0; i < m.vertices.Length; i++)
            triangles[i] = i;

        m.triangles = triangles;

        m.normals = mapGridToArray(vertexNormals, gridSize);
        return m;
    }

    float[,] initializeGrid()
    {
        grid = new float[gridSize, gridSize];
        grid[0, 0] = Random.Range(minHeight, maxHeight);
        grid[0, gridSize - 1] = Random.Range(minHeight, maxHeight);
        grid[gridSize-1, 0] = Random.Range(minHeight, maxHeight);
        grid[gridSize-1, gridSize-1] = Random.Range(minHeight, maxHeight);

        return grid;
    }

    void diamondSquare(float[,] grid, int size, float noise)
    {
        int preSize = size;
        int squareSize = size;
        while (squareSize > 2)
        {
            for (int i = 0; i < size - 1; i += squareSize - 1)
            {
                for (int j = 0; j < size - 1; j += squareSize - 1)
                {
                    diamondStep(grid, i, j, squareSize, noise);
                }
            }
            for (int i = 0; i < size - 1; i += squareSize - 1)
            {
                for (int j = 0; j < size - 1; j += squareSize - 1)
                {
                    squareStep(grid, i, j, squareSize, noise);
                }
            }
            preSize = squareSize;
            squareSize = (squareSize + 1) / 2;
            noise = noise * squareSize / preSize;
        }
    }
    void diamondStep(float[,] grid, int row, int col, int size, float noise)
    {
        int midPointRow = row + (size - 1) / 2;
        int midPointCol = col + (size - 1) / 2;
        grid[midPointRow, midPointCol] = average(grid[row, col], grid[row, col + size - 1],
            grid[row + size - 1, col], grid[row + size - 1, col + size - 1], noise);
        return;
    }

    void squareStep(float[,] grid, int row, int col, int size, float noise)
    {
        int midPointRow = row + (size - 1) / 2;
        int midPointCol = col + (size - 1) / 2;
        //Left Middle Point
        if (col == 0)
        {
            grid[midPointRow, col] = average(grid[row, col], grid[row + size - 1, col], grid[midPointRow, midPointCol],noise);
        }else
        {
            grid[midPointRow, col] = average(grid[row, col], grid[row + size - 1, col], grid[midPointRow, midPointCol], grid[midPointRow,col-(size-1)/2],noise);
        }

        //Right Middle Point
        if (col+size-1 == gridSize-1)
        {
            grid[midPointRow, col+size-1] = average(grid[row, col+size-1], grid[row + size - 1, col+size-1], grid[midPointRow, midPointCol],noise);
        }
        else
        {
            grid[midPointRow, col+size-1] = average(grid[row, col + size - 1], grid[row + size - 1, col + size - 1], grid[midPointRow, midPointCol],grid[midPointRow,col+size-1+(size-1)/2],noise);
        }

        //Top Middle Point
        if (row == 0)
        {
            grid[row, midPointCol] = average(grid[row, col], grid[row, col+size-1], grid[midPointRow, midPointCol],noise);
        }
        else
        {
            grid[row, midPointCol] = average(grid[row, col], grid[row, col + size - 1], grid[midPointRow, midPointCol], grid[row-(size-1)/2, midPointCol],noise);
        }

        //Bottom Middle Point
        if (row+size-1 == gridSize - 1)
        {
            grid[row+size-1, midPointCol] = average(grid[row + size - 1, col], grid[row + size - 1, col + size - 1], grid[midPointRow, midPointCol],noise);
        }
        else
        {
            grid[row + size - 1, midPointCol] = average(grid[row + size - 1, col], grid[row + size - 1, col + size - 1], grid[midPointRow, midPointCol], grid[row+size-1+(size-1)/2, midPointCol],noise);
        }
    }

    float average(float x1, float x2, float x3, float x4, float noise)
    {
        return (x1 + x2 + x3 + x4) / 4 + Random.Range(-noise,noise);
    }

    float average(float x1, float x2, float x3, float noise)
    {
        return (x1 + x2 + x3) / 3 + Random.Range(-noise, noise);
    }

    Vector3[] mapGridToArray(Vector3[,] grid, int size)
    {
        Vector3[] array = new Vector3[3 * 2 * size * size];
        //Debug.Log(3 * 2 * gridSize ^ 2);
        int count = 0;
        for (int i = 0; i < size - 1; i++)
        {
            for (int j = 0; j < size - 1; j++)
            {
                array[count++] = grid[i, j];
                array[count++] = grid[i, j + 1];
                array[count++] = grid[i + 1, j];
                array[count++] = grid[i + 1, j];
                array[count++] = grid[i, j + 1];
                array[count++] = grid[i + 1, j + 1];
            }
        }
        return array;
    }
	public float get_height(float x, float z)
	{
		float height;

		height = grid[(int)x, (int)z];
		return height;
	}

}
