﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Picture")]
    [Authorize]
    public class PictureController : Controller
    {
        private readonly NewsBlogContext _context;
        private readonly UserManager<User> _userManager;

        public PictureController(NewsBlogContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Picture/5
        [HttpGet]
        [Route("{id}")]
        public IEnumerable<Picture> GetPictures([FromRoute] int id)
        {
            return _context.Pictures.Where(i => i.ArticleId == id);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult PostPicture([FromBody] PictureDTO articleDTO)
        {
            try
            {
                var addedPicture = _context.Pictures.Add(new Picture
                {
                    ArticleId = articleDTO.ArticleId,
                    Image = articleDTO.Image
                });

                _context.SaveChanges(); // elmentjük az új épületet

                articleDTO.Id = addedPicture.Entity.Id;

                // visszaküldjük a létrehozott épületet
                return Created(Request.GetUri() + addedPicture.Entity.Id.ToString(), articleDTO);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Picture/5
        [HttpDelete("{id}")]
        public IActionResult DeleteItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _context.Pictures.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Pictures.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }
    }
}