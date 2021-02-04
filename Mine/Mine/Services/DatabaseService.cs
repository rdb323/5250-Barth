using Mine.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mine.Services
{
    class DatabaseService : IDataStore<ItemModel>
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ItemModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ItemModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        /// <summary>
        /// Creates a item and inserts it into the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(ItemModel item)
        {
            if (item == null)
            {
                return false;
            }

            var result = await Database.InsertAsync(item);
            if(result == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates item in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>Partial Checkin – Added UpdateAsync, need to add ID value to DatabaseService.cs
        public async Task<bool> UpdateAsync(ItemModel item)
        {
            if(item == null)
            {
                return false;
            }

            var result = await Database.UpdateAsync(item);
            if(result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes the given id if viable
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var data = await ReadAsync(id);
            if(data == null)
            {
                return false;
            }

            var result = await Database.DeleteAsync(data);
            if(result == 0)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Looks up the item in the database and returns the first one that matches
        /// The Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ItemModel> ReadAsync(string id)
        {
            if(id == null)
            {
                return null;
            }

            //Call the Database to read the ID
            //using Linq syntax Find the first record that the Id matches
            var result = Database.Table<ItemModel>().FirstOrDefaultAsync(mbox => mbox.Id.Equals(id));

            return result;
        }

        /// <summary>
        /// Gets an index of the ItemModels from the database and returns them
        /// </summary>
        /// <param name="forceRefresh">Requires a forced refresh of the table if true</param>
        /// <returns>A list of all of the ItemModels</returns>
        public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
        {
            var result = await Database.Table<ItemModel>().ToListAsync();
            return result;
        }
    }
}
