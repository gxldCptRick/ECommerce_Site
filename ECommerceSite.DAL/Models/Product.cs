using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceSite.DAL.Models
{
    public class Product: IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [Range(1.0, double.MaxValue)]
        public decimal Cost { get; set; }

        [Required]
        [MaxLength(420)]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(650)]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        public IList<string> Details { get; set; }

        public Product()
        {
            Details = new List<string>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var propDetails = validationContext.ObjectType.GetProperty(nameof(Details));
            var value = propDetails.GetValue(validationContext.ObjectInstance) as IList<string>;
            if (value is null)
            {
                yield return new ValidationResult($"{nameof(Details)} Cannot Be Null.");
            }

            foreach (var detail in value)
            {
                if (string.IsNullOrWhiteSpace(detail))
                {
                    yield return new ValidationResult($"{nameof(Details)} Cannot Contain a null or whitespace string.");
                }
                else if (detail.Length < 5)
                {
                    yield return new ValidationResult($"{nameof(Details)} Cannot Contain a string that is less than 5 characters long.");
                }
            }
        }
    }
}