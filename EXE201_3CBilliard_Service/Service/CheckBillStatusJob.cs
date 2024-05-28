using EXE201_3CBilliard_Service.Interface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Service
{
    public class CheckBillStatusJob : IJob
    {
        private readonly IBillService _billService;

        public CheckBillStatusJob(IBillService billService)
        {
            _billService = billService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _billService.CheckAndUpdateBillStatusAsync();
        }
    }
}
