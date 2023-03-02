namespace CarRentalSystem.Domain.Tests;

using FakeItEasy;

using System.Collections.Generic;
using CarRentalSystem.Fakes.Domain.Models.Dealers;

/// <summary>
/// This Bootstrapper is used to load fakes from external assembly (CarRentalSystem.Domain.Fakes)
/// </summary>
public class ScanAnExternalAssemblyBootstrapper : DefaultBootstrapper
{
    public override IEnumerable<string> GetAssemblyFileNamesToScanForExtensions()
    {
        return new [] { typeof(DealerFakes)
            .Assembly
            .Location };
    }
}
