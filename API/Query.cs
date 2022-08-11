using API.Types;
using Business;
using Key = API.Types.Key;

namespace API;

public class Query
{
    public async Task<IEnumerable<Key>> Keys([Service] IKeyService keyService)
    {
        return (await keyService.FetchAllKeys()).Select(x => x.ToGraphQl());
    }
}
