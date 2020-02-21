using BusinessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{   
    public class A1
    {
        public ResultModel Alhoritm(int[,] graph)
        {
            var resultModel = new ResultModel();
            int colorIndex = 0;
            int count = 0;
            var vertexs = CreateListVertex(graph);

            //сортируем степени по убыванию
            vertexs = vertexs.OrderByDescending(v => v.Pow).ToList();
            vertexs[0].ColorIndex = colorIndex;
            ++count;
            
            //расскраска вершин
            while(vertexs.Count != count)
            {
                for(int i = 1; i < vertexs.Count; ++i)
                {
                    bool flag = true;
                    if(vertexs[i].ColorIndex != null)
                    {
                        continue;
                    }
                    for(int j = 0; j < vertexs.Count; ++j)
                    {
                        if(graph[vertexs[i].Index, vertexs[j].Index] == 1 && vertexs[j].ColorIndex == colorIndex)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if(flag)
                    {
                        vertexs[i].ColorIndex = colorIndex;
                        ++count;
                    }
                }
                if (vertexs.Count != count)
                {
                    ++colorIndex;
                }
            }

            resultModel.ChromaticNumber = colorIndex + 1;
            resultModel.Vertices = vertexs.OrderBy(v => v.Index).ToList();
            return resultModel;
        }

        public List<Vertex> CreateListVertex(int[,] graph)
        {
            var vertexs = new List<Vertex>();
            for(int i = 0; i < graph.GetLength(0); ++i)
            {
                vertexs.Add(new Vertex { Index = i, Pow = Pow(i, graph) });
            }
            return vertexs;
        }

        public int Pow(int index, int[,] graph)
        {
            int pow = 0;

            for(int i = 0; i < graph.GetLength(1); ++i)
            {
                if(graph[index, i] == 1)
                {
                    ++pow;
                }
            }
            return pow;
        }
    }
}
