
using System.ComponentModel.DataAnnotations;

namespace CodeBridgeTestTask.Core.Entities
{
    public class Dogs:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public float TailLenght { get; set; }
        [Required]
        public float Weight { get; set; }

    }
}
