﻿using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISignInService _authService;

        public AuthController(ISignInService authService)
        {
            _authService = authService;
        }

        private ActionResult GetSuitableAnswerForException(Exception ex)
        {
            var message = $"{ex.GetType()}: {ex.Message}";
            if (ex is UserNotFoundException) return NotFound(message);
            else if (ex is InvalidDataException) return BadRequest(message);
            return new ObjectResult(message);
        }

        [HttpPost]
        public async Task<ActionResult<Result<string>>> SignIn(LoginUserDTO loginUserDTO)
        {
            try
            {
                return Ok(await _authService.LoginAsync(loginUserDTO));
            }
            catch (Exception ex)
            {
                return GetSuitableAnswerForException(ex);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task Logout()
        {
            await _authService.LogoutAsync();
        }
    }
}
