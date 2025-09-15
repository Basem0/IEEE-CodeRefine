## Deep Dives
### 1. Search

Problem: Searching in a big database is slow.

Solution: Use Elasticsearch.

Features:

Search by title, author, genre, actor

Fast and ranked by relevance, rating, popularity

### 2. Recommendations

Suggest movies users may like.

Uses watch history, ratings, reviews, and preferences.

Precomputed for speed, stored in Redis.

API: GET /users/{id}/recommendations

### 3. Notifications

Send alerts for new movies, reviews, or trends.

Event-based: system publishes events → notifications sent.

Can be in-app, push, or email.

Filters by user preferences and avoids spamming.

### 4. Data Flow

Add Movie: Admin → Database → Search Index → Notifications → Users

Add Review: User → Database → Update rating → Recommendations

### 5. Caching

Redis: for search, recommendations, sessions

CDN: for posters and trailers


### 6. Operations

Regular backups

Batch jobs for recommendations and search indexing

Scale database and search cluster as needed
