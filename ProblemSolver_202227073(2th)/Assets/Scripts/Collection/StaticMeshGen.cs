using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticMeshGen))]
public class StaticMeshGenEditor : Editor
{
    //��ư����� ����
    // �ν����� GUI�� ���� �������̵� �޼���
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen script = (StaticMeshGen)target;
        // 'Generate Mesh' ��ư�� ������ StaticMeshGen ��ũ��Ʈ�� GenerateMesh �Լ��� ȣ��
        if (GUILayout.Button("Generate Mesh"))
        {
            script.GenerateMesh();
        }

    }
}

//�޽������ ����
// �޽��� �����ϴ� �� ��ũ��Ʈ
public class StaticMeshGen : MonoBehaviour
{
    // �޽��� �����ϴ� �޼���
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

        // �� ���� ������ ��ħ
        List<Vector3> combinedVertices = new List<Vector3>(Star1);
        combinedVertices.AddRange(Star2); // �� ��° ���� ������ �߰�
        Star1 = combinedVertices.ToArray();

        // �޽ÿ� ���� �迭 �Ҵ�
        mesh.vertices = Star1;

        // �ﰢ�� �迭 ���� �� �Ҵ�
        mesh.triangles = GenerateTriangles(Star1);

        // �޽��� ���� ����
        mesh.RecalculateNormals();

        // MeshFilter �� MeshRenderer ������Ʈ ����
        MeshFilter mf = GetComponent<MeshFilter>() ?? gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>();

        mf.mesh = mesh;

        // ����� ���͸��� ���� �� ����
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>();
        Material yellowMaterial = new Material(Shader.Find("Custom/YellowShader"));
        meshRenderer.sharedMaterial = yellowMaterial;
    }

    // �� ����� ������ ����ϴ� �޼���
    Vector3[] CalculateStarVertices(float innerRadius, float outerRadius, int numPoints)
    {
        List<Vector3> Star1 = new List<Vector3>();
        float angleStep = 360.0f / (numPoints * 2); // �ܺ� �� ���� �������� ���� ���� ���� �������� ������ ������ ����
        float angleOffset = 90.0f; // ù ��° �������� �������� �ø��� ���� ���� ����

        // �߽��� �߰� (�������� �߽�)
        Star1.Add(Vector3.zero);
        // �ܺ� �� ���� �������� ����ϴ� �ݺ���
        for (int i = 0; i < numPoints * 2; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep + angleOffset); // ������ ������ �߰�
            float radius = (i % 2 == 0) ? outerRadius : innerRadius; // ¦�� �ε��������� �ܺ� ������, Ȧ�� �ε��������� ���� ������ ���
            Vector3 vertex = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Star1.Add(vertex);
        }

        // ù ��° �ܺ� �������� �ٽ� �߰��Ͽ� �� ����� �������� ��
        Star1.Add(Star1[1]);

    return Star1.ToArray();
    }


    // ���ο� �޼ҵ�: ���� �迭�� ������� �ﰢ�� �迭 ����
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

    // Update is called once per frame
    void Update()
        {
        
   
        }
}
