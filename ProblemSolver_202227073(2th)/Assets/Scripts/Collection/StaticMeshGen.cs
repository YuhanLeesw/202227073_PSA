using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;


// �����Ϳ��� ��ũ��Ʈ�� ����� ���� �ν����ͷ� Ȯ���ϴ� Ŭ����
[CustomEditor(typeof(StaticMeshGen))]
public class StaticMeshGenEditor : Editor
{
    //��ư����� ����
    // �ν����� GUI�� ���� �������̵� �޼���
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen script = (StaticMeshGen)target; // StaticMeshGen Ÿ���� ���� ��ũ��Ʈ �ν��Ͻ��� ������.
        // �ν����Ϳ� 'Generate Mesh' ��ư�� �����ϰ�, �� ��ư�� Ŭ���ϸ� GenerateMesh �Լ��� ȣ��
        if (GUILayout.Button("Mesh ������ư"))
        {
            script.GenerateMesh();
        }

    }
}


// �޽��� �����ϴ� �� ��ũ��Ʈ
public class StaticMeshGen : MonoBehaviour
{
    // �޽��� �����ϰ� �����ϴ� �޼���
    public Material material;
    private float height= 3;
    private void Start()
    {
        GenerateMesh();
    }
    public void GenerateMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        if (mf == null) mf = this.AddComponent<MeshFilter>();
        if (mr == null) mr = this.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            // ������ �׸���

            // ��� �ﰢ��            
            // �ո�
            new Vector3(0.0f, 0.0f, 0.0f), // 0 ����            
            new Vector3(-2.0f, -6.0f, 0.0f), // 1
            new Vector3(2.0f, -6.0f, 0.0f), // 2

            // ���� �ﰢ��
            new Vector3(-3.3f, -2.2f, 0.0f), // 3          

            // ������ �ﰢ��
            new Vector3(3.3f, -2.2f, 0.0f), // 4

            // �޸�
            // ��� �ﰢ��            
            new Vector3(0.0f, 0.0f, height), // 5 ����            
            new Vector3(-2.0f, -6.0f, height), // 6
            new Vector3(2.0f, -6.0f, height), // 7

            // ���� �ﰢ��
            new Vector3(-3.3f, -2.2f, height), // 8          

            // ������ �ﰢ��
            new Vector3(3.3f, -2.2f, height), // 9

            // �� �ٸ� �׸���
            // �ո�
            new Vector3(4.06f,2.09f,0f), // 10
            new Vector3(6.47f,-5.28f,0f), // 11
            new Vector3(0f,-10.00f,0f), // 12
            new Vector3(-6.47f,-5.28f,0f), // 13
            new Vector3(-4.06f, 2.09f, 0f), // 14 

            // �޸�
            new Vector3(4.06f,2.09f, height), // 15
            new Vector3(6.47f,-5.28f, height), // 16
            new Vector3(0f,-10.00f, height), // 17
            new Vector3(-6.47f,-5.28f,height), // 18
            new Vector3(-4.06f, 2.09f, height), // 19 

            // ����

                                    
        };

        // �ﰢ�� �ε��� ����
        int[] triangles = new int[]
        {
            // ������ ���
            // ������ �ո�
            0,2,1,
            3,0,1,
            0,4,2,
            
            // ������ �޸�
            5,6,7,
            5,8,6,
            7,9,5,

            // ���� ����
            3,5,0,
            8,5,3,
            3,1,6,
            8,3,6,
            // ���� ������
            0,5,9,
            0,9,4,
            2,4,7,
            7,4,9,

            // �Ʒ���
            7,1,2,
            6,1,7,

            //----------------------------------------------------------------

            // �� �׸���
            // �ո�
            4,0,10,
            2,4,11,
            1,2,12,
            3,1,13,
            0,3,14,

            // �޸�
            5,9,15,
            9,7,16,
            6,17,7,
            8,18,6,
            19,8,5,
            
            // ����
            0,5,15,
            0,15,10,

            4,10,15,
            4,15,9,

            16,4,9,
            16,11,4,

            16,2,11,
            16,7,2,

            12,2,7,
            12,7,17,

            12,6,1,
            12,17,6,

            6,13,1,
            6,18,13,

            18,3,13,
            18,8,3,

            8,14,3,
            8,19,14,

            19,0,14,
            19,5,0,
        };

        // ���� ���� ���
        Vector3[] normals = new Vector3[vertices.Length];

        // �� �ﰢ���� ���� ���� ���
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            normals[triangles[i]] += normal;
            normals[triangles[i + 1]] += normal;
            normals[triangles[i + 2]] += normal;
        }

<<<<<<< Updated upstream
        // ������ ����ȭ
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

        // Mesh ��ü�� ������, �ﰢ��, ���� �Ҵ�
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        // Material �Ҵ�
        if (material != null)
            mr.materials = new Material[] { material };
=======
        // �� ���� �������� �ϳ��� ����Ʈ�� ��ħ
        List<Vector3> combinedVertices = new List<Vector3>(Star1);
        combinedVertices.AddRange(Star2); // �� ��° ���� ������ �߰�
        Star1 = combinedVertices.ToArray();
   
        mesh.vertices = Star1; // �޽ÿ� ���� �迭 �Ҵ�
        
        // ���� ���� ��� �� �Ҵ�
        mesh.normals = CalculateNormals(mesh.vertices, mesh.triangles);
        mesh.triangles = GenerateTriangles(Star1);// �޽��� �ﰢ���� �����ϰ� �Ҵ�

        
  
        MeshFilter mf = GetComponent<MeshFilter>() ?? gameObject.AddComponent<MeshFilter>(); // MeshFilter ������Ʈ�� �������ų� ������ �߰�  
        MeshRenderer mr = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>(); // MeshRenderer ������Ʈ�� �������ų� ������ �߰�
>>>>>>> Stashed changes


        mf.mesh = mesh;
    }
<<<<<<< Updated upstream
=======

    // �־��� �������� ���� ���� �������� �� ����� �������� ����ϴ� �޼���
    Vector3[] CalculateStarVertices(float innerRadius, float outerRadius, int numPoints)
    {
        List<Vector3> Star1 = new List<Vector3>();
        float angleStep = 360.0f / (numPoints * 2);  // ������ ������ ������ ���
        float angleOffset = 90.0f; // ù ��° �������� �������� �ø��� ���� ���� ����

        Star1.Add(Vector3.zero);// �������� �߽����� �߰�

        
        for (int i = 0; i < numPoints * 2; i++)// ���� �ٱ��ʰ� ���� �������� �����ư��鼭 ���
        {
            float angle = Mathf.Deg2Rad * (i * angleStep + angleOffset); // ������ ������ �߰�
            float radius = (i % 2 == 0) ? outerRadius : innerRadius; // ¦�� �ε��������� �ܺ� ������, Ȧ�� �ε��������� ���� ������ ���
            Vector3 vertex = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Star1.Add(vertex);
        }

        // ù ��° �ܺ� �������� �ٽ� �߰��Ͽ� �� ����� �������� ��
        Star1.Add(Star1[1]); // ���� �ܰ����� �������� �������� ù ��° �ܺ� �������� �ٽ� �߰�

        return Star1.ToArray();
    }


    // ���� �迭�� ������� �޽��� �ﰢ�� �迭�� �����ϴ� �޼���
    int[] GenerateTriangles(Vector3[] vertices)
    {
        List<int> triangles = new List<int>();

        int halfLength = vertices.Length / 2; // Star1�� Star2 ������ ������ ��

        // �� ���� ���� �ո�� �޸��� �ﰢ�� ����
        for (int i = 0; i < halfLength - 1; i++)
        {
            int next = (i + 1) % (halfLength - 1);
            int nextPlusOne = (i + 2) % (halfLength - 1);

            // �ո� �ﰢ��
            triangles.Add(0); // �߽���
            triangles.Add(next); // ���� ������
            triangles.Add(i + 1); // ���� ������

            // �޸� �ﰢ�� (�ݴ� ����)
            triangles.Add(halfLength); // �߽���
            triangles.Add(halfLength + i + 1); // ���� ������
            triangles.Add(halfLength + next); // ���� ������
        }

        // Star1�� Star2�� �����ϴ� �ﰢ���� ���ؼ��� �����ϰ� ����
        for (int i = 1; i < halfLength - 1; i++)
        {
            // Star1 -> Star2 -> Star2
            triangles.Add(i);
            triangles.Add(i + halfLength);
            triangles.Add(i + halfLength + 1);

            // �ݴ� ���� (�޸�)
            triangles.Add(i + halfLength + 1);
            triangles.Add(i + halfLength);
            triangles.Add(i);

            // Star1 -> Star2 -> Star1
            triangles.Add(i);
            triangles.Add(i + halfLength + 1);
            triangles.Add(i + 1);

            // �ݴ� ���� (�޸�)
            triangles.Add(i + 1);
            triangles.Add(i + halfLength + 1);
            triangles.Add(i);
        }

        return triangles.ToArray();
    }

    // ���� ���͸� ����ϴ� �޼���
    private Vector3[] CalculateNormals(Vector3[] vertices, int[] triangles)
    {
        Vector3[] normals = new Vector3[vertices.Length];
        Vector3[] triangleNormals = new Vector3[triangles.Length / 3];

        // �ﰢ���� ���� ���
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v1 = vertices[triangles[i]];
            Vector3 v2 = vertices[triangles[i + 1]];
            Vector3 v3 = vertices[triangles[i + 2]];
            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;
            triangleNormals[i / 3] = normal;

            normals[triangles[i]] += normal;
            normals[triangles[i + 1]] += normal;
            normals[triangles[i + 2]] += normal;
        }

        // ������ ������ ����ϰ� ����ȭ
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

        return normals;
    }

>>>>>>> Stashed changes
    // Update is called once per frame
    void Update()
    {
        //��ũ��Ʈ ���� ����Ŭ
        //OnInspectorGUI:
        //CalculateStarVertices �Լ�
        //innerRadius: ���� ���� ����������, ���� ���� �ڳ�(������ �κ�)�� �ݰ��� ��Ÿ��
        //outerRadius: ���� �ܺ� ����������, ���� �ܺ� �ڳ�(������ �κ�)�� �ݰ��� ��Ÿ��
        //numPoints: ���� ������(������ �κ�)�� ���� ��Ÿ���ϴ�.�Ϲ����� ���� 5���� �������� �����Ƿ�, �������� ��Ÿ���� �� �ַ� ���
        //Unity �����Ϳ��� ����� ���� �ν�����(GUI)�� ����µ� ���
        //"Generate Mesh" ��ư�� �߰��ϰ�, ��ư�� Ŭ���� �� GenerateMesh �Լ��� ȣ���Ͽ� �޽ø� �����ϴ� �� ���

        //GenerateMesh:
        //���� ������ ���: CalculateStarVertices �Լ��� ȣ���Ͽ� ù ��° ���� �� ��° ���� �������� ���
        //�� ȸ�� �� �̵�: �� ��° ���� �������� ȸ���� �̵��� �����Ͽ�, ù ��° ���� �����ǰ� ��
        //���� �迭 ����: �� ���� ���� �迭�� �ϳ��� �����Ͽ� �޽��� ���� �迭�� ����
        //�ﰢ�� �迭 ����: GenerateTriangles �Լ��� ���� �޽ø� �����ϴ� �ﰢ���� �ε��� �迭�� ����
        //�޽� ����: ���� �迭�� �ﰢ�� �迭�� �޽ÿ� �Ҵ��ϰ�, �޽��� ������ ������
        //�޽� ������ ����: �޽ø� ������ MeshFilter ������Ʈ�� �޽ÿ� ������ ������ MeshRenderer ������Ʈ�� �����ϰų� �߰�
        //���� ����: �޽ÿ� ����� ������ ����

        //CalculateStarVertices:
        //�� ����� ������ ����ϴ� �Լ�
        //������(�� ���)�� �������� ����ϱ� ����, �ܺ� �������� ���� �������� �����ư��� ����
        //�� �Լ��� �߽������� �����Ͽ� �־��� ������ �������� ����� ���� �� �������� ��ġ�� ���
        //�� ����� �������� �������� ù ��° �������� �ٽ� �迭�� �߰�

        //GenerateTriangles:
        //�޽ø� �����ϴ� �ﰢ���� �ε��� �迭�� �����ϴ� �Լ�
        //�� ���� �ո�� �޸鿡 ���� �ﰢ���� �����ϰ�, �� ���� �����ϴ� ���鿡 ���� �ﰢ���� ����
        //�� �Լ��� vertices �迭�� �Է����� �޾�, �� �迭���� �ﰢ���� �����ϴ� �� �������� �ε����� ����ϰ� �迭�� ��ȯ

    }
}

