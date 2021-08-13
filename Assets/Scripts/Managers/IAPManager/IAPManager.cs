using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider; 
    
    public string gold1000 = "gold1000";
    public string gold5000 = "gold5000";
    public string gold10000 = "gold10000";
    public string gold100000 = "gold100000";

    public string emerald100 = "emerald100";
    public string emerald500 = "emerald500";
    public string emerald1000 = "emerald1000";
    public string emerald5000 = "emerald5000";

    public string pack1 = "pack1";

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(gold1000, ProductType.Consumable);
        builder.AddProduct(gold5000, ProductType.Consumable);
        builder.AddProduct(gold10000, ProductType.Consumable);
        builder.AddProduct(gold100000, ProductType.Consumable);

        builder.AddProduct(emerald100, ProductType.Consumable);
        builder.AddProduct(emerald500, ProductType.Consumable);
        builder.AddProduct(emerald1000, ProductType.Consumable);
        builder.AddProduct(emerald5000, ProductType.Consumable);
        
        builder.AddProduct(pack1, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyGold1000()
    {
        BuyProductID(gold1000);
    }
    public void BuyGold5000()
    {
        BuyProductID(gold5000);
    }
    public void BuyGold10000()
    {
        BuyProductID(gold10000);
    }
    public void BuyGold100000()
    {
        BuyProductID(gold100000);
    }


    public void BuyEmerald100()
    {
        BuyProductID(emerald100);
    }
    public void BuyEmerald500()
    {
        BuyProductID(emerald500);
    }
    public void BuyEmerald1000()
    {
        BuyProductID(emerald1000);
    }
    public void BuyEmerald5000()
    {
        BuyProductID(emerald5000);
    }

    public void BuyPack1()
    {
        BuyProductID(pack1);
    }


    public string GetProductPrices(string ID) 
    {
        if (m_StoreController!= null && m_StoreController.products!=null)
            return m_StoreController.products.WithID(ID).metadata.localizedPriceString;
        else
            return "";
    }


    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }



    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, gold1000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveCoin(1000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, gold5000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveCoin(5000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, gold10000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveCoin(10000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, gold100000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveCoin(100000);
        }

        else if (String.Equals(args.purchasedProduct.definition.id, emerald100, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveEmerald(100);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, emerald500, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveEmerald(500);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, emerald1000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveEmerald(1000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, emerald5000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveEmerald(5000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, pack1, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GameManager.Instance.GiveEmerald(100);
            GameManager.Instance.GiveCoin(1000);
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }
 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
