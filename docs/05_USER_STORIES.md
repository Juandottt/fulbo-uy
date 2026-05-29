# User Stories

## Existing Stories

### US01 - Register User

Status: Done

As a new user, I want to register with email and password so that I can access the application.

Acceptance criteria:

- User can submit email and password.
- Password is hashed before persistence.
- Duplicate email is rejected.
- A JWT is returned after successful registration.

### US02 - Login User

Status: Done

As a registered user, I want to log in so that I can access protected features.

Acceptance criteria:

- Valid credentials return a JWT.
- Invalid credentials are rejected.
- Response includes user identity and role information.

### US03 - Create Player Profile

Status: Done

As a player, I want to create my profile so that I can join matches with skill attributes.

Acceptance criteria:

- Authenticated user can create one profile.
- Duplicate profile creation is rejected.
- Profile includes name and football attributes.

### US04 - Update Player Profile

Status: Done

As a player, I want to update my profile so that my information stays accurate.

Acceptance criteria:

- Only the profile owner can update it.
- At least one field must be provided.
- Updated profile is returned.

### US05 - Create Match

Status: Done

As an Admin, I want to create a match so that players can join it.

Acceptance criteria:

- Only Admin users can create matches.
- Match date must be in the future.
- Field cost must be greater than zero.
- Created match is returned.

### US06 - List And View Matches

Status: Done

As an authenticated user, I want to list and view matches so that I can decide which one to join.

Acceptance criteria:

- Authenticated users can list matches.
- Authenticated users can view a match by ID.
- Missing match returns not found.

### US07 - Join Match

Status: Done

As a player, I want to join an open match so that I can participate.

Acceptance criteria:

- User must have a player profile.
- User cannot join the same match twice.
- Full or unavailable matches are rejected.
- Match status changes to full when capacity is reached.

### US08 - Balance Teams

Status: Done

As an Admin, I want to balance teams so that players are distributed fairly.

Acceptance criteria:

- Match must exist.
- Match must not be open.
- Played matches cannot be balanced.
- Players are assigned to team 1 or team 2.

### US09 - View Cost Split

Status: Done

As a player or organizer, I want to see the cost split so that everyone knows what they owe.

Acceptance criteria:

- Match must exist.
- Match must have participants.
- Cost per player is calculated from field cost and participant count.

### US10 - Confirm Payment

Status: Done in API, In Progress in frontend

As an Admin, I want to confirm participant payments so that payment status is accurate.

Acceptance criteria:

- Only Admin users can confirm payment.
- Participant must belong to the match.
- Already paid participants cannot be confirmed again.

## Planned Stories

### US15 - Product CRUD

Status: Planned

As an Admin, I want to manage products so that the application can support product-related operations.

Acceptance criteria:

- To be defined.

### Invite Link Expiration

Status: Planned

As an organizer, I want invite links to expire so that old links cannot be reused indefinitely.

Acceptance criteria:

- To be defined.

### Production Deployment

Status: Planned

As a project owner, I want the application deployed so that real users can access it.

Acceptance criteria:

- To be defined.
