using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
	[SerializeField] protected TilemapVisualizer TilemapVisualizer = null;
	[SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

	public void GenerateDungeon() {
		TilemapVisualizer.Clear();
		RunProceduralGeneration();
	}

	public void ClearDungeon() {
		TilemapVisualizer.Clear();
	}

	protected abstract void RunProceduralGeneration();
}
