using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class MockDataStore : IDataStore<ItemModel>
    {
        readonly List<ItemModel> items;

        public MockDataStore()
        {
            items = new List<ItemModel>()
            {
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Roboko", Description="Herobo", Value=6},
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Aki", Description="Alona~", Value=400000 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Ayame", Description="Yo dayo", Value=1 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Pekora", Description="Ha ^ Ha v Ha ^ Ha v Ha^", Value =  -1},
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Korone", Description="Yubi Yubi!", Value=26}
            };
        }

        public async Task<bool> CreateAsync(ItemModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(ItemModel item)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ItemModel> ReadAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ItemModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}