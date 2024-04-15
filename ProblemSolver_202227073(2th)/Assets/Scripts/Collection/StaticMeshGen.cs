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
        if (GUILayout.Button("Generate Mesh"))
        {
            script.GenerateMesh();
        }

    }
}


// �޽��� �����ϴ� �� ��ũ��Ʈ
public class StaticMeshGen : MonoBehaviour
{
    // �޽��� �����ϰ� �����ϴ� �޼���
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] Star1 = CalculateStarVertices(0.5f, 1f, 5); // ù ��° �� ���� ���       
        Vector3[] Star2 = CalculateStarVertices(0.5f, 1f, 5); // �� ��° �� ���� ���

        // �� ��° ���� ù ��° ���� �ٶ󺸴� �ݴ� �������� ȸ���ϰ�, Z������ -1��ŭ �̵�
        Quaternion rotation = Quaternion.Euler(0, 180, 0); // Y���� �߽����� 180�� ȸ���Ͽ� �ݴ� ������ �ٶ󺸰� ��
        for (int i = 0; i < Star2.Length; i++)
        {
            Star2[i] = rotation * Star2[i]; // ȸ�� ����
            Star2[i] += new Vector3(0, 0, -1); // Z������ -1��ŭ �̵�
        }

        // �� ���� �������� �ϳ��� ����Ʈ�� ��ħ
        List<Vector3> combinedVertices = new List<Vector3>(Star1);
        combinedVertices.AddRange(Star2); // �� ��° ���� ������ �߰�
        Star1 = combinedVertices.ToArray();
   
        mesh.vertices = Star1; // �޽ÿ� ���� �迭 �Ҵ�

        mesh.triangles = GenerateTriangles(Star1);// �޽��� �ﰢ���� �����ϰ� �Ҵ�

        mesh.RecalculateNormals();// �޽��� ���� ����
  
        MeshFilter mf = GetComponent<MeshFilter>() ?? gameObject.AddComponent<MeshFilter>(); // MeshFilter ������Ʈ�� �������ų� ������ �߰�  
        MeshRenderer mr = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>(); // MeshRenderer ������Ʈ�� �������ų� ������ �߰�

        mf.mesh = mesh;// �޽ø� MeshFilter ������Ʈ�� �Ҵ�
        // ���� ���� ��� �� �Ҵ�
        CalculateNormals(mesh, mesh.vertices, mesh.triangles);

        // ����� ������ �����ϰ� �޽ÿ� ����
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>();
        Material yellowMaterial = new Material(Shader.Find("Custom/CartoonShader"));
        meshRenderer.sharedMaterial = yellowMaterial;
    }

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

    // �޽��� ������ �������� ����ϰ� �����ϴ� �޼���
    public void CalculateNormals(Mesh mesh, Vector3[] vertices, int[] triangles)
    {
        Vector3[] normals = new Vector3[vertices.Length];

        // ��� ������ ���� ������ 0���� �ʱ�ȭ
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.zero;
        }

        // �ﰢ���� �� �������� ���� ������ ����
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int index0 = triangles[i];
            int index1 = triangles[i + 1];
            int index2 = triangles[i + 2];

            // �ﰢ���� ���� ���
            Vector3 triangleNormal = Vector3.Cross(
                vertices[index1] - vertices[index0],
                vertices[index2] - vertices[index0]).normalized;

            normals[index0] += triangleNormal;
            normals[index1] += triangleNormal;
            normals[index2] += triangleNormal;
        }

        // ������ ������ ����ȭ�Ͽ� ���ȭ
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
        }

        // �޽ÿ� ���� �迭 �Ҵ�
        mesh.normals = normals;
    }



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
