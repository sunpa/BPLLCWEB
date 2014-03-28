using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPLLCWEB.Domain.Abstract;
using BPLLCWEB.Domain.Entities;

namespace BPLLCWEB.Domain.Concrete
{
    public class EFRepository : IRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<ContactInfo> ContactInfos
        {
            get { return context.ContactInfos; }
        }

        public IQueryable<BrokerInfo> BrokerInfos
        {
            get { return context.BrokerInfos; }
        }

        public IQueryable<ParticipantInfo> ParticipantInfos
        {
            get { return context.ParticipantInfos;  }
        }
    }
}
