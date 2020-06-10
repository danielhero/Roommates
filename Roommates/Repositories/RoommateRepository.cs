using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Roommate";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);


                        int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int rentPortionValue = reader.GetInt32(rentPortionColumnPosition);

                        int moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDateValue = reader.GetDateTime(moveInDateColumnPosition);

                        int roomIdColumnPosition = reader.GetOrdinal("RoomId");
                        int roomIdValue = reader.GetInt32(roomIdColumnPosition);

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            MovedInDate = moveInDateValue,
                            Room = null
                        };

                        roommates.Add(roommate);
                    }

                    reader.Close();

                    return roommates;
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName"))
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }

        public List <Roommate> GetAllWithRoom(int roomId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"SELECT 
                                        roommate.FirstName, 
                                        roommate.LastName, 
                                        room.Name 
                                        FROM Roommate roommate 
                                        JOIN Room room ON roommate.RoomId = room.Id 
                                        WHERE roommate.roomId = @roomId";
                    cmd.Parameters.AddWithValue("roomId", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommateRoom = new List<Roommate>();

                    while (reader.Read())
                    {
                        Roommate eachRoommate = new Roommate
                        {
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Room = new Room()
                            {
                                Id = roomId,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };

                        roommateRoom.Add(eachRoommate);
                    };
                        
                    reader.Close();

                    return roommateRoom;
                } 
            }
        }
        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Roommate (FirstName, LastName, RentPortion, MoveInDate, RoomId) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@FirstName, @LastName, @RentPortion, @MoveInDate, @RoomId)";
                    cmd.Parameters.AddWithValue("@FirstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MovedInDate);
                    cmd.Parameters.AddWithValue("@RoomId", roommate.Room.Id);

                    int roommateId = (int)cmd.ExecuteScalar();

                    roommate.Id = roommateId;
                }
            }
        }

        public void Update(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Roommate
                                    SET FirstName = @FirstName,
                                        LastName = @LastName,
                                        RentPortion = @RentPortion,
                                        MoveInDate = @MoveInDate,
                                        RoomId = @RoomId
                                    WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@FirstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MovedInDate);
                    cmd.Parameters.AddWithValue("@RoomId", roommate.Room.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
