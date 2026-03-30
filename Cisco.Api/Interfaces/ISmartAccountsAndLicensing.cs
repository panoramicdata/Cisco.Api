using Cisco.Api.Data.SmartAccountsAndLicensing;
using Cisco.Api.Data.SmartAccountsAndLicensing.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

public interface ISmartAccountsAndLicensing
{
	/// https://apidocs-prod.cisco.com/explore;category=6083723a25042e9035f6a753 (requires login with Cisco ID granted access)

	////////////////////////////////////////
	//// Smart Accounts

	/// <summary>
	/// Search Smart Accounts based on logged user.
	/// </summary>
	/// <param name="name">Name of the smart account being searched, e.g: Demo Smart Account</param>
	/// <param name="domain">Domain of the smart account being searched, e.g: demo.mule.cisco.com</param>
	/// <param name="limit">Number of records to return; Represents the page size for pagination. Default limit will be 50.</param>
	/// <param name="offset">The start offset to fetch data from for pagination. To retrieve data for the first page with a limit of 50, the offset will be 0, for the second page the offset will be 50 and for the third page the offset will be 100 and so on.</param>
	/// <param name="type">Type of the smart account. CUSTOMER or HOLDING. This field is optional. If not provided both type of accounts are returned.</param>
	/// <param name="cancellationToken"></param>
	[Get("/v1/accounts/search")]
	Task<SearchSmartAccountsResponse> SearchSmartAccountsAsync(
		string? name,
		string? domain,
		int limit,
		int offset,
		SmartAccountType? type,
		CancellationToken cancellationToken);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<SearchSmartAccountsResponse> SearchSmartAccountsAsync(string? name, string? domain, int limit, int offset, SmartAccountType? type)
		=> SearchSmartAccountsAsync(name, domain, limit, offset, type, default);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<SearchSmartAccountsResponse> SearchSmartAccountsAsync()
		=> SearchSmartAccountsAsync(null, null, 50, 0, null, default);

	/// <summary>
	/// Provides the list of Smart Accounts to which the User has access to along with the available roles for each of the Smart Accounts.
	/// </summary>
	/// <param name="domain">Domain of the smart account being searched, e.g: demo.mule.cisco.com</param>
	/// <param name="cancellationToken"></param>
	[Get("/v2/accounts/")]
	Task<ListOfSmartAccountsResponse> ListSmartAccountsAsync(
		string? domain,
		CancellationToken cancellationToken);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<ListOfSmartAccountsResponse> ListSmartAccountsAsync(string? domain)
		=> ListSmartAccountsAsync(domain, default);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<ListOfSmartAccountsResponse> ListSmartAccountsAsync()
		=> ListSmartAccountsAsync(null, default);


	////////////////////////////////////////
	//// Licenses

	/// <summary>
	/// This API will return the License Subscriptions on the specified Smart Account Domain and the optional Virtual Accounts.
	/// Based on access you have on the Smart Account, the available Subscriptions will be fetched and returned if the optional Virtual Accounts
	/// are not specified. If the optional list of Virtual Accounts are passed as part of the request, then the Subscription Usage for the specified
	/// Virtual Accounts will be returned.
	/// </summary>
	/// <param name="smartAccountDomain">Domain of the smart account.</param>
	/// <param name="request">Request body containing status filters and optional virtual account filters</param>
	/// <param name="limit">Number of records to return; Represents the page size for pagination. If all the data is required without pagination the limit can be set to -1. Default limit will be 50</param>
	/// <param name="offset">The start offset to fetch data from for pagination. To retrieve data for the first page with a limit of 50, the offset will be 0, for the second page the offset will be 50 and for the third page the offset will be 100 and so on</param>
	/// <param name="cancellationToken"></param>
	[Post("/v1/accounts/{smartAccountDomain}/license-subscriptions")]
	Task<ListOfLicenseSubscriptionsResponse> LicenseSubscriptionsUsageAsync(
		string smartAccountDomain,
		[Body] LicenseSubscriptionsUsageRequest request,
		[Query] int limit,
		[Query] int offset,
		CancellationToken cancellationToken);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<ListOfLicenseSubscriptionsResponse> LicenseSubscriptionsUsageAsync(string smartAccountDomain, [Body] LicenseSubscriptionsUsageRequest request, [Query] int limit, [Query] int offset)
		=> LicenseSubscriptionsUsageAsync(smartAccountDomain, request, limit, offset, default);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<ListOfLicenseSubscriptionsResponse> LicenseSubscriptionsUsageAsync(string smartAccountDomain, [Body] LicenseSubscriptionsUsageRequest request)
		=> LicenseSubscriptionsUsageAsync(smartAccountDomain, request, 50, 0, default);

	/// <summary>
	/// This API will give the licenses usage in the specified Smart Account Domain and the optional Virtual Accounts.
	/// </summary>
	/// <param name="smartAccountDomain">Domain of the smart account.</param>
	/// <param name="request">Request body containing optional virtual account filters</param>
	/// <param name="limit">Number of records to return; Represents the page size for pagination. If all the data is required without pagination the limit can be set to -1. Default limit will be 50</param>
	/// <param name="offset">The start offset to fetch data from for pagination. To retrieve data for the first page with a limit of 50, the offset will be 0, for the second page the offset will be 50 and for the third page the offset will be 100 and so on</param>
	/// <param name="cancellationToken"></param>
	[Post("/v1/accounts/{smartAccountDomain}/licenses")]
	Task<ListOfLicensesResponse> SmartLicenseUsageAsync(
		string smartAccountDomain,
		[Body] SmartLicenseUsageRequest request,
		[Query] int limit,
		[Query] int offset,
		CancellationToken cancellationToken);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<ListOfLicensesResponse> SmartLicenseUsageAsync(string smartAccountDomain, [Body] SmartLicenseUsageRequest request, [Query] int limit, [Query] int offset)
		=> SmartLicenseUsageAsync(smartAccountDomain, request, limit, offset, default);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<ListOfLicensesResponse> SmartLicenseUsageAsync(string smartAccountDomain, [Body] SmartLicenseUsageRequest request)
		=> SmartLicenseUsageAsync(smartAccountDomain, request, 50, 0, default);
}
