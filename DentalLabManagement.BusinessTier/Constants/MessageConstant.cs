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
        public const string UpdateDentalSuccessMessage = "Cập nhật thông tin Dental thành công";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
    }

    public static class Category
    {
        public const string CategoryNameExisted = "Category Name đã tồn tại";
        public const string CreateNewCategoryFailedMessage = "Tạo mới Category thất bại";
        public const string EmptyCategoryIdMessage = "Category Id không hợp lệ";
        public const string CategoryNotFoundMessage = "Category không có trong hệ thống";
        public const string UpdateCategorySuccessMessage = "Category được cập nhật thành công";
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
        public const string UpdateProductSuccessMessage = "Cập nhật thông tin product thành công";
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
        public const string UpdateProductStageFailedMessage = "Cập nhật thông tin thất bại";
        public const string UpdateProductStageSuccessMessage = "Cập nhật thông tin thành công";
    }

    public static class TeethPosition
    {
        public const string TeethPositionExisted = "Teeth Position đã tồn tại";
        public const string CreateTeethPositionFailed = "Tạo mới teeth position thất bại";
        public const string EmptyTeethPositionIdMessage = "Id không hợp lệ";
        public const string IdNotFoundMessage = "Teeth Position không tồn tại trong hệ thống";
        public const string UpdateFailedMessage = "Cập nhật thông tin thất bại";
        public const string UpdateSucessMessage = "Cập nhật thông tin thành công";
        public const string ToothArchError = "Tooth Arch phải từ 1 đến 4";
        public const string NameFormatMessage = "Position Name có dạng (1-1, 1-2, ..)";

    }

    public static class Order
    {
        public const string CreateOrderFailedMessage = "Tạo mới order thất bại";
        public const string EmptyOrderIdMessage = "Id của order không hợp lệ";
        public const string InvoiceIdExistedMessage = "InvoiceId đã tồn tại";
        public const string OrderNotFoundMessage = "Order không tồn tại trong hệ thống";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái Order thất bại";
        public const string NewStatusMessage = "Order đã được tạo";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái Order thành công";
        public const string ProducingStatusMessage = "Order đang được sản xuất";
        public const string ProducingStatusRepeatMessage = "Order đã được đưa vào sản xuất";
        public const string CompletedStatusMessage = "Order đã hoàn thành";
        public const string PaidStatusMessage = "Order đã thanh toán";
        public const string CanceledStatusMessage = "Order đã bị hủy";
        public const string CanceledStatusRepeatMessage = "Không thể thay đổi trạng thái Order đã hủy";
        public const string UpdateFailedByStageMessage = "Các khâu sản xuất chưa hoàn thành";
        public const string OrderNotCompletedMessage = "Order chưa hoàn thành";
        public const string PaymentFailedMessage = "Thanh toán Order thất bại";
        public const string OrderPaidFullMessage = "Order đã thanh toán đầy đủ";
        public const string OrderNotPaidMessage = "Order chưa thanh toán xong";
        public const string EmptyOrderMessage = "Đối tác chưa có Order nào";
    }

    public static class OrderItem
    {
        public const string EmptyIdMessage = "Id của item không hợp lệ";
        public const string NotFoundMessage = "Item không tồn tại trong hệ thống";
        public const string UpdateFailedMessage = "Cập nhật thông tin Item thất bại";
        public const string UpdateSuccessMessage = "Cập nhật thông tin Item thành công";
        public const string UpdateCardFailedMessage = "Cập nhật thẻ bảo hành thất bại";
        public const string OrderItemNotMatchOrder = "Item không khớp Order";
    }

    public static class OrderItemStage
    {
        public const string EmptyOrderItemStageIdMessage = "Id của order không hợp lệ";
        public const string OrderItemStageNotFoundMessage = "Không tìm thấy khâu sản xuất";
        public const string UpdateStatusStageSuccessMessage = "Cập nhật trạng thái khâu sản xuất thành công";
        public const string UpdateStatusStageFailedMessage = "Cập nhật trạng thái khâu sản xuất thất bại";
        public const string PreviousStageNotCompletedMessage = "Khâu sản xuất trước chưa hoàn thành";

    }

    public static class CardType
    {
        public const string EmptyCardIdMessage = "Id của card không hợp lệ";
        public const string CreateCardFailedMessage = "Tạo mới thẻ thất bại";
        public const string CardNotFoundMessage = "Thẻ chưa có trong hệ thống";
        public const string UpdateCardFailedMessage = "Cập nhật thẻ thất bại";
        public const string UpdateCardSuccessMessage = "Cập nhật thẻ thành công";
        public const string UpdateStatusSuccessMessage = "Cập nhật trạng thái thành công";
        public const string UpdateStatusFailedMessage = "Cập nhật trạng thái thất bại";
    }

    public static class WarrantyCard
    {
        public const string EmptyCardIdMessage = "Id của card không hợp lệ";
        public const string CardNotFoundMessage = "Không tìm thấy thẻ bảo hành";
        public const string CardCodeExistedMessage = "Mã thẻ đã tồn tại";
        public const string CreateCardFailedMessage = "Tạo mới thẻ bảo hành thất bại";
        public const string InsertCardFailedMessage = "Cập nhật thẻ bảo hành thất bại";
        public const string InsertCardSuccessMessage = "Cập nhật thẻ bảo hành thành công";
        public const string CardNotMatchedCategoryMessage = "Thẻ không đúng với loại sản phẩm";
        public const string UpdatedSuccessMessage = "Cập nhật thành công";
        public const string UpdateFailedMessage = "Cập nhật thất bại";
    }

    public static class WarrantyRequest
    {
        public const string WarrantyRequestFailedMessage = "Yêu cầu bảo hành thất bại";
    }
    
}