using AutoMapper;
using QuizApp.Application.DTOs.Requests.QuizQuestion;
using QuizApp.Application.DTOs.Responses.QuizQuestion;

namespace QuizApp.Application.MappingProfiles.QuizQuestion;

public class QuizQuestionProfile : Profile
{
    public QuizQuestionProfile()
    {
        // Request to Entity
        CreateMap<CreateQuizQuestionRequest, Domain.Entities.QuizQuestion>();
        CreateMap<UpdateQuizQuestionRequest, Domain.Entities.QuizQuestion>();
        CreateMap<DeleteQuizQuestionRequest, Domain.Entities.QuizQuestion>();
        CreateMap<GetQuizQuestionByIdRequest, Domain.Entities.QuizQuestion>();
        CreateMap<GetQuizQuestionsRequest, Domain.Entities.QuizQuestion>();

        // Entity to Response
        CreateMap<Domain.Entities.QuizQuestion, QuizQuestionResponse>();
    }
}