using System.Net.NetworkInformation;

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
        public const string CategoryMappingProductStage = CategoryEndpoint + "/productStages";
    }

    public static class Account
    {
        public const string AccountsEndpoint = ApiEndpoint + "/accounts";
        public const string AccountEndpoint = AccountsEndpoint + "/{id}";
    }

    public static class Dental
    {
        public const string DentalsEndPoint = ApiEndpoint + "/dentals";
        public const string DentalEndPoint = DentalsEndPoint + "/{id}";
    }

    public static class Product
    {
        public const string ProductsEndPoint = ApiEndpoint + "/products";
        public const string ProductEndPoint = ProductsEndPoint + "/{id}";
        public const string ProductsInCategory = ProductsEndPoint + "/category/{categoryId}";
    }

    public static class ProductStage
    {
        public const string ProductStagesEndPoint = ApiEndpoint + "/productStage";
        public const string ProductStageEndPoint = ProductStagesEndPoint + "/{id}";
        public const string ProductStageIndexEndPoint = ProductStagesEndPoint + "/indexStage/{indexStage}";
        public const string ProductStageByCategoryEndPoint = ProductStagesEndPoint + "/category/{categoryId}";
    }

    public static class TeethPosition
    {
        public const string TeethPositonsEndPoint = ApiEndpoint + "/teethPosition";
        public const string TeethPositonEndPoint = TeethPositonsEndPoint + "/{id}";
    }

    public static class Order
    {
        public const string OrdersEndPoint = ApiEndpoint + "/orders";
        public const string OrderEndPoint = OrdersEndPoint + "/{id}";
    }

    public static class OrderItem
    {
        public const string OrderItemsEndPoint = ApiEndpoint + "/orderItems";
        public const string OrderItemEndPoint = OrderItemsEndPoint + "/{id}";
    }

    public static class OrderItemStage
    {
        public const string OrderItemStagesEndPoint = ApiEndpoint + "/orderItemStages";
        public const string OrderItemStageEndPoint = OrderItemStagesEndPoint + "/{id}";
    }

    public static class WarrantyCard
    {
        public const string WarrantyCardsEndPoint = ApiEndpoint + "/warrantyCards";
        public const string WarrantyCardEndPoint = WarrantyCardsEndPoint + "/id";
    }

}