using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /**
 * Definition for a binary tree node.
 * */
    public class TreeNode {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null) {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }


    public class InorderPreorder
    {
        public TreeNode BuildTree(int[] preorder, int[] inorder)
        {
            //check for empty input
            if (preorder.Length == 0 || inorder.Length == 0)
            {
                return null;
            }

            //initialize a hashmap for inorder traversal
            Dictionary<int, int> map = new Dictionary<int, int>();
            for (int i = 0; i < inorder.Length; i++)
            {
                map[inorder[i]] = i;
            }

            //call helper function
            return helper(preorder, 0, preorder.Length - 1, inorder, 0, inorder.Length - 1, map);
        }

        /*
         * Observation is - All elements (in Inroot array ) to the left of root element are actually in left subtree & all elements to right of root element all in right subtree of root element
         * Another observation is - Preorder array has root elements in certain order - all left root elements followed by all right root elements (note even leaf nodes occur here as root element)
         * So the root node is 0th element in Preorder array. root of the left subtree of element is the one that follows next. To calculate root of right subtree of root eleement - we just add number of left elements to left subtree root. If this approach
         * is confusing then just follow the approach of scanning preorder order for given numbers of inorder array and select first non visited element of preorder array as root element
         */
        public TreeNode helper(int[] preorder, int preStart, int preEnd, int[] inorder, int inStart, int inEnd, Dictionary<int, int> map)
        {
            Console.WriteLine("");
            Console.WriteLine($"preStart={preStart}, preEnd={preEnd}, inStart={inStart}, inEnd={inEnd}");
            StringBuilder sb = new StringBuilder();
            for (int i = preStart; i <= preEnd; i++)
            {
                sb.Append(preorder[i]);
                if (i != preEnd )
                {
                    sb.Append(",");
                }
            }
            Console.WriteLine($"preorder={sb.ToString()}");
            sb = new StringBuilder();
            for (int i = inStart; i <= inEnd; i++)
            {
                sb.Append(inorder[i]);
                if (i != inEnd)
                {
                    sb.Append(",");
                }
            }
            Console.WriteLine($"inorder={sb.ToString()}");
            //base case
            if (preStart > preEnd || inStart > inEnd)
            {
                return null;
            }

            //create the root node with the first element in the preorder traversal
            TreeNode root = new TreeNode(preorder[preStart]);

            //find the index of the root node in the inorder traversal
            int inRoot = map[root.val];

            //calculate the number of nodes in the left subtree
            int numsLeft = inRoot - inStart;

            Console.WriteLine($"inRoot={inRoot}, inStart={inStart}, numsLeft={numsLeft}");

            //recursively build the left and right subtrees
            // left subtree enter is previous preStart +1 & left sub tree ends at previous preStart + nums Left (correspondingly inorder changes)
            // right subtree enter is previous preStart +1 + numsLeft & left sub tree ends at previous preEnd (correspondingly inorder changes)
            //another alternative to this logic of finding root of right subtree is - for given subtree of numbers ([inStart - inRoot-1] (or) [inRoot+1, inEnd])  root of this subtree is first element of inorder subarray that occurs in rpeorder array and is not visited yet


            int rightRoot = preStart + numsLeft + 1; // or you could scan inorder array [inroot+1, inEnd] elements in preOrder [preStart , preEnd] array to find which one occurs first 
            int leftRoot = preStart + 1;// or you could scan  inorder array [inStart, inroot-1] elements in preOrder [preStart , preEnd] array to find which one occurs first 

            root.left = helper(preorder, leftRoot , preStart + numsLeft, inorder, inStart, inRoot - 1, map);
            root.right = helper(preorder, rightRoot , preEnd, inorder, inRoot + 1, inEnd, map);

            return root;
        }

    }
}
