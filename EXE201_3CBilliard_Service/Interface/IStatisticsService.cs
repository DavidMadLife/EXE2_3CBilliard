﻿using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IStatisticsService
    {
        Task<StatisticsResponse> GetStatisticsSummary();
    }
}