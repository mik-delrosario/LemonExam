using LemonExam.Model.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Features.Product
{
    public class ProductResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }
        public ProductEntry data { get; set; }
    }
}
