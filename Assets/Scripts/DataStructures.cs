
using System.Collections.Generic;
[System.Serializable]
public class FurnitureData
{
    public string Furniture_Name { get; set; }
    public string Furniture_ID { get; set; }
    public Category Category { get; set; }
}

[System.Serializable]
public class TotalFurnitureData
{
    public List<FurnitureData> Furnitures { get; set; }
}

[System.Serializable]
public enum Category { Chairs, Racks, Tables, Bed, Sofa, Others }
public enum CurrentMode { place, rotate, move,none }
