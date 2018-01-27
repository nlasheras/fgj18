using UnityEngine;

[CreateAssetMenu]
[CustomGridBrush ( false, true, false, "Spike" )]
public class SpikeBrush : LayerObjectBrush<Spike>
{
    public override void Paint ( GridLayout grid, GameObject layer, Vector3Int position )
    {
        if ( activeObject != null )
        {
            BrushUtility.SetDirty ( activeObject );
        }
        else
        {
            BrushUtility.Select ( BrushUtility.GetRootGrid ( false ).gameObject );
        }
        base.Paint ( grid, layer, position );
    }

    public override void Erase ( GridLayout grid, GameObject layer, Vector3Int position )
    {
        foreach ( var spike in allObjects )
        {
            if ( grid.WorldToCell ( spike.transform.position ) == position )
            {
                DestroySpike ( spike );
                BrushUtility.Select ( BrushUtility.GetRootGrid ( false ).gameObject );
                return;
            }
        }
    }

    private void DestroySpike ( Spike spike )
    {
        DestroyImmediate ( spike.gameObject );
    }
}