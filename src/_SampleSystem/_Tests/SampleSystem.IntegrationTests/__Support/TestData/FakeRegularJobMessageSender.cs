using System;
using System.Collections.Generic;

using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.Utils;
using SampleSystem.IntegrationTests.__Support.Utils.Framework;

namespace SampleSystem.IntegrationTests.__Support.TestData
{
    public class FakeRegularJobMessageSender : IMessageSender<RunRegularJobModel>
    {
        public readonly List<RunRegularJobModelStrictDTO> Queue = new List<RunRegularJobModelStrictDTO>();

        private FakeRegularJobMessageSender()
        {
        }

        public void Send(RunRegularJobModel message, TransactionMessageMode sendMessageMode = TransactionMessageMode.Auto)
        {
            var dto = new RunRegularJobModelStrictDTO
            {
                InstanceServerName = message.InstanceServerName,
                Mode = message.Mode,
                RegularJob = message.RegularJob.ToIdentityDTO()
            };

            this.Queue.Add(dto);
        }

        public static readonly FakeRegularJobMessageSender Instance = new FakeRegularJobMessageSender();
    }
}
