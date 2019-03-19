using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_3.Models
{
    public class Breed
    {
        public int Id { get; set; }
        public string BreedName { get; set; }
        public virtual ICollection<MyDog> Dogs { get; set; }
        public Breed()
        {
            Dogs = new List<MyDog>();
        }
    }
}
