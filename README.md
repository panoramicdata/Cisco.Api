# Cisco.Api

[![Nuget](https://img.shields.io/nuget/v/Cisco.Api)](https://www.nuget.org/packages/Cisco.Api/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/0d1b38ded4f44b188f9f5d7eccf49324)](https://app.codacy.com/gh/panoramicdata/Cisco.Api/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

A .NET library for accessing Cisco Support APIs, including EoX, Product Information, PSIRT, PSS, Serial Number to Information, PX Cloud, and Umbrella APIs.

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
				.GetBySerialNumbersAsync(new[] { "<SERIAL_NUMBER>" })
				.ConfigureAwait(false);

			var productDetails = productInformation.Products.First();

			Console.WriteLine($"Product Name: {productDetails.ProductName}");
		}
	}
}
```

## Supported Frameworks

- .NET 9.0
- .NET 10.0

## API Documentation

The Cisco Support APIs documentation can be found here:

- [Cisco Support APIs](https://developer.cisco.com/site/support-apis/)

## License

This project is licensed under the MIT License.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
