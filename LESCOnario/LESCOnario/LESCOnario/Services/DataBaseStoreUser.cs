﻿using Lesconario.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lesconario.Services
{
    public class DataBaseStoreUser
    {
        private User currentUser;

        readonly SQLiteAsyncConnection database;
        List<User> all = new List<User>();

        public DataBaseStoreUser(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            currentUser = new User();
            database.CreateTableAsync<User>().Wait();
        }

        public void setCurrentUser(User user)
        {
            currentUser = user;
        }

        public User getCurrentUser()
        {
            return currentUser;
        }

        public Task<List<User>> UsersAsync =>
            //Get all users.
            database.Table<User>().ToListAsync();

        public Task<User> GetUserAsync(string email, string password)
        {
            // Get a specific user.
            return database.Table<User>()
                            .Where(i => i.Email.Equals(email) && i.Password.Equals(password))
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(User note)
        {
            if (note.ID != 0)
            {
                // Update an existing user.
                return database.UpdateAsync(note);
            }
            else
            {
                // Save a new user.
                return database.InsertAsync(note);
            }
        }

        public Task<int> DeleteUserAsync(User note)
        {
            // Delete a user.
            return database.DeleteAsync(note);
        }

        private async void addList()
        {
            all = await database.QueryAsync<User>("select * from User");
        }

        public List<User> getList()
        {
            addList();
            return all;
        }

        public User getUser(int id)
        {
            List<User> aux = getList();
            User u = new User();
            for (int i = 0; i < aux.Count; i++)
            {
                if (aux[i].ID == id)
                {
                    u.ID = aux[i].ID;
                    u.UserName = aux[i].UserName;
                    u.Email = aux[i].Email;
                    u.Password = aux[i].Password;
                }
            }
            return u;
        }

    }
}
