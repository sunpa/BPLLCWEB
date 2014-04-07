using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BPLLCWEB.Domain.Entities
{
    public class LoginAttempts
    {
        [Key]
        public int UniqueId { get; set; }

        public string ClientIP { get; set; }

        public int Unlocked { get; set; }

        public DateTime? DateAdded { get; set; }

    }
}
