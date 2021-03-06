﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPLLCWEB.Domain.Entities;

namespace BPLLCWEB.Domain.Abstract
{
    public interface IProcessor
    {
        void ProcessContactInfo(ContactInfo contactinfo);

        void ProcessBrokerInfo(BrokerInfo brokerinfo);

        void ProcessParticipantInfo(ParticipantInfo participantinfo);

        void ProcessNewPasswordSendEmail(string recipient, string username, string newPasword);

        void ProcessSendErrorEmail(string errorMessage, string location);

        void ProcessHackAttemptEmail(string ipaddress, string username, string password);

        //void ProcessLoginLog(string username, string ipaddress, string creditunion, string charternumber);
    }
}
