# Project Overview

## What The Project Is

FulboUY is a web application for organizing amateur football matches. It includes a .NET backend API and a React frontend.

## Main Goal

Help players and organizers create matches, manage player profiles, join games, split field costs, track payments, and generate balanced teams.

## Current Implemented Features

- User registration and login with JWT.
- Password hashing with BCrypt.
- Player profile creation, lookup, and update.
- Match creation by Admin users.
- Match listing and lookup.
- Joining matches as an authenticated player.
- Participant listing.
- Automatic match status update when a match reaches capacity.
- Team balancing service.
- Cost split calculation.
- Payment status lookup.
- Payment confirmation endpoint for Admin users.
- Global exception middleware.
- React frontend pages for auth, profile, matches, match detail, and match creation.

## Current Missing Features

- Product CRUD / US15.
- Invite link expiration workflow.
- Complete payment confirmation UI.
- Production deployment.
- Integration tests.
- End-to-end tests.
- Full frontend test coverage.

## Target Users

- Amateur football players.
- Match organizers.
- Admin users who manage matches and payments.

## MVP Idea

The MVP should allow a user to register, create or complete a player profile, view matches, join a match, and allow an Admin to create matches, balance teams, split costs, and confirm payments.
