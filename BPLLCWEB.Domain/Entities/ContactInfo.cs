using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BPLLCWEB.Domain.Entities
{
    public class ContactInfo
    {
        [Key]
        public int id { get; set; }

        public string CreditunionName { get; set; }

        [Required(ErrorMessage = "Please enter a Name")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Please enter a Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
            ErrorMessage = "Entered Phone format is not valid.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter an Email Address")]
        [EmailAddress(ErrorMessage = "Entered Email is not valid")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter a Problem and/or Issue")]
        public string ProblemIssue { get; set; }

        public string Subject { get; set; }

        public string ContactCode { get; set; }

        [EmailAddress(ErrorMessage = "Entered Email is not valid")]
        public string MailToAddress { get; set; }
    }
}
