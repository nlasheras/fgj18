using UnityEditor;
using UnityEngine;

[CustomEditor(typeof( SpikeBrush ) )]
public class SpikeBrushEditor : LayerObjectBrushEditor<Spike> 
{
	public new SpikeBrush brush { get { return (target as SpikeBrush ); } }
	
	public void OnSceneGUI()
	{
		Grid grid = BrushUtility.GetRootGrid(false);
		if (grid != null)
		{
			if (brush.activeObject != null)
			{
				Vector3Int spikePos = grid.WorldToCell(brush.activeObject.transform.position);
			}
		}
	}

	public override void OnPaintInspectorGUI()
	{
		if (BrushEditorUtility.SceneIsPrepared())
		{
			GUILayout.Space(5f);
			GUILayout.Label("Use this brush to spikes.");
		}
		else
		{
			BrushEditorUtility.UnpreparedSceneInspector();
		}
	}
}
