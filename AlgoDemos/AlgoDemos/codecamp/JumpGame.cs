﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    internal class JumpGame
    {
        // int [,] _dp;
        int _l;
        int negative = -10000;
        public bool CanJump(int[] nums)
        {
            _l = nums.Length;
            if (_l == 1)
            {
                return true; // can always jump from node to smae node
            }
            int[,] _dp = new int[_l, _l];
            for (int i = 0; i < _l; i++)
                for (int j = 0; j < _l; j++)
                {
                    _dp[i, j] = (i == j) ? 1 : negative;
                }

            for (int i = 0; i < _l; i++)
            {
                for (int j = 1; j <= nums[i]; j++)
                {
                    if (i + j > _l - 1)
                    {
                        continue;
                    }
                    _dp[i, i + j] = (i == 0) ? 3 : 1; // initialize to 1 if current j can be reached from i. 3 means can reach from 0 node
                    if (i == 0 && j == _l - 1)
                    {
                        return true; // can jump from 0 to last node
                    }
                }
            }

            for (int i = 1; i < _l; i++)
            { // for each node
                for (int j = 0; j < i; j++)
                { // for all preceding nodes
                    if (_dp[0, i] == 3)
                    {
                        continue; //already reachable from node 0 through some way                    
                    }
                    if (_dp[0, j] + _dp[j, i] >= 4)
                    {
                        // meaning 0 to current node is possible && current node to ith node is possible => 0 node to ith node is possible
                        _dp[0, i] = 3;
                    }
                }

            }
            int output = _dp[0, _l - 1];
            return (output == 3) ? true : false;
        }
    }
}
