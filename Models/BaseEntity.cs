using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLime.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedTime { get; set; }

    }
}
