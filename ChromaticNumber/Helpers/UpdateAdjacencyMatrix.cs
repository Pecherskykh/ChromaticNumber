using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromaticNumber.Helpers
{
    public static class UpdateAdjacencyMatrix
    {
        public static int[,] AddVertex(int[,] adjacencyMatrix, int index)
        {
            int[,] newAdjacencyMatrix = new int[adjacencyMatrix.GetLength(0) + 1, adjacencyMatrix.GetLength(1) + 1];
            int slideI = 0;
            for(int i = 0; i < newAdjacencyMatrix.GetLength(0); ++i)
            {
                if(i == index)
                {
                    ++slideI;
                    continue;
                }
                int slideJ = 0;
                for (int j = 0; j < newAdjacencyMatrix.GetLength(0); ++j)
                {
                    if(j == index)
                    {
                        newAdjacencyMatrix[i, j] = 0;
                        ++slideJ;
                        continue;
                    }
                    newAdjacencyMatrix[i, j] = adjacencyMatrix[i - slideI, j - slideJ];
                }
            }
            return newAdjacencyMatrix;
        }

        public static int[,] RemoveVertex(int[,] adjacencyMatrix, int index)
        {
            int[,] newAdjacencyMatrix = new int[adjacencyMatrix.GetLength(0) - 1, adjacencyMatrix.GetLength(1) - 1];
            int slideI = 0;
            for (int i = 0; i < newAdjacencyMatrix.GetLength(0); ++i)
            {
                if (i == index)
                {
                    ++slideI;
                }
                int slideJ = 0;
                for (int j = 0; j < newAdjacencyMatrix.GetLength(0); ++j)
                {
                    if (j == index)
                    {
                        ++slideJ;
                    }
                    newAdjacencyMatrix[i, j] = adjacencyMatrix[i + slideI, j + slideJ];
                }                
            }
            return newAdjacencyMatrix;
        }
    }
}
