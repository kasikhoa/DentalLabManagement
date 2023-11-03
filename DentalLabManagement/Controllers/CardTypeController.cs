using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.CardType;
using DentalLabManagement.BusinessTier.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class CardTypeController : BaseController<CardTypeController>
    {
        private readonly ICardTypeService _cardTypeService;

        public CardTypeController(ILogger<CardTypeController> logger, ICardTypeService cardTypeService) : base(logger)
        {
            _cardTypeService = cardTypeService;
        }

        [HttpPost(ApiEndPointConstant.CardType.CardTypesEndPoint)]
        [ProducesResponseType(typeof(CardTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCardType(CardTypeRequest request)
        {
            var response = await _cardTypeService.CreateCardType(request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.CardType.CardTypesEndPoint)]
        [ProducesResponseType(typeof(CardTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCardTypes(int? categoryId, string? code, CardTypeStatus? status, int page, int size)
        {
            var response = await _cardTypeService.GetCardTypes(categoryId, code, status, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.CardType.CardTypeEndPoint)]
        [ProducesResponseType(typeof(CardTypeResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCardTypeById(int id)
        {
            var response = await _cardTypeService.GetCardTypeById(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.CardType.CardTypeEndPoint)]
        public async Task<IActionResult> UpdateCardType(int id, UpdateCardRequest request)
        {
            var isSuccessful = await _cardTypeService.UpdateCardType(id, request);
            if (!isSuccessful) return Ok(MessageConstant.CardType.UpdateCardFailedMessage);
            return Ok(MessageConstant.CardType.UpdateCardSuccessMessage);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.CardType.CardTypeEndPoint)]
        public async Task<IActionResult> DeleteCardType(int id)
        {
            var isSuccessful = await _cardTypeService.DeleteCardType(id);
            if (!isSuccessful) return Ok(MessageConstant.CardType.UpdateStatusFailedMessage);
            return Ok(MessageConstant.CardType.UpdateStatusSuccessMessage);
        }

    }
}
