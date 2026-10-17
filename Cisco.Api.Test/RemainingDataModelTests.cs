using Cisco.Api.Data.EnterpriseAgreement.Responses;
using Cisco.Api.Data.SmartAccountsAndLicensing;
using Cisco.Api.Data.Umbrella;
using Cisco.Api.Data.Hello;
using Cisco.Api.Exceptions;
using Cisco.Api.Security;
using System.Net;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip tests for the remaining small data models and the public exception and
/// authentication-response types, none of which needs a live service to exercise.
/// </summary>
public class RemainingDataModelTests
{
	/// <summary>CiscoApiException's constructors carry their arguments through to the public surface.</summary>
	[Fact]
	public void CiscoApiException_AllConstructorsWork()
	{
		new CiscoApiException().Message.Should().NotBeNull();

		using var successResponse = new HttpResponseMessage(HttpStatusCode.OK);
		new CiscoApiException(successResponse).Message.Should().Contain("API issue occurred");

		using var failureResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
		new CiscoApiException(failureResponse).Message.Should().Contain("non successful status code");

		new CiscoApiException(failureResponse, "my message").Message.Should().Be("my message");

		var inner = new InvalidOperationException("inner");
		var withInner = new CiscoApiException(failureResponse, inner);
		withInner.Message.Should().Contain("non successful status code");
		withInner.InnerException.Should().BeSameAs(inner);

		new CiscoApiException("just a message").Message.Should().Be("just a message");

		var chained = new CiscoApiException("outer", inner);
		chained.Message.Should().Be("outer");
		chained.InnerException.Should().BeSameAs(inner);
	}

	/// <summary>The other three public exception types accept their arguments.</summary>
	[Fact]
	public void OtherExceptions_AllConstructorsWork()
	{
		new PssConfigException("a").Message.Should().Be("a");
		new PssConfigException("b", new InvalidOperationException("e")).InnerException.Should().NotBeNull();

		new PxCloudReportException("c").Message.Should().Be("c");
		new PxCloudReportException("d", new InvalidOperationException("e")).InnerException.Should().NotBeNull();

		new ConfigurationException("f").Message.Should().Be("f");
	}

	/// <summary>AccessTokenResponse round-trips its properties.</summary>
	[Fact]
	public void AccessTokenResponse_EveryPropertyRoundTrips()
	{
		var response = new AccessTokenResponse
		{
			AccessToken = "tok",
			TokenType = "bearer",
			ExpiresInSeconds = 3600,
			Error = "err",
			ErrorDescription = "ed"
		};
		response.AccessToken.Should().Be("tok");
		response.TokenType.Should().Be("bearer");
		response.ExpiresInSeconds.Should().Be(3600);
		response.Error.Should().Be("err");
		response.ErrorDescription.Should().Be("ed");
	}

	/// <summary>Subscription round-trips its properties.</summary>
	[Fact]
	public void Subscription_EveryPropertyRoundTrips()
	{
		var subscription = new Subscription
		{
			SubscriptionID = "sid",
			Status = "active",
			StartDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
			EndDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
			Duration = 12,
			RemainingDuration = 6,
			DurationInMonths = 12,
			RemainingDurationInMonths = 6,
			NextTrueForward = new DateTimeOffset(2024, 7, 1, 0, 0, 0, TimeSpan.Zero),
			ArchitectureName = "x86"
		};
		subscription.SubscriptionID.Should().Be("sid");
		subscription.Status.Should().Be("active");
		subscription.Duration.Should().Be(12);
		subscription.RemainingDuration.Should().Be(6);
		subscription.DurationInMonths.Should().Be(12);
		subscription.RemainingDurationInMonths.Should().Be(6);
		subscription.ArchitectureName.Should().Be("x86");
	}

	/// <summary>SubscriptionAccount round-trips its properties.</summary>
	[Fact]
	public void SubscriptionAccount_EveryPropertyRoundTrips()
	{
		var account = new SubscriptionAccount
		{
			SmartAccountId = 42,
			SmartAccountName = "an"
		};
		account.SmartAccountId.Should().Be(42);
		account.SmartAccountName.Should().Be("an");
	}

	/// <summary>Suite round-trips its properties.</summary>
	[Fact]
	public void Suite_EveryPropertyRoundTrips()
	{
		var suite = new Suite
		{
			CustSuiteId = 1,
			SuiteName = "n",
			CustSuiteName = "cn",
			PurchasedEntitlements = 1,
			PremierEntitlements = 2,
			GrowthAllowance = 3,
			TotalEntitlements = 4,
			PreEAConsumption = 5,
			LicenseGenerated = 6,
			LicenseMigrated = 7,
			C1ToDNAMigratedCount = 8,
			TotalConsumption = 9,
			RemainingEntitlements = 10,
			SoftwareDownloads = 11,
			HealthMessage = "ok",
			CalculationMethod = "m",
			CommitmentType = "c"
		};
		suite.CustSuiteId.Should().Be(1);
		suite.SuiteName.Should().Be("n");
		suite.CustSuiteName.Should().Be("cn");
		suite.PurchasedEntitlements.Should().Be(1);
		suite.PremierEntitlements.Should().Be(2);
		suite.GrowthAllowance.Should().Be(3);
		suite.TotalEntitlements.Should().Be(4);
		suite.PreEAConsumption.Should().Be(5);
		suite.LicenseGenerated.Should().Be(6);
		suite.LicenseMigrated.Should().Be(7);
		suite.C1ToDNAMigratedCount.Should().Be(8);
		suite.TotalConsumption.Should().Be(9);
		suite.RemainingEntitlements.Should().Be(10);
		suite.SoftwareDownloads.Should().Be(11);
		suite.HealthMessage.Should().Be("ok");
		suite.CalculationMethod.Should().Be("m");
		suite.CommitmentType.Should().Be("c");
	}

	/// <summary>VirtualAccount round-trips its properties.</summary>
	[Fact]
	public void VirtualAccount_EveryPropertyRoundTrips()
	{
		var virtualAccount = new VirtualAccount
		{
			VirtualAccountId = 7,
			VirtualAccountName = "van"
		};
		virtualAccount.VirtualAccountId.Should().Be(7);
		virtualAccount.VirtualAccountName.Should().Be("van");
	}

	/// <summary>SmartAccount round-trips its required properties.</summary>
	[Fact]
	public void SmartAccount_EveryPropertyRoundTrips()
	{
		var smartAccount = new SmartAccount
		{
			Status = "active",
			Domain = "dom",
			Name = "n",
			Type = SmartAccountType.Customer
		};
		smartAccount.Status.Should().Be("active");
		smartAccount.Domain.Should().Be("dom");
		smartAccount.Name.Should().Be("n");
		smartAccount.Type.Should().Be(SmartAccountType.Customer);
	}

	/// <summary>SmartAccountWithIds adds an Id on top of SmartAccount.</summary>
	[Fact]
	public void SmartAccountWithIds_EveryPropertyRoundTrips()
	{
		var account = new SmartAccountWithIds
		{
			Status = "active",
			Domain = "dom",
			Name = "n",
			Type = SmartAccountType.Customer,
			Id = 42
		};
		account.Id.Should().Be(42);
		account.Domain.Should().Be("dom");
		account.Type.Should().Be(SmartAccountType.Customer);
	}

	/// <summary>SmartAccountRole round-trips its properties.</summary>
	[Fact]
	public void SmartAccountRole_EveryPropertyRoundTrips()
	{
		var role = new SmartAccountRole
		{
			Type = SmartAccountRoleType.AccountAdministrator,
			VirtualAccount = "va"
		};
		role.Type.Should().Be(SmartAccountRoleType.AccountAdministrator);
		role.VirtualAccount.Should().Be("va");
	}

	/// <summary>Site round-trips its required properties.</summary>
	[Fact]
	public void UmbrellaSite_EveryPropertyRoundTrips()
	{
		var site = new Site
		{
			OriginId = 1,
			Name = "n",
			SiteId = 2,
			IsDefault = true,
			ModifiedAt = new DateTime(2024, 1, 1),
			CreatedAt = new DateTime(2023, 1, 1)
		};
		site.OriginId.Should().Be(1);
		site.Name.Should().Be("n");
		site.SiteId.Should().Be(2);
		site.IsDefault.Should().BeTrue();
		site.ModifiedAt.Should().Be(new DateTime(2024, 1, 1));
		site.CreatedAt.Should().Be(new DateTime(2023, 1, 1));
	}

	/// <summary>Policy round-trips its required properties.</summary>
	[Fact]
	public void UmbrellaPolicy_EveryPropertyRoundTrips()
	{
		var policy = new Policy
		{
			PolicyId = 1,
			OrganizationId = 2,
			Name = "n",
			Priority = 3,
			CreatedAt = new DateTime(2023, 1, 1),
			IsDefault = false
		};
		policy.PolicyId.Should().Be(1);
		policy.OrganizationId.Should().Be(2);
		policy.Name.Should().Be("n");
		policy.Priority.Should().Be(3);
		policy.CreatedAt.Should().Be(new DateTime(2023, 1, 1));
		policy.IsDefault.Should().BeFalse();
	}

	/// <summary>InternalNetwork round-trips its properties.</summary>
	[Fact]
	public void UmbrellaInternalNetwork_EveryPropertyRoundTrips()
	{
		var network = new InternalNetwork
		{
			OriginId = 1,
			Name = "n",
			IpAddress = "10.0.0.1",
			PrefixLength = 24
		};
		network.OriginId.Should().Be(1);
		network.Name.Should().Be("n");
		network.IpAddress.Should().Be("10.0.0.1");
		network.PrefixLength.Should().Be(24);
	}

	/// <summary>Response (Hello) round-trips its properties.</summary>
	[Fact]
	public void HelloResponse_EveryPropertyRoundTrips()
	{
		var response = new Response
		{
			HelloResponse = new HelloResponse { Response = "Hello" }
		};
		response.HelloResponse.Response.Should().Be("Hello");
	}
}
