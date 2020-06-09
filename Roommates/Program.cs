using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  This is the address of the database.
        ///  We define it here as a constant since it will never change.
        /// </summary>
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            Console.WriteLine("Getting All Rooms:");
            Console.WriteLine();

            List<Room> allRoom = roomRepo.GetAll();

            foreach (Room room in allRoom)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

            Console.WriteLine();


            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting All Roommates:");
            Console.WriteLine();

            List<Roommate> allRoommates = roommateRepo.GetAll();

            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} {roommate.FirstName} {roommate.LastName} {roommate.RentPortion} {roommate.MovedInDate}");
            }

            Console.WriteLine();


            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Roommate with Id 2:");
            Console.WriteLine();

            Roommate singleRoommate = roommateRepo.GetById(2);

            Console.WriteLine($"{singleRoommate.Id} {singleRoommate.FirstName} {singleRoommate.LastName}");
            
            Console.WriteLine();


            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Roommate(s) with RoomId 1:");
            Console.WriteLine();

            List<Roommate> roommatesInRoom = roommateRepo.GetAllWithRoom(1);

            foreach(Roommate roommate in roommatesInRoom)
            {
                Console.WriteLine($"{roommate.FirstName} {roommate.LastName} {roommate.Room.Name}");
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine("Adding new roommate:");
            Console.WriteLine();

            List<Room> someRooms = roomRepo.GetAll();
            Room aRoom = someRooms.Last();

            Roommate roommate4 = new Roommate()
            {
                FirstName = "Nosey",
                LastName = "Nelly",
                RentPortion = 25,
                MovedInDate = DateTime.Now.AddDays(-1),
                Room = aRoom
            };

            roommateRepo.Insert(roommate4);
            
            Console.WriteLine($"Added {roommate4.FirstName} as a new roommate with id {roommate4.Id}");







        }
    }

}