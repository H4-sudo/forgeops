# ForgeOps

ForgeOps is a workshop operations platform for managing customers, vehicles, work orders, technicians, inventory and invoices.

The goal is to build a realistic full-stack application using **C#, ASP.NET Core, Angular and PostgreSQL**, while keeping the implementation simple, logical and maintainable.

The application should feel like something a real workshop could actually use.

---

## 1. Technology

### Backend

* C#
* ASP.NET Core
* Entity Framework Core
* PostgreSQL
* SignalR
* Swagger / OpenAPI

### Frontend

* Angular
* TypeScript
* Angular Router
* Reactive Forms
* RxJS
* Signals

### Development

* Docker
* Docker Compose
* GitHub Actions
* Automated tests

The project should be maintained as a **monorepo**.

---

# 2. Users

ForgeOps supports four user roles:

* Administrator
* Manager
* Receptionist
* Technician

The backend is responsible for enforcing permissions.

The frontend may hide functionality that a user cannot access, but frontend restrictions must never be considered security.

---

# 3. Customers

A customer represents a person who owns one or more vehicles.

A customer contains:

* First name
* Last name
* Email
* Phone number
* Address
* Notes
* Created date
* Updated date

Users should be able to:

* Create customers
* Edit customers
* View customers
* Search customers
* View a customer's vehicles
* View a customer's work-order history

Customers should be searchable by:

* Name
* Email
* Phone number

---

# 4. Vehicles

A vehicle belongs to a customer.

A vehicle contains:

* Make
* Model
* Year
* Registration
* VIN
* Odometer
* Colour
* Notes
* Current owner

Users should be able to:

* Create vehicles
* Edit vehicles
* View vehicles
* Search vehicles
* View vehicle history
* View previous work orders
* View inspection history
* Transfer ownership

Vehicle ownership changes must be recorded.

A vehicle cannot have two active owners at the same time.

---

# 5. Work Orders

A work order represents work being performed on a vehicle.

A work order contains:

* Work order number
* Vehicle
* Customer
* Description
* Priority
* Status
* Assigned technician
* Created by
* Created date
* Estimated completion
* Actual completion
* Customer notes
* Internal notes

### Priority

* Low
* Normal
* High
* Critical

### Status

```text
DRAFT
QUOTED
APPROVED
IN_PROGRESS
AWAITING_PARTS
QUALITY_CHECK
COMPLETED
INVOICED
CANCELLED
```

The application must enforce valid status transitions.

For example:

```text
DRAFT
  ↓
QUOTED
  ↓
APPROVED
  ↓
IN_PROGRESS
  ↓
QUALITY_CHECK
  ↓
COMPLETED
  ↓
INVOICED
```

Invalid transitions must be rejected by the backend.

---

# 6. Work Order Items

A work order can contain labour and parts.

## Labour

Each labour item contains:

* Description
* Technician
* Hours
* Hourly rate

## Parts

Each part item contains:

* Part
* Description
* Quantity
* Unit price

The backend calculates:

```text
Labour Total
+
Parts Total
=
Subtotal
```

Then:

```text
Subtotal
+
Tax
=
Total
```

Financial totals supplied by the frontend must never be trusted.

---

# 7. Technicians

Technicians can be assigned to work orders.

A technician contains:

* Name
* Email
* Phone number
* Active status

Managers should be able to:

* Create technicians
* Edit technicians
* Activate/deactivate technicians
* Assign technicians to work orders
* View technician workload

Inactive technicians cannot be assigned to new work.

---

# 8. Vehicle Inspections

A work order can contain a vehicle inspection.

The inspection contains a checklist.

Initial checklist items should include:

* Engine oil
* Coolant
* Brake fluid
* Tyres
* Brake pads
* Battery
* Lights
* Windscreen
* Suspension
* Bodywork

Each item has a status:

```text
PASS
FAIL
NOT_CHECKED
```

Inspection items may contain notes.

Example:

```text
Brake pads
Status: FAIL
Note: Front pads approximately 2mm remaining.
```

Inspection history must remain attached to the vehicle.

---

# 9. Inventory

ForgeOps tracks workshop parts.

A part contains:

* SKU
* Part number
* Name
* Description
* Quantity
* Minimum quantity
* Cost price
* Selling price
* Active status

Users should be able to:

* Create parts
* Edit parts
* Search parts
* View stock levels
* Adjust stock
* View inventory history
* Identify low-stock parts

Stock must not silently become negative.

Every stock change must be recorded.

Example:

```text
+20  STOCK_RECEIVED
-1   WORK_ORDER
+5   STOCK_ADJUSTMENT
```

The inventory history should make it possible to determine how the current stock was reached.

---

# 10. Invoices

A completed work order can produce an invoice.

An invoice contains:

* Invoice number
* Customer
* Vehicle
* Work order
* Line items
* Subtotal
* Tax
* Total
* Status
* Created date
* Paid date

Invoice statuses:

```text
DRAFT
ISSUED
PAID
VOID
```

A paid invoice must not be deleted.

---

# 11. Audit History

Important actions must be recorded.

Examples:

* Customer created
* Customer updated
* Vehicle created
* Vehicle ownership transferred
* Work order created
* Technician assigned
* Work order status changed
* Part added to work order
* Inventory adjusted
* Invoice issued
* Invoice paid

An audit entry contains:

* ID
* Timestamp
* User
* Action
* Entity type
* Entity ID
* Description

Audit history is append-only through the application.

Normal users must not be able to delete audit entries.

---

# 12. Authentication

Users must log in before accessing the application.

The system must support:

* Login
* Logout
* Password hashing
* Access tokens
* Refresh tokens
* Protected API endpoints
* Role-based authorization

Passwords must never be stored in plaintext.

---

# 13. Dashboard

The application should have a dashboard showing the current state of the workshop.

The dashboard should provide useful information such as:

### Work orders

* Open work orders
* Work orders in progress
* Awaiting parts
* Quality checks
* Completed today

### Inventory

* Low-stock parts
* Out-of-stock parts

### Workshop

* Active technicians
* Vehicles currently being worked on
* Outstanding quotes
* Unpaid invoices

### Activity

Show recent important activity.

Example:

```text
11:42  Johan moved WO-1024 to QUALITY_CHECK
11:37  Pieter added Bosch oil filter to WO-1024
11:19  Sarah created customer John Smith
10:54  Manager approved WO-1021
```

---

# 14. Search, Filtering and Pagination

Work orders must support searching/filtering by:

* Work order number
* Customer
* Registration
* VIN
* Technician
* Status
* Priority
* Date range

Results must support:

* Pagination
* Sorting
* Filtering

Filtering and pagination must happen on the backend.

The frontend must not load the entire database and filter it locally.

---

# 15. Real-Time Updates

ForgeOps should use SignalR for real-time updates.

When an important event occurs, connected clients should receive the update without refreshing the page.

At minimum, support events for:

* Work order updated
* Work order status changed
* Inventory updated
* Technician assignment changed

Example:

```text
Technician changes:

IN_PROGRESS
      ↓
QUALITY_CHECK

Manager dashboard updates automatically.
```

---

# 16. Notifications

The frontend should display useful notifications when real-time events occur.

Example:

```text
🔧 Work Order WO-1024 has entered Quality Check.
```

Notifications should be temporary but should not interrupt normal application usage.

---

# 17. Angular Application

The Angular application should be divided into sensible feature areas.

Suggested structure:

```text
src/app/

├── core/
├── shared/
│
├── features/
│   ├── auth/
│   ├── dashboard/
│   ├── customers/
│   ├── vehicles/
│   ├── work-orders/
│   ├── inventory/
│   ├── technicians/
│   └── invoices/
│
└── app.routes.ts
```

Use:

* Standalone components
* Lazy-loaded routes
* Reactive forms
* Signals where appropriate
* RxJS where appropriate
* HTTP interceptors
* Route guards

The structure is a guideline, not a mandatory architecture.

Keep the frontend understandable.

---

# 18. API

The API should expose sensible REST endpoints.

Examples:

```text
GET    /api/customers
GET    /api/customers/{id}
POST   /api/customers
PUT    /api/customers/{id}

GET    /api/vehicles
GET    /api/vehicles/{id}
POST   /api/vehicles
PUT    /api/vehicles/{id}

GET    /api/work-orders
GET    /api/work-orders/{id}
POST   /api/work-orders
PUT    /api/work-orders/{id}
POST   /api/work-orders/{id}/status

GET    /api/inventory
GET    /api/inventory/{id}
POST   /api/inventory
POST   /api/inventory/{id}/adjust

GET    /api/invoices
GET    /api/invoices/{id}
POST   /api/invoices
POST   /api/invoices/{id}/issue
POST   /api/invoices/{id}/pay
```

The exact API design is left to the developer.

Use appropriate HTTP status codes.

---

# 19. Validation

Input must be validated by the backend.

Frontend validation should provide immediate feedback to users, but backend validation remains authoritative.

Validation should cover:

* Required fields
* Invalid formats
* Invalid values
* Business rules
* Authorization
* Work-order state transitions
* Inventory constraints
* Financial calculations

---

# 20. Error Handling

API errors should have a consistent structure.

The frontend should display useful errors to users.

The application should distinguish between:

* Validation errors
* Authentication failures
* Authorization failures
* Missing resources
* Conflicts
* Unexpected server errors

Unexpected exceptions should be handled centrally.

---

# 21. Logging

The backend should use structured logging.

Log useful application events such as:

* Application startup
* Authentication failures
* Important business events
* Unexpected exceptions
* External/system failures

Do not log:

* Passwords
* Access tokens
* Refresh tokens
* Unnecessary sensitive customer information

---

# 22. Testing

The backend must contain automated tests for important business logic.

At minimum, test:

* Valid work-order transitions
* Invalid work-order transitions
* Technician assignment rules
* Inventory rules
* Invoice calculations
* Vehicle ownership rules
* Authorization rules

API integration tests should cover important endpoints.

The Angular application should contain tests for important UI/business behaviour, including:

* Form validation
* Authentication flow
* Route protection
* API error handling
* Work-order behaviour

The exact testing strategy is left to the developer.

---

# 23. Docker

The complete application should be runnable using Docker Compose.

The development environment should include:

```text
Angular
ASP.NET Core API
PostgreSQL
```

A new developer should be able to clone the repository and follow the README to get the application running without manually installing PostgreSQL.

---

# 24. CI

GitHub Actions should run automatically when code is pushed or a pull request is opened.

The pipeline should at minimum:

```text
Build backend
Run backend tests

Install frontend dependencies
Lint frontend
Run frontend tests
Build frontend
```

A failed build or test should fail the pipeline.

---

# 25. Repository

The project is a monorepo.

Suggested structure:

```text
forgeops/

├── apps/
│   ├── api/
│   └── web/
│
├── tests/
│   ├── api.unit/
│   ├── api.integration/
│   └── web/
│
├── docs/
│
├── docker/
│
├── .github/
│   └── workflows/
│
├── docker-compose.yml
├── ForgeOps.sln
├── .editorconfig
├── .gitignore
├── README.md
└── REQUIREMENTS.md
```

The exact structure may change as the project develops.

---

# 26. Out of Scope

The following are intentionally excluded from the initial version:

* Mobile application
* Online payments
* WhatsApp integration
* SMS integration
* Accounting integrations
* Supplier portal
* AI diagnostics
* Payroll
* Advanced analytics
* Multi-workshop tenancy
* Customer self-service portal
* Appointment scheduling

Do not build these unless they become necessary for another feature.

---

# 27. Engineering Philosophy

ForgeOps is an MVP.

The primary objective is to produce working software that solves the
defined problem.

Code structure does not need to be perfect on the first pass.

Prefer:

- Simple code
- Clear naming
- Logical grouping
- Explicit business rules
- Small, understandable functions
- Minimal unnecessary abstraction

Avoid introducing architectural patterns unless they solve an actual
problem in the application.

Do not implement patterns for the sake of demonstrating knowledge of
those patterns.

The following are NOT requirements:

- Clean Architecture
- CQRS
- MediatR
- Repository Pattern
- Unit of Work
- Domain-Driven Design
- Generic repositories
- Event sourcing
- Dependency injection everywhere
- Interfaces for every class
- Abstract factories
- Excessive service layers

If a pattern genuinely solves a problem encountered during development,
it may be introduced.

Otherwise, don't.

---

## Refactoring

Refactoring is expected after functionality has been proven.

If an area becomes difficult to understand, difficult to test, duplicated,
or difficult to extend, refactor it.

The first implementation does not need to be the final implementation.

Correctness comes before architectural perfection.

---

# 28. Definition of Done

ForgeOps is considered an MVP when:

* [ ] Users can log in
* [ ] Roles and permissions work
* [ ] Customers can be managed
* [ ] Vehicles can be managed
* [ ] Vehicle ownership history is preserved
* [ ] Work orders can be created
* [ ] Work-order status transitions are enforced
* [ ] Technicians can be assigned
* [ ] Labour can be recorded
* [ ] Parts can be added to work orders
* [ ] Inventory is tracked
* [ ] Inventory history is recorded
* [ ] Vehicle inspections work
* [ ] Invoices can be generated
* [ ] Audit history is recorded
* [ ] Dashboard works
* [ ] Search/filtering/pagination work
* [ ] SignalR updates work
* [ ] API validation works
* [ ] Error handling works
* [ ] Automated tests exist
* [ ] Docker Compose works
* [ ] CI works
* [ ] Swagger/OpenAPI works
* [ ] README explains how to run the project

---

# 29. The Rule

There is intentionally no prescribed architecture.

There is no requirement to implement Clean Architecture, CQRS, Repository Pattern, Unit of Work, DDD, or any other architectural pattern.

Choose the architecture that makes sense.

The goal is not to demonstrate how many patterns you know.

The goal is to build a **good piece of software**.
