using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class City {

    public Vector3 position;
    public string type;

    public City(string _type, Vector3 location) {
        position = location;
        type = _type;
    }

    // Override Equals method
    public override bool Equals(object obj) {
        if (obj is City other) {
            return type == other.type && position == other.position;
        }
        return false;
    }

    // Override GetHashCode for dictionary/hashset compatibility
    public override int GetHashCode() {
        return type.GetHashCode() ^ position.GetHashCode();
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
}
