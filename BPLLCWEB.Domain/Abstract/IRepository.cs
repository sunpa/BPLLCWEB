using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPLLCWEB.Domain.Entities;

namespace BPLLCWEB.Domain.Abstract
{
    public interface IRepository
    {
        IQueryable<ContactInfo> ContactInfos { get; }

        IQueryable<BrokerInfo> BrokerInfos { get; }

        IQueryable<ParticipantInfo> ParticipantInfos { get; }
    }
}
