using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Graph<Type> {

    List<LinkedList<Type>> adj_list;
    LinkedList<Type> test;
    bool isDirected;
    public Graph(int noOfCities, bool directed = false) {
        adj_list = new List<LinkedList<Type>>(noOfCities);
        isDirected = directed;

        for (int i = 0; i < noOfCities; i++) {
            adj_list.Add(new LinkedList<Type>());
        }
    }

    public void AddNode(Type node) {

        foreach (var list in adj_list) {
            if (list.Contains(node)) {
                Debug.Log("Node already exists");
                return;
            }
        }

        LinkedList<Type> newNode = new LinkedList<Type>();
        newNode.AddFirst(node);
        adj_list.Add(newNode);
    }

    public void AddEdge(Type firstNode, Type SecondNode) {

        for(int i = 0; i < adj_list.Count; i++) {
            if (adj_list[i].First.Equals(firstNode)) {
                adj_list[i].AddLast(SecondNode);
            }
        }
        if (!isDirected) {
            for (int i = 0; i < adj_list.Count; i++) {
                if (adj_list[i].First.Equals(SecondNode)) {
                    adj_list[i].AddLast(firstNode);
                }
            }
        }
    }
}
