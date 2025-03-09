using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
public class City {

    public string name;
    public string type;
    public Vector3 position;

    public City(string _name, string _type, Vector3 location) {
        name = _name;
        type = _type;
        position = location;
    }

    // Override Equals method
    public override bool Equals(object obj) {
        if (obj is City other) {
            return type == other.type && name == other.name;
        }
        return false;
    }

    // Override GetHashCode for dictionary/hashset compatibility
    public override int GetHashCode() {
        return name.GetHashCode() ^ type.GetHashCode();
    }

    // Overloading == operator
    public static bool operator ==(City a, City b) {
        if (ReferenceEquals(a, b)) return true; // Same reference
        if (a is null || b is null) return false; // Handle null cases
        return a.Equals(b);
    }

    // Overloading != operator
    public static bool operator !=(City a, City b) {
        return !(a == b);
    }

    public override string ToString() {
        return $"City: {name}";
    }
}
