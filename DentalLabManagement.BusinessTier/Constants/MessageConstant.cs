using System.Data;

namespace DentalLabManagement.BusinessTier.Constants;

public static class MessageConstant
{
    public static class LoginMessage
    {
        public const string InvalidUsernameOrPassword = "Tên đăng nhập hoặc mật khẩu không chính xác";
        public const string DeactivatedAccount = "Tài khoản đang bị vô hiệu hoá";
    }

    public static class Account
    {
        public const string AccountExisted = "Tài khoản đã tồn tại";
        public const string CreateAccountFailed = "Tạo tài khoản thất bại";
        public const string CreateAccountWithWrongRoleMessage = "Please create with acceptent role";
        public const string CreateDentalAccountFailMessage = "Tạo tài khoản mới cho nhãn hiệu thất bại";
        public const string CreateStaffAccountFailMessage = "Tạo tài khoản nhân viên thất bại";
        public const string UserUnauthorizedMessage = "Bạn không được phép cập nhật status cho tài khoản này";

        public const string UpdateAccountStatusRequestWrongFormatMessage =
            "Cập nhật status tài khoản request sai format";

        public const string AccountNotFoundMessage = "Không tìm thấy tài khoản";
        public const string UpdateAccountStatusSuccessfulMessage = "Cập nhật status tài khoản thành công";
        public const string UpdateAccountStatusFailedMessage = "Cập nhật status tài khoản thất bại";
        public const string EmptyAccountId = "Account id bị trống";
    }

    public static class Category
    {
        public const string CategoryNameExisted = "Category Name đã tồn tại";
        public const string CreateNewCategoryFailedMessage = "Tạo mới Category bị failed";
        public const string EmptyCategoryIdMessage = "Category Id không hợp lệ";
        public const string CategoryNotFoundMessage = "Category không có trong hệ thống";
        public const string UpdateCategorySuccessfulMessage = "Category được cập nhật thành công";
        public const string UpdateCategoryFailedMessage = "Category cập nhật thất bại";
    }

    public static class Product
    {
        public const string ProductNameExisted = "Product đã tồn tại";
        public const string CreateNewProductFailedMessage = "Tạo mới product thất bại";
        public const string UpdateProductFailedMessage = "Cập nhật thông tin product thất bại";
        public const string EmptyProductIdMessage = "Product Id không hợp lệ";
        public const string ProductNotFoundMessage = "Product không tồn tại trong hệ thống";
    }

    public static class ProductStage
    {
        public const string ProductStageExisted = "Product Stage đã tồn tại";
        public const string CreateNewProductStageFailed = "Tạo mới product stage thất bại";
    }

    public static class Order
    {
        public const string UserNotInSessionMessage = "Tài khoản không trong ca làm để tạo Order";
        public const string NoProductsInOrderMessage = "Không thể tạo order khi order không đính kèm sản phẩm bên trong";
        public const string CreateOrderFailedMessage = "Tạo mới order thất bại";
        public const string EmptyOrderIdMessage = "Id của order không hợp lệ";
        public const string OrderNotFoundMessage = "Order không tồn tại trong hệ thống";
    }

    public static class Session
    {
        public const string EmptySessionIdMessage = "Id của session không hợp lệ";
        public const string SessionNotFoundMessage = "Session không tồn tại trong store";
        public const string CreateNewSessionInvalidStartDate = "Session startDate bị trùng với session: ";
        public const string CreateNewSessionInvalidEndDate = "Session endDate bị trùng với session: ";
        public const string AlreadySessionAvailableInTimeGap = "Thời gian của session bạn đang tạo hoặc cập nhật hiện đã có một vài session vui lòng thay đổi thời gian phù hợp";
    }

    
}