
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Furn_instance",menuName = "Furniture/Furn_Object", order =1)]
public class Furn_Instance : ScriptableObject
{
    public string Furn_ID;
    public string Furn_Name;
    public Category_Enum Furn_Category;
    public Sprite Furn_Image;
    public GameObject Furn_Prefab;
}

[CreateAssetMenu(fileName = "Furn_Data", menuName = "Furniture/Furn_Data", order = 2)]
public class Furn_Data : ScriptableObject
{
    public List<Furn_Instance> totalFurnitures;
}


public enum Category_Enum { Chairs, Racks, Tables, Bed, Sofa, Others }
public enum CurrentMode { place, rotate, move,none }
