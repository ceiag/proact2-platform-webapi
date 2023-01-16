using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class SurveyAnswersBlockQueriesService : ISurveyAnswersBlockQueriesService {
        private readonly ProactDatabaseContext _database;

        public SurveyAnswersBlockQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public List<SurveyAnswersBlock> GetsAll( Guid projectId ) {
            return _database.SurveyAnswersBlocks
                .Where( x => x.ProjectId == projectId )
                .ToList();
        }

        public SurveyAnswersBlock Get( Guid answerBlockId ) {
            return _database.SurveyAnswersBlocks.FirstOrDefault( x => x.Id == answerBlockId );
        }

        private SurveyAnswersBlock CreateAnswersBlockEntity() {
            var answersBlock = new SurveyAnswersBlock() {
                Id = Guid.NewGuid(),
                Answers = new List<SurveyAnswer>()
            };

            _database.SurveyAnswersBlocks.Add( answersBlock );

            return answersBlock;
        }

        private SurveyAnswersBlock AddLabelsToAnswersBlock(
            AnswersBlockCreationRequest request, SurveyAnswersBlock answersBlock ) {
            foreach ( var label in request.Labels ) {
                var surveyAnswer = new SurveyAnswer() {
                    LabelId = label,
                    AnswersBlockId = answersBlock.Id
                };

                AddSurveyAnswerToAnswersBlock( surveyAnswer );
            }

            return answersBlock;
        }

        private SurveyAnswer AddSurveyAnswerToAnswersBlock( SurveyAnswer answer ) {
            return _database.SurveyAnswers.Add( answer ).Entity;
        }

        public SurveyAnswersBlock Create( Guid projectId, AnswersBlockCreationRequest request ) {
            var answersBlock = CreateAnswersBlockEntity();
            answersBlock.ProjectId = projectId;

            AddLabelsToAnswersBlock( request, answersBlock );

            return answersBlock;
        }
    }
}
