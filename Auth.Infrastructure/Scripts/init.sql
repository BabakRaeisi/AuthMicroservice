CREATE TABLE IF NOT EXISTS public."Users" (
    "UserID" UUID PRIMARY KEY,
    "Email" TEXT NOT NULL UNIQUE,
    "PersonName" TEXT,
    "Gender" TEXT,
    "Password" TEXT NOT NULL
);