using NUnit.Framework;
using EasyBike.Models;
using EasyBike.Models.Storage;
using GalaSoft.MvvmLight.Ioc;
using EasyBike.Config;
using System.Diagnostics;
using System;
using System.Linq;

namespace EasyBike.UITests
{
    [TestFixture]
    public class NUnitTests
    {
        [Test]
        public async void CheckAllContractsAvailability()
        {
            SimpleIoc.Default.Register<IConfigService, ConfigService>();
            var contractService = new ContractService(null, new StorageService());
            var failFlag = false;
            int failCounter = 0;
            int counter = 0;
            await contractService.GetCountries().SelectAsync(async country =>
            {
                await country.Contracts.SelectAsync(async contract =>
                {
                    counter++;
                    try
                    {
                        var stations = await contract.GetStationsAsync();
                        Assert.Greater(stations.Count, 0);
                        if (stations.Count == 0)
                        {
                            failCounter++;
                        }
                        if (stations.Count > 0)
                        {
                            Assert.AreNotEqual(stations.FirstOrDefault().Latitude, 0);
                            if (stations.FirstOrDefault().Latitude == 0)
                                failCounter++;
                        }
                    }
                    catch (Exception e)
                    {
                        failFlag = true;
                        failCounter++;
                        Debug.WriteLine("FAILED : " + contract.Name + " (" + contract.ServiceProvider + " / " + country + ") : " + e.Message);
                    }
                    return true;
                });
                return true;
            });

            Debug.WriteLine("FAILED " + failCounter + " / " + counter);
            Assert.IsFalse(failFlag);
        }

        [Test]
        public async void CheckContractAvailability()
        {
            var contractToTest = "Madrid";
            SimpleIoc.Default.Register<IConfigService, ConfigService>();
            var contractService = new ContractService(null, new StorageService());

            var contract = contractService.GetCountries().First(country => country.Contracts.Any(c => c.Name == contractToTest)).Contracts.First(c => c.Name == contractToTest);
            try
            {
                var stations = await contract.GetStationsAsync();
                Assert.Greater(stations.Count, 0);
                Debug.WriteLine("SUCCESS ! : " + stations.Count + " stations in " + contractToTest);
            }
            catch (Exception e)
            {
                Debug.WriteLine("FAILED : " + contractToTest + " : " + e.Message);
                Assert.Fail();
            }

        }
    }
}

//var tasks = test.GetCountries().Select(country => Task.Run(async () =>
//{
//    var tasks2 = country.Contracts.Select(contract => Task.Run(async () =>
//   {
//       var stations = await contract.GetStationsAsync();
//       Assert.Greater(stations.Count, 0);
//   }));
//    await Task.WhenAll(tasks2);

//}));
//await Task.WhenAll(tasks);