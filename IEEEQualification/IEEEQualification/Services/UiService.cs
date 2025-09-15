using System;
using System.Collections.Generic;
using IEEEQualification.Models;

namespace IEEEQualification.Services
{
    public class UiService
    {
        #region Auth UI
        public User? ShowAuthMenu(AuthService authService)
        {
            while (true)
            {
                Console.WriteLine("\n===== Welcome to IEEEMDB =====");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==============================");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": return HandleLogin(authService);
                    case "2": return HandleRegister(authService);
                    case "0": Environment.Exit(0); return null;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        private User? HandleLogin(AuthService authService)
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            var user = authService.Login(username, password);
            if (user == null)
                Console.WriteLine("Invalid credentials.");
            else
                Console.WriteLine($"Welcome {user.Username} ({user.Role})!");

            return user;
        }

        private User? HandleRegister(AuthService authService)
        {
            Console.Write("Username: "); var username = Console.ReadLine();
            Console.Write("Email: "); var email = Console.ReadLine();
            Console.Write("Password: "); var password = Console.ReadLine();

            try
            {
                var user = authService.Register(username, email, password);
                Console.WriteLine("Registration successful!");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        #endregion

        #region Admin UI
        public string ShowAdminMenu()
        {
            Console.WriteLine("\n===== Admin Menu =====");
            Console.WriteLine("1. Add Movie");
            Console.WriteLine("2. Edit Movie");
            Console.WriteLine("3. Delete Movie");
            Console.WriteLine("4. View All Movies");
            Console.WriteLine("0. Logout");
            Console.WriteLine("====================");
            Console.Write("Choose: ");
            return Console.ReadLine() ?? "";
        }
        #endregion

        #region User UI
        public string ShowUserMenu()
        {
            Console.WriteLine("\n===== User Menu =====");
            Console.WriteLine("1. View All Movies");
            Console.WriteLine("2. Search Movies");
            Console.WriteLine("3. View Movie Details");
            Console.WriteLine("4. Add Review");
            Console.WriteLine("5. Add Movie To Watchlist");
            Console.WriteLine("6. Mark Movie As Watched");
            Console.WriteLine("7. Mark Movie As Unwatched");
            Console.WriteLine("8. Remove Movie From Watchlist");
            Console.WriteLine("9. View Watch History");
            Console.WriteLine("10. View Current Watchlist");
            Console.WriteLine("0. Logout");
            Console.WriteLine("===================");
            Console.Write("Choose: ");
            return Console.ReadLine() ?? "";
        }
        #endregion

        #region Movie UI
        public void DisplayAllMovies(List<Movie> movies, ReviewService? reviewService = null)
        {
            if (movies.Count == 0)
            {
                Console.WriteLine("No movies found.");
                return;
            }

            Console.WriteLine("=== Movies ===");
            foreach (var m in movies)
            {
                string ratingInfo = "";
                if (reviewService != null)
                {
                    var avg = reviewService.GetAverageRating(m.Id);
                    var count = reviewService.GetReviewCount(m.Id);
                    ratingInfo = $" | Rating: {avg:F1} ({count} reviews)";
                }

                Console.WriteLine($"{m.Id}: {m.Title} - {m.Category} - {m.Author}{ratingInfo}");
            }
        }

        public (string title, string desc, string author, string category) GetMovieDetails()
        {
            Console.Write("Title: "); var title = Console.ReadLine() ?? "";
            Console.Write("Description: "); var desc = Console.ReadLine() ?? "";
            Console.Write("Author: "); var author = Console.ReadLine() ?? "";
            Console.Write("Category: "); var category = Console.ReadLine() ?? "";
            return (title, desc, author, category);
        }

        public string GetKeyword(string prompt = "Enter keyword: ")
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }

        public Movie? GetMovieSelection(List<Movie> movies)
        {
            if (movies.Count == 0)
            {
                Console.WriteLine("No matching movies found.");
                return null;
            }
            if (movies.Count == 1) return movies[0];

            Console.WriteLine("Multiple movies found. Choose one:");
            for (int i = 0; i < movies.Count; i++)
                Console.WriteLine($"{i + 1}. {movies[i].Title} - {movies[i].Category} - {movies[i].Author}");

            while (true)
            {
                Console.Write("Select number: ");
                if (int.TryParse(Console.ReadLine(), out int sel) && sel >= 1 && sel <= movies.Count)
                    return movies[sel - 1];
                Console.WriteLine("Invalid selection, try again.");
            }
        }

        public void DisplayMovieDetails(Movie m, ReviewService reviewService)
        {
            Console.WriteLine($"\n=== {m.Title} ===");
            Console.WriteLine($"Author: {m.Author}");
            Console.WriteLine($"Category: {m.Category}");
            Console.WriteLine($"Description: {m.Description}");
            var avg = reviewService.GetAverageRating(m.Id);
            var count = reviewService.GetReviewCount(m.Id);
            Console.WriteLine($"Average Rating: {avg:F1} ({count} reviews)");

            var reviews = reviewService.GetReviewsForMovie(m.Id);
            if (reviews.Count > 0)
            {
                Console.WriteLine("Reviews:");
                foreach (var r in reviews)
                    Console.WriteLine($"- {r.User.Username}: {r.Rating}/10 | {r.Content}");
            }
            else
                Console.WriteLine("No reviews yet.");
        }

        #endregion

        #region Review & Watchlist UI
        public (int movieId, int rating, string content) GetReviewDetails(Movie movie)
        {
            Console.Write("Rating (1-10): "); int.TryParse(Console.ReadLine(), out int rating);
            Console.Write("Review: "); var content = Console.ReadLine() ?? "";
            return (movie.Id, rating, content);
        }

        public void DisplayWatchlist(List<Movie> movies, string title)
        {
            Console.WriteLine($"=== {title} ===");
            if (movies.Count == 0)
            {
                Console.WriteLine("No movies found.");
                return;
            }
            foreach (var m in movies)
                Console.WriteLine($"{m.Id}: {m.Title} - {m.Category} - {m.Author}");
        }
        #endregion
    }
}
