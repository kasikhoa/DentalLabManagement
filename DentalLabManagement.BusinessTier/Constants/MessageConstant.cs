using System.Data;
using System.Net.NetworkInformation;

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
        public const string CreateStaffAccountFailMessage = "Tạo tài khoản nhân viên thất bại";
        public const string UserUnauthorizedMessage = "Bạn không được phép cập nhật status cho tài khoản này";

        public const string UpdateAccountStatusRequestWrongFormatMessage = "Cập nhật status tài khoản request sai format";

        public const string AccountNotFoundMessage = "Không tìm thấy tài khoản";
        public const string UpdateAccountSuccessfulMessage = "Cập nhật status tài khoản thành công";
        public const string UpdateAccountFailedMessage = "Cập nhật thông tin tài khoản thất bại";
        public const string UpdateAccountStatusFailedMessage = "Vô hiệu hóa tài khoản thất bại";
        public const string UpdateAccountStatusSuccessfulMessage = "Vô hiệu hóa tài khoản thành công";
        public const string EmptyAccountIdMessage = "Account Id không hợp lệ";

        public const string StaffNotFoundMessage = "Không tìm thấy nhân viên";
    }

    public static class Dental
    {
        public const string CreateDentalFailed = "Dental tạo mới thất bại";
        public const string EmptyDentalId = "Id không hợp lệ";
        public const string DentalNotFoundMessage = "Dental không có trong hệ thống";
        public const string AccountDentalNotFoundMessage = "Dental chưa có account";
        public const string UpdateDentalFailedMessage = "Cập nhật thông tin Dental thất bại";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
    }

    public static class Category
    {
        public const string CategoryNameExisted = "Category Name đã tồn tại";
        public const string CreateNewCategoryFailedMessage = "Tạo mới Category thất bại";
        public const string EmptyCategoryIdMessage = "Category Id không hợp lệ";
        public const string CategoryNotFoundMessage = "Category không có trong hệ thống";
        public const string UpdateCategorySuccessfulMessage = "Category được cập nhật thành công";
        public const string UpdateCategoryFailedMessage = "Category cập nhật thất bại";
        public const string StageForCategorySuccessfulMessage = "Stage cho Category được cập nhật thành công";
        public const string StageForCategoryFailedMessage = "Stage cho Category cập nhật thất bại";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
    }

    public static class Product
    {
        public const string ProductNameExisted = "Product đã tồn tại";
        public const string CreateNewProductFailedMessage = "Tạo mới product thất bại";
        public const string UpdateProductFailedMessage = "Cập nhật thông tin product thất bại";
        public const string EmptyProductIdMessage = "Product Id không hợp lệ";
        public const string ProductNotFoundMessage = "Product không tồn tại trong hệ thống";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
    }

    public static class ProductStage
    {
        public const string EmptyProductStageIdMessage = "Id không hợp lệ";
        public const string EmptyProductStageMessage = "Index Stage không hợp lệ";
        public const string ProductStageExisted = "Product Stage đã tồn tại";
        public const string CreateNewProductStageFailed = "Tạo mới product stage thất bại";
        public const string IndexStageNotFoundMessage = "Index Stage không tồn tại trong hệ thống";
        public const string IdNotFoundMessage = "Id không tồn tại trong hệ thống";
        public const string UpdateProductStageFailedMessage = "Cập nhật thông tin product stage thất bại";
    }

    public static class TeethPosition
    {
        public const string TeethPositionExisted = "Teeth Position đã tồn tại";
        public const string CreateTeethPositionFailed = "Tạo mới teeth position thất bại";
        public const string EmptyTeethPositionIdMessage = "Id không hợp lệ";
        public const string IdNotFoundMessage = "Teeth Position không tồn tại trong hệ thống";
        public const string UpdateTeethPositionFailedMessage = "Cập nhật thông tin teeth position thất bại";
        public const string ToothArchError = "Tooth Arch phải từ 1 đến 4";

    }

    public static class Order
    {
        public const string CreateOrderFailedMessage = "Tạo mới order thất bại";
        public const string EmptyOrderIdMessage = "Id của order không hợp lệ";
        public const string InvoiceIdExistedMessage = "InvoiceId đã tồn tại";
        public const string OrderNotFoundMessage = "Order không tồn tại trong hệ thống";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái Order thất bại";
        public const string NewStatusMessage = "Order đã được tạo";
        public const string StatusErrorMessage = "Thay đổi trạng thái Order thất bại";
        public const string ProducingStatusMessage = "Order đang được sản xuất";
        public const string ProducingStatusRepeatMessage = "Order đã được đưa vào sản xuất";
        public const string CompletedStatusMessage = "Order đã hoàn thành";
        public const string PaidStatusMessage = "Order đã thanh toán";
        public const string CompletedStatusRepeatMessage = "Không thể thay đổi trạng thái Order đã hoàn thành";
        public const string CanceledStatusMessage = "Order đã bị hủy";
        public const string CanceledStatusRepeatMessage = "Không thể thay đổi trạng thái Order đã hủy";
        public const string CannotChangeToStatusMessage = "Không thể thay đổi trạng thái Order đã hoàn thành hoặc đã hủy";
        public const string UpdateStatusFailedByStageMessage = "Các khâu sản xuất chưa hoàn thành";
        public const string OrderNotCompletedMessage = "Order chưa hoàn thành";
        public const string PaymentFailedMessage = "Thanh toán Order thất bại";
        public const string OrderPaidFullMessage = "Order đã thanh toán đầy đủ";
        public const string OrderNotPaidMessage = "Order chưa thanh toán xong";
    }

    public static class OrderItem
    {
        public const string EmptyIdMessage = "Id của item không hợp lệ";
        public const string NotFoundMessage = "Item không tồn tại trong hệ thống";
        public const string UpdateFailedMessage = "Cập nhật thông tin Item thất bại";
        public const string UpdateCardFailedMessage = "Cập nhật thẻ bảo hành thất bại";
    }

    public static class OrderItemStage
    {
        public const string EmptyOrderItemStageIdMessage = "Id của order không hợp lệ";
        public const string OrderItemStageNotFoundMessage = "Không tìm thấy khâu sản xuất";
        public const string UpdateStatusStageSuccessMessage = "Cập nhật trạng thái khâu sản xuất thành công";
        public const string UpdateStatusStageFailedMessage = "Cập nhật trạng thái khâu sản xuất thất bại";
        public const string PreviousStageNotCompletedMessage = "Khâu sản xuất trước chưa hoàn thành";

    }

    public static class WarrantyCard
    {
        public const string EmptyCardIdMessage = "Id của card không hợp lệ";
        public const string CardNotFoundMessage = "Không tìm thấy thẻ bảo hành";
        public const string CardCodeExistedMessage = "Mã thẻ đã tồn tại";
        public const string CreateCardFailedMessage = "Tạo mới thẻ bảo hành thất bại";
        public const string UpdateCardFailedMessage = "Cập nhật thẻ bảo hành thất bại";
        public const string CardNotMatchedCategoryMessage = "Thẻ không đúng với loại sản phẩm";

    }
    
}