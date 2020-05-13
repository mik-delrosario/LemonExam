using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Model.Master
{
    [Table("Category")]
    public class CategoryEntry
    {
        [Key]
        public Int32 ID { get; set; }

        public string Name { get; set; }
    }
}
