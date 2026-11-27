# Cisco.Api

[![Nuget](https://img.shields.io/nuget/v/Cisco.Api)](https://www.nuget.org/packages/Cisco.Api/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/0d1b38ded4f44b188f9f5d7eccf49324)](https://app.codacy.com/gh/panoramicdata/Cisco.Api/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

> **Need help with APIs or .NET?** Panoramic Data are experts in APIs and .NET, and we can help with your development. [Contact us](https://www.panoramicdata.com/contact-us) or see our [software services](https://www.panoramicdata.com/services/software).

A .NET library for accessing Cisco Support APIs, including EoX, Product Information, PSIRT, PSS, Serial Number to Information, PX Cloud, and Umbrella APIs.

Supports .NET 11.0.

## Prerequisites

You must have an **SNTC (Smart Net Total Care)** or **PSS (Product Support Services)** account in order to use this library.

## Installation

### Using .NET CLI

```bash
dotnet add package Cisco.Api
```

### Using Package Manager Console

```powershell
Install-Package Cisco.Api
```

### Using Visual Studio

1. Open your project in Visual Studio
2. Right-click on the project and click "Manage NuGet packages"
3. Search for "Cisco.Api"
4. Click "Install" to add the latest version to your project

### Using PackageReference

Add the following to your `.csproj` file:

```xml
<PackageReference Include="Cisco.Api" Version="*" />
```

## Setup

Before using the library, you need to obtain API credentials:

1. Go to the [Cisco API Console](https://apiconsole.cisco.com/)
2. Sign in with your SNTC or PSS account
3. Click "Register a New App"
4. Add a new Application, selecting the API endpoints that you intend to use
5. Note the **Client ID** and **Client Secret** - you'll need these for authentication

## Example Code

```csharp
using Cisco.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace My.Project
{
	public static class Program
	{
		public static async Task Main()
		{
			var ciscoClient = new CiscoClient(
				new CiscoClientOptions
				{
					ClientId = "<YOUR_CLIENT_ID>",
					ClientSecret = "<YOUR_CLIENT_SECRET>"
				});

			var productInformation = await ciscoClient
				.ProductInfo
				.GetBySerialNumbersAsync(new[] { "<SERIAL_NUMBER>" }, default)
				.ConfigureAwait(false);

			var productDetails = productInformation.Products.First();

			Console.WriteLine($"Product Name: {productDetails.ProductName}");
		}
	}
}
```

## Breaking changes

- `CommerceSku.Create(...)`, a public static factory taking 21 positional parameters, has been removed. It only assigned each argument to the property of the same name, so use an object initializer instead: `new CommerceSku { Eol = true, CustSuiteId = 1, SuiteCommerceSku = "sku", ... }`. All properties are unchanged.

## Migrating from 3.0.104

Releases after 3.0.104 removed the optional parameters from the interface methods and turned the old overloads into compile errors (`[Obsolete(..., true)]`). Code that compiled against 3.0.104 by omitting `pageIndex`, `alias` or the `CancellationToken` no longer compiles. Pass every argument explicitly, using `default` for the `CancellationToken` if you have no token to hand.

### `pageIndex` is now required

`pageIndex` (previously defaulting to `1`) must be passed on these methods, followed by a `CancellationToken`:

- `IEox.GetBySerialNumberAsync(string serialNumber, int pageIndex, CancellationToken cancellationToken)`
- `IEox.GetBySoftwareReleaseStringAsync(IEnumerable<string> softwareReleaseStrings, int pageIndex, CancellationToken cancellationToken)`
- `ISoftwareSuggestion.GetByProductIdsAsync(IEnumerable<string> productIds, int pageIndex, CancellationToken cancellationToken)`

```csharp
// Before (3.0.104)
var eox = await client.Eox.GetBySerialNumberAsync(serialNumber);
var suggestions = await client.SoftwareSuggestion.GetByProductIdsAsync(productIds);

// After
var eox = await client.Eox.GetBySerialNumberAsync(serialNumber, 1, cancellationToken);
var suggestions = await client.SoftwareSuggestion.GetByProductIdsAsync(productIds, 1, cancellationToken);
```

### `alias` is now required

`ISecurityAdvisory.GetAdvisoriesByOsTypeAndVersion` previously defaulted `alias` to an empty string:

```csharp
// Before (3.0.104)
var advisories = await client.SecurityAdvisory.GetAdvisoriesByOsTypeAndVersion("ios", "15.2(4)M1");

// After
var advisories = await client.SecurityAdvisory.GetAdvisoriesByOsTypeAndVersion("ios", "15.2(4)M1", "", cancellationToken);
```

### `CancellationToken` is now required

Every overload without a `CancellationToken` is now a compile error. Add the token as the last argument, or `default`. This includes:

- `ISerialNumberToInfo.GetCoverageStatusBySerialNumbersAsync`
- `ISerialNumberToInfo.GetCoverageSummaryBySerialNumbersAsync`
- `IProductInfo.GetBySerialNumbersAsync`
- `ISecurityAdvisory.GetOsVersionDataByType`
- `ISecurityAdvisory.GetAdvisoriesByCveName`
- `ISecurityAdvisory.GetAdvisoriesByProduct`

```csharp
// Before (3.0.104)
var products = await client.ProductInfo.GetBySerialNumbersAsync(serialNumbers);
var cve = await client.SecurityAdvisory.GetAdvisoriesByCveName("CVE-2024-20353");

// After
var products = await client.ProductInfo.GetBySerialNumbersAsync(serialNumbers, cancellationToken);
var cve = await client.SecurityAdvisory.GetAdvisoriesByCveName("CVE-2024-20353", cancellationToken);
```
## Supported Frameworks

- .NET 11.0
- .NET 11.0

## API Documentation

The Cisco Support APIs documentation can be found here:

- [Cisco Support APIs](https://developer.cisco.com/site/support-apis/)

## License

This project is licensed under the MIT License.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
