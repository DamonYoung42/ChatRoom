﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections;

namespace ChatRoomServer
{
    public class BinaryTree : IEnumerable
    {

        private Node top;
        private int _count = 0;

        public BinaryTree()
        {
            top = null;
            _count = 0;
        }


        // Recursive destruction of binary search tree, called by method clear
        // and destroy. Can be used to kill a sub-tree of a larger tree.
        // This is a hanger on from its Delphi origins, it might be dispensable
        // given the garbage collection abilities of .NET
        private void killTree(ref Node key)
        {
            if (key != null)
            {
                killTree(ref key.left);
                killTree(ref key.right);
                key = null;
            }
        }

        public void Clear()
        {
            killTree(ref top);
            _count = 0;
        }

        public int Count()
        {
            return _count;
        }

        public Node Search(string name)
        {
            Node searchNode = top;
            while (searchNode != null)
            {
                if (String.Compare(name, searchNode.name) == 0)
                    return searchNode;

                if (String.Compare(name, searchNode.name) < 0)
                    searchNode = searchNode.left;
                else
                    searchNode = searchNode.right;
            }
            return null;
        }


        private void Add(Node node, ref Node tree)
        {
            if (tree == null)
                tree = node;
            else
            {
                // If we find a node with the same name then it's 
                // a duplicate and we can't continue
                //int comparison = String.Compare(node.name, tree.name);
                //if (comparison == 0)
                //    throw new Exception();

                if (String.Compare(node.name, tree.name) < 0)
                {
                    Add(node, ref tree.left);
                }
                else
                {
                    Add(node, ref tree.right);
                }
            }
        }

        public Node Insert(string name, TcpClient client)
        {
            Node nodeToAdd = new Node(name, client);
            try
            {
                if (top == null)
                    top = nodeToAdd;
                else
                    Add(nodeToAdd, ref top);
                    _count++;
                return nodeToAdd;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Node LocateParent(string name, ref Node parent)
        {
            Node searchNode = top;
            parent = null;

            while (searchNode != null)
            {
                if (String.Compare(name, searchNode.name) == 0)
                    return searchNode;

                if (String.Compare(name, searchNode.name) < 0)
                {
                    parent = searchNode;
                    searchNode = searchNode.left;
                }
                else
                {
                    parent = searchNode;
                    searchNode = searchNode.right;
                }
            }
            return null;
        }

        public Node LocateSuccessor(Node startNode, ref Node parent)
        {
            parent = startNode;
            // Look for the left-most node on the right side
            startNode = startNode.right;
            while (startNode.left != null)
            {
                parent = startNode;
                startNode = startNode.left;
            }
            return startNode;
        }

        public void Delete(string key)
        {
            Node parent = null;
            // First find the node to delete and its parent
            Node nodeToDelete = LocateParent(key, ref parent);
            if (nodeToDelete == null)
                throw new Exception("Unable to delete node: " + key.ToString());  // can't find node, then say so 

            // Three cases to consider, leaf, one child, two children

            // If it is a simple leaf then just null what the parent is pointing to
            if ((nodeToDelete.left == null) && (nodeToDelete.right == null))
            {
                if (parent == null)
                {
                    top = null;
                    return;
                }

                // find out whether left or right is associated 
                // with the parent and null as appropriate
                if (parent.left == nodeToDelete)
                    parent.left = null;
                else
                    parent.right = null;
                _count--;
                return;
            }

            // One of the children is null, in this case
            // delete the node and move child up
            if (nodeToDelete.left == null)
            {
                // Special case if we're at the root
                if (parent == null)
                {
                    top = nodeToDelete.right;
                    return;
                }

                // Identify the child and point the parent at the child
                if (parent.left == nodeToDelete)
                    parent.right = nodeToDelete.right;
                else
                    parent.left = nodeToDelete.right;
                nodeToDelete = null; // Clean up the deleted node
                _count--;
                return;
            }

            // One of the children is null, in this case
            // delete the node and move child up
            if (nodeToDelete.right == null)
            {
                // Special case if we're at the root			
                if (parent == null)
                {
                    top = nodeToDelete.left;
                    return;
                }

                // Identify the child and point the parent at the child
                if (parent.left == nodeToDelete)
                    parent.left = nodeToDelete.left;
                else
                    parent.right = nodeToDelete.left;
                nodeToDelete = null; // Clean up the deleted node
                _count--;
                return;
            }

            // Both children have nodes, therefore find the successor, 
            // replace deleted node with successor and remove successor
            // The parent argument becomes the parent of the successor
            Node successor = LocateSuccessor(nodeToDelete, ref parent);
            // Make a copy of the successor node
            Node successorCopy = new Node(successor.name, successor.tcpClient);
            // Find out which side the successor parent is pointing to the
            // successor and remove the successor
            if (parent.left == successor)
                parent.left = null;
            else
                parent.right = null;

            // Copy over the successor values to the deleted node position
            nodeToDelete.name = successorCopy.name;
            nodeToDelete.tcpClient = successorCopy.tcpClient;
            _count--;
        }
        public IEnumerator GetEnumerator()
        {
            return top.GetEnumerator();
        }

    }
}