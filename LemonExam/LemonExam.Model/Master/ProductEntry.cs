using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LemonExam.Model.Master
{
    [Table("Product")]
    public class ProductEntry
    {
        [Key]
        public Int32 ID { get; set; }
        public Int32 CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
