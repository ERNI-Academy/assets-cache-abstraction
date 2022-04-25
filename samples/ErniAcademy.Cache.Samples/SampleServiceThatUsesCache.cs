using ErniAcademy.Cache.Contracts;

namespace ErniAcademy.Cache.Samples;

public class SampleServiceThatUsesCache
{
    private readonly ICacheManager _cacheManager;

    public SampleServiceThatUsesCache(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    public async Task RunAsync() 
    {
        var item = new MyItem
        {
            MyCustomProperty = "hi"
        };

        //set an Item into cache
        await _cacheManager.SetAsync<MyItem>("ke1", item);

        //get an Item from cache
        var cachedItem = await _cacheManager.GetAsync<MyItem>("key1");
    }
}
