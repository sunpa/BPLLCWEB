using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPLLCWEB.Domain.Entities;
using System.Data.Entity;

namespace BPLLCWEB.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("EFDbContext")
        {
        }

        public DbSet<ContactInfo> ContactInfos { get; set; }

        public DbSet<BrokerInfo> BrokerInfos { get; set; }

        public DbSet<ParticipantInfo> ParticipantInfos { get; set; }

        public DbSet<Users> users { get; set; }

        public DbSet<Logins> logins { get; set; }

    }
}
