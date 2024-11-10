
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Furn_instance",menuName = "Furniture/Furn_Object", order =1)]
public class Furn_Instance : ScriptableObject
{
    public string Furn_ID;
    public Category_Enum Furn_Category_ID;
    public string Furn_Name;
    public Sprite Furn_Image;
    public GameObject Furn_Prefab;
}

[CreateAssetMenu(fileName = "Furn_Data", menuName = "Furniture/Furn_Data", order = 2)]
public class Furn_Data : ScriptableObject
{
    public List<Furn_Instance> totalFurnitures;
    public HashSet<Furn_Category> totalCategories;
}

[CreateAssetMenu(fileName ="Furn_Category",menuName ="Furniture/Furn_Category",order =3)]
public class Furn_Category:ScriptableObject
{
    public Category_Enum Category_ID;
    public string Category_Name;
    public Sprite Category_Sprite;
}

public enum Category_Enum
{
    ctg_ch,//for chairs
    ctg_sof,//for sofas
    ctg_bed//for bed
}
