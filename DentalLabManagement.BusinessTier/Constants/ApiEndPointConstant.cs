﻿using System.Net.NetworkInformation;

namespace DentalLabManagement.BusinessTier.Constants;

public static class ApiEndPointConstant
{

    public const string RootEndPoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndPoint + ApiVersion;

    public static class Authentication
    {
        public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
        public const string Login = AuthenticationEndpoint + "/login";
    }

    public static class Category
    {
        public const string CategoriesEndpoint = ApiEndpoint + "/categories";
        public const string CategoryEndpoint = CategoriesEndpoint + "/{id}";       
    }

    public static class Account
    {
        public const string AccountsEndPoint = ApiEndpoint + "/accounts";
        public const string AccountEndPoint = AccountsEndPoint + "/{id}";
        public const string StaffEndPoint = AccountEndPoint + "/staff";
        public const string DentalAcccountEndPoint = AccountEndPoint + "/dental";
    }

    public static class Partner
    {
        public const string DentalsEndPoint = ApiEndpoint + "/partners";
        public const string DentalEndPoint = DentalsEndPoint + "/{id}";
        public const string OrdersEndPoint = DentalEndPoint + "/orders";
    }

    public static class Product
    {
        public const string ProductsEndPoint = ApiEndpoint + "/products";
        public const string ProductEndPoint = ProductsEndPoint + "/{id}";
        public const string ProductStageMapping = ProductEndPoint + "/productionStages";
        public const string ExtraProductEndPoint = ProductEndPoint + "/extra-products";
    }

    public static class ProductStage
    {
        public const string ProductStagesEndPoint = ApiEndpoint + "/productionStages";
        public const string ProductStageEndPoint = ProductStagesEndPoint + "/{id}";
        public const string ProductStageByCategoryEndPoint = ProductStagesEndPoint + "/category/{categoryId}";
    }

    public static class TeethPosition
    {
        public const string TeethPositonsEndPoint = ApiEndpoint + "/teethPositions";
        public const string TeethPositonEndPoint = TeethPositonsEndPoint + "/{id}";
    }

    public static class Order
    {
        public const string OrdersEndPoint = ApiEndpoint + "/orders";
        public const string OrderEndPoint = OrdersEndPoint + "/{id}";
        public const string OrderPaymentsEndPoint = OrderEndPoint + "/payment";
        public const string WarrantyRequestsEndPoint = OrderEndPoint + "/warrantyRequest";
        public const string OrderHistoryEndPoint = OrderEndPoint + "/orderHistory";
    }

    public static class OrderItem
    {
        public const string OrderItemsEndPoint = ApiEndpoint + "/orderItems";
        public const string OrderItemEndPoint = OrderItemsEndPoint + "/{id}";
        public const string OrderItemCardEndPoint = OrderItemEndPoint + "/warrantyCard";
        public const string OrderItemWarrantyEndPoint = OrderItemEndPoint + "/warrantyStatus";
    }

    public static class OrderItemStage
    {
        public const string OrderItemStagesEndPoint = ApiEndpoint + "/orderItemStages";
        public const string OrderItemStageEndPoint = OrderItemStagesEndPoint + "/{id}";
        public const string TransferStageEndPoint = OrderItemStageEndPoint + "/transfer";
    }

    public static class CardType
    {
        public const string CardTypesEndPoint = ApiEndpoint + "/cardTypes";
        public const string CardTypeEndPoint = CardTypesEndPoint + "/{id}";
    }

    public static class WarrantyCard
    {
        public const string WarrantyCardsEndPoint = ApiEndpoint + "/warrantyCards";
        public const string WarrantyCardEndPoint = WarrantyCardsEndPoint + "/{id}";
        public const string WarrantyCardEndPointv2 = ApiEndpoint + "/warrantyCard";
    }

    public static class MaterialStock
    {
        public const string MaterialStocksEndPoint = ApiEndpoint + "/materialStocks";
        public const string MaterialStockEndPoint = MaterialStocksEndPoint + "/{id}";
    }
}