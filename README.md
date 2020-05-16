# Cisco.Api

You must have an SNTC or PSS account in order to be able to use this library.

To use the Cisco nuget package:

## Visual Studio

1. Open your project in Visual Studio
1. Right-click on the project and click "Manage Nuget packages"
1. Find the package "Cisco.Api" - install the latest version
1. Add your application in the Cisco API Console
1. Add a new Application, selecting the end points that you intend to use
1. Note the client id and secret.

## Example code (C# 8.0):

``` C#
using Cisco.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Project
{
	public static class Program
	{
		public static async Task Main()
		{
			var dnaCenterClient = new CiscoClient("<ID>", "<SECRET>");

			var productInformation = await CiscoClient
				.GetProductInformationBySerialNumber("<SERIAL NUMBER>")
				.ConfigureAwait(false);

			Console.WriteLine($"Product Name: {productInformation.ProductName}");
		}
	}
}
````

## API Documentation

The Cisco Support APIs documentation can be found here:

- [Cisco Support APIs](https://developer.cisco.com/site/support-apis/)
