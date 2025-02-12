﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ParkingLotApi.Models;
using MongoDB.Driver.Linq;

namespace ParkingLotApi.Repositories
{
    public class ParkingLotsRepository : IParkingLotsRepository
    {
        private readonly IMongoCollection<ParkingLot> parkingLotCollection;

        public ParkingLotsRepository(IOptions<ParkingLotDatabaseSettings> parkingLotDatabaseSettingsWithOptions)
        {
            ParkingLotDatabaseSettings parkingLotDatabaseSettings = parkingLotDatabaseSettingsWithOptions.Value;
            var mongoClient = new MongoClient(parkingLotDatabaseSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(parkingLotDatabaseSettings.DatabaseName);
            parkingLotCollection = mongoDatabase.GetCollection<ParkingLot>(parkingLotDatabaseSettings.CollectionName);
        }

        public async Task<ParkingLot> CreateParkingLotAsync(ParkingLot parkingLot)
        {
            await parkingLotCollection.InsertOneAsync(parkingLot);
            return await GetParkingLotByNameAsync(parkingLot.Name);
        }

        public void DeleteParkingLot(string id)
        {
            parkingLotCollection.DeleteOne(_ => _.id == id);
        }

        public async Task<ParkingLot> GetParkingLotByNameAsync(string name)
        {
            return await parkingLotCollection.Find(_ => _.Name == name).FirstOrDefaultAsync();
        }

        public async Task<ParkingLot> GetParkingLotByIdAsync(string id)
        {
            return await parkingLotCollection.Find(_ => _.id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ParkingLot>> GetParkingLotWithPageSizePageIndex(int pageSize, int pageIndex)
        {
            return await parkingLotCollection.AsQueryable().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<ParkingLot>> GetAllAsync()
        {
            return await parkingLotCollection.AsQueryable().ToListAsync();
        }

        public async Task<ParkingLot> UpdateParkingLotAsync(string id, ParkingLot parkingLot)
        {
            await parkingLotCollection.ReplaceOneAsync((_) => _.id == id, parkingLot);
            return await GetParkingLotByIdAsync(id);
        }
    }
}
