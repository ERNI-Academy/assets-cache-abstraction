# About Cache Abstraction
 Define a contract and many implementations (OnMemory, Azure Storage Blobs, Azure Redis) for caching

ERNI Academy StarterKit, PoC, or Gidelines. This is an about description of your repository.

<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
<!-- ALL-CONTRIBUTORS-BADGE:END -->

## Built With

This section should list any major frameworks that you built your project using. Leave any add-ons/plugins for the acknowledgements section. Here are a few examples.

- [.Net 6.0](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6)
- [c# 11](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11)
- [assets-serializers-abstraction (refereced as git submodule)](https://github.com/ERNI-Academy/assets-serializers-abstraction)

## Features

 Get<TItem>
 GetAsync<TItem>
 Set<TItem>
 SetAsync<TItem>
 Exists
 ExistsAsync
 Remove
 RemoveAsync

## Getting Started

This is an example of how you may give instructions on setting up your project locally. To get a local copy up and running follow these simple example steps.

## Prerequisites

.net 6
Visual Studio or Visual Studio Code

## Installation

Installation instructions Cache Abstraction by running:

1. Clone the repo

```sh
git clone --recurse-submodules https://github.com/ERNI-Academy/assets-cache-abstraction.git
```

> `Important Note`  
> All implementations heavly depends on Microsoft Options Pattern for configurations. See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0
> So it is expected a proper configuration in order to work take a look at the samples to see how to configure each
> All implementatins also depends on Microsoft logging. See https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line


2. Cache Basic use

```c#
class MyItem
{
	public string MyCustomProperty { get; set; }
}

var item = new MyItem { MyCustomProperty = "hi" };

//you can choose between many impl
ICacheManager cacheManager = new ErniAcademy.Cache.OnMemory.OnMemoryCacheManager();//args ommited for simplicity
ICacheManager cacheManager = new ErniAcademy.Cache.Redis.RedisCacheManager();//args ommited for simplicity
ICacheManager cacheManager = new ErniAcademy.Cache.StorageBlobs.StorageBlobsCacheManager();//args ommited for simplicity

//set an Item into cache
await cacheManager.SetAsync("key1", item);

//get an Item from cache
var cachedItem = await cacheManager.GetAsync<MyItem>("key1");
```

3. Cache Depency injection (ServiceCollection)

```c#
class MyItem
{
	public string MyCustomProperty { get; set; }
}

//when configuring your ServiceCollection use the extension methods defined in each library for easy of use. 
//This sample is provided with no arguments, take a look on the extensions to see the rest of the arguments, like IConfiguration, ISerializer etc.
services.AddCacheOnMemory();//args ommited for simplicity
services.AddCacheRedis();//args ommited for simplicity
services.AddCacheStorageBlobs();//args ommited for simplicity

//then just inject ICacheManager directly in your classes

class MyService
{
  private readonly ICacheManager _cacheManager;

  public MyService(ICacheManager cacheManager)
  {
    _cacheManager = cacheManager;
  }

  public async Task SomeMethod()
  {
      //... some logic
      
     var item = new MyItem { MyCustomProperty = "hi" };

     //set an Item into cache
     await _cacheManager.SetAsync("key1", item);

     //get an Item from cache
     var cachedItem = await _cacheManager.GetAsync<MyItem>("key1");
  }
}
```

## Contributing

Please see our [Contribution Guide](CONTRIBUTING.md) to learn how to contribute.

## License

![MIT](https://img.shields.io/badge/License-MIT-blue.svg)

(LICENSE) © 2022 [ERNI - Swiss Software Engineering](https://www.betterask.erni)

## Code of conduct

Please see our [Code of Conduct](CODE_OF_CONDUCT.md)

## Stats

![Alt](https://repobeats.axiom.co/api/embed/de239ffb5f028358f6bb691959b51f8af059b8dc.svg "Repobeats analytics image")

## Follow us

[![Twitter Follow](https://img.shields.io/twitter/follow/ERNI?style=social)](https://www.twitter.com/ERNI)
[![Twitch Status](https://img.shields.io/twitch/status/erni_academy?label=Twitch%20Erni%20Academy&style=social)](https://www.twitch.tv/erni_academy)
[![YouTube Channel Views](https://img.shields.io/youtube/channel/views/UCkdDcxjml85-Ydn7Dc577WQ?label=Youtube%20Erni%20Academy&style=social)](https://www.youtube.com/channel/UCkdDcxjml85-Ydn7Dc577WQ)
[![Linkedin](https://img.shields.io/badge/linkedin-31k-green?style=social&logo=Linkedin)](https://www.linkedin.com/company/erni)

## Contact

📧 [esp-services@betterask.erni](mailto:esp-services@betterask.erni)

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- ALL-CONTRIBUTORS-LIST:END -->
This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!