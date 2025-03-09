using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Graph<Type> {

    List<LinkedList<Type>> adj_list;
    bool isDirected;
    public int currentListSize = 0;
    public Graph(int noOfCities, bool directed = false) {
        adj_list = new List<LinkedList<Type>>(noOfCities);
        isDirected = directed;

        for (int i = 0; i < noOfCities; i++) {
            adj_list.Add(new LinkedList<Type>());
        }
    }

    public void AddNode(Type node) {
        for (int i = 0; i < currentListSize; i++) {
            if (adj_list[i].First.Value.Equals(node)) {
                Debug.Log("Node already exits.");
                return;
            }
        }

        adj_list[currentListSize].AddFirst(node);
        currentListSize++;
    }

    public void AddEdge(Type firstNode, Type secondNode) {
        for (int i = 0; i < currentListSize; i++) {
            LinkedList<Type> currentList = adj_list[i];
            if (currentList.First.Value.Equals(firstNode)) {
                currentList.AddLast(secondNode);
            }
        }
        if (!isDirected) {
            for (int i = 0; i < currentListSize; i++) {
                LinkedList<Type> currentList = adj_list[i];
                if (currentList.First.Value.Equals(secondNode)) {
                    currentList.AddLast(firstNode);
                }
            }
        }
    }

    public override string ToString() {
        string allConnections = "";
        for(int i = 0; i < currentListSize; i++) {
            string connections = "";
            LinkedList<Type> currentLList = adj_list[i];
            connections += currentLList.First.Value;
            LinkedListNode<Type> nextNode = currentLList.First.Next;
            while (nextNode != null) {
                connections += " -> " + nextNode.Value;
                nextNode = nextNode.Next;
            }

            allConnections += connections + "\n";
        }
        return allConnections;
    }
}
