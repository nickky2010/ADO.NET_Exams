using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3.Models
{
    public class MyDog
    {
        public int Id { get; set; }
        public int BreedId { get; set; }
        public string Nickname { get; set; }
        public int Height { get; set; }
        public virtual Breed Breed { get; set; }
    }
}
