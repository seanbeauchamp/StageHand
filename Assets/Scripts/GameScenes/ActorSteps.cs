using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSteps
{
    public string actorName;
    public List<Step> steps = new List<Step>();

    public ActorSteps(string name)
    {
        actorName = name;
    }
}

public class Step
{
    public int Order;
    public string Action;
    public double Value;

    public Step(int order, string action, double value)
    {
        Order = order;
        Action = action;
        Value = value;
    }
}
