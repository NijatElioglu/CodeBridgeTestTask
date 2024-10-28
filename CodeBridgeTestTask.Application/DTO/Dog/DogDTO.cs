
using System.ComponentModel.DataAnnotations;

namespace CodeBridgeTestTask.Application.DTO.Dog
{
    public class DogDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public float TailLenght { get; set; }
        [Required]
        public float Weight { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; } 
        public bool IsDeleted { get; set; } 

    }
}
