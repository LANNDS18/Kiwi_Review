using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.DisplayDto;

namespace Kiwi_review.Services
{
    public class HighlightService : IHighlightService
    {
        private readonly IUnitOfWorkWrapper _unitOfWork;
        private readonly ICheckService _check;

        public HighlightService(IUnitOfWorkWrapper unitOfWorkWrapper, ICheckService checkService)
        {
            _unitOfWork = unitOfWorkWrapper;
            _check = checkService;
        }

        public List<HighlightShowModel> GetAll(int reviewId, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return null!;
            var highlights = _unitOfWork.Highlights.FindByCondition(
                l => l.ReviewId == reviewId && l.IsDelete == false).ToList();
            if (!highlights.Any()) return null!;
            var highlightShowModels = highlights.Select(TransferHighlightShowModel).ToList();
            return highlightShowModels;
        }

        public bool Add(HighlightAddingModel highlight, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var movie = _unitOfWork.Movies.FindByCondition(t => t.MovieId == highlight.MovieId).SingleOrDefault();
            if (movie == null) return false;
            var countForUser = MovieHighlightCountPerUser(movie.MovieId, (int) userId).Count;
            if (movie.OnlyHighlightOnce && !CheckOnlyOneHighlightPerReview(highlight.ReviewId, (int) userId)) return false;
            if (countForUser >= movie.MaxHighlightPerUser) return false;
            var newHighlight = new Highlight
            {
                MovieId = highlight.MovieId,
                ReviewId = highlight.ReviewId,
                UserId = (int) userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDelete = false
            };
            _unitOfWork.Highlights.Creat(newHighlight);
            _unitOfWork.Save();
            return true;
        }

        public bool Cancel(int highlightId, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var highlight = _unitOfWork.Highlights.FindByCondition(
                l => l.HighlightId == highlightId).SingleOrDefault();

            if (highlight == null || highlight.UserId != userId) return false;

            highlight.UpdatedAt = DateTime.Now;
            highlight.IsDelete = true;
            _unitOfWork.Highlights.Update(highlight);
            _unitOfWork.Save();
            return true;
        }

        public void Delete(int highlightId)
        {
            var cancelHighlight = _unitOfWork.Highlights.FindByCondition(
                l => l.HighlightId == highlightId).SingleOrDefault();

            if (cancelHighlight == null) return;

            cancelHighlight.UpdatedAt = DateTime.Now;
            cancelHighlight.IsDelete = true;
            _unitOfWork.Highlights.Update(cancelHighlight);
            _unitOfWork.Save();
        }

        public List<Highlight> MovieHighlightCountPerUser(int movieId, int userId)
        {
            var highlights = _unitOfWork.Highlights.FindByCondition(
                l => l.MovieId == movieId && l.UserId == userId && l.IsDelete == false).ToList();
            return highlights;
        }
        
        public bool CheckOnlyOneHighlightPerReview(int reviewId, int userId)
        {
            var highlights = _unitOfWork.Highlights.FindByCondition(
                l => l.ReviewId == reviewId && l.UserId == userId && l.IsDelete == false).ToList();
            return !highlights.Any();
        }

        public HighlightShowModel ShowHighlight(Highlight highlight)
        {
            return TransferHighlightShowModel(highlight);
        }

        private static HighlightShowModel TransferHighlightShowModel(Highlight highlight)
        {
            if (highlight == null) return null;
            var highlightDisplay = new HighlightShowModel
            {
                HighlightId = highlight.HighlightId
            };
            return highlightDisplay;
        }
    }
}