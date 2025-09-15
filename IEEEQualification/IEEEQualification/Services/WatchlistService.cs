using System;
using System.Collections.Generic;
using System.Linq;
using IEEEQualification.Data;
using IEEEQualification.Models;

namespace IEEEQualification.Services
{
    public class WatchlistService
    {
        private readonly AppDbContext _context;

        public WatchlistService(AppDbContext context)
        {
            _context = context;
        }

        public WatchlistItem AddToWatchlist(int userId, int movieId)
        {
            var exists = _context.WatchlistItems.Any(w => w.UserId == userId && w.MovieId == movieId);
            if (exists)
                throw new Exception("Movie already in watchlist.");

            var user = _context.Users.Find(userId);
            if (user == null)
                throw new Exception("User not found");

            var movie = _context.Movies.Find(movieId);
            if (movie == null)
                throw new Exception("Movie not found");

            var item = new WatchlistItem
            {
                UserId = userId,
                MovieId = movieId,
                Watched = false
            };

            _context.WatchlistItems.Add(item);
            _context.SaveChanges();
            return item;
        }

        public bool MarkAsWatched(int userId, int movieId)
        {
            var item = _context.WatchlistItems
                .FirstOrDefault(w => w.UserId == userId && w.MovieId == movieId);

            if (item == null)
                return false;

            item.Watched = true;
            _context.SaveChanges();
            return true;
        }

        public bool MarkAsUnwatched(int userId, int movieId)
        {
            var item = _context.WatchlistItems
                .FirstOrDefault(w => w.UserId == userId && w.MovieId == movieId);

            if (item == null)
                return false;

            item.Watched = false;
            _context.SaveChanges();
            return true;
        }

        public bool RemoveFromWatchlist(int userId, int movieId)
        {
            var item = _context.WatchlistItems
                .FirstOrDefault(w => w.UserId == userId && w.MovieId == movieId);

            if (item == null)
                return false;

            _context.WatchlistItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public List<Movie> GetWatchHistory(int userId)
        {
            return _context.WatchlistItems
                .Where(w => w.UserId == userId && w.Watched)
                .Select(w => w.Movie)
                .ToList();
        }

        public List<Movie> GetCurrentWatchlist(int userId)
        {
            return _context.WatchlistItems
                .Where(w => w.UserId == userId && !w.Watched)
                .Select(w => w.Movie)
                .ToList();
        }

        public int GetWatchlistCount(int userId)
        {
            return _context.WatchlistItems.Count(w => w.UserId == userId && !w.Watched);
        }

        public int GetWatchedCount(int userId)
        {
            return _context.WatchlistItems.Count(w => w.UserId == userId && w.Watched);
        }

        public bool IsInWatchlist(int userId, int movieId)
        {
            return _context.WatchlistItems
                .Any(w => w.UserId == userId && w.MovieId == movieId);
        }
    }
}