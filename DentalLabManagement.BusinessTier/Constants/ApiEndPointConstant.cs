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


    public static class Brand
    {
        public const string BrandsEndpoint = ApiEndpoint + "/brands";
        public const string BrandEndpoint = BrandsEndpoint + "/{id}";
        public const string BrandAccountEndpoint = BrandEndpoint + "/users";
        public const string StoresInBrandEndpoint = BrandEndpoint + "/stores";
    }

    public static class Store
    {
        public const string StoresEndpoint = ApiEndpoint + "/stores";
        public const string StoreEndpoint = StoresEndpoint + "/{id}";
        public const string StoreUpdateEmployeeEndpoint = StoresEndpoint + "/{storeId}/users/{id}";
        public const string StoreAccountEndpoint = StoresEndpoint + "/{storeId}/users";
        public const string MenuProductsForStaffEndPoint = StoresEndpoint + "/menus";
        public const string StoreOrdersEndpoint = StoreEndpoint + "/orders";
        public const string StoreSessionsEndpoint = StoreEndpoint + "/sessions";
        public const string StoreSessionEndpoint = StoresEndpoint + "/{storeId}/sessions/{id}";
        public const string StoreEndDayReportEndpoint = StoreEndpoint + "/day-report";
        public const string GetPromotion = StoreEndpoint + "/promotion";
    }

    public static class Account
    {
        public const string AccountsEndpoint = ApiEndpoint + "/accounts";
        public const string AccountEndpoint = AccountsEndpoint + "/{id}";
    }


    public static class Product
    {
        public const string ProductsEndPoint = ApiEndpoint + "/products";
        public const string ProductEndPoint = ProductsEndPoint + "/{id}";
        public const string ProductsInCategory = ProductsEndPoint + "/category/{categoryId}";
    }

    public static class ProductStage
    {
        public const string ProductStageEndPoint = ApiEndpoint + "/productStage";
    }


    public static class Order
    {
        public const string OrdersEndPoint = Store.StoresEndpoint + "/{storeId}/orders";
        public const string OrderEndPoint = OrdersEndPoint + "/{id}";
    }


}