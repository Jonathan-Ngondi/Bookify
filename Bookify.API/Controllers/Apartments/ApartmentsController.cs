using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bookify.API.Controllers.Apartments
{
    [Authorize]
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly ISender _sender;

        public ApartmentsController(ISender sender)
        {
            _sender = sender;
        }


        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> SearchApartments(
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken)
        {
            var query = new SearchApartmentsQuery(startDate, endDate);

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }

    }
}

