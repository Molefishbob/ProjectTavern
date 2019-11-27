using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGlassFromTable : PlayerUseable
{
    private TableInteractions _table;

    protected override void Awake()
    {
        base.Awake();
        _table = gameObject.GetComponent<TableInteractions>();
        _timer.OnTimerCompleted += TakeGlassFromTable;
    }

    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= TakeGlassFromTable;
    }

    public override void Use(PlayerState player)
    {
        for (int i = 0; i < _table.GlassesOnTable.Length; i++)
        {
            if (_table.GlassesOnTable[i] != null)
            {
                _timer.StartTimer(_interactionTime);
                User = player;
            }
        }
        
    }

    private void TakeGlassFromTable()
    {
        Transform trans = null;

        for (int i = 0; i < _table.GlassesOnTable.Length; i++)
        {
            if(_table.GlassesOnTable[i] != null)
            {
                trans = CheckTransformPosition(_table.GlassesOnTable[i].gameObject.transform);
                _table.GlassesOnTable[i].TakeGlass();
                _table.GlassesOnTable[i] = null;
                break;
            }
        }

        if(trans != null)
        {
            for (int i = 0; i < _table.GlassPlaces.Length; i++)
            {
                if (_table.GlassPlaces[i] == null)
                {
                    _table.GlassPlaces[i] = trans;
                }
            }
        }
        
    }

    private Transform CheckTransformPosition(Transform trans)
    {
        Transform result = null;
        for(int i = 0; i < _table.GlassPlaces.Length; i++)
        {
            if (_table.GlassPlaces[i] != null)
            {
                if (trans.position == _table.GlassPlaces[i].transform.position)
                {
                    result = _table.GlassPlaces[i].transform;
                }
            }
        }

        return result;
    }
}
