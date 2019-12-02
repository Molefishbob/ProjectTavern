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

    protected override void OnDestroy()
    {
        base.OnDestroy();
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
                break;
            }
        }

    }


    private void TakeGlassFromTable()
    {
        Transform trans = null;

        for (int i = 0; i < _table.GlassesOnTable.Length; i++)
        {
            if (_table.GlassesOnTable[i] != null)
            {
                trans = CheckTransformPosition(_table.GlassesOnTable[i].gameObject.transform);
                _table.GlassesOnTable[i].Use(User);
                _table.GlassesOnTable[i] = null;
                break;
            }
        }

        if (trans != null)
        {
            for (int i = 0; i < _table.GlassPlaces.Length; i++)
            {
                if (_table.GlassPlaces[i] == null)
                {
                    _table.GlassPlaces[i] = trans;
                    break;
                }
            }
        }
    }

    private Transform CheckTransformPosition(Transform trans)
    {
        GameObject temp = _table.transform.GetChild(transform.childCount - 1).gameObject;
        Transform result = null;
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            if (trans.position == temp.transform.GetChild(i).transform.position)
            {
                result = temp.transform.GetChild(i).transform;
                break;
            }
        }

        return result;
    }
}
