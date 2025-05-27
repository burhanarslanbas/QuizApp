using Microsoft.AspNetCore.Mvc;
using QuizApp.Application.DTOs.Responses.Quiz;
using QuizApp.Application.DTOs.Requests.Question;
using QuizApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDetailResponse>> GetById(Guid id)
        {
            var request = new GetQuestionByIdRequest { Id = id };
            var question = await _questionService.GetByIdAsync(request);
            return Ok(question);
        }

        [HttpGet]
        public async Task<ActionResult<List<QuestionDetailResponse>>> GetAll([FromQuery] GetQuestionsRequest request)
        {
            var questions = await _questionService.GetAllAsync(request);
            return Ok(questions);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDetailResponse>> Create(CreateQuestionRequest request)
        {
            var result = await _questionService.CreateAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<QuestionDetailResponse>> Update(UpdateQuestionRequest request)
        {
            var result = await _questionService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var request = new DeleteQuestionRequest { Id = id };
            await _questionService.DeleteAsync(request);
            return NoContent();
        }

        [HttpDelete("range")]
        public async Task<ActionResult> DeleteRange([FromBody] DeleteRangeQuestionRequest request)
        {
            await _questionService.DeleteRangeAsync(request);
            return NoContent();
        }
    }
} 