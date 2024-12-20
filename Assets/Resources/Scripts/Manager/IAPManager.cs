using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using System;
using UnityEngine.Purchasing.Security;

public class IAPManager : IStoreListener
{
	IStoreController _controller;
	IExtensionProvider _extensions;
	public bool IsNoAds { get; private set; }
	bool _init = false;

    // 제품 ID 정의
    public const string PURCHASE_1 = "purchase1";
    public const string PURCHASE_2 = "purchase2";
    public const string PURCHASE_3 = "purchase3";
    public const string PURCHASE_4 = "purchase4";
    public const string PURCHASE_5 = "purchase5";
    public const string PURCHASE_6 = "purchase6";
    const string NO_ADS = "ant_noads";

	Action<Product, PurchaseFailureReason> _onPurchased;

    // 제품 등록
    [Obsolete]
    public void Init()
	{
		if (_init)
			return;

		Debug.Log("Init IAPManager");


		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // 제품을 여기서 등록한다
		builder.AddProduct(id: PURCHASE_1, ProductType.Consumable, new IDs()
		{
			{ PURCHASE_1, AppleAppStore.Name },
			{ PURCHASE_1, GooglePlay.Name }
		});
		builder.AddProduct(id: PURCHASE_2, ProductType.Consumable, new IDs()
		{
			{ PURCHASE_2, AppleAppStore.Name },
			{ PURCHASE_2, GooglePlay.Name }
		});
		builder.AddProduct(id: PURCHASE_3, ProductType.Consumable, new IDs()
		{
			{ PURCHASE_3, AppleAppStore.Name },
			{ PURCHASE_3, GooglePlay.Name }
		});
		builder.AddProduct(id: PURCHASE_4, ProductType.Consumable, new IDs()
		{
			{ PURCHASE_4, AppleAppStore.Name },
			{ PURCHASE_4, GooglePlay.Name }
		});
		builder.AddProduct(id: PURCHASE_5, ProductType.Consumable, new IDs()
		{
			{ PURCHASE_5, AppleAppStore.Name },
			{ PURCHASE_5, GooglePlay.Name }
		});
		builder.AddProduct(id: PURCHASE_6, ProductType.Consumable, new IDs()
		{
			{ PURCHASE_6, AppleAppStore.Name },
			{ PURCHASE_6, GooglePlay.Name }
		});

    	builder.AddProduct(id: NO_ADS, ProductType.NonConsumable, new IDs() 
		{
			{ NO_ADS, AppleAppStore.Name },
			{ NO_ADS, GooglePlay.Name }
		});

        UnityPurchasing.Initialize(this, builder);

    }

    public void Purchase(string productID, Action<Product, PurchaseFailureReason> onPurchased)
	{
		if (_init == false)
			return;

		_onPurchased = onPurchased;

		try
		{
			Product product = _controller.products.WithID(productID);

			if (product != null && product.availableToPurchase)
			{
				Debug.Log($"IAPManager Purchase OK : {productID}");
				_controller.InitiatePurchase(product);
			}
			else
			{
				Debug.Log($"IAPManager Purchase FAIL : {productID}");
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex);
		}
	}	
    public bool HadPurchased(string productID)
	{
		if (_init == false)
			return false;

		var product = _controller.products.WithID(productID);

		if (product != null)
			return product.hasReceipt;

		return false;
	}


    #region IStoreListener 인터페이스 구현

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		Debug.Log("IAPManager OnInitialized");
		_controller = controller;
		_extensions = extensions;

		_init = true;

		IsNoAds = HadPurchased("ant_noads");
	}

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("IAPManager Initialization Failed: " + error + ", " );
    }


    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("IAPManager Initialization Failed: " + error + ", " + message);
    }

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
#if UNITY_ANDROID || UNITY_STANDALONE_OSX
//|| UNITY_IOS 
		var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), GooglePlayTangle.Data() , Application.identifier);
		try
		{
			// On Google Play, result has a single product ID.
			// On Apple stores, receipts contain multiple products.
			var result = validator.Validate(args.purchasedProduct.receipt);

			Debug.Log("IAPManager Valid Receipt");
			foreach (IPurchaseReceipt productReceipt in result)
			{
				Debug.Log(productReceipt.productID);
				Debug.Log(productReceipt.purchaseDate);
				Debug.Log(productReceipt.transactionID);

				GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
				if (null != google)
				{
					// This is Google's Order ID.
					// Note that it is null when testing in the sandbox
					// because Google's sandbox does not provide Order IDs.
					Debug.Log(google.orderID);
					Debug.Log(google.purchaseState);
					Debug.Log(google.purchaseToken);
					ProceedGoogle(args.purchasedProduct, google);
				}

				AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
				if (null != apple)
				{
					Debug.Log(apple.originalTransactionIdentifier);
					Debug.Log(apple.subscriptionExpirationDate);
					Debug.Log(apple.cancellationDate);
					Debug.Log(apple.quantity);
					ProceedApple(args.purchasedProduct, apple);
				}
			}

			return PurchaseProcessingResult.Complete;
		}
		catch (IAPSecurityException ex)
		{
			Debug.Log($"IAPManager Invalid Receipt {ex}");
		}
#endif

		return PurchaseProcessingResult.Pending;
	}

	public void ProceedGoogle(Product product, GooglePlayReceipt google)
	{
		Debug.Log($"IAPManager ProceedGoogle : {product.definition.id}");

		if (product.definition.id == NO_ADS)
			IsNoAds = true;

		_onPurchased?.Invoke(product, PurchaseFailureReason.Unknown);
	}

	public void ProceedApple(Product product, AppleInAppPurchaseReceipt apple)
	{
		Debug.Log($"IAPManager ProceedApple : {product.definition.id}");
		if (product.definition.id == NO_ADS)
			IsNoAds = true;

		_onPurchased?.Invoke(product, PurchaseFailureReason.Unknown);
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.LogWarning($"IAPManager OnPurchaseFailed : {product.definition.id}, {failureReason}");

		if (failureReason == PurchaseFailureReason.DuplicateTransaction)
		{
			if (product.definition.id == NO_ADS)
				IsNoAds = true;
		}

		_onPurchased?.Invoke(product, failureReason);
	}

	#endregion
}
