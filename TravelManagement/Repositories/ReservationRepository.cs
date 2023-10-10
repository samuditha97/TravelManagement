﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TravelManagement.Interfaces;
using TravelManagement.Models;
using TravelManagement.Repositories;

public class ReservationRepository : IReservationService
{
    private readonly IMongoCollection<Reservation> _reservationCollection;

    public ReservationRepository(IMongoDatabase db)
    {
        _reservationCollection = db.GetCollection<Reservation>("Reservations");
    }

    public async Task CreateReservation(Reservation reservation)
    {
        // Check if there are already 4 reservations with the same ReferenceId
        var existingReservationsCount = await _reservationCollection.CountDocumentsAsync(r => r.ReferenceId == reservation.ReferenceId);

        // Perform validation for the reservation
        if (reservation.ReservationDate < DateTime.Now ||
            existingReservationsCount >= 4 ||
            reservation.TicketCount <= 0)
        {
            throw new ArgumentException("Invalid reservation request.");
        }

        reservation.ReferenceId = GenerateReferenceId();

        await _reservationCollection.InsertOneAsync(reservation);
    }


    public async Task UpdateReservation(string referenceId, Reservation updatedReservation)
    {
        var existingReservation = await GetReservation(referenceId);

        if (existingReservation == null)
        {
            throw new NotFoundException("Reservation not found or cannot be updated.");
        }

        // Do not allow updates to ReferenceId
        updatedReservation.ReferenceId = existingReservation.ReferenceId;

        var filter = Builders<Reservation>.Filter.Eq(r => r.ReferenceId, referenceId);
        var update = Builders<Reservation>.Update
            .Set(r => r.Train, updatedReservation.Train)
            .Set(r => r.TrainClass, updatedReservation.TrainClass)
            .Set(r => r.TicketCount, updatedReservation.TicketCount)
            .Set(r => r.CheckIn, updatedReservation.CheckIn)
            .Set(r => r.CheckOut, updatedReservation.CheckOut)
            .Set(r => r.ReservationDate, updatedReservation.ReservationDate)
            .Set(r => r.IsCanceled, updatedReservation.IsCanceled);

        await _reservationCollection.UpdateOneAsync(filter, update);
    }

    public async Task CancelReservation(string referenceId)
    {
        var existingReservation = await GetReservation(referenceId);

        if (existingReservation == null || (existingReservation.IsCanceled ?? false) || (existingReservation.ReservationDate <= DateTime.Now))
        {
            throw new NotFoundException("Reservation not found or cannot be canceled.");
        }

        var filter = Builders<Reservation>.Filter.Eq(r => r.ReferenceId, referenceId);
        var update = Builders<Reservation>.Update.Set(r => r.IsCanceled, true);

        await _reservationCollection.UpdateOneAsync(filter, update);
    }


    public async Task<Reservation> GetReservation(string referenceId)
    {
        return await _reservationCollection.Find(r => r.ReferenceId == referenceId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        var reservations = await _reservationCollection.Find(_ => true).ToListAsync();
        return reservations;
    }

    private string GenerateReferenceId()
    {

        Random random = new Random();

        int minValue = 10000;
        int maxValue = 99999;

        int randomValue = random.Next(minValue, maxValue);

        return randomValue.ToString();
    }

}