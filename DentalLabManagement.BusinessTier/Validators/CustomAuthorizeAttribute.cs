using Microsoft.AspNetCore.Authorization;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Utils;

namespace DentalLabManagement.BusinessTier.Validators;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
	public CustomAuthorizeAttribute(params RoleEnum[] roleEnums)
	{
		var allowedRolesAsString = roleEnums.Select(x => x.GetDescriptionFromEnum());
		Roles = string.Join(",", allowedRolesAsString);
	}
}