using API.Models.DTOs.Rentals;
using API.Models.Rentals;
using API.Services.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Rentals
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(PaymentsService service)
        : BaseApiController<Payment, PaymentDto, PaymentDto>(service)
    {

        protected override int GetEntityId(PaymentDto entity)
        {
            return entity.PaymentId;
        }
    }
}
