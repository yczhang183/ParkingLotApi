﻿namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public string? Location { get; set; }
    }
}
