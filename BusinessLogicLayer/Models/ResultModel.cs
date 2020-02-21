using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Models
{
    public class ResultModel
    {
        public int ChromaticNumber { get; set; }
        public List<Vertex> Vertices { get; set; }
    }
}
