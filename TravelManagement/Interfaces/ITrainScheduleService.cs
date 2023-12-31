﻿using System;
using TravelManagement.Models;

namespace TravelManagement.Interfaces
{
    //train schedule interface
	public interface ITrainScheduleService
	{
        Task<IEnumerable<TrainSchedule>> GetAllTrainSchedules();
        Task<TrainSchedule> GetTrainScheduleById(string trainId);
        Task CreateTrainSchedule(TrainSchedule trainSchedule);
        Task UpdateTrainSchedule(TrainSchedule trainSchedule);
        Task DeleteTrainSchedule(string trainId);
    }
}

