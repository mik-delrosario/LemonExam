using LemonExam.Model.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Features.Category
{
    public class CategoryResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }
        public CategoryEntry data { get; set; }
    }
}
