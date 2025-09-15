# IEEE-CodeRefine
System Design For Code Refine

## The System Design
<img width="2935" height="2824" alt="غير معنون-2025-05-21-1601" src="https://github.com/user-attachments/assets/aa83286f-b0db-4b29-9d80-27c7099c0b57" />

## Image For System Run :
<img width="317" height="164" alt="image" src="https://github.com/user-attachments/assets/d12dc93e-d6bb-491b-bd6e-73a75a4d8ddc" />

### User Menu
<img width="379" height="303" alt="image" src="https://github.com/user-attachments/assets/166a9b05-7049-4f8b-94c2-99319f30a8e8" />

### Admin Menu
<img width="290" height="146" alt="image" src="https://github.com/user-attachments/assets/2483ddc8-8738-4696-859a-206d1ced6b68" />

## Back-of-the-Envelope Estimation (Bonus Part) :

#### 1. Users

Assume 1 million registered users.
Active users per day: 10% => 100000 DAU (Daily Active Users)

#### 2. Movies

Assume 100,000 movies in the database
Average size per movie record: 1 KB
Total storage: 100000 × 1 KB = 100 MB

#### 3. Reviews

Assume 10 reviews per user on average → 10 million reviews
Average review record: 500 bytes (rating + content + references)
Total storage: 10000000 × 0.5 KB = 5 GB

#### 4. Watchlist

Assume 50% of users use watchlists => 500,000 users
Average 20 movies per watchlist → 10 million watchlist items
Average item record: 100 bytes
Total storage: 10000000 × 0.1 KB = 1 GB

## Notes:
#### Note That Implementation Not Complete Because NO Time Enough
#### Note I Use Console App C# & Entity FrameWork Using Sqlite
#### DataBase Uploaded With Code But Empty
#### To Test Admin Feauter Add "admin" As UserName & "admin123" As Password

## Team:
### Ahmed Basem Mohamed
### Mohamed ElMetwally Sobhy
### Khaled Mohamed

