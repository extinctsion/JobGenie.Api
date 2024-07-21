﻿using GenAI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Resume.Application.DTOs;
using Resume.Application.Service;

namespace Resume.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IGenAI _genAi;
        public ResumeController(IGenAI genAi)
        {
            _genAi = genAi;
        }
        [HttpPost]
        public async Task<IActionResult> Create(ResumeRequestDto resumeDto)
        {
            string resumeStr = JsonConvert.SerializeObject(resumeDto);
            string requestPrompt = $"Get a resume response generated with available information: {resumeStr}. Try to generate as much info as possible. Don't give null for int values.";
            ResumeDtoResponse resumeDtoResponse = await _genAi.GenerateResponseObject<ResumeDtoResponse>(requestPrompt);
            var generatedPdfByteArray = PdfService.GeneratePdf(resumeDtoResponse.Resume);
            return File(generatedPdfByteArray, "application/pdf", "Resume.pdf");
        }
    }
}
