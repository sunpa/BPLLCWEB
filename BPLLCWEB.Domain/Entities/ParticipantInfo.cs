using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BPLLCWEB.Domain.Entities
{
    public class ParticipantInfo
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Please enter a Credit Union Name")]
        public string CreditunionName { get; set; }

        [Required(ErrorMessage = "Please enter a Contact Name")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Please enter a Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
            ErrorMessage = "Entered Phone format is not valid.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter an Email Address")]
        [EmailAddress(ErrorMessage = "Entered Email is not valid")]
        public string EmailAddress { get; set; }

        public string InterestedIn { get; set; }

        [EmailAddress(ErrorMessage = "Entered Email is not valid")]
        public string MailToAddress { get; set; }

        public string Subject { get; set; }
    }
}
