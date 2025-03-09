using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class City {

    public Vector3 position;
    public string Name;

    public City(string _name, Vector3 location) {
        position = location;
        Name = _name;
    }

    // Override Equals method
    public override bool Equals(object obj) {
        if (obj is City other) {
            return Name == other.Name;
        }
        return false;
    }

    // Override GetHashCode for dictionary/hashset compatibility
    public override int GetHashCode() {
        return Name.GetHashCode() ^ position.GetHashCode();
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
        return $"City: {Name}";
    }
}
