using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace BPLLCWEB.Domain.Entities
{
    public class Logins
    {
        [Key]
        public int UniqueId { get; set; }

        public int UID { get; set; }

        public string UserName { get; set; }

        public string SecretQuestion { get; set; }

        public string SecretAnswer { get; set; }

        public int Errors { get; set; }

        public DateTime? Modified_On { get; set; }

        public byte[] EncryptedPassword { get; set; }

    }

}