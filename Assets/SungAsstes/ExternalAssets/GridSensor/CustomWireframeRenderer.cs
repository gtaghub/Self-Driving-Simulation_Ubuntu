using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MBaske.Sensors.Grid;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomWireframeRenderer : MonoBehaviour
{
	private MeshFilter meshFilter;
	private Vector3[] vertices;
	private int[] indices;
	private Vector2[] uv;

	private void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();

		GenerateMesh();
	}

	private void GenerateMesh()
	{
		// ���� ����
		vertices = new Vector3[24]
		{
			// Front
			new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 0),
			// Back
			new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 1),
			// Right
			new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0),
			// Left
			new Vector3(0, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 1),
			// Up
			new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0), new Vector3(0, 1, 0),
			// Down
			new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(0, 0, 1)
		};

		// ������ �մ� ������ ����
		indices = new int[36]
		{
			// Front
			0, 1, 2, 0, 2, 3,
			// Back
			4, 5, 6, 4, 6, 7,
			// Right
			8, 9, 10, 8, 10, 11,
			// Left
			12, 13, 14, 12, 14, 15,
			// Up
			16, 17, 18, 16, 18, 19,
			// Down
			20, 21, 22, 20, 22, 23
		};

		// �� ������ uv ��ǥ ����
		uv = new Vector2[24]
		{
			// Front
			new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0),
			// Back
			new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0),
			// Right
			new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0),
			// Left
			new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0),
			// Up
			new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0),
			// Down
			new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0)
		};

		// �޽ø� �����ϰ�, ����(vertices)�� ������ �� meshFilter�� mesh�� ���
		Mesh mesh = new Mesh();

		mesh.name = "PrimitiveCube";
		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.uv = uv;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		meshFilter.mesh = mesh;
	}
}