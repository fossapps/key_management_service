using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Micro.KeyStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro.KeyStore.Api.Keys
{
    public class KeyRepository : IKeyRepository
    {
        private readonly ApplicationContext _db;

        public KeyRepository(ApplicationContext db)
        {
            _db = db;
        }

        public Task<Key> FindById(string id)
        {
            return _db.Keys.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Key> FindByShortSha(string shortSha)
        {
            return _db.Keys.AsNoTracking().FirstAsync(x => x.ShortSha == shortSha);
        }

        public Task<Key> FindBySha(string sha)
        {
            return _db.Keys.AsNoTracking().FirstAsync(x => x.Sha == sha);
        }

        public async Task<IEnumerable<Key>> FindCreatedAfter(DateTime createdAfter)
        {
            return await _db.Keys.AsNoTracking().Where(x => x.CreatedAt > createdAfter).ToListAsync();
        }

        public async Task<string> FindNextShortSha(string sha)
        {
            var info = await _db.Keys.AsNoTracking().Where(x => sha.StartsWith(x.ShortSha)).Select(x => new {x.ShortSha.Length}).OrderByDescending(x => x.Length).FirstAsync();
            return sha.Substring(0, info.Length + 1);
        }

        public async Task Remove(string id)
        {
            _db.Keys.Remove(new Key { Id = id });
            await _db.SaveChangesAsync();
        }

        public async Task<Key> Save(Key key)
        {
            var result = await _db.Keys.AddAsync(key);
            await _db.SaveChangesAsync();
            return result.Entity;
        }
    }
}