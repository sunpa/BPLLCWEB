using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace BPLLCWEB.Domain.Entities
{
    public class Users
    {
        [Key]
        public int uniqueID { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string EmailAdd { get; set; }
    }
}