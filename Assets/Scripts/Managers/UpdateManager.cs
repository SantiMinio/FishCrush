using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager
{
    List<IUpdate> updateList = new List<IUpdate>();
    public bool stopUpdate;

    public void OnUpdate()
    {
        if (stopUpdate) return;
        for (int i = 0; i < updateList.Count; i++)
        {
            updateList[i].OnUpdate();
            if (stopUpdate) break;
        }
    }


    public void SuscribeToUpdate(IUpdate update)
    {
        if (!updateList.Contains(update)) updateList.Add(update);
    }

    public void DesuscribeToUpdate(IUpdate update)
    {
        if (updateList.Contains(update)) updateList.Remove(update);
    }
}
