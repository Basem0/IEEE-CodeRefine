using System;
using System.Collections.Generic;
using System.Linq;
using IEEEQualification.Data;
using IEEEQualification.Models;

namespace IEEEQualification.Services
{
    public class ReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public Review AddReview(int userId, int movieId, int rating, string content)
        {
            if (rating < 1 || rating > 10)
                throw new Exception("Rating must be between 1 and 10.");

            var existingReview = _context.Reviews
                .FirstOrDefault(r => r.UserId == userId && r.MovieId == movieId);

            if (existingReview != null)
                throw new Exception("You have already reviewed this movie");

            var user = _context.Users.Find(userId);
            if (user == null)
                throw new Exception("User not found");

            var movie = _context.Movies.Find(movieId);
            if (movie == null)
                throw new Exception("Movie not found");

            var review = new Review
            {
                UserId = userId,
                MovieId = movieId,
                Rating = rating,
                Content = content?.Trim() ?? ""
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();
            return review;
        }

        public List<Review> GetReviewsForMovie(int movieId)
        {
            return _context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToList();
        }

        public List<Review> GetReviewsByUser(int userId)
        {
            return _context.Reviews
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public double GetAverageRating(int movieId)
        {
            var reviews = GetReviewsForMovie(movieId);
            if (reviews.Count == 0) return 0;

            return reviews.Average(r => r.Rating);
        }

        public int GetReviewCount(int movieId)
        {
            return _context.Reviews.Count(r => r.MovieId == movieId);
        }

        public bool DeleteReview(int reviewId, int userId)
        {
            var review = _context.Reviews.Find(reviewId);
            if (review == null || review.UserId != userId)
                return false;

            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return true;
        }
    }
}