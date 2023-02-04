using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SimpleTypeFloatDict : SerializableDictionary<SimplifiedResourceType,float> 
{
}
[Serializable]
public class TypeFloatDict : SerializableDictionary<ResourceType, float>
{
}
public class GameObjectIntDict : SerializableDictionary<GameObject, int>
{
}