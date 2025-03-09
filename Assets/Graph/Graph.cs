using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Graph<Type> {

    public List<LinkedList<Type>> adj_list;
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
        for(int i = 0; i < currentListSize; i++) {
            if (adj_list[i].First.Equals(node)) {
                Debug.Log("City already exists");
                return;
            }
        }
        adj_list[currentListSize].AddFirst(node);
        currentListSize++;
    }
    public void AddEdge(Type firstNode, Type secondNode) {
        for (int i = 0; i < currentListSize; i++) {
            if (adj_list[i].First.Value.Equals(firstNode)){
                adj_list[i].AddLast(secondNode);
            }
        }

        if (!isDirected) {
            for (int i = 0; i < currentListSize; i++) {
                if (adj_list[i].First.Value.Equals(secondNode)) {
                    adj_list[i].AddLast(firstNode);
                }
            }
        }
    }

    public void VisualizeGraph() {

        for(int i = 0; i < currentListSize; i++) {
            string connections = "";
            LinkedList <Type> currentNode = adj_list[i];

            LinkedListNode<Type> Head = currentNode.First;
            connections += Head.Value;
            LinkedListNode<Type> nextNode = Head.Next;
            while(nextNode!= null) {
                connections += " -> " + nextNode.Value;
                nextNode = nextNode.Next;
            }
            Debug.Log(connections);
        }
    }
}
