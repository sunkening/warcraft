using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoadTaskGroup
{
    List<ResourceLoadTask> taskList=new List<ResourceLoadTask>();
    public int progress=0;
    public int state;
    public void addTask(ResourceLoadTask task)
    {
        taskList.Add(task);
    }

    public List<ResourceLoadTask> getTaskList()
    {
        return taskList;
    }
}
